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
    using System.Collections.Generic;

    internal static class DictionaryExtensions
    {
        public static V Find<K, V>(this IDictionary<K, V> dict, K key)
        {
            return Find(dict, key, default(V));
        }

        public static V Find<K, V>(this IDictionary<K, V> dict, K key, V @default)
        {
            V value;
            return dict.TryGetValue(key, out value) ? value : @default;
        }

        public static V Pop<K, V>(this IDictionary<K, V> dict, K key)
        {
            var value = dict[key];
            dict.Remove(key);
            return value;
        }

        public static V TryPop<K, V>(this IDictionary<K, V> dict, K key)
        {
            return TryPop(dict, key, default(V));
        }

        public static V TryPop<K, V>(this IDictionary<K, V> dict, K key, V @default)
        {
            var value = dict.Find(key, @default);
            dict.Remove(key);
            return value;
        }
    }
}
