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
    using System;
    using System.Linq;
    using System.Text;

    [Serializable]
    internal sealed class Issue
    {
        private string _type;
        private string _status;
        private string _milestone;
        private string _priority;
        private string _stars;
        private string _owner;
        private string _summary;

        public int Id { get; set; }
        public string Type { get { return _type ?? string.Empty; } set { _type = value; } }
        public string Status { get { return _status ?? string.Empty; } set { _status = value; } }
        public string Milestone { get { return _milestone ?? string.Empty; } set { _milestone = value; } }
        public string Priority { get { return _priority ?? string.Empty; } set { _priority = value; } }
        public string Stars { get { return _stars ?? string.Empty; } set { _stars = value; } }
        public string Owner { get { return _owner ?? string.Empty; } set { _owner = value; } }
        public string Summary { get { return _summary ?? string.Empty; } set { _summary = value; } }

        public bool HasOwner
        {
            get
            {
                var owner = this.Owner;
                return owner.Length > 0 && !owner.All(ch => ch == '-');
            }
        }

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