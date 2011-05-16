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
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    #endregion

    internal sealed partial class IssueUpdatePage : UserControl
    {
        public event EventHandler<RevisionEventArgs> RevisionClicked;
        public event EventHandler SkipChanged;

        public IssueUpdatePage()
        {
            InitializeComponent();
        }

        [DefaultValue("")]
        public string Summary
        {
            get { return summaryLabel.Text; }
            set { summaryLabel.Text = value; }
        }

        [Browsable(false)]
        public Uri Url { get; set; }

        [DefaultValue("")]
        public string Comment
        {
            get { return commentBox.Text; }
            set { commentBox.Text = value; }
        }

        [DefaultValue("")]
        public string Status
        {
            get { return statusBox.Text; }
            set { statusBox.Text = value; }
        }

        [DefaultValue(false)]
        public bool Skip
        {
            get { return skipBox.Checked; }
            set { skipBox.Checked = value; }
        }

        private void OnSkipChanged(EventArgs args)
        {
            Debug.Assert(args != null);
            var handler = SkipChanged;
            if (handler != null) handler(this, args);
        }

        public void LoadStatusOptions(IEnumerable<string> options)
        {
            var items = statusBox.Items;
            items.Clear();
            items.AddRange(options.ToArray());
        }

        private void Summary_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Url == null)
                return;

            Process.Start(Url.ToString());
        }

        private void RevisionsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OnRevisionClicked(new RevisionEventArgs((int) e.Link.LinkData));
        }

        private void OnRevisionClicked(RevisionEventArgs args)
        {
            Debug.Assert(args != null);
            var handler = RevisionClicked;
            if (handler != null) handler(this, args);
        }

        private void CommentBox_TextChanged(object sender, EventArgs e)
        {
            var label = revisionsLabel;
            var re = new Regex(@"\br([0-9]{1,6})\b", RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace);
            var matches = re.Matches(commentBox.Text);
            label.Visible = matches.Count > 0;
            label.Text = "Revisions: " + string.Join(", ", matches.Cast<Match>().Select(m => m.Value).Distinct().ToArray());
            var links = label.Links;
            links.Clear();
            foreach (Match match in re.Matches(label.Text))
                links.Add(match.Index, match.Length, int.Parse(match.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture));
        }

        private void StatusBox_TextChanged(object sender, EventArgs e)
        {
            var status = statusBox.Text.Trim();
            if (status.Length == 0)
                return;

            var items = statusBox.Items;
            if (items.Count == 0)
                return;

            var found = items.Cast<string>().Any(s => s.Equals(status, StringComparison.OrdinalIgnoreCase));

            statusBox.ForeColor = found 
                                ? SystemColors.WindowText 
                                : Color.FromKnownColor(KnownColor.Red);
        }

        private void SkipBox_CheckedChanged(object sender, EventArgs e)
        {
            statusBox.Enabled = commentBox.Enabled = revisionsLabel.Enabled = !skipBox.Checked;
            OnSkipChanged(EventArgs.Empty);
        }
    }    
}
