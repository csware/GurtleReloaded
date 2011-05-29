#region License, Terms and Author(s)
//
// Gurtle - IBugTraqProvider for Google Code
// Copyright (c) 2008, 2009 Atif Aziz. All rights reserved.
// Copyright (c) 2011 Sven Strickroth. All rights reserved.
//
//  Author(s):
//
//      Sven Strickroth, <email@cs-ware.de>
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

    using Gurtle.Providers;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    #endregion

    [Serializable]
    public sealed class Parameters
    {
        private IProvider _provider;
        private string _project;
        private string _status;
        private string _user;
        private bool _noOnCommitFinished = false;
        private string _commitTemplate;

        public static Parameters Parse(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            str = str.Trim();
            if (str.Length == 0)
                return new Parameters();

            var dict = ParsePairs(str).ToDictionary(
                           p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase);

            Parameters parameters;

            try
            {
                parameters = new Parameters
                {
                    _provider = Providers.ProviderFactory.getProvider(dict.TryPop("provider"), dict.TryPop("project")),
                    User = dict.TryPop("user"),
                    Status = dict.TryPop("status"),
                    CommitTemplate = dict.TryPop("commitTemplate"),
                };
                string noCommitFinished = dict.TryPop("noOnCommitFinished");
                if (noCommitFinished != null && (noCommitFinished == "true" || noCommitFinished == "1"))
                {
                    parameters.NoOnCommitFinished = true;
                }
                parameters._project = parameters._provider.ProjectName;
            }
            catch (ArgumentException e)
            {
                throw new ParametersParseException(e.Message, e);
            }

            if (dict.Any())
            {
                throw new ParametersParseException(string.Format(
                    "Parameter '{0}' is unknown.", dict.Keys.First()));
            }

            return parameters;
        }

        public string Project
        {
            get { return _project ?? string.Empty; }

            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Invalid project name.", "value");

                _provider = Providers.ProviderFactory.getProvider(Provider.Name, value);

                _project = value;
            }
        }

        internal IProvider Provider
        {
            set
            {
                _provider = value;
            }
            get { return _provider; }
        }

        public string User
        {
            get { return _user ?? string.Empty; }
            set { _user = value; }
        }

        public string Status
        {
            get { return _status ?? string.Empty; }
            set { _status = value; }
        }

        public bool NoOnCommitFinished
        {
            get { return _noOnCommitFinished; }
            set { _noOnCommitFinished = value; }
        }

        public string CommitTemplate
        {
            get { return _commitTemplate ?? "Fixed issue #%BUGID%: %SUMMARY%"; }
            set { _commitTemplate = value; }
        }

        public override string ToString()
        {
            var list = new List<KeyValuePair<string, string>>();

            list.Add(Pair("provider", Provider.Name));

            list.Add(Pair("project", Project));

            list.Add(Pair("user", User));

            list.Add(Pair("status", Status));

            if (NoOnCommitFinished)
                list.Add(Pair("noOnCommitFinished", "true"));

            list.Add(Pair("commitTemplate", CommitTemplate));

            return string.Join(";",
                list
                .Where(e => e.Value.Length > 0)
                .Select(e => e.Key + "=" + (e.Value.Contains(';') || e.Value.Contains('"') ? '"' + e.Value.Replace("\"", "\\\"") + '"' : e.Value))
                .ToArray());
        }

        private static IEnumerable<KeyValuePair<string, string>> Pairs(params KeyValuePair<string, string>[] pairs)
        {
            return pairs;
        }

        private static KeyValuePair<string, string> Pair(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }

        private static IEnumerable<KeyValuePair<string, string>> ParsePairs(string str)
        {
            Debug.Assert(str != null);

            int lastObjectStart = 0;
            bool inQuote = false;
            bool valueIsInQuote = false;
            bool inKey = true;
            bool lastWasBackslash = false;
            string lastKey = "";
            for (int n = 0; n < str.Length; n++)
            {
                if (inKey)
                {
                    if (str[n] == ';')
                    {
                        if (n - lastObjectStart == 0)
                        {
                            // handle: separator occours twice
                            lastObjectStart++;
                        }
                        else
                        {
                            // separator occours in key name
                            throw new ArgumentException("invalid char ';' in key found.");
                        }
                    }
                    else if (str[n] == '=')
                    {
                        lastKey = str.Substring(lastObjectStart, n - lastObjectStart).Trim();
                        lastObjectStart = n + 1;
                        inKey = false;
                    }
                }
                else
                {
                    if (inQuote)
                    {
                        if (str[n] == '"' && !lastWasBackslash)
                        {
                            inQuote = false;
                            valueIsInQuote = true;
                        }
                        else if (str[n] == '\\')
                        {
                            lastWasBackslash = !lastWasBackslash;
                        }
                        else
                        {
                            lastWasBackslash = false;
                        }
                    }
                    else
                    {
                        if (str[n] == '=')
                        {
                            throw new ArgumentException("invalid char '=' in value found.");
                        }
                        else if (str[n] == '"')
                        {
                            if (n - lastObjectStart > 0)
                            {
                                throw new ArgumentException("invalid char '\"' in value found.");
                            }
                            inQuote = true;
                            lastObjectStart++;
                        }
                        else if (str[n] == ';')
                        {
                            yield return new KeyValuePair<string, string>(lastKey, str.Substring(lastObjectStart, n - lastObjectStart - (valueIsInQuote ? 1 : 0)).Trim());
                            lastObjectStart = n + 1;
                            inKey = true;
                            valueIsInQuote = false;
                            lastKey = "";
                        }
                    }
                }
            }
            if (str.Length - lastObjectStart > 0)
            {
                if (inQuote)
                {
                    throw new ArgumentException("string not closed");
                }
                else
                {
                    yield return new KeyValuePair<string, string>(lastKey, str.Substring(lastObjectStart, str.Length - lastObjectStart - (valueIsInQuote ? 1 : 0)).Trim());
                }
            }
        }
    }
}
