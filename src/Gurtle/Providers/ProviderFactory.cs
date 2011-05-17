#region License, Terms and Author(s)
//
// Gurtle - IBugTraqProvider for Google Code
// Copyright (c) 2011 Sven Strickroth. All rights reserved.
//
//  Author(s):
//
//      Sven Strickroth, <email@cs-ware.de>
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Gurtle.Providers
{
    class ProviderFactory
    {
        public static IProvider getProvider(string provider, string projectName)
        {
            if (provider == null)
            {
                throw new ArgumentException(null, "Provider missing");
            }
            switch (provider.ToLower())
            {
                case "googlecode":
                    return new GoogleCode.GoogleCodeProject(projectName);
                case "github":
                    return new GitHub.GitHubRepository(projectName);
                case "trac":
                    return new Trac.TracProject(projectName);
                default:
                    throw new ArgumentException("Provider invalid", provider);
            }
        }
    }
}
