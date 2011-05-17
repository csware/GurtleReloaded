using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Gurtle.Providers.GoogleCode
{
    internal sealed class GoogleCodeIssue : Issue
    {
        private string _milestone;
        private string _priority;
        private string _stars;
        private string _owner;

        public string Milestone { get { return _milestone ?? string.Empty; } set { _milestone = value; } }
        public string Priority { get { return _priority ?? string.Empty; } set { _priority = value; } }
        public string Stars { get { return _stars ?? string.Empty; } set { _stars = value; } }
        public string Owner { get { return _owner ?? string.Empty; } set { _owner = value; } }

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

        public new bool HasOwner
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
