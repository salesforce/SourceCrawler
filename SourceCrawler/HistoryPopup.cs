using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class HistoryPopup : Form
    {
        private readonly IEnumerable<string> _listValues = new Collection<string>();

        public HistoryPopup()
        {
            InitializeComponent();
        }

        public HistoryPopup(IEnumerable<string> historyList) : base()
        {
            InitializeComponent();
            _listValues = historyList;
        }

        private void HistoryPopup_Load(object sender, EventArgs e)
        {
            lbxHistory.DataSource = new List<string>(_listValues);
        }
    }
}
