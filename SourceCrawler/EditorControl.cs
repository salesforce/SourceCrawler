/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
 */

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
