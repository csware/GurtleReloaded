namespace Gurtle
{
    using System.Windows.Forms;

    partial class WebProxyDialog
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
            System.Windows.Forms.Button btnCancel;
            System.Windows.Forms.Button btnOK;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label5;
            this._testButton = new System.Windows.Forms.Button();
            this._noExpectContinueBox = new System.Windows.Forms.CheckBox();
            this._autoBox = new System.Windows.Forms.RadioButton();
            this._manualBox = new System.Windows.Forms.RadioButton();
            this._passwordBox = new System.Windows.Forms.TextBox();
            this._addressBox = new System.Windows.Forms.TextBox();
            this._domainBox = new System.Windows.Forms.TextBox();
            this._userNameBox = new System.Windows.Forms.TextBox();
            this._portBox = new System.Windows.Forms.TextBox();
            btnCancel = new System.Windows.Forms.Button();
            btnOK = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(327, 285);
            btnCancel.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(90, 30);
            btnCancel.TabIndex = 15;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnOK.Location = new System.Drawing.Point(231, 285);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(90, 30);
            btnOK.TabIndex = 14;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(31, 191);
            label4.Margin = new System.Windows.Forms.Padding(21, 0, 3, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(71, 17);
            label4.TabIndex = 10;
            label4.Text = "Pa&ssword:";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(31, 71);
            label1.Margin = new System.Windows.Forms.Padding(21, 0, 3, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(102, 17);
            label1.TabIndex = 2;
            label1.Text = "&Proxy Address:";
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(31, 131);
            label2.Margin = new System.Windows.Forms.Padding(21, 0, 3, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 17);
            label2.TabIndex = 6;
            label2.Text = "&Domain:";
            label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(31, 161);
            label3.Margin = new System.Windows.Forms.Padding(21, 0, 3, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(79, 17);
            label3.TabIndex = 8;
            label3.Text = "&User Name:";
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(31, 101);
            label5.Margin = new System.Windows.Forms.Padding(21, 0, 3, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(80, 17);
            label5.TabIndex = 4;
            label5.Text = "P&roxy Port:";
            label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _testButton
            // 
            this._testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._testButton.Location = new System.Drawing.Point(13, 285);
            this._testButton.Name = "_testButton";
            this._testButton.Size = new System.Drawing.Size(90, 30);
            this._testButton.TabIndex = 13;
            this._testButton.Text = "&Test";
            this._testButton.UseVisualStyleBackColor = true;
            this._testButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // _noExpectContinueBox
            // 
            this._noExpectContinueBox.AutoSize = true;
            this._noExpectContinueBox.Location = new System.Drawing.Point(146, 223);
            this._noExpectContinueBox.Margin = new System.Windows.Forms.Padding(3, 8, 3, 1);
            this._noExpectContinueBox.Name = "_noExpectContinueBox";
            this._noExpectContinueBox.Size = new System.Drawing.Size(207, 21);
            this._noExpectContinueBox.TabIndex = 12;
            this._noExpectContinueBox.Text = "Do not expect 100-Continue";
            this._noExpectContinueBox.UseVisualStyleBackColor = true;
            // 
            // _autoBox
            // 
            this._autoBox.AutoSize = true;
            this._autoBox.Checked = true;
            this._autoBox.Location = new System.Drawing.Point(13, 14);
            this._autoBox.Name = "_autoBox";
            this._autoBox.Size = new System.Drawing.Size(245, 21);
            this._autoBox.TabIndex = 0;
            this._autoBox.TabStop = true;
            this._autoBox.Text = "&Automatically detect proxy settings";
            this._autoBox.UseVisualStyleBackColor = true;
            // 
            // _manualBox
            // 
            this._manualBox.AutoSize = true;
            this._manualBox.Location = new System.Drawing.Point(13, 41);
            this._manualBox.Name = "_manualBox";
            this._manualBox.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this._manualBox.Size = new System.Drawing.Size(204, 27);
            this._manualBox.TabIndex = 1;
            this._manualBox.Text = "&Use following proxy settings:";
            this._manualBox.UseVisualStyleBackColor = true;
            // 
            // _passwordBox
            // 
            this._passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._passwordBox.Location = new System.Drawing.Point(146, 188);
            this._passwordBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this._passwordBox.Name = "_passwordBox";
            this._passwordBox.PasswordChar = '*';
            this._passwordBox.Size = new System.Drawing.Size(272, 24);
            this._passwordBox.TabIndex = 11;
            // 
            // _addressBox
            // 
            this._addressBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._addressBox.Location = new System.Drawing.Point(146, 68);
            this._addressBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this._addressBox.Name = "_addressBox";
            this._addressBox.Size = new System.Drawing.Size(272, 24);
            this._addressBox.TabIndex = 3;
            // 
            // _domainBox
            // 
            this._domainBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._domainBox.Location = new System.Drawing.Point(146, 128);
            this._domainBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this._domainBox.Name = "_domainBox";
            this._domainBox.Size = new System.Drawing.Size(272, 24);
            this._domainBox.TabIndex = 7;
            // 
            // _userNameBox
            // 
            this._userNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._userNameBox.Location = new System.Drawing.Point(145, 158);
            this._userNameBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this._userNameBox.Name = "_userNameBox";
            this._userNameBox.Size = new System.Drawing.Size(273, 24);
            this._userNameBox.TabIndex = 9;
            // 
            // _portBox
            // 
            this._portBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._portBox.Location = new System.Drawing.Point(146, 98);
            this._portBox.Name = "_portBox";
            this._portBox.Size = new System.Drawing.Size(63, 24);
            this._portBox.TabIndex = 5;
            // 
            // WebProxyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 326);
            this.Controls.Add(this._noExpectContinueBox);
            this.Controls.Add(this._autoBox);
            this.Controls.Add(this._testButton);
            this.Controls.Add(btnCancel);
            this.Controls.Add(this._manualBox);
            this.Controls.Add(btnOK);
            this.Controls.Add(label4);
            this.Controls.Add(this._passwordBox);
            this.Controls.Add(label1);
            this.Controls.Add(label2);
            this.Controls.Add(label3);
            this.Controls.Add(this._addressBox);
            this.Controls.Add(this._domainBox);
            this.Controls.Add(this._userNameBox);
            this.Controls.Add(label5);
            this.Controls.Add(this._portBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WebProxyDialog";
            this.Padding = new System.Windows.Forms.Padding(10, 12, 10, 8);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Web Proxy";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioButton _autoBox;
        private RadioButton _manualBox;
        private TextBox _passwordBox;
        private TextBox _addressBox;
        private TextBox _domainBox;
        private TextBox _userNameBox;
        private TextBox _portBox;
        private CheckBox _noExpectContinueBox;
        private Button _testButton;

    }
}