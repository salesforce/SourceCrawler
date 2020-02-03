/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

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
