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

    [ Serializable ]
    internal sealed class IssueUpdate
    {
        private string _status;
        private string _comment;

        public IssueUpdate(Issue issue) : 
            this(issue, null, null) {}

        public IssueUpdate(Issue issue, string status, string comment)
        {
            if (issue == null) throw new ArgumentNullException("issue");

            Issue = issue;
            _status = status;
            _comment = comment;
        }

        public Issue Issue { get; private set; }

        public string Status
        {
            get { return _status ?? string.Empty; }
            set { _status = value; }
        }

        public string Comment
        {
            get { return _comment ?? string.Empty; }
            set { _comment = value; }
        }
    }
}
