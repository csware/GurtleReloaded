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
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;

    #endregion

    internal static class CredentialPrompt
    {
        public static NetworkCredential Prompt(IWin32Window owner, string realm, string path)
        {
            return PromptImpl(owner, realm, Path.IsPathRooted(path) ? path : AppPaths.GetLocal(path));
        }

        private static NetworkCredential PromptImpl(IWin32Window owner, string realm, string path)
        {
            var credential = TryLoadFromFile(path);
            
            using (var dialog = new CredentialsDialog { Realm = realm })
            {
                if (credential != null)
                {
                    dialog.UserName = credential.UserName;
                    dialog.Password = credential.Password;
                    dialog.SavePassword = credential.Password.Length > 0;
                }

                if (dialog.ShowDialog(owner) != DialogResult.OK)
                    return null;

                if (dialog.UserName.Length == 0 || dialog.Password.Length == 0)
                    return null;

                credential = new NetworkCredential(dialog.UserName, dialog.Password);
                    
                if (dialog.SavePassword)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    SaveToFile(path, credential);
                }
                else
                {
                    if (File.Exists(path))
                        File.Delete(path);
                }
            }

            return credential;
        }

        private static void SaveToFile(string path, NetworkCredential credential)
        {
            Debug.Assert(credential != null);
            SaveToFile(path, credential.UserName, credential.Password);
        }

        private static void SaveToFile(string path, string userName, string password)
        {
            Debug.Assert(userName != null);
            Debug.Assert(userName.Length > 0);
            Debug.Assert(password != null);
            Debug.Assert(password.Length > 0);

            var entropy = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(entropy);
            var secret = ProtectedData.Protect(Encoding.Unicode.GetBytes(password), entropy, DataProtectionScope.CurrentUser);
            File.WriteAllLines(path, new[]
            {
                userName, 
                Convert.ToBase64String(entropy), 
                Convert.ToBase64String(secret)
            });
        }

        private static NetworkCredential TryLoadFromFile(string path)
        {
            return File.Exists(path) ? TryLoad(File.ReadAllLines(path)) : null;
        }

        private static NetworkCredential TryLoad(IEnumerable<string> lines)
        {
            if (lines == null)
                return null;

            return TryLoadImpl(lines.Select(line => line.Trim())
                                    .Where(line => line.Length > 0 && line[0] != '#')
                                    .Take(3)
                                    .ToArray());

        }
        
        private static NetworkCredential TryLoadImpl(string[] lines)
        {
            if (lines.Length < 3)
                return null;

            var userName = lines[0];
            var entropy = Convert.FromBase64String(lines[1]);
            var secret = Convert.FromBase64String(lines[2]);

            byte[] data;
            try
            {
                data = ProtectedData.Unprotect(secret, entropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Trace.TraceWarning(e.Message);
                return null;
            }

            var password = Encoding.Unicode.GetString(data);
            return new NetworkCredential(userName, password);
        }
    }
}
