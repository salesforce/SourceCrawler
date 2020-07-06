using System.Windows.Forms;
using ScintillaNET;

namespace SourceCrawler
{
    public partial class EditorControl : UserControl
    {
        public EditorControl()
        {
            InitializeComponent();
            txtSrcFileFull.Dock = DockStyle.Fill;
            scintilla1.Dock = DockStyle.Fill;
        }

        public Scintilla GetEditorControl
        {
            get { return scintilla1; }
        }

        public TextBox GetTextBox
        {
            get { return txtSrcFileFull; }
        }
    }
}
