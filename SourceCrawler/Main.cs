/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using ScintillaNET_FindReplaceDialog;
using SourceCrawler.Properties;
using Timer = System.Windows.Forms.Timer;

namespace SourceCrawler
{
    public partial class Main : Form
    {
        RepositoryFile _repo;
        int _hitAt;
        private string _nameToCopyToClipboard;
        private string _pwd = String.Empty;
        private string _domainName;
        private string _userName;

        private const int MILLISECOND_SEARCH_DELAY = 400;
        private const int HIT_FOUND_MARKER = 1;
        private const int HIT_AT_MARKER = 2;
        
        Timer _timerGrep;
        private Scintilla _editor;
        private TextBox _srcFileFull;
        private readonly TabControl _tc = new TabControl();

        Collection<int> _hitLines;
        readonly FindReplace _find;

        private Collection<HistoryItem> _history = new Collection<HistoryItem>();
        private int _historyCurrent = -1;

        private const int TCM_HITTEST = 0x130D;
        private enum TCHITTESTFLAGS
        {
            TCHT_NOWHERE = 1,
            TCHT_ONITEMICON = 2,
            TCHT_ONITEMLABEL = 4,
            TCHT_ONITEM = TCHT_ONITEMICON | TCHT_ONITEMLABEL
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct TCHITTESTINFO
        {
            public Point pt;
            public TCHITTESTFLAGS flags;
            public TCHITTESTINFO(int x, int y)
            {
                pt = new Point(x, y);
                flags = TCHITTESTFLAGS.TCHT_ONITEM;
            }
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam, ref TCHITTESTINFO lParam);

        /// WM_VSCROLL -> 0x0115
        public const int WM_VSCROLL = 277;

        /// SB_BOTTOM -> 7
        public const int SB_BOTTOM = 7;

        public Main()
        {
            InitializeComponent();

            this.Text += " (Community Edition)";

            tsCurrentSolution.Text = String.Empty;

            _tc.ShowToolTips = true;
            _tc.SelectedIndexChanged += _tc_SelectedIndexChanged;
            _tc.Dock = DockStyle.Fill;
            _tc.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            _tc.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this._tc_DrawItem); ;
            _tc.MouseDown += _tc_MouseDown;
            _tc.SizeMode = TabSizeMode.Fixed;
            _tc.ContextMenuStrip = mnuContextTabCtl;

            _domainName = ConfigurationManager.AppSettings["DomainName"] ?? "ET";
            _userName = ConfigurationManager.AppSettings["UserName"] ?? Environment.UserName;
            picWaiting.Visible = false;
            
            lblRoot.Text = "Caching...";

            //Find dialog
            _find = new FindReplace();
            _find.KeyPressed += find_KeyPressed; ;

            CreateNewTab();

            splitContainer1.Panel2.Controls.Add(_tc);

            gridResults.RowEnter += gridResults_RowEnter;

            FillOperatorCombo(cboOperator);
            FillOperatorCombo(cboOperDLL);

            tsCurrentSolution.AutoToolTip = true;

            progressRefresh.Maximum = 100;
            progressRefresh.Minimum = 0;
            progressRefresh.Value = 0;
            btnDown.Enabled = false;
            btnUp.Enabled = false;
            lblHits.Text = String.Empty;

            lblHistoryPosition.Text = String.Empty;
            lblQueryTime.Text = String.Empty;
        }

        private void _tc_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var HTI = new TCHITTESTINFO(e.X, e.Y);
                var hotTab = _tc.TabPages[SendMessage(_tc.Handle, TCM_HITTEST, IntPtr.Zero, ref HTI)];
                _tc.SelectedIndex = hotTab.TabIndex;
            }

