using System;
using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class Confirm : Form
    {
        public bool ShowConfirm { get; set; }
        private string _optionKey;

        public Confirm(string optionKey, string LabelText)
        {
            InitializeComponent();
            _optionKey = optionKey;
            lblConfirmText.Text = LabelText;
            chkShowConfirm.Checked = RepositoryUtils.GetOptionValue(_optionKey) == "1" ? true : false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            RepositoryUtils.UpdateOptionValue(_optionKey, chkShowConfirm.Checked ? "1" : "0");
            DialogResult = DialogResult.OK;
        }
    }
}
