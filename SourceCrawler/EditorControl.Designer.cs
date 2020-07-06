namespace SourceCrawler
{
    partial class EditorControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSrcFileFull = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSrcFileFull);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(875, 24);
            this.panel1.TabIndex = 4;
            // 
            // txtSrcFileFull
            // 
            this.txtSrcFileFull.Location = new System.Drawing.Point(53, 0);
            this.txtSrcFileFull.Margin = new System.Windows.Forms.Padding(2);
            this.txtSrcFileFull.Name = "txtSrcFileFull";
            this.txtSrcFileFull.ReadOnly = true;
            this.txtSrcFileFull.Size = new System.Drawing.Size(514, 20);
            this.txtSrcFileFull.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.scintilla1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(875, 485);
            this.panel2.TabIndex = 5;
            // 
            // scintilla1
            // 
            this.scintilla1.Location = new System.Drawing.Point(50, 45);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(337, 224);
            this.scintilla1.TabIndex = 4;
            this.scintilla1.Text = "scintilla1";
            // 
            // EditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "EditorControl";
            this.Size = new System.Drawing.Size(875, 509);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtSrcFileFull;
        private System.Windows.Forms.Panel panel2;
        private ScintillaNET.Scintilla scintilla1;
    }
}
