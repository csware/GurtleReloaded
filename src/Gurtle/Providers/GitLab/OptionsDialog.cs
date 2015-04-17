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
namespace Gurtle.Providers.GitLab
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Windows.Forms;
    using Properties;

    #endregion

    public sealed partial class OptionsDialog : Form
    {
        private Parameters _parameters;

        public OptionsDialog()
        {
            InitializeComponent();
        }

        public Parameters Parameters
        {
            get
            {
                if (_parameters == null)
                    _parameters = new Parameters();
                return _parameters;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _parameters = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            _projectNameBox.Text = Parameters.Project;
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (DialogResult != DialogResult.OK)
                return;

            Parameters.Project = _projectNameBox.Text;    
        }

        private void ProjectNameBox_TextChanged(object sender, EventArgs e)
        {
            _okButton.Enabled = new GitLabRepository().IsValidProjectName(_projectNameBox.Text);
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            try
            {
                var projectName = _projectNameBox.Text;
                var url = new GitLabRepository(projectName).Url;
                using (CurrentCursorScope.EnterWait())
                    new WebClient().DownloadData(url);
                var message = string.Format("The GitLab repository '{0}' appears valid and reachable at {1}.", projectName, url);
                MessageBox.Show(message, "Test Passed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (WebException we)
            {
                MessageBox.Show(we.Message, "Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetSettings_Click(object sender, EventArgs e)
        {
            var reply = MessageBox.Show(this, 
                            "Reset all settings to their defaults?", "Reset Settings", 
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (DialogResult.Yes != reply)
                return;

            Settings.Default.Reset();
        }
    }
}
