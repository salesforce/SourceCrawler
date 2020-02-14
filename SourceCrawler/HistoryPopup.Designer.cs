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

        #region Component Designer generated code

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
            this.lbxHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxHistory.FormattingEnabled = true;
            this.lbxHistory.Location = new System.Drawing.Point(0, 0);
            this.lbxHistory.Name = "lbxHistory";
            this.lbxHistory.Size = new System.Drawing.Size(339, 182);
            this.lbxHistory.TabIndex = 0;
            this.lbxHistory.DoubleClick += new System.EventHandler(this.lbxHistory_DoubleClick);
            this.lbxHistory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lbxHistory_KeyDown);
            // 
            // HistoryPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbxHistory);
            this.Name = "HistoryPopup";
            this.Size = new System.Drawing.Size(339, 182);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbxHistory;
    }
}
