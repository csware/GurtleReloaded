namespace Gurtle
{
    partial class CredentialsDialog
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Button cancelButton;
            this.leadLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.userNameBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.saveBox = new System.Windows.Forms.CheckBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 44);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(78, 17);
            label2.TabIndex = 1;
            label2.Text = "&User name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 74);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(71, 17);
            label3.TabIndex = 3;
            label3.Text = "&Password:";
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(252, 142);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(86, 32);
            cancelButton.TabIndex = 7;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // leadLabel
            // 
            this.leadLabel.AutoSize = true;
            this.leadLabel.Location = new System.Drawing.Point(12, 9);
            this.leadLabel.Name = "leadLabel";
            this.leadLabel.Size = new System.Drawing.Size(173, 17);
            this.leadLabel.TabIndex = 0;
            this.leadLabel.Text = "Enter your {0} credentials:";
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Enabled = false;
            this.okButton.Location = new System.Drawing.Point(160, 142);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(86, 32);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // userNameBox
            // 
            this.userNameBox.Location = new System.Drawing.Point(96, 44);
            this.userNameBox.MaxLength = 100;
            this.userNameBox.Name = "userNameBox";
            this.userNameBox.Size = new System.Drawing.Size(242, 24);
            this.userNameBox.TabIndex = 2;
            this.userNameBox.TextChanged += new System.EventHandler(this.UserName_Changed);
            // 
            // passwordBox
            // 
            this.passwordBox.Location = new System.Drawing.Point(96, 74);
            this.passwordBox.MaxLength = 100;
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.PasswordChar = '*';
            this.passwordBox.Size = new System.Drawing.Size(242, 24);
            this.passwordBox.TabIndex = 4;
            this.passwordBox.TextChanged += new System.EventHandler(this.Password_Changed);
            // 
            // saveBox
            // 
            this.saveBox.AutoSize = true;
            this.saveBox.Location = new System.Drawing.Point(96, 104);
            this.saveBox.Name = "saveBox";
            this.saveBox.Size = new System.Drawing.Size(183, 21);
            this.saveBox.TabIndex = 5;
            this.saveBox.Text = "&Remember my password";
            this.saveBox.UseVisualStyleBackColor = true;
            // 
            // CredentialsDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cancelButton;
            this.ClientSize = new System.Drawing.Size(347, 186);
            this.Controls.Add(this.okButton);
            this.Controls.Add(cancelButton);
            this.Controls.Add(this.saveBox);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(label3);
            this.Controls.Add(this.userNameBox);
            this.Controls.Add(label2);
            this.Controls.Add(this.leadLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CredentialsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to {0}";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox userNameBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.CheckBox saveBox;
        private System.Windows.Forms.Label leadLabel;
        private System.Windows.Forms.Button okButton;
    }
}