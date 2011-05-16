namespace Gurtle
{
    internal partial class IssueUpdateInfoDialog
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
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueUpdateInfoDialog));
            System.Windows.Forms.CheckBox checkBox1;
            this.linkLabel = new System.Windows.Forms.LinkLabel();
            this.okButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // linkLabel
            // 
            this.linkLabel.AutoSize = true;
            this.linkLabel.Location = new System.Drawing.Point(10, 130);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(199, 17);
            this.linkLabel.TabIndex = 2;
            this.linkLabel.TabStop = true;
            this.linkLabel.Tag = "http://code.google.com/p/gurtle/wiki/IssueUpdateSetup";
            this.linkLabel.Text = "How do I enable issue updates?";
            this.linkLabel.UseMnemonic = false;
            this.linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_Clicked);
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoEllipsis = true;
            label1.Location = new System.Drawing.Point(10, 37);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(468, 92);
            label1.TabIndex = 3;
            label1.Text = resources.GetString("label1.Text");
            label1.UseMnemonic = false;
            // 
            // checkBox1
            // 
            checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            checkBox1.AutoSize = true;
            checkBox1.Checked = global::Gurtle.Properties.Settings.Default.HideIssueUpdateTip;
            checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Gurtle.Properties.Settings.Default, "HideIssueUpdateTip", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            checkBox1.Location = new System.Drawing.Point(14, 181);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new System.Drawing.Size(161, 21);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "Don\'t show this again";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(394, 175);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(86, 30);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Close";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Did You Know?";
            this.label2.UseMnemonic = false;
            // 
            // IssueUpdateInfoDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 215);
            this.Controls.Add(this.label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.linkLabel);
            this.Controls.Add(checkBox1);
            this.Controls.Add(this.okButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IssueUpdateInfoDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gurtle Tip";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel;
    }
}