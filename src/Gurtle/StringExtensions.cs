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
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Web;

    #endregion

    internal static class StringExtensions
    {
        /// <summary>
        /// Masks an empty string with a given mask such that the result
        /// is never an empty string. If the input string is null or
        /// empty then it is masked, otherwise the original is returned.
        /// </summary>
        /// <remarks>
        /// Use this method to guarantee that you never get an empty
        /// string. Note that if the mask itself is an empty string then
        /// this method could yield an empty string!
        /// </remarks>

        [DebuggerStepThrough]
        public static string MaskEmpty(this string str, string mask)
        {
            return !string.IsNullOrEmpty(str) ? str : mask;
        }

        [DebuggerStepThrough]
        public static string MaskEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str) ? str : string.Empty;
        }

        /// <summary>
        /// Masks the null value. If the given string is null then the 
        /// result is an empty string otherwise it is the original string.
        /// </summary>
        /// <remarks>
        /// Use this method to guarantee that you never get a null string
        /// and where the distinction between a null and an empty string
        /// is irrelevant.
        /// </remarks>

        [DebuggerStepThrough]
        public static string MaskNull(this string str)
        {
            return str ?? string.Empty;
        }

        [DebuggerStepThrough]
        public static string MaskNull(this string str, string mask)
        {
            return str ?? mask.MaskNull();
        }

        /// <summary>
        /// Returns a section of a string.
        /// </summary>
        /// <remarks>
        /// The slice method copies up to, but not including, the element
        /// indicated by end. If start is negative, it is treated as length +
        /// start where length is the length of the string. If end is negative,
        /// it is treated as length + end where length is the length of the
        /// string. If end occurs before start, no characters are copied to the
        /// new string.
        /// </remarks>

        public static string Slice(this string str, int? start, int? end)
        {
            return Slice(str, start ?? 0, end ?? str.MaskNull().Length);
        }

        public static string Slice(this string str, int start)
        {
            return Slice(str, start, null);
        }

        public static string Slice(this string str, int start, int end)
        {
            var length = str.MaskNull().Length;

            if (start < 0)
            {
                start = length + start;
                if (start < 0)
                    start = 0;
            }
            else
            {
                if (start > length)
                    start = length;
            }

            if (end < 0)
            {
                end = length + end;
                if (end < 0)
                    end = 0;
            }
            else
            {
                if (end > length)
                    end = length;
            }

            var sliceLength = end - start;

            return sliceLength > 0 ?
                str.Substring(start, sliceLength) : string.Empty;
        }

        public static string FormatWith(this string format, params object[] args)
        {
            return format.FormatWith(null, null, args);
        }

        public static string FormatWith(this string format, IFormatProvider provider, params object[] args)
        {
            return format.FormatWith(provider, null, args);
        }

        public static string FormatWith(this string format, Func<string, object[], IFormatProvider, string> binder, params object[] args)
        {
            return format.FormatWith(null, binder, args);
        }

        public static string FormatWith(this string format,
            IFormatProvider provider, Func<string, object[], IFormatProvider, string> binder, params object[] args)
        {
            return format.FormatWithImpl(provider, binder ?? FormatTokenBinder, args);
        }

        private static string FormatWithImpl(this string format,
            IFormatProvider provider, Func<string, object[], IFormatProvider, string> binder, params object[] args)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            Debug.Assert(binder != null);

            var result = new StringBuilder(format.Length * 2);
            var token = new StringBuilder();

            var e = format.GetEnumerator();
            while (e.MoveNext())
            {
                var ch = e.Current;
                if (ch == '{')
                {
                    while (true)
                    {
                        if (!e.MoveNext())
                            throw new FormatException();

                        ch = e.Current;
                        if (ch == '}')
                        {
                            if (token.Length == 0)
                                throw new FormatException();

                            result.Append(binder(token.ToString(), args, provider));
                            token.Length = 0;
                            break;
                        }
                        if (ch == '{')
                        {
                            result.Append(ch);
                            break;
                        }
                        token.Append(ch);
                    }
                }
                else if (ch == '}')
                {
                    if (!e.MoveNext() || e.Current != '}')
                        throw new FormatException();
                    result.Append('}');
                }
                else
                {
                    result.Append(ch);
                }
            }

            return result.ToString();
        }

        private static string FormatTokenBinder(string token, object[] args, IFormatProvider provider)
        {
            Debug.Assert(token != null);

            var source = args[0];
            var dotIndex = token.IndexOf('.');
            int sourceIndex;
            if (dotIndex > 0 && int.TryParse(token.Substring(0, dotIndex), NumberStyles.None, CultureInfo.InvariantCulture, out sourceIndex))
            {
                source = args[sourceIndex];
                token = token.Substring(dotIndex + 1);
            }

            var format = string.Empty;

            var colonIndex = token.IndexOf(':');
            if (colonIndex > 0)
            {
                format = "{0:" + token.Substring(colonIndex + 1) + "}";
                token = token.Substring(0, colonIndex);
            }

            if (int.TryParse(token, NumberStyles.None, CultureInfo.InvariantCulture, out sourceIndex))
            {
                source = args[sourceIndex];
                token = null;
            }

            object result;

            try
            {
                result = source.DataBind(token) ?? string.Empty;
            }
            catch (HttpException e)
            {
                throw new FormatException(e.Message, e);
            }

            return !string.IsNullOrEmpty(format)
                 ? string.Format(provider, format, result)
                 : result.ToString();
        }

        /// <summary>
        /// Splits a string into a key and a value part using a specified 
        /// character to separate the two.
        /// </summary>
        /// <remarks>
        /// The key or value of the resulting pair is never <c>null</c>.
        /// </remarks>

        public static KeyValuePair<string, string> SplitPair(this string str, char separator)
        {
            if (str == null) throw new ArgumentNullException("str");

            return SplitPair(str, str.IndexOf(separator), 1);
        }

        /// <summary>
        /// Splits a string into a key and a value part using any of a 
        /// specified set of characters to separate the two.
        /// </summary>
        /// <remarks>
        /// The key or value of the resulting pair is never <c>null</c>.
        /// </remarks>

        public static KeyValuePair<string, string> SplitPair(this string str, params char[] separators)
        {
            if (str == null) throw new ArgumentNullException("str");

            return separators == null || separators.Length == 0
                 ? new KeyValuePair<string, string>(str, string.Empty)
                 : SplitPair(str, str.IndexOfAny(separators), 1);
        }

        /// <summary>
        /// Splits a string into a key and a value part by removing a
        /// portion of the string.
        /// </summary>
        /// <remarks>
        /// The key or value of the resulting pair is never <c>null</c>.
        /// </remarks>

        public static KeyValuePair<string, string> SplitPair(this string str, int index, int count)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (count <= 0) throw new ArgumentOutOfRangeException("count", count, null);

            return new KeyValuePair<string, string>(
                /* key   */ index < 0 ? str : str.Substring(0, index),
                /* value */ index < 0 || index + 1 >= str.Length
                            ? string.Empty
                            : str.Substring(index + count));
        }
    }
}
