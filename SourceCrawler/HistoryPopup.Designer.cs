/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
 */

namespace SourceCrawler
{
    partial class HistoryPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbxHistory = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbxHistory
            // 
            this.lbxHistory.FormattingEnabled = true;
            this.lbxHistory.Location = new System.Drawing.Point(34, 26);
            this.lbxHistory.Name = "lbxHistory";
            this.lbxHistory.Size = new System.Drawing.Size(120, 95);
            this.lbxHistory.TabIndex = 0;
            // 
            // HistoryPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 258);
            this.ControlBox = false;
            this.Controls.Add(this.lbxHistory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "HistoryPopup";
            this.Text = "HistoryPopup";
            this.Load += new System.EventHandler(this.HistoryPopup_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbxHistory;
    }
}
