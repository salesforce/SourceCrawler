using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class RecrawlProgressCommandLine : Form
    {
        public RecrawlProgressCommandLine()
        {
            InitializeComponent();
        }

        public void SetProgress(int progressValue, string workingOn)
        {
            progressBar1.Value = progressValue > 100 ? 100 : progressValue < 0 ? 0 : progressValue;
            lblWorkingOn.Text = workingOn;
        }

        public void KillSelf()
        {
            this.Close();
        }

        public int GetCurrentProgressValue()
        {
            return progressBar1.Value;
        }

        public int GetMaxProgressValue()
        {
            return progressBar1.Maximum;
        }
    }
}
