/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.IO;
using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class Options : Form
    {
        bool _OkToClose = true;
        public Options()
        {
            InitializeComponent();

            chkRecrawlConfirm.Checked = RepositoryUtils.GetOptionValue(Constants.CONFIRM_RECRAWL) == "1" ? true : false;
            var vsLoc = RepositoryUtils.GetOptionValue(Constants.VS_LOCATION);
            txtVSLocation.Text = String.IsNullOrWhiteSpace(vsLoc) ? Constants.DEFAULT_VS_LOCATION : vsLoc;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _OkToClose = File.Exists(txtVSLocation.Text);
            if (_OkToClose)
            {
                RepositoryUtils.UpdateOptionValue(Constants.CONFIRM_RECRAWL, chkRecrawlConfirm.Checked ? "1" : "0");
                RepositoryUtils.UpdateOptionValue(Constants.VS_LOCATION, txtVSLocation.Text);
            }
        }

        private void btnVSLocation_Click(object sender, EventArgs e)
        {
            var vsLoc = RepositoryUtils.GetOptionValue(Constants.VS_LOCATION);
            var exec = String.IsNullOrWhiteSpace(vsLoc) ? Constants.DEFAULT_VS_LOCATION : vsLoc;

            var initDir = File.Exists(exec) ? Path.GetDirectoryName(exec) : String.Empty;
            var f = new OpenFileDialog { FileName = Path.GetFileName(exec), InitialDirectory = initDir };
            if (f.ShowDialog() == DialogResult.OK)
            {
                txtVSLocation.Text = f.FileName;
            }
        }

        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_OkToClose)
            {
                MessageBox.Show($"File {txtVSLocation.Text} doesn't exist.");
                e.Cancel = true;
                _OkToClose = true;
            }
        }
    }
}
