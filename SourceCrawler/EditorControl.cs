using System.Windows.Forms;
using ScintillaNET;

namespace SourceCrawler
{
    public partial class EditorControl : UserControl
    {
        private readonly Scintilla _editor = new Scintilla();

        public EditorControl()
        {
            InitializeComponent();
            _editor.Dock = DockStyle.Fill;
            _editor.Name = "ScintillaEditor";
            this.Controls.Add(_editor);
        }
    }
}
