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
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    #endregion

    internal class TracProject : IProvider
    {
        public event EventHandler Loaded;

        public string Name { get { return "trac"; } }
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
        public IList<string> ClosedStatuses { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsLoading { get { return false; } }

        public bool CanHandleIssueUpdates()
        {
            return true;
        }

        public TracProject()
        {
            commonConstructor();
        }

        private void commonConstructor()
        {
            ClosedStatuses = new string[1];
            ClosedStatuses[0] = "fixed";
            IsLoaded = true; 
        }

        public TracProject(string projectName)
        {
            ProjectName = projectName;
            commonConstructor();
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
            Debug.Assert(ProjectName != null);
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
            Debug.Assert(ProjectName != null);
            if (IsLoading)
                return;
            OnLoaded(null);
        }

        public bool IsValidProjectName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return name.Length > 0 && Regex.IsMatch(name, @"^https?://.*$");
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

            client.UploadStringCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    if (onCompleted != null)
                        onCompleted(args.Cancelled, args.Error);

                    return;
                }

                JsonData data = JsonMapper.ToObject(args.Result);
                if (data["error"] != null)
                {
                    if (onCompleted != null)
                        onCompleted(false, new Exception((string)data["error"]["message"]));

                    return;
                }
                else if (data["result"] != null && data["result"].Count > 0)
                {
                    var client2 = new WebClient();
                    client2.Headers.Add("Content-Type", "application/json");
                    var jsonRPCQuery = new JsonWriter();
                    jsonRPCQuery.WriteObjectStart();
                    jsonRPCQuery.WritePropertyName("method");
                    jsonRPCQuery.Write("system.multicall");
                    jsonRPCQuery.WritePropertyName("params");
                    jsonRPCQuery.WriteArrayStart();
                    for (int i = 0; i < data["result"].Count - 1; i++)
                    {
                        jsonRPCQuery.WriteObjectStart();
                        jsonRPCQuery.WritePropertyName("method");
                        jsonRPCQuery.Write("ticket.get");
                        jsonRPCQuery.WritePropertyName("params");
                        jsonRPCQuery.WriteArrayStart();
                        jsonRPCQuery.Write((int)data["result"][i]);
                        jsonRPCQuery.WriteArrayEnd();
                        jsonRPCQuery.WriteObjectEnd();
                    }

                    client2.UploadStringCompleted += (sender2, args2) =>
                    {
                        if (args2.Cancelled || args2.Error != null)
                        {
                            return;
                        }

                        int lastObjectStart = 0;
                        int count = 0;
                        bool inQuote = false;
                        bool lastWasBackslash = false;
                        for (int n = 1; n < args2.Result.Length; n++)
                        {
                            if (inQuote)
                            {
                                if (args2.Result[n] == '"' && !lastWasBackslash)
                                {
                                    inQuote = false;
                                }
                                else if (args2.Result[n] == '\\')
                                {
                                    lastWasBackslash = !lastWasBackslash;
                                }
                                else
                                {
                                    lastWasBackslash = false;
                                }
                            }
                            else
                            {
                                if (args2.Result[n] == '"')
                                {
                                    inQuote = true;
                                }
                                else if (args2.Result[n] == '}')
                                {
                                    --count;
                                    if (count == 0)
                                    {
                                        n++;
                                        JsonData issueData = JsonMapper.ToObject(args2.Result.Substring(lastObjectStart, n - lastObjectStart));
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
                                    }
                                }
                                else if (args2.Result[n] == '{')
                                {
                                    if (count == 0)
                                    {
                                        lastObjectStart = n;
                                    }
                                    ++count;
                                }
                            }
                        }

                        if (onCompleted != null)
                            onCompleted(false, null);
                    };

                    jsonRPCQuery.WriteArrayEnd();
                    jsonRPCQuery.WriteObjectEnd();
                    client2.UploadStringAsync(this.RPCUrl(), jsonRPCQuery.ToString());

                    if (onProgress != null)
                        client.DownloadProgressChanged += (sender2, args2) => onProgress(args2);
                }
            };

            if (onProgress != null)
                client.DownloadProgressChanged += (sender, args) => onProgress(args);

            var jsonRPCQueryTicktes = new JsonWriter();
            jsonRPCQueryTicktes.WriteObjectStart();
            jsonRPCQueryTicktes.WritePropertyName("method");
            jsonRPCQueryTicktes.Write("ticket.query");
            jsonRPCQueryTicktes.WritePropertyName("qstr");
            jsonRPCQueryTicktes.Write("status!=closed");
            jsonRPCQueryTicktes.WriteObjectEnd();

            client.UploadStringAsync(this.RPCUrl(), jsonRPCQueryTicktes.ToString());

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
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(credential.UserName + ":" + credential.Password)));

            var jsonRPCQuery = new JsonWriter();
            jsonRPCQuery.WriteObjectStart();
            jsonRPCQuery.WritePropertyName("method");
            jsonRPCQuery.Write("ticket.update");
            jsonRPCQuery.WritePropertyName("params");
            jsonRPCQuery.WriteArrayStart();
            jsonRPCQuery.Write(update.Issue.Id);
            jsonRPCQuery.Write(update.Comment);
            jsonRPCQuery.WriteObjectStart();
            jsonRPCQuery.WritePropertyName("action");
            jsonRPCQuery.Write("resolve");
            jsonRPCQuery.WritePropertyName("action_resolve_resolve_resolution");
            jsonRPCQuery.Write("fixed");
            jsonRPCQuery.WriteObjectEnd();
            jsonRPCQuery.Write(true);
            jsonRPCQuery.WriteArrayEnd();
            jsonRPCQuery.WriteObjectEnd();

            client.UploadString(this.RPCUrl(), jsonRPCQuery.ToString());
        }
    }
}
