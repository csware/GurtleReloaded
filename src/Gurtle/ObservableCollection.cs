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
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;

    #endregion

    /// <summary>
    /// Represents a dynamic data collection that provides notifications 
    /// when items get added, removed, or when the collection is cleared. 
    /// </summary>

    [ Serializable ]
    internal class ObservableCollection<T> : Collection<T>
    {
        public event EventHandler Changed;
        public event EventHandler Clearing;
        public event EventHandler Cleared;
        public event EventHandler<CollectionItemEventArgs<T>> ItemAdding;
        public event EventHandler<CollectionItemEventArgs<T>> ItemAdded;
        public event EventHandler<CollectionItemEventArgs<T>> ItemRemoving;
        public event EventHandler<CollectionItemEventArgs<T>> ItemRemoved;
        public event EventHandler<CollectionItemBatchEventArgs<T>> ItemsAdding;
        public event EventHandler<CollectionItemBatchEventArgs<T>> ItemsAdded;

        public ObservableCollection() : 
            base(new List<T>()) {}

        public void AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            
            var items = collection.ToArray();
            if (items.Length == 0) 
                return;
            
            foreach (var item in items)
                ValidateItem(item);
            
            var args = new CollectionItemBatchEventArgs<T>(items);
            OnItemsAdding(args);
            
            ((List<T>) Items).AddRange(items);
            
            OnItemsAdded(args);
            OnChanged();
        }

        protected override void InsertItem(int index, T item)
        {
            ValidateItem(item);
            var args = new CollectionItemEventArgs<T>(item);
            OnItemAdding(args);
            base.InsertItem(index, item);
            OnItemAdded(args);
            OnChanged();
        }

        protected override void SetItem(int index, T item)
        {
            ValidateItem(item);
            var oldItem = this[index];
            
            var removeArgs = new CollectionItemEventArgs<T>(oldItem);
            OnItemRemoving(removeArgs);
            
            var addArgs = new CollectionItemEventArgs<T>(item);
            OnItemAdding(addArgs);
            
            base.SetItem(index, item);
            
            OnItemRemoved(removeArgs);
            OnItemAdded(addArgs);
            OnChanged();
        }

        protected override void ClearItems()
        {
            if (Count == 0)
                return;

            OnClearing(EventArgs.Empty);
            base.ClearItems();
            OnCleared(EventArgs.Empty);
            OnChanged();
        }

        protected override void RemoveItem(int index)
        {
            var item = this[index];
            var args = new CollectionItemEventArgs<T>(item);
            OnItemRemoving(args);
            base.RemoveItem(index);
            OnItemRemoved(args);
            OnChanged();
        }

        protected virtual void ValidateItem(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
        }

        private void OnChanged()
        {
            OnChanged(EventArgs.Empty);
        }

        protected virtual void OnChanged(EventArgs args)
        {
            Fire(Changed, args);
        }

        protected virtual void OnClearing(EventArgs args)
        {
            Fire(Clearing, args);
        }

        protected virtual void OnCleared(EventArgs args)
        {
            Fire(Cleared, args);
        }
        
        protected void OnItemAdding(CollectionItemEventArgs<T> args)
        {
            Fire(ItemAdding, args);
        }

        protected virtual void OnItemAdded(CollectionItemEventArgs<T> args)
        {
            Fire(ItemAdded, args);
        }

        protected void OnItemRemoving(CollectionItemEventArgs<T> args)
        {
            Fire(ItemRemoving, args);
        }

        protected virtual void OnItemRemoved(CollectionItemEventArgs<T> args)
        {
            Fire(ItemRemoved, args);
        }

        private void Fire(EventHandler handler, EventArgs args)
        {
            Debug.Assert(args != null);

            if (handler != null)
                handler(this, args);
        }

        protected void OnItemsAdding(CollectionItemBatchEventArgs<T> args)
        {
            Fire(ItemsAdding, args);
        }

        protected virtual void OnItemsAdded(CollectionItemBatchEventArgs<T> args)
        {
            Fire(ItemsAdded, args);
        }

        private void Fire<Args>(EventHandler<Args> handler, Args args) where Args : EventArgs
        {
            Debug.Assert(args != null);

            if (handler != null)
                handler(this, args);
        }
    }
}
