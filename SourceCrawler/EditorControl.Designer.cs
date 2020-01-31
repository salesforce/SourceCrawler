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
            this.txtSrcFileFull = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtSrcFileFull
            // 
            this.txtSrcFileFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSrcFileFull.Location = new System.Drawing.Point(0, 0);
            this.txtSrcFileFull.Margin = new System.Windows.Forms.Padding(2);
            this.txtSrcFileFull.Name = "txtSrcFileFull";
            this.txtSrcFileFull.ReadOnly = true;
            this.txtSrcFileFull.Size = new System.Drawing.Size(875, 20);
            this.txtSrcFileFull.TabIndex = 1;
            // 
            // EditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtSrcFileFull);
            this.Name = "EditorControl";
            this.Size = new System.Drawing.Size(875, 509);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSrcFileFull;
    }
}
