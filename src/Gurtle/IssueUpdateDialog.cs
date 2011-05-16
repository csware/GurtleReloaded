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
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    #endregion

    public sealed partial class IssueUpdateDialog : Form
    {
        private GoogleCodeProject _project;
        private readonly string _titleFormat;
        private int _revision;

        public IssueUpdateDialog()
        {
            InitializeComponent();

            _titleFormat = Text;
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

        private void UpdateTitle()
        {
            Text = string.Format(_titleFormat, Revision, ProjectName);
        }

        internal IList<IssueUpdate> Issues { get; set; }

        public int Revision
        {
            get { return _revision; }
            set { _revision = value; UpdateTitle(); }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (Project == null)
                throw new InvalidOperationException();

            var issues = Issues;

            if (issues != null && issues.Count > 0)
            {
                foreach (var issue in issues)
                {
                    var tab = new TabPage("Issue #" + issue.Issue.Id)
                    {
                        ToolTipText = issue.Issue.Summary,
                        Tag = issue
                    };
                    var page = CreateIssuePage(issue);
                    page.SkipChanged += delegate { tab.ImageIndex = page.Skip ? 0 : -1; };
                    tab.Controls.Add(page);
                    tabs.TabPages.Add(tab);
                }
            }

            if (!Project.IsLoaded)
            {
                Project.Loaded += OnProjectLoaded;
                Project.Load();
            }
            else
            {
                OnProjectLoadedImpl();
            }

            base.OnLoad(e);
        }

        private void OnProjectLoaded(object sender, EventArgs e)
        {
            Project.Loaded -= OnProjectLoaded;
            OnProjectLoadedImpl();
        }

        private void OnProjectLoadedImpl()
        {
            foreach (var page in GetPages())
                page.LoadStatusOptions(Project.ClosedStatuses);
        }

        private IEnumerable<IssueUpdatePage> GetPages()
        {
            return from TabPage tab in tabs.TabPages
                   select (IssueUpdatePage) tab.Controls[0];
        }

        private IssueUpdatePage CreateIssuePage(IssueUpdate issue) 
        {
            Debug.Assert(issue != null);

            var page = new IssueUpdatePage
            {
                Dock = DockStyle.Fill,
                Summary = issue.Issue.Summary,
                Comment = issue.Comment,
                Status = issue.Status,
                Url = _project.IssueDetailUrl(issue.Issue.Id)
            };

            page.RevisionClicked += (sender, args) => Process.Start(_project.RevisionDetailUrl(args.Revision).ToString());
            
            return page;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (Project != null)
                Project.CancelLoad();

            if (DialogResult == DialogResult.OK)
                OnOK();

            base.OnClosed(e);
        }

        private void OnOK()
        {
            var pages = GetPages().ToArray();
            for (var i = 0; i < pages.Length; i++)
            {
                var page = pages[i];
                var issue = Issues[i];
                issue.Status = page.Status;
                issue.Comment = page.Comment;
            }
            for (var i = pages.Length - 1; i >= 0; i--)
            {
                if (pages[i].Skip)
                    Issues.RemoveAt(i);
            }
        }
    }
}
