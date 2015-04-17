namespace Gurtle.Providers.GitLab
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this._projectNameBox = new System.Windows.Forms.TextBox();
            this._testButton = new System.Windows.Forms.Button();
            this._okButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._gitlabInstanceUrlBox = new System.Windows.Forms.TextBox();
            this._privateTokenBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            _toolTip = new System.Windows.Forms.ToolTip(this.components);
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 97);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(219, 13);
            label1.TabIndex = 0;
            label1.Text = "&Repository Name (Namespace/Projectname)";
            // 
            // _projectNameBox
            // 
            this._projectNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._projectNameBox.Location = new System.Drawing.Point(12, 117);
            this._projectNameBox.Name = "_projectNameBox";
            this._projectNameBox.Size = new System.Drawing.Size(416, 21);
            this._projectNameBox.TabIndex = 1;
            _toolTip.SetToolTip(this._projectNameBox, "Enter GitLab repository name");
            this._projectNameBox.TextChanged += new System.EventHandler(this.ProjectNameBox_TextChanged);
            // 
            // _testButton
            // 
            this._testButton.Enabled = false;
            this._testButton.Location = new System.Drawing.Point(12, 144);
            this._testButton.Name = "_testButton";
            this._testButton.Size = new System.Drawing.Size(75, 30);
            this._testButton.TabIndex = 5;
            this._testButton.Text = "&Test";
            _toolTip.SetToolTip(this._testButton, "Test if the supplied GitLab repository is reachable online or not");
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
            // _gitlabInstanceUrlBox
            // 
            this._gitlabInstanceUrlBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._gitlabInstanceUrlBox.Location = new System.Drawing.Point(12, 29);
            this._gitlabInstanceUrlBox.Name = "_gitlabInstanceUrlBox";
            this._gitlabInstanceUrlBox.Size = new System.Drawing.Size(416, 21);
            this._gitlabInstanceUrlBox.TabIndex = 7;
            this._gitlabInstanceUrlBox.Text = "https://gitlab.com/";
            _toolTip.SetToolTip(this._gitlabInstanceUrlBox, "Enter GitLab repository name");
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(12, 9);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(115, 13);
            label2.TabIndex = 6;
            label2.Text = "&URL to GitLab instance";
            // 
            // _privateTokenBox
            // 
            this._privateTokenBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._privateTokenBox.Location = new System.Drawing.Point(12, 73);
            this._privateTokenBox.Name = "_privateTokenBox";
            this._privateTokenBox.Size = new System.Drawing.Size(416, 21);
            this._privateTokenBox.TabIndex = 9;
            _toolTip.SetToolTip(this._privateTokenBox, "Enter GitLab repository name");
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(12, 53);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(71, 13);
            label3.TabIndex = 8;
            label3.Text = "&Private token";
            // 
            // OptionsDialog
            // 
            this.AcceptButton = this._okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(440, 217);
            this.Controls.Add(this._privateTokenBox);
            this.Controls.Add(label3);
            this.Controls.Add(this._gitlabInstanceUrlBox);
            this.Controls.Add(label2);
            this.Controls.Add(this._testButton);
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
            this.Text = "GitLab Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _projectNameBox;
        private System.Windows.Forms.Button _okButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _testButton;
        private System.Windows.Forms.TextBox _gitlabInstanceUrlBox;
        private System.Windows.Forms.TextBox _privateTokenBox;
    }
}