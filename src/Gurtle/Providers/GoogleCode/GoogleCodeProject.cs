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

namespace Gurtle.Providers.GoogleCode
{
    #region Imports

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.Schema;

    #endregion

    internal class GoogleCodeProject : IProvider
    {
        public static readonly Uri HostingUrl = new Uri("http://code.google.com/hosting/");

        private string token = null;

        public event EventHandler Loaded;

        public string Name { get { return "googlecode"; } }
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
        public Uri Url { get { return FormatUrl(null); } }
        public IList<string> ClosedStatuses { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsLoading { get { return false; } }

        public bool CanHandleIssueUpdates()
        {
            return true;
        }

        public GoogleCodeProject()
        {
            commonConstructor();
        }

        private void commonConstructor()
        {
            ClosedStatuses = new string[0];
            IsLoaded = true;
        }

        public GoogleCodeProject(string projectName)
        {
            ProjectName = projectName;
            commonConstructor();
        }

        public Uri DnsUrl()
        {
            Debug.Assert(ProjectName != null);
            return new Uri("http://" + ProjectName + ".googlecode.com/");
        }

        public Uri IssueDetailUrl(int id)
        {
            return FormatUrl("issues/detail?id={0}", id);
        }

        public Uri RevisionDetailUrl(int revision)
        {
            return FormatUrl("source/detail?r={0}", revision);
        }

        public Uri IssuesCsvUrl(int start)
        {
            return IssuesCsvUrl(start, false);
        }

        public Uri IssuesCsvUrl(int start, bool includeClosed)
        {
            return FormatUrl("issues/csv?start={0}&colspec={1}{2}",
                       start.ToString(CultureInfo.InvariantCulture),
                       string.Join("%20", Enum.GetNames(typeof(GoogleCodeIssue.IssueField))),
                       includeClosed ? "&can=1" : string.Empty);
        }

        private Uri FormatUrl(string relativeUrl)
        {
            Debug.Assert(ProjectName != null);
            var baseUrl = new Uri("http://code.google.com/p/" + ProjectName + "/");
            return string.IsNullOrEmpty(relativeUrl) ? baseUrl : new Uri(baseUrl, relativeUrl);
        }

        private Uri FormatUrl(string relativeUrl, params object[] args)
        {
            return FormatUrl(string.Format(CultureInfo.InvariantCulture, relativeUrl, args));
        }

        public bool IsClosedStatus(string status)
        {
            return !string.IsNullOrEmpty(status)
                && ClosedStatuses.Any(s => status.Equals(s, StringComparison.InvariantCultureIgnoreCase));
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

        public void CancelLoad()
        {
        }

        public void Load()
        {
            if (IsLoaded)
                return;
            Reload();
        }

        public void Reload()
        {
            Debug.Assert(ProjectName != null);
            if (IsLoading)
                return;

            IsLoaded = true;
            OnLoaded();
        }

        public bool IsValidProjectName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            //
            // From http://code.google.com/hosting/createProject:
            //
            //   "...project's projectName must consist of a lowercase letter, 
            //    followed by lowercase letters, digits, and dashes, 
            //    with no spaces."
            //

            return name.Length > 0 && Regex.IsMatch(name, @"^[a-z][a-z0-9-]*$");
        }

        public Action DownloadIssues(string project, int start, bool includeClosedIssues,
                Func<IEnumerable<Issue>, bool> onData,
                Action<DownloadProgressChangedEventArgs> onProgress,
                Action<bool, Exception> onCompleted)
        {
            Debug.Assert(project != null);
            Debug.Assert(onData != null);

            var client = new WebClient();

            Action<int> pager = next => client.DownloadStringAsync(
                this.IssuesCsvUrl(next, includeClosedIssues));

            client.DownloadStringCompleted += (sender, args) =>
            {
                if (args.Cancelled || args.Error != null)
                {
                    if (onCompleted != null)
                        onCompleted(args.Cancelled, args.Error);

                    return;
                }

                var issues = IssueTableParser.Parse(new StringReader(args.Result)).ToArray();
                var more = onData(issues);

                if (more)
                {
                    start += issues.Length;
                    pager(start);
                }
                else
                {
                    if (onCompleted != null)
                        onCompleted(false, null);
                }
            };

            if (onProgress != null)
                client.DownloadProgressChanged += (sender, args) => onProgress(args);

            pager(start);

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
                              issue => (IComparable) ((GoogleCodeIssue)issue).Priority,
                              issue => (IComparable) ((GoogleCodeIssue)issue).Owner,
                              issue => (IComparable) issue.Summary
                          }
                                  );
        }

