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
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using System.Text.RegularExpressions;

    #endregion

    internal class GoogleCodeProject
    {
        public static readonly Uri HostingUrl = new Uri("http://code.google.com/hosting/");

        private WebClient _wc;

        public event EventHandler Loaded;

        public string Name { get; private set; }
        public Uri Url { get; private set; }
        public IList<string> ClosedStatuses { get; private set; }
        public bool IsLoaded { get; private set; }
        public bool IsLoading { get { return _wc != null; } }

        public GoogleCodeProject(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (!IsValidProjectName(name)) throw new ArgumentException(null, "name");

            Debug.Assert(name != null);
            Debug.Assert(IsValidProjectName(name));

            Name = name;
            Url = FormatUrl(null);
            ClosedStatuses = new string[0];
        }

        public Uri DnsUrl()
        {
            return new Uri("http://" + Name + ".googlecode.com/");
        }

        public Uri IssueDetailUrl(int id)
        {
            return FormatUrl("issues/detail?id={0}", id);
        }

        public Uri RevisionDetailUrl(int revision)
        {
            return FormatUrl("source/detail?r={0}", revision);
        }

        public Uri IssueOptionsFeedUrl()
        {
            return FormatUrl("feeds/issueOptions");
        }        

        public Uri IssuesCsvUrl(int start)
        {
            return IssuesCsvUrl(start, false);
        }

        public Uri IssuesCsvUrl(int start, bool includeClosed)
        {
            return FormatUrl("issues/csv?start={0}&colspec={1}{2}",
                       start.ToString(CultureInfo.InvariantCulture),
                       string.Join("%20", Enum.GetNames(typeof(IssueField))),
                       includeClosed ? "&can=1" : string.Empty);
        }

        private Uri FormatUrl(string relativeUrl)
        {
            var baseUrl = new Uri("http://code.google.com/p/" + Name + "/");
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
            if (!IsLoading) 
                return;
            _wc.CancelAsync();
            _wc = null;
        }

        public void Load()
        {
            if (IsLoaded)
                return;
            Reload();
        }

        public void Reload()
        {
            if (IsLoading)
                return;

            var wc = new WebClient();

            wc.DownloadStringCompleted += (sender, args) =>
            {
                _wc = null;

                if (args.Cancelled || args.Error != null)
                    return;

                var contentType = wc.ResponseHeaders[HttpResponseHeader.ContentType]
                                        .MaskNull().Split(new[] { ';' }, 2)[0];

                var jsonContentTypes = new[] {
                    "application/json", 
                    "application/x-javascript", 
                    "text/javascript",
                };

                if (!jsonContentTypes.Any(s => s.Equals(contentType, StringComparison.OrdinalIgnoreCase)))
                    return;

                using (var sc = new ScriptControl { Language = "JavaScript" })
                {
                    var data = sc.Eval("(" + args.Result + ")"); // TODO: JSON sanitization

                    ClosedStatuses = new ReadOnlyCollection<string>(
                        new OleDispatchDriver(data)
                           .Get<IEnumerable>("closed")
                           .Cast<object>()
                           .Select(o => new OleDispatchDriver(o).Get<string>("name"))
                           .ToArray());
                }

                IsLoaded = true;
                OnLoaded();
            };

            wc.DownloadStringAsync(IssueOptionsFeedUrl());
            _wc = wc;
        }

        public static bool IsValidProjectName(string name)
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

        public static Action DownloadIssues(string project, int start, bool includeClosedIssues,
                Func<IEnumerable<Issue>, bool> onData,
                Action<DownloadProgressChangedEventArgs> onProgress,
                Action<bool, Exception> onCompleted)
        {
            Debug.Assert(project != null);
            Debug.Assert(onData != null);

            var client = new WebClient();

            Action<int> pager = next => client.DownloadStringAsync(
                new GoogleCodeProject(project).IssuesCsvUrl(next, includeClosedIssues));

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

        internal ListViewSorter<ListViewItem<Issue>, Issue> GenerateListViewSorter(ListView issueListView)
        {
            return new ListViewSorter<ListViewItem<Issue>, Issue>(issueListView,
                                      item => item.Tag,
                                      new Func<Issue, IComparable>[] {
                              issue => (IComparable) issue.Id,
                              issue => (IComparable) issue.Type,
                              issue => (IComparable) issue.Status,
                              issue => (IComparable) issue.Priority,
                              issue => (IComparable) issue.Owner,
                              issue => (IComparable) issue.Summary
                          }
                                  );
        }

        internal void GeneratorSubItems(ListViewItem<Issue> item, Issue issue)
        {
            var subItems = item.SubItems;
            subItems.Add(issue.Type);
            subItems.Add(issue.Status);
            subItems.Add(issue.Priority);
            subItems.Add(issue.Owner);
            subItems.Add(issue.Summary);
        }

        internal void SetupListView(ListView issueListView)
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

        internal void FillSearchItems(ComboBox.ObjectCollection searchSourceItems)
        {
            foreach (IssueField field in Enum.GetValues(typeof(IssueField)))
            {
                searchSourceItems.Add(new Gurtle.IssueBrowserDialog.SingleFieldIssueSearchSource(field.ToString(), MetaIssue.GetPropertyByField(field),
                    field == IssueField.Summary
                    || field == IssueField.Id
                    || field == IssueField.Stars
                    ? Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.None
                    : Gurtle.IssueBrowserDialog.SearchableStringSourceCharacteristics.Predefined));
            }
        }

        internal void UpdateIssue(IssueUpdate update, NetworkCredential credential,
            Action<string> stdout, Action<string> stderr)
        {
            string commentPath = null;

            var comment = update.Comment;
            if (comment.Length > 0)
            {
                if (comment.IndexOfAny(new[] { '\r', '\n', '\f' }) >= 0)
                {
                    commentPath = Path.GetTempFileName();
                    File.WriteAllText(commentPath, comment, Encoding.UTF8);
                    comment = "@" + commentPath;
                }
                else if (comment[0] == '@')
                {
                    comment = "@" + comment;
                }
            }

            try
            {
                var commandLine = Environment.GetEnvironmentVariable("GURTLE_ISSUE_UPDATE_CMD")
                                  ?? string.Empty;

                stderr("GURTLE_ISSUE_UPDATE_CMD: " + commandLine);

                var args = CommandLineUtils.CommandLineToArgs(commandLine);
                var command = args.First();

                for (var i = 0; i < args.Length; i++)
                    stderr(string.Format("[{0}]: {1}", i, args[i]));

                args = args.Skip(1)
                           .Select(arg => arg.FormatWith(CultureInfo.InvariantCulture, new
                           {
                               credential.UserName,
                               credential.Password,
                               Project = this,
                               Issue = update.Issue,
                               Status = update.Status,
                               Comment = comment,
                           }))
                           .Select(arg => CommandLineUtils.EncodeCommandLineArg(arg))
                           .ToArray();

                var formattedCommandLineArgs = string.Join(" ", args);
                stderr(formattedCommandLineArgs.Replace(credential.Password, "**********"));

                using (var process = Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    FileName = command,
                    Arguments = formattedCommandLineArgs,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                }))
                {
                    Debug.Assert(process != null);

                    stderr("PID: " + process.Id);

                    process.OutputDataReceived += (sender, e) => stdout(e.Data);
                    process.ErrorDataReceived += (sender, e) => stderr(e.Data);
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception(
                            string.Format("Issue update command failed with an exit code of {0}.",
                            process.ExitCode));
                    }
                }

            }
            finally
            {
                if (!string.IsNullOrEmpty(commentPath) && File.Exists(commentPath))
                    File.Delete(commentPath);
            }
        }
    }
}