            for (var i = 0; i < _tc.TabPages.Count; i++)
            {
                var tabRect = this._tc.GetTabRect(i);
                tabRect.Inflate(-2, -2);
                var closeImage = new Bitmap(Resources._5657_close);
                var imageRect = new Rectangle(
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2,
                    closeImage.Width,
                    closeImage.Height);

                if (imageRect.Contains(e.Location) && _tc.TabPages.Count > 1)
                {
                    _tc.TabPages.RemoveAt(i);
                    break;
                }
            }
        }

        private void CreateNewTab()
        {
            var ec = new EditorControl();
            ec.Dock = DockStyle.Fill;

            _tc.TabPages.Add("");

            _tc.TabPages[_tc.TabCount-1].Controls.Add(ec);
            _editor = ec.GetEditorControl;
            _srcFileFull = ec.GetTextBox;

            _editor.Dock = DockStyle.Fill;
            _editor.Margins[0].Width = 0;
            _editor.Margins[1].Width = 16;
            _editor.Insert += _editor_Insert;
            _editor.KeyDown += genericScintilla_KeyDown;
            _find.Scintilla = _editor;
            SetLexer();

            _tc.SelectTab(_tc.TabCount - 1);
            txtSourceFile.Text = String.Empty;
            txtGrep.Text = String.Empty;
            txtDLL.Text = String.Empty;

            SetTabTooltip(false);
        }

        void SetTabTooltip(bool FromDoSearch)
        {
            var currentPage = _tc.TabPages[_tc.SelectedIndex];

            currentPage.ToolTipText = !String.IsNullOrWhiteSpace(txtSourceFile.Text) ? "File: " + txtSourceFile.Text + "\r\n" : String.Empty;
            currentPage.ToolTipText += !String.IsNullOrWhiteSpace(txtGrep.Text) ? "Search: " + txtGrep.Text + "\r\n" : String.Empty;
            currentPage.ToolTipText += !String.IsNullOrWhiteSpace(txtDLL.Text) ? "DLL: " + txtDLL.Text : String.Empty;

            var t = new TabTag { SourceFileName = txtSourceFile.Text, CodeGrep = txtGrep.Text, DLLFileName = txtDLL.Text };

            var setSelectedRow = true;
            if (currentPage.Tag != null && (currentPage.Tag as TabTag) != null && _tc.TabPages.Count > 1)
            {
                var tg = currentPage.Tag as TabTag;
                var fromTabSwitch = tg.FromTabSwitch;
                setSelectedRow = !fromTabSwitch;
                t.FromTabSwitch = fromTabSwitch;
                t.RowSelected = tg.RowSelected;
            }

            if (setSelectedRow)
            {
                if (gridResults.SelectedRows.Count > 0)
                {
                    t.RowSelected = gridResults.SelectedRows[0].Index;
                }
                else
                {
                    t.RowSelected = 0;
                }
            }
            else
            {
                if (gridResults.Rows.Count > 0 && gridResults.Rows.Count - 1 >= t.RowSelected)
                {
                    gridResults.Rows[t.RowSelected].Selected = true;

                }
            }

            if (FromDoSearch)
            {
                t.FromTabSwitch = false;
            }

#if DEBUG
            currentPage.ToolTipText += "SELECTED ROW: " + t.RowSelected.ToString();
#endif
            currentPage.Tag = t;
        }

        private void _tc_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tabPage = this._tc.TabPages[e.Index];
            var tabRect = this._tc.GetTabRect(e.Index);

            var col = e.Index == _tc.SelectedIndex ? Color.Yellow : Color.LightGray;   
            e.Graphics.FillRectangle(new SolidBrush(col), e.Bounds);
            var paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -2);
            //e.Graphics.DrawString(tabPage.Text, this.Font, SystemBrushes.HighlightText, paddedBounds);
            TextRenderer.DrawText(e.Graphics, tabPage.Text, Font, paddedBounds, tabPage.ForeColor);

            if (_tc.TabPages.Count > 1)
            {
                var closeImage = new Bitmap(Resources._5657_close);
                e.Graphics.DrawImage(closeImage,
                    (tabRect.Right - closeImage.Width),
                    tabRect.Top + (tabRect.Height - closeImage.Height) / 2);
            }
        }

        private void _tc_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentPage = _tc.TabPages[_tc.SelectedIndex];
            _editor = currentPage.Controls[0].Controls.OfType<Scintilla>().First();
            _find.Scintilla = _editor;
            _srcFileFull = currentPage.Controls[0].Controls.OfType<TextBox>().First();
            if (currentPage.Tag != null && (currentPage.Tag as TabTag) != null)
            {
                (currentPage.Tag as TabTag).FromTabSwitch = true;
            }
            SetSearchFromTab(currentPage);
        }

        void SetSearchFromTab(TabPage page)
        {
            if (page.Tag == null || (page.Tag as TabTag) == null)
            {
                txtSourceFile.Text = String.Empty;
                txtGrep.Text = String.Empty;
                txtDLL.Text = String.Empty;
                return;
            }

            var t = page.Tag as TabTag;
            txtSourceFile.Text = t.SourceFileName;
            txtGrep.Text = t.CodeGrep;
            txtDLL.Text = t.DLLFileName;
        }

        void SetSelectedRowIndexFromTab()
        {
            var page = _tc.TabPages[_tc.SelectedIndex];
            if (page.Tag == null || (page.Tag as TabTag) == null)
            {
                return;
            }

            var t = page.Tag as TabTag;
            if (gridResults.Rows.Count > 0 && gridResults.Rows.Count - 1 >= t.RowSelected)
            {
                gridResults.Rows[t.RowSelected].Selected = true;
                gridResults.FirstDisplayedScrollingRowIndex = t.RowSelected;
            }
        }

        void find_KeyPressed(object sender, KeyEventArgs e)
        {
            genericScintilla_KeyDown(sender, e);
        }

        /// <summary>
        /// Key down event for each Scintilla. Tie each Scintilla to this event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void genericScintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                _find.ShowFind();
                e.SuppressKeyPress = true;
            }
            else if (e.Shift && e.KeyCode == Keys.F3)
            {
                _find.Window.FindPrevious();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F3)
            {
                _find.Window.FindNext();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                _find.ShowIncrementalSearch();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                GoTo MyGoTo = new GoTo((Scintilla)sender);
                MyGoTo.ShowGoToDialog();
                e.SuppressKeyPress = true;
            }
        }
        
        private void FillOperatorCombo(ComboBox ctl)
        {
            ctl.Items.Add(new Operator { oper = Enums.Opers.operContains, DisplayText = "Contains" });
            ctl.Items.Add(new Operator { oper = Enums.Opers.operEquals, DisplayText = "Equals" });
            ctl.Items.Add(new Operator { oper = Enums.Opers.operStartWith, DisplayText = "Starts With" });
            ctl.Items.Add(new Operator { oper = Enums.Opers.opersEndsWith, DisplayText = "Ends With" });
            ctl.DisplayMember = "DisplayText";
            ctl.SelectedIndex = 0;

            ctl.SelectedIndexChanged += new System.EventHandler(cboOperator_SelectedIndexChanged);
        }

        void _editor_Insert(object sender, ModificationEventArgs e)
        {
            var maxLineNumberCharLength = _editor.Lines.Count.ToString().Length;
            _editor.Margins[0].Width = _editor.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + 2;
            _editor.Lines[e.Position].MarginText = _editor.LineFromPosition(e.Position).ToString();
        }

        private void SetToolStrip(string text, Color foreColor)
        {
            tsCurrentSolution.Text = text;
            tsCurrentSolution.ForeColor = foreColor;
            statusStrip1.Update();
        }
        void gridResults_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (gridResults.SelectedRows.Count == 0) return;

                var src = _repo.GetSourceData(gridResults.SelectedRows[0].Cells[0].Value.ToString());

                _editor.ReadOnly = false;
                _editor.Text = src.FullSourceText;

                SetToolStrip(src.SolutionPathAndFileName, Color.Black);

                _srcFileFull.Text = src.SourcePathAndFileName;

                if (!String.IsNullOrWhiteSpace(txtGrep.Text))
                {
                    HighlightWord(txtGrep.Text);
                    btnDown.Enabled = true;
                    btnUp.Enabled = true;

                }
                else
                {
                    lblHitAt.Text = String.Empty;
                    lblHits.Text = String.Empty;
                    btnDown.Enabled = false;
                    btnUp.Enabled = false;
                }

                _editor.ReadOnly = true;

                SetTabTooltip(false);

                btnDown_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void SetLexer()
        {
            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            _editor.StyleResetDefault();
            _editor.Styles[Style.Default].Font = "Consolas";
            _editor.Styles[Style.Default].Size = 10;
            _editor.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            _editor.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            _editor.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            _editor.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            _editor.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            _editor.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            _editor.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            _editor.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            _editor.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            _editor.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            _editor.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            _editor.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            _editor.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            _editor.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            _editor.Lexer = Lexer.Cpp;

            // Set the keywords
            _editor.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            _editor.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");

            //For marking selected text
            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
            const int NUM = 8;

            // Remove all uses of our indicator
            _editor.IndicatorCurrent = NUM;
            _editor.IndicatorClearRange(0, _editor.TextLength);

            // Update indicator appearance
            _editor.Indicators[NUM].Style = IndicatorStyle.StraightBox;
            _editor.Indicators[NUM].Under = true;
            _editor.Indicators[NUM].ForeColor = Color.Yellow;
            _editor.Indicators[NUM].OutlineAlpha = 50;
            _editor.Indicators[NUM].Alpha = 200; // 30;
        }

        private void HighlightWord(string text)
        {
            // Search the document
            _editor.TargetStart = 0;
            _editor.TargetEnd = _editor.TextLength;
            _editor.SearchFlags = SearchFlags.None;
            
            var StartPos = 0;
            var marker = _editor.Markers[HIT_FOUND_MARKER];
            marker.Symbol = MarkerSymbol.Arrow;
            marker.SetBackColor(Color.DeepSkyBlue);
            marker.SetForeColor(Color.Black);

            var markerAt = _editor.Markers[HIT_AT_MARKER];
            markerAt.Symbol = MarkerSymbol.Circle;
            markerAt.SetBackColor(Color.Red);
            markerAt.SetForeColor(Color.Black);

            _hitLines = new Collection<int>();
            while (_editor.SearchInTarget(text) != -1)
            {
                // Mark the search results with the current indicator
                _editor.IndicatorFillRange(_editor.TargetStart, _editor.TargetEnd - _editor.TargetStart);
                
                StartPos = _editor.TargetStart;

                // Search the remainder of the document
                _editor.TargetStart = _editor.TargetEnd;
                _editor.TargetEnd = _editor.TextLength;

                //Add bookmarker
                var line = _editor.Lines[_editor.LineFromPosition(StartPos)];
                _hitLines.Add(line.Index);
                line.MarkerAdd(HIT_FOUND_MARKER);
            }
            lblHits.Text = "Hits: " + _hitLines.Count.ToString();
        }

        void RefreshCount()
        {
            var count = _repo.GetSourceCount();
            lblCount.Text = count.ToString();
            if (count == 0)
            {
                btnRefresh_Click(null, null);
            }
        }

        private void ClearEditor()
        {
            _editor.ReadOnly = false;
            _editor.Text = String.Empty;
            _editor.ReadOnly = true;

            lblResultCount.Text = String.Empty;
            lblHits.Text = String.Empty;
            lblHitAt.Text = String.Empty;
            _editor.Margins[0].Width = 0;
            tsCurrentSolution.Text = String.Empty;
            _srcFileFull.Text = String.Empty;
            btnDown.Enabled = false;
            btnUp.Enabled = false;
            lblQueryTime.Text = String.Empty;
        }

        void _timerGrep_Tick(object sender, EventArgs e)
        {
            _timerGrep.Stop();
            progressRefresh.Value = 0;
            if (string.IsNullOrWhiteSpace(txtSourceFile.Text) && string.IsNullOrWhiteSpace(txtGrep.Text) && string.IsNullOrWhiteSpace(txtDLL.Text))
            {
                gridResults.DataSource = null;
                ClearEditor();

                return;
            }

            DoSearch();
        }

        private void ManageControls(bool IsBusy)
        {
            btnRefresh.Enabled = !IsBusy;
            txtSourceFile.Enabled = !IsBusy;
            txtGrep.Enabled = !IsBusy;
            txtDLL.Enabled = !IsBusy;
            picWaiting.Visible = IsBusy;
            cboOperator.Enabled = !IsBusy;
            cboOperDLL.Enabled = !IsBusy;
            toolStripButtonRoots.Enabled = !IsBusy;
            toolStripButtonOptions.Enabled = !IsBusy;

            if (IsBusy)
            {
                lblCount.Text = "0";
                gridResults.DataSource = null;
                ClearEditor();
            }
            else
            {
                tsCurrentSolution.Text = String.Empty;
            }
            //tabs
            foreach (var t in splitContainer1.Panel2.Controls.OfType<TabControl>())
            {
                t.Enabled = !IsBusy;
            }

            mnuContextTabCtl.Enabled = !IsBusy;
        }

        private void DoSearch()
        {
            lblQueryTime.Text = "(...)";
            DataSet ds = null;

            var sourceFile = txtSourceFile.Text;
            var grep = txtGrep.Text;
            var dll = txtDLL.Text;

            var o = new[] { (cboOperator.SelectedItem as Operator).oper, (cboOperDLL.SelectedItem as Operator).oper };
            var perf = new Stopwatch();
            var t = Task.Run(() =>
            {
                perf.Start();
                ds = _repo.DoSearch(sourceFile, grep.Replace("'", "''"), dll, o); // single quotes won't work in the CHARS_TO_ESCAPE constant since Sqlite requires an escape character specified which is \
                perf.Stop();
                return ds;
            });
            t.ContinueWith(s =>
            {
                gridResults.DataSource = ds.Tables[0];
                gridResults.Columns[0].Visible = false;
                var c = ds.Tables[0].Rows.Count;
                lblResultCount.Text = c.ToString();
                if (c == 0)
                {
                    ClearEditor();
                }
                lblQueryTime.Text = $"({perf.Elapsed.Milliseconds.ToString()}ms)";
                SetSelectedRowIndexFromTab();
                SetTabTooltip(true);

                if (!_history.Any(h => h.FileValue.Equals(sourceFile) && h.GrepyValue.Equals(grep) && h.DLLValue.Equals(dll)))
                {
                    _historyCurrent++;
                    lblHistoryPosition.Text = (_historyCurrent + 1).ToString();

                    _history.Add(new HistoryItem
                    {
                        FileValue = sourceFile,
                        GrepyValue = grep,
                        DLLValue = dll,
                        Position = _historyCurrent,
                        Timestamp = DateTime.Now,
                        RestultCount = lblResultCount.Text.SafeToInt32()
                    });
                }
            }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

            t.ContinueWith(s1 => { MessageBox.Show("Exception:" + s1.Exception.Message); }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void SearchBoxTextChanged(object sender, EventArgs e)
        {
            _timerGrep.Stop();
            if (string.IsNullOrWhiteSpace(txtSourceFile.Text) && string.IsNullOrWhiteSpace(txtGrep.Text) && string.IsNullOrWhiteSpace(txtDLL.Text))
            {
                gridResults.DataSource = null;
                ClearEditor();
            }
            else
            {
                _timerGrep.Start();
            }
        }

        private void SearchBoxTextChangedKey(object sender, KeyEventArgs e)
        {
            SearchBoxTextChanged(sender, e);
        }

        private void cboOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchBoxTextChanged(null, null);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                var conf = new Confirm(Constants.CONFIRM_RECRAWL, "Are you sure?");
                var dr = DialogResult.OK;

                if (RepositoryUtils.GetOptionValue(Constants.CONFIRM_RECRAWL) == "1" && _repo.GetSourceCount() > 0)
                {
                    dr = conf.ShowDialog();
                }

                if (dr == DialogResult.OK)
                {
                    RemoveAllButMainTab();
                    txtSourceFile.Text = String.Empty;
                    txtGrep.Text = String.Empty;
                    txtDLL.Text = String.Empty;

                    if (_timerGrep != null) _timerGrep_Tick(null, null);

                    ManageControls(true);
                    lblCount.Text = "0";
                    lblResultCount.Text = "0";
                    lblHitAt.Text = String.Empty;

                    var progressHandler = new Progress<ProgressResult>(value =>
                    {
                        var percent = (int) (((double) progressRefresh.Value / (double) progressRefresh.Maximum) * 100);
                        progressRefresh.ProgressBar.Refresh();
                        progressRefresh.ProgressBar.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float) 8.25, FontStyle.Regular), Brushes.Black, new PointF(progressRefresh.Width / 2 - 10, progressRefresh.Height / 2 - 7));
                        progressRefresh.Value = value.ProgressValue;
                        tsCurrentSolution.Text = value.WorkingOn;
                    });
                    var progress = progressHandler as IProgress<ProgressResult>;
                    
                    var sw = Stopwatch.StartNew();
                    var t = Task.Run(() => { _repo.CrawlSource(progress); });

                    t.ContinueWith(r =>
                    {
                        lblCount.Text = _repo.GetSourceCount().ToString();
                        ManageControls(false);
                        progressRefresh.Value = 0;

                        sw.Stop();
                        _timerGrep = new Timer { Interval = MILLISECOND_SEARCH_DELAY };
                        _timerGrep.Tick += _timerGrep_Tick;
                    }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

                    t.ContinueWith(r =>
                    {
                        sw.Stop();
                        ManageControls(false);
                    }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
           GetRoot();
           cboOperator.SelectedIndexChanged += SearchBoxTextChanged;
        }

        private void GetRoot()
        {
            ManageControls(true);
            
            cboOperator.SelectedIndex = 0;
            txtSourceFile.Text = String.Empty;
            txtGrep.Text = String.Empty;
            lblHitAt.Text = String.Empty;

            var defaultRoot = RepositoryUtils.GetDefaultRootId();
            if (defaultRoot != null)
            {
                var progressHandler = new Progress<ProgressResult>(value =>
                {
                    var percent = (int)(((double)progressRefresh.Value / (double)progressRefresh.Maximum) * 100);
                    progressRefresh.ProgressBar.Refresh();
                    progressRefresh.ProgressBar.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.Black, new PointF(progressRefresh.Width / 2 - 10, progressRefresh.Height / 2 - 7));
                    progressRefresh.Value = value.ProgressValue;
                    tsCurrentSolution.Text = value.WorkingOn;
                });
                var progress = progressHandler as IProgress<ProgressResult>;

                var t = Task.Run(() => _repo = new RepositoryFile(defaultRoot[0], progress));
                t.ContinueWith(r =>
                {
                    MessageBox.Show($"Error retrieving root {defaultRoot[1]}. Please remove and re-add it.");
                    ManageControls(false);
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, TaskScheduler.FromCurrentSynchronizationContext());

                t.ContinueWith(r =>
                {
                    lblRoot.Text = _repo.Root.RootPath;
                    ManageControls(false);
                    RefreshCount();
                    lblResultCount.Text = "0";
                    _timerGrep = new Timer { Interval = MILLISECOND_SEARCH_DELAY };
                    _timerGrep.Tick += _timerGrep_Tick;
                }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext())
                .ContinueWith(r1 => Task.Run(() => System.Threading.Thread.Sleep(3000))
                    .ContinueWith(r2 =>
                    {
                        progressRefresh.Value = 0;
                    }, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext()),
                    CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                btnRefresh.Enabled = false;
                picWaiting.Visible = false;
                toolStripButtonRoots.Enabled = true;
                toolStripButtonOptions.Enabled = true;
                lblRoot.Text = "No default source root found. Please add at least one.";
            } 
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) //Explore Here
        {
            if (gridResults.SelectedRows.Count == 0)
            {
                return;
            }

            var data = GetPathAndFile();
            try
            {
                var proc = new Process();
                proc.StartInfo.FileName = "C:\\Windows\\SysWOW64\\explorer.exe";
                proc.StartInfo.Arguments = "/select, " + data.Item1;
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void openCMDtoolStripMenuItem_Click(object sender, EventArgs e) //Command prompt here
        {
            var data = GetPathAndFile();

            try
            {
                var cmdProc = new ProcessStartInfo("cmd")
                {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    WorkingDirectory = data.Item2
                };

                var proc = new Process { StartInfo = cmdProc };

                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception! Path = " + data.Item2 + "\r\n" + ex.Message);
            }
        }

        private void openDeveloperCommandPromptHereToolStripMenuItem_Click(object sender, EventArgs e) // VS2017 command prompt here
        {
            var data = GetPathAndFile();

            try
            {
                var cmdProc = new ProcessStartInfo("cmd")
                {
                    Arguments = @"/T:17 /k ""C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsDevCmd.bat""",
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    WorkingDirectory = data.Item2
                };
                var proc = new Process { StartInfo = cmdProc };
                proc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception! Path = " + data.Item2 + "\r\n" + ex.Message);
            }
        }

        private void toolStripButtonRoots_Click(object sender, EventArgs e)
        {
            try
            {
                if (_timerGrep != null)
                {
                    _timerGrep.Stop();
                }

                var f = new ManageRoots(_repo);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    GetRoot();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception!\r\n" + ex.Message);
            }
        }

        private void gridResults_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                gridResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                gridResults_RowEnter(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception!\r\n" + ex.Message);
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            try
            {
                var curLine = _editor.Lines[_editor.CurrentLine];
                var line = _editor.LineFromPosition(_editor.CurrentPosition);
                var nextLine = _editor.Lines[++line].MarkerNext(1 << HIT_FOUND_MARKER);
                if (nextLine != -1)
                {
                    curLine.MarkerDelete(HIT_AT_MARKER);
                    _editor.Lines[nextLine].Goto();
                    _editor.Lines[_editor.CurrentLine].MarkerAdd(HIT_AT_MARKER);
                    lblHitAt.Text = (_hitLines.IndexOf(_editor.CurrentLine) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception!\r\n" + ex.Message);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                var curLine = _editor.Lines[_editor.CurrentLine];
                var line = _editor.LineFromPosition(_editor.CurrentPosition);
                var prevLine = _editor.Lines[--line].MarkerPrevious(1 << HIT_FOUND_MARKER);
                if (prevLine != -1)
                {
                    curLine.MarkerDelete(HIT_AT_MARKER);
                    _editor.Lines[prevLine].Goto();
                    _editor.Lines[_editor.CurrentLine].MarkerAdd(HIT_AT_MARKER);

                    lblHitAt.Text = (_hitLines.IndexOf(_editor.CurrentLine) + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception!\r\n" + ex.Message);
            }
        }

        private void OpenSolution()
        {
            if (gridResults.Rows.Count == 0)
            {
                return;
            }

            var file = String.Empty;
            var errors = new Collection<string>();
            var vsLoc = RepositoryUtils.GetOptionValue(Constants.VS_LOCATION);
            var exec = String.IsNullOrWhiteSpace(vsLoc) ? Constants.DEFAULT_VS_LOCATION : vsLoc;

            try
            {
                var sol = _repo.GetSourceData(gridResults.SelectedRows[0].Cells[0].Value.ToString());
                file = sol.SolutionPathAndFileName;
                if (!File.Exists(sol.SolutionPathAndFileName))
                {
                    errors.Add("File doesn't exist: " + sol.SolutionPathAndFileName);
                }
                else if (!File.Exists(exec))
                {
                    errors.Add($"Visual Studio executable file doesn't exist: {exec}. Please change it in the Options.");
                }
                else
                {
                    System.Diagnostics.Process.Start(exec, file);
                }

            }
            catch (Exception ex)
            {
                errors.Add("File = " + file + "\r\n" + ex.Message);
            }
        
            if (errors.Any())
            {
                var er = String.Join("\r\n", errors);
                MessageBox.Show("Exception!\r\n" + er);
            }
        }

        private void gridResults_DoubleClick(object sender, EventArgs e)
        {
            OpenSolution();
        }

        private void openSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSolution();
        }

        private void gridResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && gridResults.Rows.Count > 0)
            {
                int rowSelected = e.RowIndex;
                if (e.RowIndex != -1)
                {
                    gridResults.Rows[rowSelected].Selected = true;
                    _hitAt = e.ColumnIndex;
                }
            }
        }

        private void toolStripButtonOptions_Click(object sender, EventArgs e)
        {
            var opt = new Options();
            opt.ShowDialog();
        }

        /// <summary>
        /// Gets File and Path based on mouse location
        /// </summary>
        /// <returns>Tuple: File, Directory</returns>
        private Tuple<string, string> GetPathAndFile()
        {
            if (gridResults.SelectedRows.Count == 0)
            {
                //no rows selected
                return new Tuple<string, string>(string.Empty, string.Empty);
            }

            var data = _repo.GetSourceData(gridResults.SelectedRows[0].Cells[0].Value.ToString());
            var FullFile = String.Empty;
            var Directory = String.Empty;

            if (_hitAt == 1 || _hitAt == 4) //Solution
            {
                FullFile = data.SolutionPathAndFileName;
                Directory = data.PathToSolution;
            }
            else if (_hitAt == 2) //Project
            {
                FullFile = data.ProjectPathAndFileName;
                Directory = data.PathToProject;
            }
            else if (_hitAt == 3) // Source
            {
                FullFile = data.SourcePathAndFileName;
                Directory = data.PathToSource;
            }
            else
            {
                FullFile = FullFile = data.SolutionPathAndFileName;
            }

            return new Tuple<string, string>(FullFile, Directory);
        }

        private void mnuContext_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = gridResults.Rows.Count == 0;
        }

        private void gridResults_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            try
            {
                var FullFile = String.Empty;
                var data = _repo.GetSourceData(gridResults.Rows[e.RowIndex].Cells[0].Value.ToString());
                if (e.ColumnIndex == 1 || e.ColumnIndex == 4) //Solution
                {
                    FullFile = data.SolutionPathAndFileName;
                }
                else if (e.ColumnIndex == 2) //Project
                {
                    FullFile = data.ProjectPathAndFileName;
                }
                else if (e.ColumnIndex == 3) // Source
                {
                    FullFile = data.SourcePathAndFileName;
                }
                else
                {
                    FullFile = FullFile = data.SolutionPathAndFileName;
                }

                gridResults.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = FullFile;
                _nameToCopyToClipboard = FullFile;
            }
            catch (Exception ex)
            {
                //eat this
            }
        }

        private void newResultsTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTab();
        }

        private void addTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTab();
        }

        void RemoveAllButMainTab()
        {
            while (_tc.TabPages.Count > 1)
            {
                removeTabToolStripMenuItem_Click(null, null);
            }

            _tc.TabPages[0].ToolTipText = String.Empty;
        }

        private void removeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _tc.TabPages.RemoveAt(_tc.SelectedIndex);
        }

        private void mnuContextTabCtl_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            removeTabToolStripMenuItem.Enabled = _tc.TabPages.Count > 1;
        }

        private void lblHistoryPosition_MouseHover(object sender, EventArgs e)
        {
            var tt = new ToolTip();
            tt.SetToolTip(lblHistoryPosition, "History:" + Environment.NewLine +
                String.Join(Environment.NewLine, _history.OrderByDescending(h => h.Timestamp).Select(hv => hv)));
        }

        private void lblHistoryPosition_Click(object sender, EventArgs e)
        {
            if (_history.Any())
            {
                var hist = new Form();
                hist.Height = 150;
                hist.FormBorderStyle = FormBorderStyle.None;
                var ctl = new HistoryPopup(_history);
                ctl.Dock = DockStyle.Fill;
                hist.Controls.Add(ctl);
                hist.StartPosition = FormStartPosition.Manual;
                hist.Left = System.Windows.Forms.Cursor.Position.X + 8;
                hist.Top = System.Windows.Forms.Cursor.Position.Y + 8;
                hist.Deactivate += Hist_Deactivate;
                hist.Show();
            }
        }

        private void Hist_Deactivate(object sender, EventArgs e)
        {
            var frm = sender as Form;
            var ctl = frm.Controls[0] as HistoryPopup;
            if (ctl.SelectedHistoryItem != null)
            {
                txtSourceFile.Text = ctl.SelectedHistoryItem.FileValue;
                txtGrep.Text = ctl.SelectedHistoryItem.GrepyValue;
                txtDLL.Text = ctl.SelectedHistoryItem.DLLValue;

                _history.FirstOrDefault(h => h.Equals(ctl.SelectedHistoryItem)).Timestamp = DateTime.Now;
            }

            frm.Hide();
            frm.Deactivate -= Hist_Deactivate;
        }

        private void copyNameToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(_nameToCopyToClipboard);

        }
    }
}