        public void GeneratorSubItems(ListViewItem<Issue> item, Issue issue)
        {
            var subItems = item.SubItems;
            subItems.Add(issue.Type);
            subItems.Add(issue.Status);
            subItems.Add(((GoogleCodeIssue)issue).Priority);
            subItems.Add(((GoogleCodeIssue)issue).Owner);
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
            foreach (GoogleCodeIssue.IssueField field in Enum.GetValues(typeof(GoogleCodeIssue.IssueField)))
            {
                searchSourceItems.Add(new Gurtle.IssueBrowserDialog.SingleFieldIssueSearchSource(field.ToString(), MetaIssue.GetPropertyByField(field),
                    field == GoogleCodeIssue.IssueField.Summary
                    || field == GoogleCodeIssue.IssueField.Id
                    || field == GoogleCodeIssue.IssueField.Stars
                    ? Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.None
                    : Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.Predefined));
            }
        }

        public void UpdateIssue(IssueUpdate update, NetworkCredential credential,
            Action<string> stdout, Action<string> stderr)
        {
            var client = new WebClient();
            if (token == null)
            {
                System.Collections.Specialized.NameValueCollection data = new System.Collections.Specialized.NameValueCollection(1);
                data.Add("accountType", "GOOGLE");
                data.Add("Email", credential.UserName);
                data.Add("Passwd", credential.Password);
                data.Add("service", "code");
                data.Add("source", "opensource-gurtlereloaded-1");

                string[] result = ByteArrayToString(client.UploadValues("https://www.google.com/accounts/ClientLogin", data)).Split('\n');

                for (int i = 0; i < result.Length; i++)
                {
                    if (result[i].StartsWith("auth=", StringComparison.InvariantCultureIgnoreCase))
                    {
                        token = result[i];
                        break;
                    }
                }
            }

            var doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            XmlSchema schema = new XmlSchema();
            schema.Namespaces.Add("xmlns", "http://www.w3.org/2005/Atom");
            schema.Namespaces.Add("issues", "http://schemas.google.com/projecthosting/issues/2009");
            doc.Schemas.Add(schema);
            XmlElement entry = doc.CreateElement("entry", "http://www.w3.org/2005/Atom");
            doc.AppendChild(entry);
            XmlElement author = doc.CreateElement("author", "http://www.w3.org/2005/Atom");
            entry.AppendChild(author);
            XmlElement name = doc.CreateElement("name", "http://www.w3.org/2005/Atom");
            name.InnerText = "empty";
            author.AppendChild(name);
            if (update.Comment.Length > 0)
            {
                XmlElement content = doc.CreateElement("content", "http://www.w3.org/2005/Atom");
                XmlAttribute contentType = doc.CreateAttribute("type");
                contentType.InnerText = "html";
                content.Attributes.Append(contentType);
                content.InnerText = update.Comment;
                entry.AppendChild(content);
            }
            XmlElement updates = doc.CreateElement("issues", "updates", "http://schemas.google.com/projecthosting/issues/2009");
            entry.AppendChild(updates);
            XmlElement status = doc.CreateElement("issues", "status", "http://schemas.google.com/projecthosting/issues/2009");
            status.InnerText = update.Status;
            updates.AppendChild(status);


            client.Headers.Add("Content-Type", "application/atom+xml");
            client.Headers.Add("Authorization", "GoogleLogin " + token);

            client.UploadString(new Uri("https://code.google.com/feeds/issues/p/" + ProjectName + "/issues/"+ update.Issue.Id +"/comments/full"), doc.InnerXml);
        }

        private string ByteArrayToString(byte[] arr)
        {
            ASCIIEncoding enc = new ASCIIEncoding();
            return enc.GetString(arr);
        }
    }
}
