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
    using System.Diagnostics;

    #endregion

    internal interface IProperty
    {
        Type ObjectType { get; }
        Type PropertyType { get; }
        bool ReadOnly { get; }
        object GetValue(object obj);
        void SetValue(object obj, object value);
    }

    internal interface IProperty<T> : IProperty
    {
        object GetValue(T obj);
        void SetValue(T obj, object value);
    }

    internal interface IProperty<T, PT> : IProperty<T>
    {
        new PT GetValue(T obj);
        void SetValue(T obj, PT value);
    }

    [ Serializable ]
    internal sealed class Property<T, PT> : IProperty<T, PT>
    {
        private readonly Func<T, PT> _getter;
        private readonly Action<T, PT> _setter;

        public Property(Func<T, PT> getter) :
            this(getter, null) {}

        public Property(Func<T, PT> getter, Action<T, PT> setter)
        {
            Debug.Assert(getter != null);

            _getter = getter;
            _setter = setter;
        }

        public Type ObjectType { get { return typeof(T); } }
        public Type PropertyType { get { return typeof(PT); } }
        public bool ReadOnly { get { return _setter == null; } }
        
        public PT GetValue(T obj) { return _getter(obj); }

        public void SetValue(T obj, PT value)
        {
            if (_setter == null)
                throw new InvalidOperationException("Cannot write to a read-only property.");

            _setter(obj, value);
        }

        object IProperty<T>.GetValue(T obj) { return GetValue(obj); }
        void IProperty<T>.SetValue(T obj, object value) { SetValue(obj, (PT) value); }
        object IProperty.GetValue(object obj) { return GetValue((T) obj); }
        void IProperty.SetValue(object obj, object value) { SetValue((T) obj, (PT) value); }
    }
}