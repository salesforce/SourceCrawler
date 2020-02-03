/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

namespace SourceCrawler
{
    partial class ManageRoots
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
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lbxRoots = new System.Windows.Forms.ListBox();
            this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNewRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRootToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mnuContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(118, 214);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 26);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(208, 214);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 26);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lbxRoots
            // 
            this.lbxRoots.FormattingEnabled = true;
            this.lbxRoots.Location = new System.Drawing.Point(9, 33);
            this.lbxRoots.Margin = new System.Windows.Forms.Padding(2);
            this.lbxRoots.Name = "lbxRoots";
            this.lbxRoots.Size = new System.Drawing.Size(381, 147);
            this.lbxRoots.TabIndex = 0;
            this.lbxRoots.DoubleClick += new System.EventHandler(this.lbxRoots_DoubleClick);
            this.lbxRoots.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbxRoots_MouseUp);
            // 
            // mnuContext
            // 
            this.mnuContext.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewRootToolStripMenuItem,
            this.deleteRootToolStripMenuItem});
            this.mnuContext.Name = "mnuContext";
            this.mnuContext.Size = new System.Drawing.Size(161, 48);
            // 
            // addNewRootToolStripMenuItem
            // 
            this.addNewRootToolStripMenuItem.Name = "addNewRootToolStripMenuItem";
            this.addNewRootToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.addNewRootToolStripMenuItem.Text = "Add New Root...";
            this.addNewRootToolStripMenuItem.Click += new System.EventHandler(this.addNewRootToolStripMenuItem_Click);
            // 
            // deleteRootToolStripMenuItem
            // 
            this.deleteRootToolStripMenuItem.Name = "deleteRootToolStripMenuItem";
            this.deleteRootToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.deleteRootToolStripMenuItem.Text = "Delete Root...";
            this.deleteRootToolStripMenuItem.Click += new System.EventHandler(this.deleteRootToolStripMenuItem_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(9, 185);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(21, 19);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(34, 185);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(21, 19);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Selected row is the default root.";
            // 
            // ManageRoots
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(398, 260);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbxRoots);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ManageRoots";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Roots";
            this.mnuContext.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListBox lbxRoots;
        private System.Windows.Forms.ContextMenuStrip mnuContext;
        private System.Windows.Forms.ToolStripMenuItem addNewRootToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRootToolStripMenuItem;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Label label1;
    }
}
