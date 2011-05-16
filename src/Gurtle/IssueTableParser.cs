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
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion

    internal static class IssueTableParser
    {
        public static readonly Regex _csvex = new Regex(
            "(?:^|,)\"((?:\"{2}|[^\"])+)?\"",
            RegexOptions.CultureInvariant
            | RegexOptions.IgnorePatternWhitespace
            | RegexOptions.Compiled);

        public static IEnumerable<Issue> Parse(TextReader reader)
        {
            Debug.Assert(reader != null);

            var firstLine = reader.ReadLine();
            if (string.IsNullOrEmpty(firstLine))
                return Enumerable.Empty<Issue>();

            var headers = ParseValues(firstLine).ToArray();

            var bindings = Enum.GetNames(typeof(IssueField))
                .Select(n => Array.FindIndex(headers, h => n.Equals(h, StringComparison.OrdinalIgnoreCase)))
                .Select(i => (Func<IEnumerable<string>, string>)(values => values.ElementAtOrDefault(i) ?? string.Empty))
                .ToArray();

            return //...
                from line in reader.ReadLines()
                let values = ParseValues(line).ToArray()
                let id = ParseInteger(bindings[(int)IssueField.Id](values), CultureInfo.InvariantCulture)
                where id != null && id.Value > 0
                let issue = new Issue 
                {
                    Id = id.Value, 
                    Type = bindings[(int)IssueField.Type](values), 
                    Status = bindings[(int)IssueField.Status](values), 
                    Milestone = bindings[(int)IssueField.Milestone](values), 
                    Priority = bindings[(int)IssueField.Priority](values), 
                    Stars = bindings[(int)IssueField.Stars](values), 
                    Owner = bindings[(int)IssueField.Owner](values), 
                    Summary = bindings[(int)IssueField.Summary](values)
                }
                select issue;
        }

        private static IEnumerable<string> ParseValues(string line)
        {
            Debug.Assert(line != null);

            return from Match m in _csvex.Matches(line)
                   select m.Groups[1].Value.Replace("\"\"", "\"");
        }

        private static int? ParseInteger(string s, IFormatProvider provider)
        {
            int result;
            return int.TryParse(s, NumberStyles.Integer, provider, out result) ? result : (int?) null;
        }
    }
}
