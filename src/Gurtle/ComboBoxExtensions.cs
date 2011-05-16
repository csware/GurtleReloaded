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

    internal static class ComboBoxExtensions
    {
        /// <summary>
        /// This method enables the shortcut <kbd>CTRL+A</kbd> on the given 
        /// <see cref="ComboBox"/>instance to select all its text.
        /// </summary>
        /// Do not call this method more than once for the same 
        /// <see cref="ComboBox"/> instance.
        /// </remarks>

        internal static void EnableShortcutToSelectAllText(this ComboBox comboBox)
        {
            if (comboBox == null) throw new ArgumentNullException("comboBox");

            comboBox.KeyPress += (sender, e) =>
            {
                if (e.KeyChar != 1) return;
                comboBox.SelectAll();
                e.Handled = true;
            };
        }
    }
}
