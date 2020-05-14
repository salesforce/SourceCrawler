/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class HistoryPopup : UserControl
    {
        public HistoryItem SelectedHistoryItem { get; private set; }

        public HistoryPopup()
        {
            InitializeComponent();
        }

        public HistoryPopup(IEnumerable<HistoryItem> historyList)
        {
            InitializeComponent();
            lbxHistory.DataSource = new List<HistoryItem>(historyList.OrderByDescending(h => h.Timestamp));
        }

        private void lbxHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectedHistoryItem = lbxHistory.SelectedItem as HistoryItem;
            }

            if (e.KeyCode == Keys.Escape)
            {
                SelectedHistoryItem = null;
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                var mi = Parent.GetType().GetMethod("OnDeactivate", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(Parent, new object[] { e });
            }
        }

        private void lbxHistory_DoubleClick(object sender, EventArgs e)
        {
            SelectedHistoryItem = lbxHistory.SelectedItem as HistoryItem;
            lbxHistory_KeyDown(null, new KeyEventArgs(Keys.Enter));
        }
    }
}
