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
    using System.Windows.Forms;

    #endregion

    public sealed partial class CredentialsDialog : Form
    {
        private string _userName;
        private string _realm;
        private string _password;
        private readonly string _titleFormat;
        private readonly string _leadFormat;

        public CredentialsDialog()
        {
            InitializeComponent();
            _titleFormat = Text;
            _leadFormat = leadLabel.Text;
        }

        public string UserName
        {
            get { return _userName ?? string.Empty; }
            set { _userName = value; }
        }

        public string Password
        {
            get { return _password ?? string.Empty; }
            set { _password = value; }
        }

        public string Realm
        {
            get { return _realm ?? string.Empty; }
            set { _realm = value; }
        }

        public bool SavePassword { get; set; }

        protected override void OnShown(EventArgs e)
        {
            Text = string.Format(_titleFormat, Realm);
            leadLabel.Text = string.Format(_leadFormat, Realm);
            userNameBox.Text = UserName;
            passwordBox.Text = Password;
            saveBox.Checked = SavePassword;
            ValidateInputs();
            base.OnShown(e);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                UserName = userNameBox.Text;
                Password = passwordBox.Text;
                SavePassword = saveBox.Checked;
            }
            base.OnClosing(e);
        }

        private void UserName_Changed(object sender, EventArgs e)
        {
            ValidateInputs();
        }

        private void Password_Changed(object sender, EventArgs e)
        {
            ValidateInputs();
        }

        private void ValidateInputs()
        {
            okButton.Enabled = userNameBox.Text.Trim().Length > 0 
                               && passwordBox.Text.Trim().Length > 0;
        }
    }
}
