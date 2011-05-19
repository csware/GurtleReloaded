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

using System.Text;

namespace Gurtle.Providers.GoogleCode
{
    internal sealed class GoogleCodeIssue : Issue
    {
        private string _priority;
        private string _stars;

        public string Priority { get { return _priority ?? string.Empty; } set { _priority = value; } }
        public string Stars { get { return _stars ?? string.Empty; } set { _stars = value; } }

        internal new enum IssueField
        {
            Id,
            Type,
            Status,
            Milestone,
            Priority,
            Stars,
            Owner,
            Summary
        };

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("{ Id = ").Append(Id);
            builder.Append(", Type = ").Append(Type);
            builder.Append(", Status = ").Append(Status);
            builder.Append(", Milestone = ").Append(Milestone);
            builder.Append(", Priority = ").Append(Priority);
            builder.Append(", Stars = ").Append(Stars);
            builder.Append(", Owner = ").Append(Owner);
            builder.Append(", Summary = ").Append(Summary);
            builder.Append(" }");
            return builder.ToString();
        }
    }
}
