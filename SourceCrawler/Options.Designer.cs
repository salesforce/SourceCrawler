namespace SourceCrawler
{
    partial class Options
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.chkRecrawlConfirm = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtVSLocation = new System.Windows.Forms.TextBox();
            this.btnVSLocation = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(160, 91);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 26);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(75, 91);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 26);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // chkRecrawlConfirm
            // 
            this.chkRecrawlConfirm.AutoSize = true;
            this.chkRecrawlConfirm.Location = new System.Drawing.Point(19, 25);
            this.chkRecrawlConfirm.Margin = new System.Windows.Forms.Padding(2);
            this.chkRecrawlConfirm.Name = "chkRecrawlConfirm";
            this.chkRecrawlConfirm.Size = new System.Drawing.Size(104, 17);
            this.chkRecrawlConfirm.TabIndex = 4;
            this.chkRecrawlConfirm.Text = "Confirm recrawl?";
            this.chkRecrawlConfirm.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Vusial Studio Location";
            // 
            // txtVSLocation
            // 
            this.txtVSLocation.Location = new System.Drawing.Point(138, 53);
            this.txtVSLocation.Name = "txtVSLocation";
            this.txtVSLocation.Size = new System.Drawing.Size(466, 20);
            this.txtVSLocation.TabIndex = 6;
            // 
            // btnVSLocation
            // 
            this.btnVSLocation.Location = new System.Drawing.Point(611, 51);
            this.btnVSLocation.Name = "btnVSLocation";
            this.btnVSLocation.Size = new System.Drawing.Size(25, 23);
            this.btnVSLocation.TabIndex = 7;
            this.btnVSLocation.Text = "...";
            this.btnVSLocation.UseVisualStyleBackColor = true;
            this.btnVSLocation.Click += new System.EventHandler(this.btnVSLocation_Click);
            // 
            // Options
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(648, 139);
            this.ControlBox = false;
            this.Controls.Add(this.btnVSLocation);
            this.Controls.Add(this.txtVSLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkRecrawlConfirm);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Options_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chkRecrawlConfirm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVSLocation;
        private System.Windows.Forms.Button btnVSLocation;
    }
}