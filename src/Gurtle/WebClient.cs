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
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Mime;
    using System.Text;

    #endregion

    internal sealed class WebClient : System.Net.WebClient
    {
        private static readonly string _defaultUserAgentString;

        public new event EventHandler<DownloadStringCompletedEventArgs> DownloadStringCompleted;

        private bool _downloadingString;

        static WebClient()
        {
            var assemblyName = typeof(WebClient).Assembly.GetName();
            _defaultUserAgentString = assemblyName.Name.ToLowerInvariant()
                                    + "/" + assemblyName.Version.ToString(2);
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            if (address == null) throw new ArgumentNullException("address");

            var request = base.GetWebRequest(address);

            var httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                //
                // If this is an HTTP request and the user agent string
                // has not been set then use a reasonable default that
                // identifies this client.
                //

                if (string.IsNullOrEmpty(httpRequest.UserAgent))
                    httpRequest.UserAgent = DefaultUserAgentString;
            }

            return request;
        }

        public static string DefaultUserAgentString
        {
            get { return _defaultUserAgentString; }
        }

        public new void DownloadStringAsync(Uri address)
        {
            Debug.Assert(address.IsAbsoluteUri);

            try
            {
                _downloadingString = true;
                DownloadDataAsync(address);
            }
            catch (Exception)
            {
                _downloadingString = false;
                throw;
            }
        }

        protected override void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
        {
            if (_downloadingString)
            {
                _downloadingString = false;
                
                var result = !e.Cancelled && e.Error == null 
                           ? GetDownloadEncoding().GetString(e.Result) 
                           : null;
                
                var handler = DownloadStringCompleted;
                if (handler != null)
                    DownloadStringCompleted(this, new DownloadStringCompletedEventArgs(result, e.Error, e.Cancelled, e.UserState));
            }
            else
            {
                base.OnDownloadDataCompleted(e);
            }
        }

        private Encoding GetDownloadEncoding() 
        {
            var header = ResponseHeaders[HttpResponseHeader.ContentType];
            if (string.IsNullOrEmpty(header))
                return Encoding;

            ContentType contentType;
            try
            {
                contentType = new ContentType(header);
            }
            catch (FormatException)
            {
                return Encoding;
            }

            try
            {
                return Encoding.GetEncoding(contentType.CharSet);
            }
            catch (ArgumentException)
            {
                return Encoding;
            }
        }
    }
}
