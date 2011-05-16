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
    using System.Collections;

    internal sealed class DelegatingComparer<T> : IComparer
    {
        private readonly Comparison<T> _comparison;

        public DelegatingComparer(Comparison<T> comparison)
        {
            if (comparison == null) throw new ArgumentNullException("comparison");
            _comparison = comparison;
        }

        public int Compare(object x, object y)
        {
            return x == null && y == null ? 0 
                 : x == null ? -1 
                 : y == null ? 1 
                 : _comparison((T) x, (T) y);
        }
    }
}