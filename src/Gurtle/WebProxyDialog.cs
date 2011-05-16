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
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Forms;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;

    #endregion

    public partial class WebProxyDialog : Form
    {
        public WebProxyDialog()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
                OnOK();

            base.OnClosing(e);
        }

        private void OnOK()
        {
            // TODO Save settings
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            // TODO Test settings
        }
    }
}
