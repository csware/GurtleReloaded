#region License, Terms and Author(s)
//
// Gurtle - IBugTraqProvider for Google Code
// Copyright (c) 2008, 2009 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace Gurtle
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    #endregion

    internal sealed partial class WorkProgressForm : Form
    {
        private string _successStatusText = "Finished";
        private string _errorStatusText = "Error: {Message}";
        private string _cancelledStatusText = "User-Cancelled";

        public event EventHandler WorkFailed;

        public bool StartWorkOnShow { get; set; }
        public bool CloseOnCompletion { get; set; }

        public string SuccessStatusText
        {
            get { return _successStatusText ?? string.Empty; }
            set { _successStatusText = value; }
        }

        public string ErrorStatusText
        {
            get { return _errorStatusText ?? string.Empty; }
            set { _errorStatusText = value; }
        }

        public string CancelledStatusText
        {
            get { return _cancelledStatusText ?? string.Empty; }
            set { _cancelledStatusText = value; }
        }

        public bool Cancelled { get; private set; }
        public Exception Error { get; private set; }
        public object Result { get; private set; }
        
        public BackgroundWorker Worker { get { return _worker; } }

        public WorkProgressForm()
        {
            InitializeComponent();
        }

        public void ReportProgress(int percentage)
        {
            ReportProgressImpl(percentage, null);
        }

        public void ReportProgress(string status)
        {
            ReportProgress(-1, status);
        }

        public void ReportProgress(int percentage, string status)
        {
            ReportProgressImpl(percentage, () => _status.Text = status);
        }

        public void ReportDetailLine(string line)
        {
            ReportProgressImpl(-1, () => _detailsBox.AppendText(line + Environment.NewLine));
        }

        private void ReportProgressImpl(int percentage, Action update)
        {
            if (InvokeRequired)
                Worker.ReportProgress(percentage, update);
            else
                OnProgressChanged(percentage, update);
        }

        protected override void OnShown(EventArgs e)
        {
            if (StartWorkOnShow)
                Worker.RunWorkerAsync();
            base.OnShown(e);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgressChanged(e.ProgressPercentage, (Action) e.UserState);
        }

        private void OnProgressChanged(int percentage, Action update)
        {
            if (percentage >= 0)
                _bar.Value = percentage;
            if (update != null)
                update();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _bar.Value = _bar.Maximum;
            var cancelled = Cancelled = e.Cancelled;

            string status = null;

            if ((Error = e.Error) == null)
            {
                if (cancelled)
                {
                    if (CancelledStatusText.Length > 0)
                        status = CancelledStatusText;
                }
                else
                {
                    Result = e.Result;
                    if (SuccessStatusText.Length > 0)
                        status = SuccessStatusText;
                }
            }
            else
            {
                if (ErrorStatusText.Length > 0)
                    status = ErrorStatusText.FormatWith(e.Error);

                var handler = WorkFailed;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }

            if (status != null)
                _status.Text = status;

            if (CloseOnCompletion)
            {
                Close();
            }
            else
            {
                _cancelButton.Hide();

                var button = new Button
                {
                    Text = "&Close", 
                    Location = _cancelButton.Location,
                    Size = _cancelButton.Size,
                    TabIndex = _cancelButton.TabIndex,
                    Anchor = _cancelButton.Anchor,
                };
                button.Click += delegate { Close(); };
                CancelButton = button;
                Controls.Add(button);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            _cancelButton.Enabled = false;
            _worker.CancelAsync();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = _worker.IsBusy;
            base.OnClosing(e);
        }

        private void DetailsButton_Click(object sender, EventArgs e)
        {
            var swap = (string) _detailsButton.Tag;
            _detailsButton.Tag = _detailsButton.Text;
            _detailsButton.Text = swap;
            var delta = (_detailsBox.Visible ? -1 : 1) * _detailsBox.Size.Height;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + delta);
            _detailsBox.Visible = !_detailsBox.Visible;
        }
    }
}
