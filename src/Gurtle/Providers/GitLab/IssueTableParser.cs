#region License, Terms and Author(s)
//
// Gurtle - IBugTraqProvider for Google Code
// Copyright (c) 2011 Sven Strickroth. All rights reserved.
//
//  Author(s):
//
//      Sven Strickroth <email@cs-ware.de>
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

namespace Gurtle.Providers.GitLab
{
    #region Imports

    using LitJson;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    internal static class IssueTableParser
    {
        public static IEnumerable<Issue> Parse(string full)
        {
            if (string.IsNullOrEmpty(full))
                return Enumerable.Empty<Issue>();

            LinkedList<Issue> list = new LinkedList<Issue>();

            JsonData data = JsonMapper.ToObject(full);
            for (int i = 0; i < data.Count; i++)
            {
                Issue issue = new Issue();
                issue.Id = (int)data[i]["iid"];
                issue.Summary = (string)data[i]["title"];
                issue.Status = (string)data[i]["state"];
                //issue.Owner = (string)data[i]["assignee"];
                /*if (data[i]["milestone"] != null)
                {
                    issue.Milestone = (string)data[i]["milestone"]["title"];
                }*/
                list.AddLast(issue);
            }
            return list;
        }
    }
}
