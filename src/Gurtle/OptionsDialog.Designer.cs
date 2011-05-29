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
            this._commitTemplate = new System.Windows.Forms.ComboBox();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._providers = new System.Windows.Forms.ComboBox();
            this._configureProviderButton = new System.Windows.Forms.Button();
            this._handleOnCommitFinished = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            _toolTip = new System.Windows.Forms.ToolTip(this.components);
            _resetButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(9, 9);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(83, 13);
            label1.TabIndex = 0;
            label1.Text = "&Select provider:";
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
            // _commitTemplate
            // 
            this._commitTemplate.FormattingEnabled = true;
            this._commitTemplate.Items.AddRange(new object[] {
            "(%TYPETEXT1% %TYPE% #%BUGID%: %SUMMARY%)",
            "%TYPETEXT2% %TYPE% #%BUGID%: %SUMMARY%",
            "Fixed issue #%BUGID%: %SUMMARY%"});
            this._commitTemplate.Location = new System.Drawing.Point(12, 100);
            this._commitTemplate.Name = "_commitTemplate";
            this._commitTemplate.Size = new System.Drawing.Size(416, 21);
            this._commitTemplate.TabIndex = 12;
            _toolTip.SetToolTip(this._commitTemplate, "Commit text template. Use %BUGID% for one bug/ticket id, %SUMMARY, %TYPE% bug/tic" +
                    "ket type; %TYPETEXT1% for Fixes/Resolves and %TYPETEXT2% for Rixed/Resolved base" +
                    "d on bug/ticket type");
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
            // _providers
            // 
            this._providers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._providers.FormattingEnabled = true;
            this._providers.Location = new System.Drawing.Point(12, 26);
            this._providers.Name = "_providers";
            this._providers.Size = new System.Drawing.Size(277, 21);
            this._providers.TabIndex = 7;
            this._providers.SelectedIndexChanged += new System.EventHandler(this._providers_SelectedIndexChanged);
            // 
            // _configureProviderButton
            // 
            this._configureProviderButton.Enabled = false;
            this._configureProviderButton.Location = new System.Drawing.Point(295, 26);
            this._configureProviderButton.Name = "_configureProviderButton";
            this._configureProviderButton.Size = new System.Drawing.Size(133, 21);
            this._configureProviderButton.TabIndex = 8;
            this._configureProviderButton.Text = "Configure provider";
            this._configureProviderButton.UseVisualStyleBackColor = true;
            this._configureProviderButton.Click += new System.EventHandler(this._configureProviderButton_Click);
            // 
            // _handleOnCommitFinished
            // 
            this._handleOnCommitFinished.AutoSize = true;
            this._handleOnCommitFinished.Location = new System.Drawing.Point(12, 54);
            this._handleOnCommitFinished.Name = "_handleOnCommitFinished";
            this._handleOnCommitFinished.Size = new System.Drawing.Size(244, 17);
            this._handleOnCommitFinished.TabIndex = 9;
            this._handleOnCommitFinished.Text = "Handle OnCommitFinished issue/ticket update";
            this._handleOnCommitFinished.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Commit text template:";
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(440, 217);
            this.Controls.Add(this._commitTemplate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._handleOnCommitFinished);
            this.Controls.Add(this._configureProviderButton);
            this.Controls.Add(this._providers);
            this.Controls.Add(_resetButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okButton);
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

        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.ComboBox _providers;
        private System.Windows.Forms.Button _configureProviderButton;
        private System.Windows.Forms.CheckBox _handleOnCommitFinished;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox _commitTemplate;
    }
}