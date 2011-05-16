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
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using IssueListViewItem = ListViewItem<Issue>;

    #endregion

    public sealed partial class IssueBrowserDialog : Form
    {
        private GoogleCodeProject _project;
        private readonly string _titleFormat;
        private readonly string _foundFormat;
        private Action _aborter;
        private long _totalBytesDownloaded;
        private ReadOnlyCollection<int> _selectedIssues;
        private ReadOnlyCollection<Issue> _roSelectedIssueObjects;
        private readonly List<Issue> _selectedIssueObjects;
        private string _userNamePattern;
        private string _statusPattern;
        private bool _closed;
        private WebClient _updateClient;
        private Func<IWin32Window, DialogResult> _upgrade;
        private readonly ObservableCollection<IssueListViewItem> _issues;
        private readonly ListViewSorter<IssueListViewItem, Issue> _sorter;
        private readonly Font _deadFont;

        public IssueBrowserDialog()
        {
            InitializeComponent();

            _titleFormat = Text;
            _foundFormat = foundLabel.Text;

            var issues = _issues = new ObservableCollection<IssueListViewItem>();
            issues.ItemAdded += (sender, args) => ListIssues(Enumerable.Repeat(args.Item, 1));
            issues.ItemsAdded += (sender, args) => ListIssues(args.Items);
            issues.ItemRemoved += (sender, args) => issueListView.Items.Remove(args.Item);
            issues.Cleared += delegate { issueListView.Items.Clear(); };

            _selectedIssueObjects = new List<Issue>();

            _deadFont = new Font(issueListView.Font, FontStyle.Strikeout);

            _sorter = new ListViewSorter<IssueListViewItem, Issue>(issueListView, 
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
            _sorter.AutoHandle();
            _sorter.SortByColumn(0);

            var searchSourceItems = searchFieldBox.Items;
            searchSourceItems.Add(new MultiFieldIssueSearchSource("All fields", MetaIssue.Properties));

            foreach (IssueField field in Enum.GetValues(typeof(IssueField)))
            {
                searchSourceItems.Add(new SingleFieldIssueSearchSource(field.ToString(), MetaIssue.GetPropertyByField(field),
                    field == IssueField.Summary
                    || field == IssueField.Id
                    || field == IssueField.Stars
                    ? SearchableStringSourceCharacteristics.None
                    : SearchableStringSourceCharacteristics.Predefined));
            }

            searchFieldBox.SelectedIndex = 0;

            searchBox.EnableShortcutToSelectAllText();

            _updateClient = new WebClient();

            includeClosedCheckBox.DataBindings.Add("Enabled", refreshButton, "Enabled");

            UpdateControlStates();
        }

        public string ProjectName
        {
            get { return Project != null ? Project.Name : string.Empty; }
            set { Project = new GoogleCodeProject(value); }
        }

        internal GoogleCodeProject Project
        {
            get { return _project; }
            set { _project = value; UpdateTitle(); }
        }

        public string UserNamePattern
        {
            get { return _userNamePattern ?? string.Empty; }
            set { _userNamePattern = value; }
        }

        public string StatusPattern
        {
            get { return _statusPattern ?? string.Empty; }
            set { _statusPattern = value; }
        }

        public bool UpdateCheckEnabled { get; set; }

        public IList<int> SelectedIssues
        {
            get
            {
                if (_selectedIssues == null)
                    _selectedIssues = new ReadOnlyCollection<int>(SelectedIssueObjects.Select(issue => issue.Id).ToList());
                
                return _selectedIssues;
            }
        }

        internal IList<Issue> SelectedIssueObjects
        {
            get
            {
                if (_roSelectedIssueObjects == null)
                    _roSelectedIssueObjects = new ReadOnlyCollection<Issue>(_selectedIssueObjects);

                return _roSelectedIssueObjects;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (ProjectName.Length > 0)
            {
                DownloadIssues();
                Project.Load();
            }

            if (UpdateCheckEnabled)
            {
                var updateClient = new WebClient();

                updateClient.DownloadStringCompleted += (sender, args) =>
                {
                    _updateClient = null;

                    if (_closed || args.Cancelled || args.Error != null)
                        return;

                    var updateAction = _upgrade = OnVersionDataDownloaded(args.Result);

                    if (updateAction == null) 
                        return;
                    
                    updateNotifyIcon.Visible = true;
                    updateNotifyIcon.ShowBalloonTip(15 * 1000);
                };

                updateClient.DownloadStringAsync(new Uri("http://gurtle.googlecode.com/svn/www/update.txt"));
                _updateClient = updateClient;
            }

            base.OnLoad(e);
        }

        private static Func<IWin32Window, DialogResult> OnVersionDataDownloaded(string data)
        {
            Debug.Assert(data != null);

            var separators = new[] { ':', '=' };

            var headers = (
                    from line in new StringReader(data).ReadLines()
                    where line.Length > 0 && line[0] != '#'
                    let parts = line.Split(separators, 2)
                    where parts.Length == 2
                    let key = parts[0].Trim()
                    let value = parts[1].Trim()
                    where key.Length > 0 && value.Length > 0
                    let pair = new KeyValuePair<string, string>(key, value)
                    group pair by pair.Key into g
                    select g
                )
                .ToDictionary(g => g.Key, p => p.Last().Value, StringComparer.OrdinalIgnoreCase);

            Version version;

            try
            {
                version = new Version(headers.Find("version"));
                
                //
                // Zero out build and revision if not supplied in the string
                // format, e.g. 2.0 -> 2.0.0.0.
                //

                version = new Version(version.Major, version.Minor, 
                    Math.Max(version.Build, 0), Math.Max(0, version.Revision));
            }
            catch (ArgumentException) { return null; }
            catch (FormatException) { return null; }
            catch (OverflowException) { return null; }

            var href = headers.Find("href").MaskNull();

            if (href.Length == 0 || !Uri.IsWellFormedUriString(href, UriKind.Absolute))
                href = new GoogleCodeProject("gurtle").DownloadsListUrl().ToString();

            var thisVersion = typeof(Plugin).Assembly.GetName().Version;
            if (version <= thisVersion)
                return null;

            return owner =>
            {
                var message = new StringBuilder()
                    .AppendLine("There is a new version of Gurtle available. Would you like to update now?")
                    .AppendLine()
                    .Append("Your version: ").Append(thisVersion).AppendLine()
                    .Append("New version: ").Append(version).AppendLine()
                    .ToString();

                var reply = MessageBox.Show(owner, message,
                    "Update Notice", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (reply == DialogResult.Yes)
                    Process.Start(href);

                return reply;
            };
        }

        private void UpdateNotifyIcon_Click(object sender, EventArgs e)
        {
            Debug.Assert(_upgrade != null);

            var reply = _upgrade(this);

            if (reply == DialogResult.Cancel)
                return;

            updateNotifyIcon.Visible = false;

            if (reply == DialogResult.Yes)
                Close();
        }

        private void DownloadIssues()
        {
            Debug.Assert(_aborter == null);

            refreshButton.Enabled = false;
            workStatus.Visible = true;
            statusLabel.Text = "Downloading\x2026";

            _aborter = DownloadIssues(ProjectName, 0, includeClosedCheckBox.Checked,
                                      OnIssuesDownloaded, 
                                      OnUpdateProgress, 
                                      OnDownloadComplete);
        }

        private void OnDownloadComplete(bool cancelled, Exception e)
        {
            if (_closed)
                return; // orphaned notification

            _aborter = null;
            refreshButton.Enabled = true;
            workStatus.Visible = false;

            if (cancelled)
            {
                statusLabel.Text = "Download aborted";
                return;
            }

            if (e != null)
            {
                statusLabel.Text = "Error downloading";
                MessageBox.Show(this, e.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            statusLabel.Text = string.Format("{0} issue(s) downloaded", _issues.Count.ToString("N0"));
            UpdateTitle();
        }

        private void OnUpdateProgress(DownloadProgressChangedEventArgs args)
        {
            if (_closed)
                return; // orphaned notification

            _totalBytesDownloaded += args.BytesReceived;

            statusLabel.Text = string.Format("Downloading\x2026{0} transferred",
                ByteSizeFormatter.StrFormatByteSize(_totalBytesDownloaded));
            
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            Text = string.Format(_titleFormat, ProjectName, _issues.Count.ToString("N0"));
        }

        private IEnumerable<Issue> GetSelectedIssuesFromListView()
        {
            return GetSelectedIssuesFromListView(null);
        }

        private IEnumerable<Issue> GetSelectedIssuesFromListView(Func<ListView, IEnumerable> itemsSelector)
        {
            return from IssueListViewItem item 
                   in (itemsSelector != null ? itemsSelector(issueListView) : issueListView.CheckedItems)
                   select item.Tag;
        }

        private void IssueListView_DoubleClick(object sender, EventArgs e)
        {
            AcceptButton.PerformClick();
        }

        private void DetailButton_Click(object sender, EventArgs e)
        {
            var issue = GetSelectedIssuesFromListView(lv => lv.SelectedItems).FirstOrDefault();
            if (issue != null)
                ShowIssueDetails(issue);
        }

        private void ShowIssueDetails(Issue issue)
        {
            Debug.Assert(issue != null);
            Process.Start(Project.IssueDetailUrl(issue.Id).ToString());
        }

        private void IssueListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateControlStates();
        }

        private void IssueListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControlStates();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            issueListView.Items.Clear();
            ListIssues(_issues);
        }

        private void SearchFieldBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var provider = (ISearchSourceStringProvider<Issue>)searchFieldBox.SelectedItem;
            if (provider == null)
                return;

            var definitions = searchBox.Items;
            definitions.Clear();

            var isPredefined = SearchableStringSourceCharacteristics.Predefined == (
                provider.SourceCharacteristics & SearchableStringSourceCharacteristics.Predefined);

            if (!isPredefined)
                return;

            // TODO: Update definitions if issues are still being downloaded

            definitions.AddRange(_issues
                .Select(lvi => provider.ToSearchableString(lvi.Tag))
                .Distinct(StringComparer.CurrentCultureIgnoreCase).ToArray());
        }

        private void UpdateControlStates()
        {
            detailButton.Enabled = issueListView.SelectedItems.Count == 1;
            okButton.Enabled = issueListView.CheckedItems.Count > 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            Debug.Assert(!_closed);

            Release(ref _aborter, a => a());
            Release(ref _updateClient, wc => wc.CancelAsync());
            
            if (Project != null)
                Project.CancelLoad();

            _closed = true;

            base.OnClosed(e);
        }

        private static void Release<T>(ref T member, Action<T> free) where T : class
        {
            Debug.Assert(free != null);

            var local = member;
            if (local == null)
                return;
            member = null;
            free(local);
        }
        
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            if (DialogResult != DialogResult.OK || e.Cancel)
                return;

            var selectedIssues = _issues.Where(lvi => lvi.Checked)
                                        .Select(lvi => lvi.Tag)
                                        .ToArray();

            //
            // If the user has selected unowned or closed issues then
            // provide a warning and confirm the intention.
            //

            var warnIssues = selectedIssues.Where(issue => !issue.HasOwner 
                                                        || Project.IsClosedStatus(issue.Status));
            if (warnIssues.Any())
            {
                var message = string.Format(
                    "You selected one or more unowned or closed issues ({0}). "
                    + "Normally you should only select open issues with owners assigned. "
                    + "Proceed anyway?",
                    string.Join(", ", warnIssues.OrderBy(issue => issue.Id)
                                                .Select(issue => "#" + issue.Id).ToArray()));
                
                var reply = MessageBox.Show(this, message, "Unowned/Closed Issues Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, 
                    /* no */ MessageBoxDefaultButton.Button2);

                if (reply == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

            }

            _selectedIssueObjects.AddRange(selectedIssues);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            _issues.Clear();            
            OnIssuesDownloaded(Enumerable.Empty<Issue>());
            UpdateTitle();
            DownloadIssues();
        }

        private bool OnIssuesDownloaded(IEnumerable<Issue> issues)
        {
            Debug.Assert(issues != null);

            if (_closed)
                return false; // orphaned notification

            if (UserNamePattern.Length > 0)
                issues = issues.Where(issue => Regex.IsMatch(issue.Owner, UserNamePattern));

            if (StatusPattern.Length > 0)
                issues = issues.Where(issue => Regex.IsMatch(issue.Status, StatusPattern));

            var items = issues.Select(issue =>
                {
                    var id = issue.Id.ToString(CultureInfo.InvariantCulture);

                    var item = new IssueListViewItem(id)
                    {
                        Tag = issue,
                        UseItemStyleForSubItems = true
                    };

                    if (Project.IsClosedStatus(issue.Status))
                    {
                        item.ForeColor = SystemColors.GrayText;
                        item.Font = _deadFont;
                    }
                    else if (!issue.HasOwner)
                    {
                        item.ForeColor = SystemColors.GrayText;
                    }

                    var subItems = item.SubItems;
                    subItems.Add(issue.Type);
                    subItems.Add(issue.Status);
                    subItems.Add(issue.Priority);
                    subItems.Add(issue.Owner);
                    subItems.Add(issue.Summary);

                    return item;
                })
                .ToArray();

            _issues.AddRange(items);

            return items.Length > 0;
        }

        private void ListIssues(IEnumerable<IssueListViewItem> items)
        {
            Debug.Assert(items != null);

            var searchWords = searchBox.Text.Split().Where(s => s.Length > 0);
            if (searchWords.Any())
            {
                var provider = (ISearchSourceStringProvider<Issue>) searchFieldBox.SelectedItem;
                items = from item in items
                        let issue = item.Tag
                        where searchWords.All(word => provider.ToSearchableString(issue).IndexOf(word, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        select item;
            }

            //
            // We need to stop listening to the ItemChecked event because it 
            // is raised for each item added and this has visually noticable 
            // performance implications for the user on large lists.
            //

            ItemCheckedEventHandler onItemChecked = IssueListView_ItemChecked;
            issueListView.ItemChecked -= onItemChecked;

            issueListView.Items.AddRange(items.ToArray());

            //
            // Update control states once and start listening to the 
            // ItemChecked event once more.
            //

            UpdateControlStates();
            issueListView.ItemChecked += onItemChecked;

            foundLabel.Text = string.Format(_foundFormat, issueListView.Items.Count.ToString("N0"));
            foundLabel.Visible = searchWords.Any();
        }

        private static Action DownloadIssues(string project, int start, bool includeClosedIssues,
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

        [ Serializable, Flags ]
        private enum SearchableStringSourceCharacteristics
        {
            None,
            Predefined
        }

        /// <summary>
        /// Represents a provider that yields the string for an object that 
        /// can be used in text-based searches and indexing.
        /// </summary>

        private interface ISearchSourceStringProvider<T>
        {
            SearchableStringSourceCharacteristics SourceCharacteristics { get; }
            string ToSearchableString(T item);
        }

        /// <summary>
        /// Base class for transforming an <see cref="Issue"/> into a 
        /// searchable string.
        /// </summary>

        private abstract class IssueSearchSource : ISearchSourceStringProvider<Issue>
        {
            private readonly string _label;

            protected IssueSearchSource(string label, SearchableStringSourceCharacteristics sourceCharacteristics)
            {
                Debug.Assert(label != null);
                Debug.Assert(label.Length > 0);

                _label = label;
                SourceCharacteristics = sourceCharacteristics;
            }

            public SearchableStringSourceCharacteristics SourceCharacteristics { get; private set; }
            public abstract string ToSearchableString(Issue issue);

            public override string ToString()
            {
                return _label;
            }
        }

        /// <summary>
        /// An <see cref="IssueSearchSource"/> implementation that uses a 
        /// property of an <see cref="Issue"/> as the searchable string.
        /// </summary>

        private sealed class SingleFieldIssueSearchSource : IssueSearchSource
        {
            private readonly IProperty<Issue> _property;

            public SingleFieldIssueSearchSource(string label, IProperty<Issue> property) : 
                this(label, property, SearchableStringSourceCharacteristics.None) {}

            public SingleFieldIssueSearchSource(string label, IProperty<Issue> property, 
                SearchableStringSourceCharacteristics sourceCharacteristics) :
                base(label, sourceCharacteristics)
            {
                Debug.Assert(property != null);
                _property = property;
            }

            public override string ToSearchableString(Issue issue)
            {
                Debug.Assert(issue != null);
                return _property.GetValue(issue).ToString();
            }
        }

        /// <summary>
        /// An <see cref="IssueSearchSource"/> implementation that uses 
        /// concatenates multiple properties of an <see cref="Issue"/> as 
        /// the searchable string.
        /// </summary>

        private sealed class MultiFieldIssueSearchSource : IssueSearchSource
        {
            private readonly IProperty<Issue>[] _properties;

            public MultiFieldIssueSearchSource(string label, IEnumerable<IProperty<Issue>> properties) :
                base(label, SearchableStringSourceCharacteristics.None)
            {
                Debug.Assert(properties != null);
                _properties = properties.Where(p => p != null).ToArray();
            }

            public override string ToSearchableString(Issue issue)
            {
                Debug.Assert(issue != null);

                return _properties.Aggregate(new StringBuilder(),
                    (sb, p) => sb.Append(p.GetValue(issue)).Append(' ')).ToString();
            }
        }
    }
}
