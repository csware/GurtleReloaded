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
    using System.Collections.ObjectModel;
 
    #endregion

    [ Serializable ]
    internal static class MetaIssue
    {
        public static readonly IProperty<Issue, int> Id = new Property<Issue, int>(issue => issue.Id, (issue, value) => issue.Id = value);
        public static readonly IProperty<Issue, string> Type = new Property<Issue, string>(issue => issue.Type, (issue, value) => issue.Type = value);
        public static readonly IProperty<Issue, string> Status = new Property<Issue, string>(issue => issue.Status, (issue, value) => issue.Status = value);
        public static readonly IProperty<Issue, string> Milestone = new Property<Issue, string>(issue => issue.Milestone, (issue, value) => issue.Milestone = value);
        public static readonly IProperty<Issue, string> Priority = new Property<Issue, string>(issue => issue.Priority, (issue, value) => issue.Priority = value);
        public static readonly IProperty<Issue, string> Stars = new Property<Issue, string>(issue => issue.Stars, (issue, value) => issue.Stars = value);
        public static readonly IProperty<Issue, string> Owner = new Property<Issue, string>(issue => issue.Owner, (issue, value) => issue.Owner = value);
        public static readonly IProperty<Issue, string> Summary = new Property<Issue, string>(issue => issue.Summary, (issue, value) => issue.Summary = value);

        public static ReadOnlyCollection<IProperty<Issue>> Properties { get; private set; }

        static MetaIssue()
        {
            Properties = new ReadOnlyCollection<IProperty<Issue>>(new IProperty<Issue>[]
            {
                Id,
                Type,
                Status,
                Milestone,
                Priority,
                Stars,
                Owner,
                Summary
            });
        }

        public static IProperty<Issue> GetPropertyByField(IssueField field)
        {
            var index = (int) field;

            if (index < 0 || index >= Properties.Count)
                throw new ArgumentOutOfRangeException("field", field, null);
            
            return Properties[index];
        }
    }
}