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

        public Uri DownloadsListUrl()
        {
            return FormatUrl("downloads/list");
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
    }
}