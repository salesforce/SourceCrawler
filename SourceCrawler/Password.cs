using System;
using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class Password : Form
    {
        private string _pwd;

        public string PWD
        {
            get { return _pwd;}
            set { value = _pwd; }
        }

        public Password()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _pwd = txtPassword.Text;
        }
    }
}
