namespace Gurtle
{
    internal partial class IssueUpdatePage
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label1;
            System.Windows.Forms.ToolTip toolTip;
            this.statusBox = new System.Windows.Forms.ComboBox();
            this.commentBox = new System.Windows.Forms.TextBox();
            this.revisionsLabel = new System.Windows.Forms.LinkLabel();
            this.summaryLabel = new System.Windows.Forms.LinkLabel();
            this.skipBox = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(7, 37);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(52, 17);
            label1.TabIndex = 1;
            label1.Text = "Status:";
            // 
            // statusBox
            // 
            this.statusBox.FormattingEnabled = true;
            this.statusBox.Location = new System.Drawing.Point(65, 37);
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(151, 24);
            this.statusBox.TabIndex = 2;
            toolTip.SetToolTip(this.statusBox, "Issue resolution status");
            this.statusBox.TextChanged += new System.EventHandler(this.StatusBox_TextChanged);
            // 
            // commentBox
            // 
            this.commentBox.AcceptsReturn = true;
            this.commentBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.commentBox.Location = new System.Drawing.Point(10, 67);
            this.commentBox.Multiline = true;
            this.commentBox.Name = "commentBox";
            this.commentBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commentBox.Size = new System.Drawing.Size(575, 172);
            this.commentBox.TabIndex = 4;
            toolTip.SetToolTip(this.commentBox, "Closing comment for issue");
            this.commentBox.TextChanged += new System.EventHandler(this.CommentBox_TextChanged);
            // 
            // revisionsLabel
            // 
            this.revisionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.revisionsLabel.AutoEllipsis = true;
            this.revisionsLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.revisionsLabel.Location = new System.Drawing.Point(236, 37);
            this.revisionsLabel.Name = "revisionsLabel";
            this.revisionsLabel.Size = new System.Drawing.Size(349, 23);
            this.revisionsLabel.TabIndex = 3;
            this.revisionsLabel.Text = "Revisions: {0}";
            toolTip.SetToolTip(this.revisionsLabel, "Revision numbers mentioned in comment text");
            this.revisionsLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RevisionsLabel_LinkClicked);
            // 
            // summaryLabel
            // 
            this.summaryLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.summaryLabel.AutoEllipsis = true;
            this.summaryLabel.Location = new System.Drawing.Point(3, 8);
            this.summaryLabel.Name = "summaryLabel";
            this.summaryLabel.Size = new System.Drawing.Size(582, 17);
            this.summaryLabel.TabIndex = 0;
            this.summaryLabel.TabStop = true;
            this.summaryLabel.Text = "Summary";
            this.summaryLabel.UseMnemonic = false;
            this.summaryLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Summary_LinkClicked);
            // 
            // skipBox
            // 
            this.skipBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.skipBox.AutoSize = true;
            this.skipBox.Location = new System.Drawing.Point(10, 245);
            this.skipBox.Name = "skipBox";
            this.skipBox.Size = new System.Drawing.Size(179, 21);
            this.skipBox.TabIndex = 5;
            this.skipBox.Text = "S&kip updating this issue";
            this.skipBox.UseVisualStyleBackColor = true;
            this.skipBox.CheckedChanged += new System.EventHandler(this.SkipBox_CheckedChanged);
            // 
            // IssueUpdatePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.skipBox);
            this.Controls.Add(this.revisionsLabel);
            this.Controls.Add(label1);
            this.Controls.Add(this.summaryLabel);
            this.Controls.Add(this.commentBox);
            this.Controls.Add(this.statusBox);
            this.Name = "IssueUpdatePage";
            this.Size = new System.Drawing.Size(595, 273);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox statusBox;
        private System.Windows.Forms.TextBox commentBox;
        private System.Windows.Forms.LinkLabel summaryLabel;
        private System.Windows.Forms.LinkLabel revisionsLabel;
        private System.Windows.Forms.CheckBox skipBox;


    }
}
