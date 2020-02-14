/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class HistoryPopup : UserControl
    {
        public string SelectedText { get; private set; }

        public HistoryPopup()
        {
            InitializeComponent();
        }

        public HistoryPopup(IEnumerable<string> historyList)
        {
            InitializeComponent();
            lbxHistory.DataSource = new List<string>(historyList);
        }

        private void lbxHistory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectedText = lbxHistory.SelectedItem.ToString();
            }

            if (e.KeyCode == Keys.Escape)
            {
                SelectedText = String.Empty;
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape)
            {
                var mi = Parent.GetType().GetMethod("OnDeactivate", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(Parent, new object[] { e });
            }
        }

        private void lbxHistory_DoubleClick(object sender, EventArgs e)
        {
            SelectedText = lbxHistory.SelectedItem.ToString();
            lbxHistory_KeyDown(null, new KeyEventArgs(Keys.Enter));
        }
    }
}
