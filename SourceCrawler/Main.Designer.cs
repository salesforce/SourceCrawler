/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

namespace SourceCrawler
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressRefresh = new System.Windows.Forms.ToolStripProgressBar();
            this.lblQueryTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsCurrentSolution = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.txtGrep = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridResults = new System.Windows.Forms.DataGridView();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newResultsTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openExploreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCMDtoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDeveloperCommandPromptHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyNameToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRoots = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOptions = new System.Windows.Forms.ToolStripButton();
            this.label4 = new System.Windows.Forms.Label();
            this.lblRoot = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboOperator = new System.Windows.Forms.ComboBox();
            this.lblHits = new System.Windows.Forms.Label();
            this.lblHitAt = new System.Windows.Forms.Label();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.picWaiting = new System.Windows.Forms.PictureBox();
            this.txtDLL = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboOperDLL = new System.Windows.Forms.ComboBox();
            this.mnuContextTabCtl = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblHistoryPosition = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
            this.mnuContext.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).BeginInit();
            this.mnuContextTabCtl.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSourceFile
            // 
            this.txtSourceFile.Location = new System.Drawing.Point(20, 72);
            this.txtSourceFile.Margin = new System.Windows.Forms.Padding(2);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.Size = new System.Drawing.Size(256, 20);
            this.txtSourceFile.TabIndex = 0;
            this.txtSourceFile.TextChanged += new System.EventHandler(this.SearchBoxTextChanged);
            this.txtSourceFile.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchBoxTextChangedKey);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Source File match:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressRefresh,
            this.lblQueryTime,
            this.tsCurrentSolution});
            this.statusStrip1.Location = new System.Drawing.Point(0, 890);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1596, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressRefresh
            // 
            this.progressRefresh.Name = "progressRefresh";
            this.progressRefresh.Size = new System.Drawing.Size(75, 16);
            // 
            // lblQueryTime
            // 
            this.lblQueryTime.Name = "lblQueryTime";
            this.lblQueryTime.Size = new System.Drawing.Size(61, 17);
            this.lblQueryTime.Text = "querytime";
            // 
            // tsCurrentSolution
            // 
            this.tsCurrentSolution.Name = "tsCurrentSolution";
            this.tsCurrentSolution.Size = new System.Drawing.Size(59, 17);
            this.tsCurrentSolution.Text = "initial text";
            this.tsCurrentSolution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(445, 33);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(86, 23);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Recrawl";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(847, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Source files crawled:";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.Location = new System.Drawing.Point(966, 39);
            this.lblCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(14, 13);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "0";
            // 
            // lblResultCount
            // 
            this.lblResultCount.AutoSize = true;
            this.lblResultCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultCount.ForeColor = System.Drawing.Color.Green;
            this.lblResultCount.Location = new System.Drawing.Point(966, 75);
            this.lblResultCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(14, 13);
            this.lblResultCount.TabIndex = 8;
            this.lblResultCount.Text = "0";
            // 
            // txtGrep
            // 
            this.txtGrep.Location = new System.Drawing.Point(284, 72);
            this.txtGrep.Margin = new System.Windows.Forms.Padding(2);
            this.txtGrep.Name = "txtGrep";
            this.txtGrep.Size = new System.Drawing.Size(256, 20);
            this.txtGrep.TabIndex = 1;
            this.txtGrep.TextChanged += new System.EventHandler(this.SearchBoxTextChanged);
            this.txtGrep.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchBoxTextChangedKey);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(18, 101);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridResults);
            this.splitContainer1.Size = new System.Drawing.Size(1569, 783);
            this.splitContainer1.SplitterDistance = 736;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 11;
            // 
            // gridResults
            // 
            this.gridResults.AllowDrop = true;
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.AllowUserToDeleteRows = false;
            this.gridResults.AllowUserToOrderColumns = true;
            this.gridResults.AllowUserToResizeRows = false;
            this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridResults.ContextMenuStrip = this.mnuContext;
            this.gridResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridResults.Location = new System.Drawing.Point(0, 0);
            this.gridResults.Margin = new System.Windows.Forms.Padding(2);
            this.gridResults.MultiSelect = false;
            this.gridResults.Name = "gridResults";
            this.gridResults.RowHeadersVisible = false;
            this.gridResults.RowTemplate.ContextMenuStrip = this.mnuContext;
            this.gridResults.RowTemplate.Height = 24;
            this.gridResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridResults.Size = new System.Drawing.Size(736, 783);
            this.gridResults.TabIndex = 0;
            this.gridResults.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridResults_CellMouseDown);
            this.gridResults.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridResults_CellMouseEnter);
            this.gridResults.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.gridResults_DataBindingComplete);
            this.gridResults.DoubleClick += new System.EventHandler(this.gridResults_DoubleClick);
            // 
            // mnuContext
            // 
            this.mnuContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newResultsTabToolStripMenuItem,
            this.openSolutionToolStripMenuItem,
            this.openExploreToolStripMenuItem,
            this.openCMDtoolStripMenuItem,
            this.openDeveloperCommandPromptHereToolStripMenuItem,
            this.copyNameToClipboardToolStripMenuItem});
            this.mnuContext.Name = "mnuContext";
            this.mnuContext.Size = new System.Drawing.Size(271, 136);
            this.mnuContext.Opening += new System.ComponentModel.CancelEventHandler(this.mnuContext_Opening);
            // 
            // newResultsTabToolStripMenuItem
            // 
            this.newResultsTabToolStripMenuItem.Name = "newResultsTabToolStripMenuItem";
            this.newResultsTabToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.newResultsTabToolStripMenuItem.Text = "New Results Tab";
            this.newResultsTabToolStripMenuItem.Click += new System.EventHandler(this.newResultsTabToolStripMenuItem_Click);
            // 
            // openSolutionToolStripMenuItem
            // 
            this.openSolutionToolStripMenuItem.Name = "openSolutionToolStripMenuItem";
            this.openSolutionToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.openSolutionToolStripMenuItem.Text = "Open Solution";
            this.openSolutionToolStripMenuItem.Click += new System.EventHandler(this.openSolutionToolStripMenuItem_Click);
            // 
            // openExploreToolStripMenuItem
            // 
            this.openExploreToolStripMenuItem.Name = "openExploreToolStripMenuItem";
            this.openExploreToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.openExploreToolStripMenuItem.Text = "Explore here";
            this.openExploreToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // openCMDtoolStripMenuItem
            // 
            this.openCMDtoolStripMenuItem.Name = "openCMDtoolStripMenuItem";
            this.openCMDtoolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.openCMDtoolStripMenuItem.Text = "Open command prompt here";
            this.openCMDtoolStripMenuItem.Click += new System.EventHandler(this.openCMDtoolStripMenuItem_Click);
            // 
            // openDeveloperCommandPromptHereToolStripMenuItem
            // 
            this.openDeveloperCommandPromptHereToolStripMenuItem.Name = "openDeveloperCommandPromptHereToolStripMenuItem";
            this.openDeveloperCommandPromptHereToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.openDeveloperCommandPromptHereToolStripMenuItem.Text = "Open VS2017 command prompt here";
            this.openDeveloperCommandPromptHereToolStripMenuItem.Click += new System.EventHandler(this.openDeveloperCommandPromptHereToolStripMenuItem_Click);
            // 
            // copyNameToClipboardToolStripMenuItem
            // 
            this.copyNameToClipboardToolStripMenuItem.Name = "copyNameToClipboardToolStripMenuItem";
            this.copyNameToClipboardToolStripMenuItem.Size = new System.Drawing.Size(270, 22);
            this.copyNameToClipboardToolStripMenuItem.Text = "Copy name to clipboard";
            this.copyNameToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyNameToClipboardToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(284, 53);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Code search:";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRoots,
            this.toolStripButtonOptions});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1596, 27);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonRoots
            // 
            this.toolStripButtonRoots.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRoots.Image")));
            this.toolStripButtonRoots.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRoots.Name = "toolStripButtonRoots";
            this.toolStripButtonRoots.Size = new System.Drawing.Size(130, 24);
            this.toolStripButtonRoots.Text = "Root Management";
            this.toolStripButtonRoots.Click += new System.EventHandler(this.toolStripButtonRoots_Click);
            // 
            // toolStripButtonOptions
            // 
            this.toolStripButtonOptions.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOptions.Image")));
            this.toolStripButtonOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOptions.Name = "toolStripButtonOptions";
            this.toolStripButtonOptions.Size = new System.Drawing.Size(82, 24);
            this.toolStripButtonOptions.Text = "Options...";
            this.toolStripButtonOptions.Click += new System.EventHandler(this.toolStripButtonOptions_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 28);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Current Root:";
            // 
            // lblRoot
            // 
            this.lblRoot.AutoSize = true;
            this.lblRoot.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoot.Location = new System.Drawing.Point(104, 28);
            this.lblRoot.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRoot.Name = "lblRoot";
            this.lblRoot.Size = new System.Drawing.Size(29, 13);
            this.lblRoot.TabIndex = 15;
            this.lblRoot.Text = "root";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(882, 77);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Result count:";
            // 
            // cboOperator
            // 
            this.cboOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOperator.FormattingEnabled = true;
            this.cboOperator.Location = new System.Drawing.Point(118, 50);
            this.cboOperator.Margin = new System.Windows.Forms.Padding(2);
            this.cboOperator.Name = "cboOperator";
            this.cboOperator.Size = new System.Drawing.Size(111, 21);
            this.cboOperator.TabIndex = 18;
            // 
            // lblHits
            // 
            this.lblHits.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHits.AutoSize = true;
            this.lblHits.Location = new System.Drawing.Point(1370, 76);
            this.lblHits.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHits.Name = "lblHits";
            this.lblHits.Size = new System.Drawing.Size(25, 13);
            this.lblHits.TabIndex = 20;
            this.lblHits.Text = "Hits";
            // 
            // lblHitAt
            // 
            this.lblHitAt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHitAt.AutoSize = true;
            this.lblHitAt.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblHitAt.Location = new System.Drawing.Point(1526, 71);
            this.lblHitAt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHitAt.Name = "lblHitAt";
            this.lblHitAt.Size = new System.Drawing.Size(15, 13);
            this.lblHitAt.TabIndex = 21;
            this.lblHitAt.Text = "H";
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Image = global::SourceCrawler.Properties.Resources.Sort_Up_48;
            this.btnUp.Location = new System.Drawing.Point(1550, 58);
            this.btnUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(37, 37);
            this.btnUp.TabIndex = 19;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Image = global::SourceCrawler.Properties.Resources.Sort_Down_48;
            this.btnDown.Location = new System.Drawing.Point(1485, 58);
            this.btnDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(37, 37);
            this.btnDown.TabIndex = 17;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // picWaiting
            // 
            this.picWaiting.Image = global::SourceCrawler.Properties.Resources.Animation;
            this.picWaiting.Location = new System.Drawing.Point(1012, 30);
            this.picWaiting.Margin = new System.Windows.Forms.Padding(2);
            this.picWaiting.Name = "picWaiting";
            this.picWaiting.Size = new System.Drawing.Size(39, 40);
            this.picWaiting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picWaiting.TabIndex = 9;
            this.picWaiting.TabStop = false;
            // 
            // txtDLL
            // 
            this.txtDLL.Location = new System.Drawing.Point(568, 72);
            this.txtDLL.Margin = new System.Windows.Forms.Padding(2);
            this.txtDLL.Name = "txtDLL";
            this.txtDLL.Size = new System.Drawing.Size(244, 20);
            this.txtDLL.TabIndex = 2;
            this.txtDLL.TextChanged += new System.EventHandler(this.SearchBoxTextChanged);
            this.txtDLL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SearchBoxTextChangedKey);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(568, 57);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "DLL/EXE search:";
            // 
            // cboOperDLL
            // 
            this.cboOperDLL.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOperDLL.FormattingEnabled = true;
            this.cboOperDLL.Location = new System.Drawing.Point(662, 50);
            this.cboOperDLL.Margin = new System.Windows.Forms.Padding(2);
            this.cboOperDLL.Name = "cboOperDLL";
            this.cboOperDLL.Size = new System.Drawing.Size(111, 21);
            this.cboOperDLL.TabIndex = 24;
            // 
            // mnuContextTabCtl
            // 
            this.mnuContextTabCtl.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTabToolStripMenuItem,
            this.removeTabToolStripMenuItem});
            this.mnuContextTabCtl.Name = "mnuContextTabCtl";
            this.mnuContextTabCtl.Size = new System.Drawing.Size(139, 48);
            this.mnuContextTabCtl.Opening += new System.ComponentModel.CancelEventHandler(this.mnuContextTabCtl_Opening);
            // 
            // addTabToolStripMenuItem
            // 
            this.addTabToolStripMenuItem.Name = "addTabToolStripMenuItem";
            this.addTabToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.addTabToolStripMenuItem.Text = "Add Tab";
            this.addTabToolStripMenuItem.Click += new System.EventHandler(this.addTabToolStripMenuItem_Click);
            // 
            // removeTabToolStripMenuItem
            // 
            this.removeTabToolStripMenuItem.Name = "removeTabToolStripMenuItem";
            this.removeTabToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.removeTabToolStripMenuItem.Text = "Remove Tab";
            this.removeTabToolStripMenuItem.Click += new System.EventHandler(this.removeTabToolStripMenuItem_Click);
            // 
            // lblHistoryPosition
            // 
            this.lblHistoryPosition.AutoSize = true;
            this.lblHistoryPosition.Location = new System.Drawing.Point(546, 76);
            this.lblHistoryPosition.Name = "lblHistoryPosition";
            this.lblHistoryPosition.Size = new System.Drawing.Size(23, 13);
            this.lblHistoryPosition.TabIndex = 25;
            this.lblHistoryPosition.Text = "hist";
            this.lblHistoryPosition.Click += new System.EventHandler(this.lblHistoryPosition_Click);
            this.lblHistoryPosition.MouseHover += new System.EventHandler(this.lblHistoryPosition_MouseHover);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1596, 912);
            this.Controls.Add(this.lblHistoryPosition);
            this.Controls.Add(this.cboOperDLL);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDLL);
            this.Controls.Add(this.lblHitAt);
            this.Controls.Add(this.lblHits);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.cboOperator);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblRoot);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.txtGrep);
            this.Controls.Add(this.picWaiting);
            this.Controls.Add(this.lblResultCount);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSourceFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = ".Net Source File Crawler (2020.02)";
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
            this.mnuContext.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaiting)).EndInit();
            this.mnuContextTabCtl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressRefresh;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Label lblResultCount;
        private System.Windows.Forms.PictureBox picWaiting;
        private System.Windows.Forms.TextBox txtGrep;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridResults;
        private System.Windows.Forms.ContextMenuStrip mnuContext;
        private System.Windows.Forms.ToolStripMenuItem openExploreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCMDtoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSolutionToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblRoot;
        private System.Windows.Forms.ToolStripButton toolStripButtonRoots;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.ComboBox cboOperator;
        private System.Windows.Forms.ToolStripStatusLabel tsCurrentSolution;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Label lblHits;
        private System.Windows.Forms.Label lblHitAt;
        private System.Windows.Forms.ToolStripButton toolStripButtonOptions;
        private System.Windows.Forms.ToolStripMenuItem openDeveloperCommandPromptHereToolStripMenuItem;
        private System.Windows.Forms.TextBox txtDLL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboOperDLL;
        private System.Windows.Forms.ToolStripMenuItem newResultsTabToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip mnuContextTabCtl;
        private System.Windows.Forms.ToolStripMenuItem addTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTabToolStripMenuItem;
        private System.Windows.Forms.Label lblHistoryPosition;
        private System.Windows.Forms.ToolStripStatusLabel lblQueryTime;
        private System.Windows.Forms.ToolStripMenuItem copyNameToClipboardToolStripMenuItem;
    }
}

