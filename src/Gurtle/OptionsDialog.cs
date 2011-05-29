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
namespace Gurtle
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Windows.Forms;
    using Properties;
    using Gurtle.Providers;

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
                _providers.Items.AddRange(Gurtle.Providers.ProviderFactory.getProviders());
                if (_parameters.Provider != null)
                {
                    _providers.SelectedIndex = _providers.Items.IndexOf(_parameters.Provider.Name);
                    _okButton.Enabled = _parameters.Project != null && _parameters.Project.Length > 0;
                }
                _handleOnCommitFinished.Checked = !_parameters.NoOnCommitFinished;
                _commitTemplate.Text = _parameters.CommitTemplate;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (DialogResult != DialogResult.OK)
                return;

            Parameters.NoOnCommitFinished = !_handleOnCommitFinished.Checked;
            _parameters.CommitTemplate = _commitTemplate.Text;
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

        private void _providers_SelectedIndexChanged(object sender, EventArgs e)
        {
            _configureProviderButton.Enabled = _providers.SelectedIndex >= 0;
        }

        private void _configureProviderButton_Click(object sender, EventArgs e)
        {
            IProvider provider = ProviderFactory.getProvider((string)_providers.Text);
            Parameters.Provider = provider;
            if (provider.ShowOptions(Parameters) == DialogResult.OK)
            {
                _okButton.Enabled = true;
            }
        }
    }
}
