/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
 */

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
