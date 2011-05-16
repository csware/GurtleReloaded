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
    using System.Linq;
    using System.Web.UI;

    #endregion

    /// <summary>
    /// Provides data expression evaluation facilites similar to 
    /// <see cref="System.Web.UI.DataBinder"/> in ASP.NET.
    /// </summary>
    
    internal static class DataBindingExtensions
    {
        public static T DataBind<T>(this object obj, string expression)
        {
            var value = obj.DataBind(expression);
            return (T) (Convert.IsDBNull(value) ? null : value);
        }

        /// <summary>
        /// Evaluates data-binding expressions at run time using an expression
        /// syntax that is similar to C# and Visual Basic for accessing
        /// properties or indexing into collections.
        /// </summary>
        /// <remarks>
        /// This method performs late-bound evaluation, using run-time reflection
        /// and therefore can cause performance less than optimal.
        /// </remarks>

        public static object DataBind(this object obj, string expression)
        {
            //
            // The ASP.NET DataBinder.Eval method does not like an empty or null
            // expression. Rather than making it an unnecessary exception, we
            // turn a nil-expression to mean, "evaluate to obj."
            //

            return string.IsNullOrEmpty(expression) ? obj : DataBinder.Eval(obj, expression);
        }

        public static string DataBind(this object obj, string format, params string[] expressions)
        {
            if (expressions == null) throw new ArgumentNullException("expressions");

            return string.Format(format, expressions.Select(e => obj.DataBind(e)).ToArray());
        }
    }
}