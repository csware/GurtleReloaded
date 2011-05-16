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
    using System.Windows.Forms;

    #endregion

    internal sealed class ListViewSorter<LVI, T> where LVI : ListViewItem
    {
        private static readonly string[] _orderIndicators = new[] { "+", "-" };

        private readonly Func<LVI, T> _itemSelector;
        private readonly Func<T, IComparable>[] _subSelectors;
        private SortOrder _order;
        private int _index;
        private string _baseText;
        
        public ListViewSorter(ListView listView,
            Func<LVI, T> itemSelector,
            IEnumerable<Func<T, IComparable>> subSelectors)
        {
            Debug.Assert(listView != null);
            Debug.Assert(itemSelector != null);
            Debug.Assert(subSelectors != null);

            ListView = listView;
            _itemSelector = itemSelector;
            _subSelectors = subSelectors.ToArray();
            _index = -1;

            Debug.Assert(_subSelectors.Length == listView.Columns.Count);
        }

        public ListView ListView { get; private set; }

        public void AutoHandle()
        {
            ListView.ColumnClick += (sender, e) => SortByColumn(e.Column);
        }

        public void SortByColumn(int index)
        {
            var changed = _index != index;
            var columns = ListView.Columns;
            var newColumn = columns[index];

            _order = changed
                   ? SortOrder.Ascending 
                   : _order == SortOrder.Ascending 
                     ? SortOrder.Descending 
                     : SortOrder.Ascending;

            if (changed)
            {
                if (_index >= 0)
                    columns[_index].Text = _baseText;

                _baseText = newColumn.Text;
            }

            _index = index;
            newColumn.Text = _baseText + _orderIndicators[_order - SortOrder.Ascending];

            var subSelector = _subSelectors[index];
            Comparison<LVI> comparison = (x, y) => subSelector(_itemSelector(x)).CompareTo(subSelector(_itemSelector(y)));
                
            ListView.ListViewItemSorter = new DelegatingComparer<LVI>(
                _order == SortOrder.Descending ? (x, y) => comparison(x, y) * -1 : comparison);
        }
    }
}
