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
    using System.Text;
    using System.Runtime.InteropServices;

    #endregion

    /// <summary>
    /// Provides formatting for a numeric value into a string that 
    /// represents the number expressed as a size value in bytes, 
    /// kilobytes, megabytes, or gigabytes, depending on the size.
    /// </summary>
    /// <remarks>
    /// This formatter relies on base operating system services
    /// for formatting and therefore does not consider the 
    /// current UI culture for any text returned in the formatted
    /// string.
    /// </remarks>

    internal sealed class ByteSizeFormatter : CustomFormatter
    {
        public static readonly ByteSizeFormatter Default = new ByteSizeFormatter();

        protected override string FormatImpl(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null) throw new ArgumentNullException("arg");

            return !string.IsNullOrEmpty(format) 
                   ? BaseFormat(format, arg, formatProvider) 
                   : StrFormatByteSize(Convert.ToInt64(arg));
        }

        public static string StrFormatByteSize(long size)
        {
            var buffer = new StringBuilder(20);
            StrFormatByteSize(size, buffer, 20);
            return buffer.ToString();
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long StrFormatByteSize(long size, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);
    }
}
