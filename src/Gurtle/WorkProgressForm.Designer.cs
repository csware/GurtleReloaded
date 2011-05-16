namespace Gurtle
{
    internal partial class WorkProgressForm
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
            this._status = new System.Windows.Forms.Label();
            this._bar = new System.Windows.Forms.ProgressBar();
            this._cancelButton = new System.Windows.Forms.Button();
            this._detailsButton = new System.Windows.Forms.Button();
            this._detailsBox = new System.Windows.Forms.TextBox();
            this._worker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // _status
            // 
            this._status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._status.AutoEllipsis = true;
            this._status.Location = new System.Drawing.Point(12, 24);
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(558, 17);
            this._status.TabIndex = 0;
            this._status.Text = "Working, please wait...";
            this._status.UseMnemonic = false;
            // 
            // _bar
            // 
            this._bar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._bar.Location = new System.Drawing.Point(15, 53);
            this._bar.Name = "_bar";
            this._bar.Size = new System.Drawing.Size(555, 23);
            this._bar.TabIndex = 1;
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._cancelButton.Location = new System.Drawing.Point(475, 91);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(95, 30);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            this._cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // _detailsButton
            // 
            this._detailsButton.Location = new System.Drawing.Point(12, 91);
            this._detailsButton.Name = "_detailsButton";
            this._detailsButton.Size = new System.Drawing.Size(95, 30);
            this._detailsButton.TabIndex = 2;
            this._detailsButton.Tag = "Show &Details";
            this._detailsButton.Text = "Hide &Details";
            this._detailsButton.UseVisualStyleBackColor = true;
            this._detailsButton.Visible = false;
            this._detailsButton.Click += new System.EventHandler(this.DetailsButton_Click);
            // 
            // _detailsBox
            // 
            this._detailsBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._detailsBox.Location = new System.Drawing.Point(15, 138);
            this._detailsBox.Multiline = true;
            this._detailsBox.Name = "_detailsBox";
            this._detailsBox.ReadOnly = true;
            this._detailsBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._detailsBox.Size = new System.Drawing.Size(555, 207);
            this._detailsBox.TabIndex = 4;
            this._detailsBox.WordWrap = false;
            // 
            // _worker
            // 
            this._worker.WorkerReportsProgress = true;
            this._worker.WorkerSupportsCancellation = true;
            this._worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Worker_RunWorkerCompleted);
            this._worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.Worker_ProgressChanged);
            // 
            // WorkProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(582, 357);
            this.Controls.Add(this._detailsBox);
            this.Controls.Add(this._detailsButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._bar);
            this.Controls.Add(this._status);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "WorkProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Working";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _status;
        private System.Windows.Forms.ProgressBar _bar;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Button _detailsButton;
        private System.Windows.Forms.TextBox _detailsBox;
        private System.ComponentModel.BackgroundWorker _worker;
    }
}