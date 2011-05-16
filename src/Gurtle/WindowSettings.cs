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
    using System.ComponentModel;
    using System.Configuration;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Saves and restores the location, size and state of a form to and 
    /// from a specific <see cref="SettingsBase" /> object.
    /// </summary>

    [Serializable]
    internal sealed class WindowSettings
    {
        private Form _form;
        private SettingsBase _settings;
        private SettingsProperty _location;
        private SettingsProperty _size;
        private SettingsProperty _windowState;

        public WindowSettings(SettingsBase settings, Form form) :
            this(settings, form, null) {}

        public WindowSettings(SettingsBase settings, Form form, string prefix)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (form == null) throw new ArgumentNullException("form");

            _form = form;
            _settings = settings;

            if (string.IsNullOrEmpty(prefix))
                prefix = form.GetType().Name;

            var properties = settings.Properties;
            _location = properties[prefix + "Location"];
            _size = properties[prefix + "Size"];
            _windowState = properties[prefix + "WindowState"];

            form.Load += OnFormLoad;
            form.Closing += OnFormClosing;
            form.Disposed += OnFormDisposed;
        }

        public Point? Location
        {
            get { return (Point?) _settings[_location.Name]; }
            set { _settings[_location.Name] = value; }
        }

        public Size? Size
        {
            get { return (Size?) _settings[_size.Name]; }
            set { _settings[_size.Name] = value; }
        }

        public FormWindowState? WindowState
        {
            get { return (FormWindowState?) _settings[_windowState.Name]; }
            set { _settings[_windowState.Name] = value; }
        }

        private void OnFormDisposed(object sender, EventArgs e)
        {
            var form = _form;

            _form = null;
            _settings = null;
            _location = _size = _windowState = null;

            Debug.Assert(form != null);

            form.Load -= OnFormLoad;
            form.Closing -= OnFormClosing;
            form.Disposed -= OnFormDisposed;
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            Recall();
        }

        private void OnFormClosing(object sender, CancelEventArgs e)
        {
            Remember();
        }

        public void Remember()
        {
            var form = _form;
            if (form == null)
                throw new InvalidOperationException();

            var bounds = FormBoundsFromWindowState(form);

            if (bounds != null)
            {
                if (!IsOnScreen(bounds.Value.Location, bounds.Value.Size))
                    return;

                Location = bounds.Value.Location;
                Size = bounds.Value.Size;
            }

            WindowState = form.WindowState;
        }

        public void Recall()
        {
            var form = _form;
            if (form == null)
                throw new InvalidOperationException();
            
            var location = Location ?? form.Location;
            var size = Size ?? form.Size;

            if (IsOnScreen(location, size))
            {
                form.Location = location;
                form.Size = size;
            }

            var windowState = WindowState;
            if (windowState != null)
                form.WindowState = WindowState.Value;
        }

        private static bool IsOnScreen(Point location, Size size)
        {
            return IsOnScreen(location) && IsOnScreen(location + size);
        }

        private static bool IsOnScreen(Point location)
        {
            return Screen.AllScreens.Any(screen => screen.WorkingArea.Contains(location));
        }

        private static Rectangle? FormBoundsFromWindowState(Form form)
        {
            Debug.Assert(form != null);

            switch (form.WindowState)
            {
                case FormWindowState.Maximized: return form.RestoreBounds;
                case FormWindowState.Normal: return form.Bounds;
                default: return null; // Don't record anything when closing while minimized.
            }
        }
    }
}