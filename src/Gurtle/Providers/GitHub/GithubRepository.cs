#region License, Terms and Author(s)
//
// Gurtle - IBugTraqProvider for Google Code
// Copyright (c) 2011 Sven Strickroth. All rights reserved.
// Copyright (c) 2008, 2009 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Sven Strickroth, <email@cs-ware.de>
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

namespace Gurtle.Providers.GitHub
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    #endregion

    internal class GitHubRepository : IProvider
    {
        public static readonly Uri HostingUrl = new Uri("https://www.github.com/");

        public event EventHandler Loaded;

        public string Name { get { return "github"; } }
        private string _projectName = null;
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                if (value == null) throw new ArgumentNullException("projectName");
                if (!IsValidProjectName(value)) throw new ArgumentException("invalid project name.", value);
                _projectName = value;
            }
        }
        public Uri Url { get {
            Debug.Assert(ProjectName != null);
            return new Uri(HostingUrl, ProjectName); }
        }
        public IList<string> ClosedStatuses { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsLoading { get { return false; } }

        public bool CanHandleIssueUpdates()
        {
            return true;
        }

        public GitHubRepository()
        {
            commonConstructor();
        }

        private void commonConstructor()
        {
            ClosedStatuses = new string[1];
            ClosedStatuses[0] = "closed";
            IsLoaded = true;
        }
        public GitHubRepository(string projectName)
        {
            ProjectName = projectName;
            commonConstructor();
        }

        public Uri IssueDetailUrl(int id)
        {
            return FormatUrl("/" + ProjectName + "/issues/{0}", id);
        }

        public Uri RevisionDetailUrl(int revision)
        {
            return FormatUrl("source/detail?r={0}", revision);
        }

        public Uri IssuesUrl()
        {
            Debug.Assert(ProjectName != null);
            return new Uri("https://api.github.com/repos/" + ProjectName + "/issues");
        }

        private Uri FormatUrl(string relativeUrl)
        {
            Debug.Assert(ProjectName != null);
            var baseUrl = new Uri("https://github.com/api/v2/json/");
            return string.IsNullOrEmpty(relativeUrl) ? baseUrl : new Uri(baseUrl, relativeUrl);
        }

        private Uri FormatUrl(string relativeUrl, params object[] args)
        {
            return FormatUrl(string.Format(CultureInfo.InvariantCulture, relativeUrl, args));
        }

        public bool IsClosedStatus(string status)
        {
            return !string.IsNullOrEmpty(status)
                && status == "closed";
        }

        public void CancelLoad()
        {
        }

        public void Load()
        {
            if (IsLoaded)
                return;
            Reload();
        }

        private void OnLoaded()
        {
            OnLoaded(null);
        }

        private void OnLoaded(EventArgs args)
        {
            var handler = Loaded;
            if (handler != null)
                handler(this, args ?? EventArgs.Empty);
        }

        public void Reload()
        {
            Debug.Assert(ProjectName != null);
            if (IsLoading)
                return;
            OnLoaded(null);
        }

        public bool IsValidProjectName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return name.Length > 0 && Regex.IsMatch(name, @"^[A-Za-z][A-Za-z0-9-]*/[A-Za-z][A-Za-z0-9-]*$");
        }

        public Uri CloseIssueUrl(string status, int issueid)
        {
            return FormatUrl("issues/close/" + ProjectName + "/" + issueid);
        }

        public Uri CommentIssueUrl(int issueid)
        {
            return FormatUrl("issues/comment/" + ProjectName + "/" + issueid);
        }

        public Action DownloadIssues(string project, int start, bool includeClosedIssues,
               Func<IEnumerable<Issue>, bool> onData,
               Action<DownloadProgressChangedEventArgs> onProgress,
               Action<bool, Exception> onCompleted)
        {
            Debug.Assert(project != null);
            Debug.Assert(onData != null);

            var client = new Gurtle.WebClient();

            client.DownloadStringAsync(this.IssuesUrl());

            client.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    if (onCompleted != null)
                        onCompleted(args.Cancelled, args.Error);

                    return;
                }
                var issues = IssueTableParser.Parse(args.Result).ToArray();
                onData(issues);

                if (onCompleted != null)
                    onCompleted(false, null);
            };

            if (onProgress != null)
                client.DownloadProgressChanged += (sender, args) => onProgress(args);

            return client.CancelAsync;
        }

        public ListViewSorter<ListViewItem<Issue>, Issue> GenerateListViewSorter(ListView issueListView)
        {
            return new ListViewSorter<ListViewItem<Issue>, Issue>(issueListView,
                                      item => item.Tag,
                                      new Func<Issue, IComparable>[] {
                                            issue => (IComparable) issue.Id,
                                            issue => (IComparable) issue.Status,
                                            issue => (IComparable) issue.Owner,
                                            issue => (IComparable) issue.Summary
                                        });
        }

        public void GeneratorSubItems(ListViewItem<Issue> item, Issue issue)
        {
            var subItems = item.SubItems;
            subItems.Add(issue.Status);
            subItems.Add(issue.Owner);
            subItems.Add(issue.Summary);
        }

        public void SetupListView(ListView issueListView)
        {
            System.Windows.Forms.ColumnHeader statusColumn = new System.Windows.Forms.ColumnHeader();
            System.Windows.Forms.ColumnHeader ownerColumn = new System.Windows.Forms.ColumnHeader();
            System.Windows.Forms.ColumnHeader summaryColumn = new System.Windows.Forms.ColumnHeader();

            statusColumn.Text = "Status";
            statusColumn.Width = 100;
            ownerColumn.Text = "Owner";
            ownerColumn.Width = 100;
            summaryColumn.Text = "Summary";
            summaryColumn.Width = 1000;

            issueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                statusColumn,
                ownerColumn,
                summaryColumn});
        }

        public void FillSearchItems(ComboBox.ObjectCollection searchSourceItems)
        {
            searchSourceItems.Add(new Gurtle.IssueBrowserDialog.MultiFieldIssueSearchSource("All fields", MetaIssue.Properties));
            foreach (Issue.IssueField field in Enum.GetValues(typeof(Issue.IssueField)))
            {
                searchSourceItems.Add(new Gurtle.IssueBrowserDialog.SingleFieldIssueSearchSource(field.ToString(), MetaIssue.GetPropertyByField(field),
                    field == Issue.IssueField.Summary
                    || field == Issue.IssueField.Id
                    ? Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.None
                    : Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.Predefined));
            }
        }

        public void UpdateIssue(IssueUpdate update, NetworkCredential credential,
            Action<string> stdout, Action<string> stderr)
        {
            var client = new Gurtle.WebClient();
            System.Collections.Specialized.NameValueCollection data = new System.Collections.Specialized.NameValueCollection(1);
            if (update.Comment.Length > 0)
            {
                data.Add("comment", update.Comment);
            }

            client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credential.UserName + ":" + credential.Password)));
            client.UploadValues(CommentIssueUrl(update.Issue.Id), data);
            data.Clear();
            client.UploadValues(CloseIssueUrl(update.Status, update.Issue.Id), data);
        }

        public DialogResult ShowOptions(Parameters parameters) {
            OptionsDialog optionsDialog = new OptionsDialog { Parameters = parameters };
            return optionsDialog.ShowDialog();
        }
    }
}
