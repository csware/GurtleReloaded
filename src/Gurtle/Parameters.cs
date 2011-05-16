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
    using System.Linq;    

    #endregion

    [ Serializable ]
    public sealed class Parameters
    {
        private string _project;
        private string _status;
        private string _user;

        public static Parameters Parse(string str)
        {
            if (str == null) throw new ArgumentNullException("str");

            str = str.Trim();
            if (str.Length == 0)
                return new Parameters();

            var dict = ParsePairs(str.Split(';')).ToDictionary(
                           p => p.Key, p => p.Value, StringComparer.OrdinalIgnoreCase);

            Parameters parameters;
            
            try
            {
                parameters = new Parameters
                {
                    Project = dict.TryPop("project"),
                    User = dict.TryPop("user"),
                    Status = dict.TryPop("status"),
                };
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
                if (!string.IsNullOrEmpty(value) && !GoogleCodeProject.IsValidProjectName(value))
                    throw new ArgumentException("Invalid project name.", "value");

                _project = value;
            }
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

        public override string ToString()
        {
            var list = new List<KeyValuePair<string, string>>();

            if (Project.Length > 0)
                list.Add(Pair("project", Project));
            
            if (User.Length > 0)
                list.Add(Pair("user", User));
            
            if (Status.Length > 0)
                list.Add(Pair("status", Status));

            return string.Join(";", 
                Pairs(
                    Pair("project", Project), 
                    Pair("user", User), 
                    Pair("status", Status))
                .Where(e => e.Value.Length > 0)
                .Select(e => e.Key + "=" + e.Value)
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

        private static IEnumerable<KeyValuePair<string, string>> ParsePairs(IEnumerable<string> parameters)
        {
            Debug.Assert(parameters != null);

            foreach (var parameter in parameters)
            {
                var pair = parameter.Split(new[] { '=' }, 2);
                var key = pair[0].Trim();
                var value = pair.Length > 1 ? pair[1].Trim() : string.Empty;
                yield return new KeyValuePair<string, string>(key, value);
            }
        }
    }
}
