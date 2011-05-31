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
    using System.IO;
    using System.Reflection;

    #endregion

    internal static class AppPaths
    {
        public static string GetLocal()
        {
            return GetLocal(null);
        }

        public static string GetLocal(string path)
        {
            return !string.IsNullOrEmpty(path) && Path.IsPathRooted(path) 
                 ? path : GetLocalImpl(path);
        }

        private static string GetLocalImpl(string tailPath)
        {
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            var path = Path.Combine(localAppDataPath, assemblyName.Name);
            return !string.IsNullOrEmpty(tailPath) ? Path.Combine(path, tailPath) : path;
        }
    }
}
