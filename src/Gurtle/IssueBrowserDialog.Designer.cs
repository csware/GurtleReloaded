namespace Gurtle
{
    public partial class IssueBrowserDialog
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
            System.Windows.Forms.Button cancelButton;
            System.Windows.Forms.StatusStrip statusStrip;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueBrowserDialog));
            System.Windows.Forms.ToolTip toolTip;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.workStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.detailButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.issueListView = new System.Windows.Forms.ListView();
            this.idColumn = new System.Windows.Forms.ColumnHeader();
            this.typeColumn = new System.Windows.Forms.ColumnHeader();
            this.statusColumn = new System.Windows.Forms.ColumnHeader();
            this.priorityColumn = new System.Windows.Forms.ColumnHeader();
            this.ownerColumn = new System.Windows.Forms.ColumnHeader();
            this.summaryColumn = new System.Windows.Forms.ColumnHeader();
            this.foundLabel = new System.Windows.Forms.Label();
            this.searchFieldBox = new System.Windows.Forms.ComboBox();
            this.updateNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.includeClosedCheckBox = new System.Windows.Forms.CheckBox();
            this.searchBox = new System.Windows.Forms.ComboBox();
            cancelButton = new System.Windows.Forms.Button();
            statusStrip = new System.Windows.Forms.StatusStrip();
            toolTip = new System.Windows.Forms.ToolTip(this.components);
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Location = new System.Drawing.Point(761, 427);
            cancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 30);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.workStatus});
            statusStrip.Location = new System.Drawing.Point(0, 462);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            statusStrip.Size = new System.Drawing.Size(849, 25);
            statusStrip.TabIndex = 11;
            // 
            // statusLabel
            // 
            this.statusLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(835, 20);
            this.statusLabel.Spring = true;
            this.statusLabel.Text = "Ready";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // workStatus
            // 
            this.workStatus.AutoSize = false;
            this.workStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.workStatus.Image = ((System.Drawing.Image)(resources.GetObject("workStatus.Image")));
            this.workStatus.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.workStatus.Name = "workStatus";
            this.workStatus.Size = new System.Drawing.Size(45, 20);
            this.workStatus.Visible = false;
            // 
            // detailButton
            // 
            this.detailButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.detailButton.Location = new System.Drawing.Point(12, 427);
            this.detailButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.detailButton.Name = "detailButton";
            this.detailButton.Size = new System.Drawing.Size(75, 30);
            this.detailButton.TabIndex = 6;
            this.detailButton.Text = "&Details";
            toolTip.SetToolTip(this.detailButton, "Open details of selected issue in the browser");
            this.detailButton.UseVisualStyleBackColor = true;
            this.detailButton.Click += new System.EventHandler(this.DetailButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshButton.Enabled = false;
            this.refreshButton.Location = new System.Drawing.Point(93, 427);
            this.refreshButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 30);
            this.refreshButton.TabIndex = 7;
            this.refreshButton.Text = "&Refresh";
            toolTip.SetToolTip(this.refreshButton, "Reload the issue list");
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 7);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(55, 17);
            label1.TabIndex = 0;
            label1.Text = "&Search:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(316, 11);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(18, 17);
            label2.TabIndex = 2;
            label2.Text = "in";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(681, 427);
            this.okButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 30);
            this.okButton.TabIndex = 9;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // issueListView
            // 
            this.issueListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.issueListView.CheckBoxes = true;
            this.issueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.idColumn,
            this.typeColumn,
            this.statusColumn,
            this.priorityColumn,
            this.ownerColumn,
            this.summaryColumn});
            this.issueListView.FullRowSelect = true;
            this.issueListView.GridLines = true;
            this.issueListView.HideSelection = false;
            this.issueListView.Location = new System.Drawing.Point(0, 38);
            this.issueListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.issueListView.MultiSelect = false;
            this.issueListView.Name = "issueListView";
            this.issueListView.Size = new System.Drawing.Size(848, 382);
            this.issueListView.TabIndex = 5;
            this.issueListView.UseCompatibleStateImageBehavior = false;
            this.issueListView.View = System.Windows.Forms.View.Details;
            this.issueListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.IssueListView_ItemChecked);
            this.issueListView.SelectedIndexChanged += new System.EventHandler(this.IssueListView_SelectedIndexChanged);
            this.issueListView.DoubleClick += new System.EventHandler(this.IssueListView_DoubleClick);
            // 
            // idColumn
            // 
            this.idColumn.Text = "ID";
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            this.typeColumn.Width = 100;
            // 
            // statusColumn
            // 
            this.statusColumn.Text = "Status";
            this.statusColumn.Width = 100;
            // 
            // priorityColumn
            // 
            this.priorityColumn.Text = "Priority";
            this.priorityColumn.Width = 100;
            // 
            // ownerColumn
            // 
            this.ownerColumn.Text = "Owner";
            this.ownerColumn.Width = 100;
            // 
            // summaryColumn
            // 
            this.summaryColumn.Text = "Summary";
            this.summaryColumn.Width = 1000;
            // 
            // foundLabel
            // 
            this.foundLabel.AutoSize = true;
            this.foundLabel.Location = new System.Drawing.Point(490, 11);
            this.foundLabel.Name = "foundLabel";
            this.foundLabel.Size = new System.Drawing.Size(70, 17);
            this.foundLabel.TabIndex = 4;
            this.foundLabel.Text = "{0} found";
            this.foundLabel.Visible = false;
            // 
            // searchFieldBox
            // 
            this.searchFieldBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.searchFieldBox.FormattingEnabled = true;
            this.searchFieldBox.Location = new System.Drawing.Point(341, 7);
            this.searchFieldBox.Name = "searchFieldBox";
            this.searchFieldBox.Size = new System.Drawing.Size(121, 25);
            this.searchFieldBox.TabIndex = 3;
            this.searchFieldBox.SelectedIndexChanged += new System.EventHandler(this.SearchFieldBox_SelectedIndexChanged);
            // 
            // updateNotifyIcon
            // 
            this.updateNotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.updateNotifyIcon.BalloonTipText = "There is a new version of Gurtle available. Click here to find out more.";
            this.updateNotifyIcon.BalloonTipTitle = "Gurtle Update Available";
            this.updateNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("updateNotifyIcon.Icon")));
            this.updateNotifyIcon.Text = "Gurtle Update Available";
            this.updateNotifyIcon.BalloonTipClicked += new System.EventHandler(this.UpdateNotifyIcon_Click);
            this.updateNotifyIcon.Click += new System.EventHandler(this.UpdateNotifyIcon_Click);
            // 
            // includeClosedCheckBox
            // 
            this.includeClosedCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.includeClosedCheckBox.AutoSize = true;
            this.includeClosedCheckBox.Location = new System.Drawing.Point(175, 432);
            this.includeClosedCheckBox.Name = "includeClosedCheckBox";
            this.includeClosedCheckBox.Size = new System.Drawing.Size(155, 21);
            this.includeClosedCheckBox.TabIndex = 8;
            this.includeClosedCheckBox.Text = "I&nclude closed issues";
            this.includeClosedCheckBox.UseVisualStyleBackColor = true;
            this.includeClosedCheckBox.CheckedChanged += new System.EventHandler(this.RefreshButton_Click);
            // 
            // searchBox
            // 
            this.searchBox.FormattingEnabled = true;
            this.searchBox.Location = new System.Drawing.Point(76, 7);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(233, 25);
            this.searchBox.TabIndex = 1;
            this.searchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            // 
            // IssueBrowserDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = cancelButton;
            this.ClientSize = new System.Drawing.Size(849, 487);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.includeClosedCheckBox);
            this.Controls.Add(this.searchFieldBox);
            this.Controls.Add(this.foundLabel);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(statusStrip);
            this.Controls.Add(this.okButton);
            this.Controls.Add(cancelButton);
            this.Controls.Add(this.detailButton);
            this.Controls.Add(this.issueListView);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimizeBox = false;
            this.Name = "IssueBrowserDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Issues for {0} ({1})";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView issueListView;
        private System.Windows.Forms.ColumnHeader idColumn;
        private System.Windows.Forms.ColumnHeader summaryColumn;
        private System.Windows.Forms.ColumnHeader statusColumn;
        private System.Windows.Forms.ColumnHeader priorityColumn;
        private System.Windows.Forms.ColumnHeader ownerColumn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.Button detailButton;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ToolStripStatusLabel workStatus;
        private System.Windows.Forms.Label foundLabel;
        private System.Windows.Forms.ComboBox searchFieldBox;
        private System.Windows.Forms.NotifyIcon updateNotifyIcon;
        private System.Windows.Forms.CheckBox includeClosedCheckBox;
        private System.Windows.Forms.ComboBox searchBox;
    }
}

