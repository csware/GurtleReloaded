namespace Gurtle
{
    public partial class OptionsDialog
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.ToolTip _toolTip;
            System.Windows.Forms.Button _resetButton;
            this._projectNameBox = new System.Windows.Forms.TextBox();
            this._linkLabel = new System.Windows.Forms.LinkLabel();
            this._testButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            _toolTip = new System.Windows.Forms.ToolTip(this.components);
            _resetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 11);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(178, 17);
            label1.TabIndex = 0;
            label1.Text = "&Google Code Project Name:";
            // 
            // _projectNameBox
            // 
            this._projectNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._projectNameBox.Location = new System.Drawing.Point(12, 31);
            this._projectNameBox.Name = "_projectNameBox";
            this._projectNameBox.Size = new System.Drawing.Size(416, 24);
            this._projectNameBox.TabIndex = 1;
            _toolTip.SetToolTip(this._projectNameBox, "Enter Google Code hosted project name");
            this._projectNameBox.TextChanged += new System.EventHandler(this.ProjectNameBox_TextChanged);
            // 
            // _linkLabel
            // 
            this._linkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._linkLabel.AutoEllipsis = true;
            this._linkLabel.Location = new System.Drawing.Point(12, 62);
            this._linkLabel.Name = "_linkLabel";
            this._linkLabel.Size = new System.Drawing.Size(416, 17);
            this._linkLabel.TabIndex = 2;
            this._linkLabel.TabStop = true;
            this._linkLabel.Text = "http://code.google.com/hosting/";
            _toolTip.SetToolTip(this._linkLabel, "Open URL in the default Web browser");
            this._linkLabel.UseMnemonic = false;
            this._linkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel_LinkClicked);
            // 
            // _testButton
            // 
            this._testButton.Enabled = false;
            this._testButton.Location = new System.Drawing.Point(12, 91);
            this._testButton.Name = "_testButton";
            this._testButton.Size = new System.Drawing.Size(75, 30);
            this._testButton.TabIndex = 5;
            this._testButton.Text = "&Test";
            _toolTip.SetToolTip(this._testButton, "Test if the supplied Google Code project is reachable online or not");
            this._testButton.UseVisualStyleBackColor = true;
            this._testButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // _okButton
            // 
            this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._okButton.Enabled = false;
            this._okButton.Location = new System.Drawing.Point(272, 175);
            this._okButton.Name = "_okButton";
            this._okButton.Size = new System.Drawing.Size(75, 30);
            this._okButton.TabIndex = 3;
            this._okButton.Text = "OK";
            this._okButton.UseVisualStyleBackColor = true;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(353, 175);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 30);
            this._cancelButton.TabIndex = 4;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _resetButton
            // 
            _resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            _resetButton.Location = new System.Drawing.Point(12, 175);
            _resetButton.Name = "_resetButton";
            _resetButton.Size = new System.Drawing.Size(128, 30);
            _resetButton.TabIndex = 6;
            _resetButton.Text = "Reset Settings";
            _toolTip.SetToolTip(_resetButton, "Resets all settings to the defaults");
            _resetButton.UseVisualStyleBackColor = true;
            _resetButton.Click += new System.EventHandler(this.ResetSettings_Click);
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(440, 217);
            this.Controls.Add(_resetButton);
            this.Controls.Add(this._testButton);
            this.Controls.Add(this._linkLabel);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
            this.Controls.Add(this._projectNameBox);
            this.Controls.Add(label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _projectNameBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.LinkLabel _linkLabel;
        private System.Windows.Forms.Button _testButton;
    }
}