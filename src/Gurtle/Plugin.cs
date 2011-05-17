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

    using Gurtle.Providers;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using Interop.BugTraqProvider;

    #endregion

    [ComVisible(true)]
#if WIN64
    [Guid("A0557FA7-7C95-485b-8F40-31303F762C57")]
#else
    [Guid("91974081-2DC7-4FB1-B3BE-0DE1C8D6CE4E")]
#endif
    [ClassInterface(ClassInterfaceType.None)]
    public sealed class Plugin : IBugTraqProvider2
    {
        private IList<Issue> _issues;
        private IProvider _project;

        public string GetCommitMessage(
            IntPtr hParentWnd,
            string parameters, string commonRoot, string[] pathList,
            string originalMessage)
        {
            return GetCommitMessage(WindowHandleWrapper.TryCreate(hParentWnd), Parameters.Parse(parameters), originalMessage);
        }

        [ComVisible(false)]
        public string GetCommitMessage(
            IWin32Window parentWindow,
            Parameters parameters, string originalMessage)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");

                var project = parameters.Project;
                if (project.Length == 0)
                    throw new ApplicationException("Missing Google Code project specification.");

                _project = parameters.Provider;

                IList<Issue> issues;

                using (var dialog = new IssueBrowserDialog(_project)
                {
                    UserNamePattern = parameters.User,
                    StatusPattern = parameters.Status,
                    UpdateCheckEnabled = true,
                })
                {
                    var settings = Properties.Settings.Default;
                    new WindowSettings(settings, dialog);

                    var reply = dialog.ShowDialog(parentWindow);
                    issues = dialog.SelectedIssueObjects;

                    settings.Save();

                    if (reply != DialogResult.OK || issues.Count == 0)
                        return originalMessage;

                    _issues = issues;
                }

                var message = new StringBuilder(originalMessage);

                if (originalMessage.Length > 0 && !originalMessage.EndsWith("\n"))
                    message.AppendLine();

                foreach (var issue in issues)
                {
                    message
                        .Append("(")
                        .Append(GetIssueTypeAddress(issue.Type)).Append(" issue #")
                        .Append(issue.Id).Append(") : ")
                        .AppendLine(issue.Summary);
                }

                return message.ToString();

        }

        public bool ValidateParameters(IntPtr hParentWnd, string parameters)
        {
            return ValidateParameters(WindowHandleWrapper.TryCreate(hParentWnd), parameters);
        }

        [ComVisible(false)]
        public bool ValidateParameters(IWin32Window parentWindow, string parameters)
        {
            return true; // TODO validation
        }

        public string GetLinkText(IntPtr hParentWnd, string parameters)
        {
            return "Select Issue";
        }

        public string GetCommitMessage2(IntPtr hParentWnd,
            string parameters,
            string commonURL, string commonRoot, string[] pathList,
            string originalMessage, string bugID, out string bugIDOut,
            out string[] revPropNames, out string[] revPropValues)
        {
            bugIDOut = bugID;

            Parameters p = Parameters.Parse(parameters);
            if (p.Project.Length == 0)
            {
                // we can extract the project name from the url, e.g.:
                // https://gurtle.googlecode.com/svn/trunk
                // the project name is the first part of the domain
                Uri url = new Uri(commonURL);
                string projectName = url.Host.Substring(0, url.Host.IndexOf('.'));
                parameters += " project=" + projectName;
            }

            // If no revision properties are to be set, 
            // the plug-in MUST return empty arrays. 

            revPropNames = new string[0];
            revPropValues = new string[0];

            return GetCommitMessage(WindowHandleWrapper.TryCreate(hParentWnd), Parameters.Parse(parameters), originalMessage);
        }

        public string CheckCommit(IntPtr hParentWnd,
            string parameters,
            string commonURL, string commonRoot, string[] pathList,
            string commitMessage)
        {
            return null;
        }

        public string OnCommitFinished(IntPtr hParentWnd,
            string commonRoot, string[] pathList,
            string logMessage, int revision)
        {
            return OnCommitFinished(WindowHandleWrapper.TryCreate(hParentWnd), commonRoot, pathList, logMessage, revision);
        }

        [ComVisible(false)]
        public string OnCommitFinished(IWin32Window parentWindow,
            string commonRoot, string[] pathList,
            string logMessage, int revision)
        {
            var project = _project;
            var issues = _issues;
            OnCommitFinished(parentWindow, revision, project, issues);
            return null;
        }

        private static void OnCommitFinished(IWin32Window parentWindow, int revision, IProvider project, ICollection<Issue> issues)
        {
            if (project == null)
                return;

            if (issues == null || issues.Count == 0)
                return;

            if (!project.CanHandleIssueUpdates())
                return;

            var settings = Properties.Settings.Default;

            var updates = issues.Select(e => new IssueUpdate(e)
            {
                Status = project.ClosedStatuses.FirstOrDefault(),
                Comment = string.Format("{0} in rev. {1}.", GetIssueTypeAddress(e.Type), revision)
            })
            .ToList();

            while (updates.Count > 0)
            {
                using (var dialog = new IssueUpdateDialog(project)
                {
                    Issues = updates,
                    Revision = revision
                })
                {
                    new WindowSettings(settings, dialog);
                    if (DialogResult.OK != dialog.ShowDialog(parentWindow))
                        return;
                }

                if (updates.Count == 0)
                    break;

                var credential = CredentialPrompt.Prompt(parentWindow, project.Name, project.ProjectName + ".gccred");
                if (credential == null)
                    continue;

                using (var form = new WorkProgressForm
                {
                    Text = "Updating Issues",
                    StartWorkOnShow = true,
                })
                {
                    var worker = form.Worker;
                    worker.DoWork += (sender, args) =>
                    {
                        var startCount = updates.Count;
                        while (updates.Count > 0)
                        {
                            if (worker.CancellationPending)
                            {
                                args.Cancel = true;
                                break;
                            }

                            var issue = updates[0];

                            form.ReportProgress(string.Format(
                                @"Updating issue #{0}: {1}",
                                issue.Issue.Id,
                                issue.Issue.Summary));

                            UpdateIssue(project, issue, credential, form.ReportDetailLine);
                            updates.RemoveAt(0);

                            form.ReportProgress((int)((startCount - updates.Count) * 100.0 / startCount));
                        }
                    };

                    form.WorkFailed += delegate
                    {
                        var error = form.Error;
                        foreach (var line in new StringReader(error.ToString()).ReadLines())
                            form.ReportDetailLine(line);
                        ShowErrorBox(form, error);
                    };

                    if (parentWindow == null)
                        form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog(parentWindow);
                }
            }

            settings.Save();
        }

        public bool HasOptions()
        {
            return true;
        }

        public string ShowOptionsDialog(IntPtr hParentWnd, string parameters)
        {
            return ShowOptionsDialog(WindowHandleWrapper.TryCreate(hParentWnd), parameters);
        }

        [ComVisible(false)]
        public static string ShowOptionsDialog(IWin32Window parentWindow, string parameterString)
        {
            Parameters parameters;

            try
            {
                parameters = Parameters.Parse(parameterString);
            }
            catch (ParametersParseException e)
            {
                MessageBox.Show(e.Message, "Invalid Parameters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return parameterString;
            }

            var dialog = new OptionsDialog { Parameters = parameters };
            return dialog.ShowDialog(parentWindow) == DialogResult.OK
                 ? dialog.Parameters.ToString()
                 : parameterString;
        }

        private static string GetIssueTypeAddress(string issueType)
        {
            Debug.Assert(issueType != null);

            switch (issueType.ToLowerInvariant())
            {
                case "defect": return "Fixed";
                case "bug": return "Fixed";
                case "enhancement": return "Closed";
                case "task": return "Closed";
                case "review": return "Closed";
                default: return "Resolved";
            }
        }

        private static void ShowErrorBox(IWin32Window parent, Exception e)
        {
            MessageBox.Show(parent, e.Message,
                e.Source + ": " + e.GetType().Name,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void UpdateIssue(IProvider project, IssueUpdate issue, NetworkCredential credential,
            Action<string> stdout)
        {
            project.UpdateIssue(issue, credential, stdout, stdout);
        }
    }
}
