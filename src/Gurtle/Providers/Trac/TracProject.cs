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

namespace Gurtle.Providers.Trac
{
    #region Imports

    using LitJson;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    #endregion

    internal class TracProject : IProvider
    {
        public event EventHandler Loaded;

        public string Name { get { return "github"; } }
        public string ProjectName { get; private set; }
        public Uri Url { get; private set; }
        public IList<string> ClosedStatuses { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsLoading { get { return false; } }

        public bool CanHandleIssueUpdates()
        {
            return false;
        }

        public TracProject(string projectName)
        {
            if (projectName == null) throw new ArgumentNullException("projectName");
            if (!IsValidProjectName(projectName)) throw new ArgumentException("invalid project name.", projectName);

            ProjectName = projectName;
            Url = FormatUrl(null);
            ClosedStatuses = new string[0];
        }

        public Uri IssueDetailUrl(int id)
        {
            return FormatUrl("ticket/{0}", id);
        }

        public Uri RevisionDetailUrl(int revision)
        {
            return null;
        }

        public Uri RPCUrl()
        {
            return FormatUrl("rpc");
        }

        private Uri FormatUrl(string relativeUrl)
        {
            var baseUrl = new Uri(ProjectName);
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
            if (IsLoading)
                return;
            OnLoaded(null);
        }

        public static bool IsValidProjectName(string name)
        {
            return true;
            if (name == null)
                throw new ArgumentNullException("name");

            return name.Length > 0 && Regex.IsMatch(name, @"^[A-Za-z][A-Za-z0-9-]*/[A-Za-z][A-Za-z0-9-]*$");
        }

        public Action DownloadIssues(string project, int start, bool includeClosedIssues,
               Func<IEnumerable<Issue>, bool> onData,
               Action<DownloadProgressChangedEventArgs> onProgress,
               Action<bool, Exception> onCompleted)
        {
            Debug.Assert(project != null);
            Debug.Assert(onData != null);

            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.UploadStringAsync(this.RPCUrl(), "{\"method\": \"ticket.query\", \"qstr\": \"status!=closed\"}");

            client.UploadStringCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    if (onCompleted != null)
                        onCompleted(args.Cancelled, args.Error);

                    return;
                }

                JsonData data = JsonMapper.ToObject(args.Result);
                if (data["result"].Count > 0)
                {
                    for (int i = 0; i < data["result"].Count - 1; i++)
                    {
                        var client2 = new WebClient();
                        client2.Headers.Add("Content-Type", "application/json");
                        client2.UploadStringCompleted += (sender2, args2) =>
                        {
                            if (args2.Cancelled || args2.Error != null)
                            {
                                return;
                            }

                            JsonData issueData = JsonMapper.ToObject(args2.Result);
                            TracIssue[] issues = new TracIssue[1];
                            issues[0] = new TracIssue();
                            issues[0].Id = (int)issueData["result"][0];
                            if (issueData["result"][3].ToString().Contains("milestone"))
                            {
                                issues[0].Milestone = (string)issueData["result"][3]["milestone"];
                            }
                            issues[0].Owner = (string)issueData["result"][3]["owner"];
                            issues[0].Type = (string)issueData["result"][3]["type"];
                            issues[0].Priority = (string)issueData["result"][3]["priority"];
                            issues[0].Status = (string)issueData["result"][3]["status"];
                            issues[0].Summary = (string)issueData["result"][3]["summary"];
                            onData(issues);
                            if (onCompleted != null)
                                onCompleted(false, null);
                        };
                        client2.UploadStringAsync(this.RPCUrl(), "{\"method\": \"ticket.get\", \"params\": [" + data["result"][i] + "]}");
                    }
                }
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
                              issue => (IComparable) issue.Type,
                              issue => (IComparable) issue.Status,
                              issue => (IComparable) ((TracIssue)issue).Priority,
                              issue => (IComparable) ((TracIssue)issue).Owner,
                              issue => (IComparable) issue.Summary
                                        });
        }

        public void GeneratorSubItems(ListViewItem<Issue> item, Issue issue)
        {
            var subItems = item.SubItems;
            subItems.Add(issue.Type);
            subItems.Add(issue.Status);
            subItems.Add(((TracIssue)issue).Priority);
            subItems.Add(((TracIssue)issue).Owner);
            subItems.Add(issue.Summary);
        }

        public void SetupListView(ListView issueListView)
        {
            System.Windows.Forms.ColumnHeader priorityColumn = new System.Windows.Forms.ColumnHeader();
            System.Windows.Forms.ColumnHeader ownerColumn = new System.Windows.Forms.ColumnHeader();
            System.Windows.Forms.ColumnHeader typeColumn = new System.Windows.Forms.ColumnHeader();
            System.Windows.Forms.ColumnHeader statusColumn = new System.Windows.Forms.ColumnHeader();
            System.Windows.Forms.ColumnHeader summaryColumn = new System.Windows.Forms.ColumnHeader();

            typeColumn.Text = "Type";
            typeColumn.Width = 100;
            statusColumn.Text = "Status";
            statusColumn.Width = 100;
            priorityColumn.Text = "Priority";
            priorityColumn.Width = 100;
            ownerColumn.Text = "Owner";
            ownerColumn.Width = 100;
            summaryColumn.Text = "Summary";
            summaryColumn.Width = 1000;

            issueListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            typeColumn,
            statusColumn,
            priorityColumn,
            ownerColumn,
            summaryColumn});
        }

        public void FillSearchItems(ComboBox.ObjectCollection searchSourceItems)
        {
            searchSourceItems.Add(new Gurtle.IssueBrowserDialog.MultiFieldIssueSearchSource("All fields", MetaIssue.Properties));
            foreach (TracIssue.IssueField field in Enum.GetValues(typeof(TracIssue.IssueField)))
            {
                searchSourceItems.Add(new Gurtle.IssueBrowserDialog.SingleFieldIssueSearchSource(field.ToString(), MetaIssue.GetPropertyByField(field),
                    field == TracIssue.IssueField.Summary
                    || field == TracIssue.IssueField.Id
                    ? Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.None
                    : Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.Predefined));
            }
        }

        public void UpdateIssue(IssueUpdate update, NetworkCredential credential,
            Action<string> stdout, Action<string> stderr)
        {
        }
    }
}
