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
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Windows.Forms;

    #endregion

    [ Serializable ]
    internal class ListViewItem<T> : ListViewItem
    {
        public ListViewItem() {}
        public ListViewItem(string text) : base(text) {}
        public ListViewItem(string text, int imageIndex) : base(text, imageIndex) {}
        public ListViewItem(string[] items) : base(items) {}
        public ListViewItem(string[] items, int imageIndex) : base(items, imageIndex) {}
        public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font) : base(items, imageIndex, foreColor, backColor, font) {}
        public ListViewItem(ListViewSubItem[] subItems, int imageIndex) : base(subItems, imageIndex) {}
        public ListViewItem(ListViewGroup group) : base(group) {}
        public ListViewItem(string text, ListViewGroup group) : base(text, group) {}
        public ListViewItem(string text, int imageIndex, ListViewGroup group) : base(text, imageIndex, group) {}
        public ListViewItem(string[] items, ListViewGroup group) : base(items, group) {}
        public ListViewItem(string[] items, int imageIndex, ListViewGroup group) : base(items, imageIndex, group) {}
        public ListViewItem(string[] items, int imageIndex, Color foreColor, Color backColor, Font font, ListViewGroup group) : base(items, imageIndex, foreColor, backColor, font, group) {}
        public ListViewItem(ListViewSubItem[] subItems, int imageIndex, ListViewGroup group) : base(subItems, imageIndex, group) {}
        public ListViewItem(string text, string imageKey) : base(text, imageKey) {}
        public ListViewItem(string[] items, string imageKey) : base(items, imageKey) {}
        public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font) : base(items, imageKey, foreColor, backColor, font) {}
        public ListViewItem(ListViewSubItem[] subItems, string imageKey) : base(subItems, imageKey) {}
        public ListViewItem(string text, string imageKey, ListViewGroup group) : base(text, imageKey, group) {}
        public ListViewItem(string[] items, string imageKey, ListViewGroup group) : base(items, imageKey, group) {}
        public ListViewItem(string[] items, string imageKey, Color foreColor, Color backColor, Font font, ListViewGroup group) : base(items, imageKey, foreColor, backColor, font, group) {}
        public ListViewItem(ListViewSubItem[] subItems, string imageKey, ListViewGroup group) : base(subItems, imageKey, group) {}
        protected ListViewItem(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public new T Tag
        {
            get { return (T) base.Tag;  }
            set { base.Tag = value;  }
        }
    }
}
