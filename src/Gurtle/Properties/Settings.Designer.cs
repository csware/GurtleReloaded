﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.34209
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gurtle.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Drawing.Point IssueBrowserDialogLocation {
            get {
                return ((global::System.Drawing.Point)(this["IssueBrowserDialogLocation"]));
            }
            set {
                this["IssueBrowserDialogLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Drawing.Size IssueBrowserDialogSize {
            get {
                return ((global::System.Drawing.Size)(this["IssueBrowserDialogSize"]));
            }
            set {
                this["IssueBrowserDialogSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.Forms.FormWindowState IssueBrowserDialogWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["IssueBrowserDialogWindowState"]));
            }
            set {
                this["IssueBrowserDialogWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Drawing.Point IssueUpdateDialogLocation {
            get {
                return ((global::System.Drawing.Point)(this["IssueUpdateDialogLocation"]));
            }
            set {
                this["IssueUpdateDialogLocation"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Drawing.Size IssueUpdateDialogSize {
            get {
                return ((global::System.Drawing.Size)(this["IssueUpdateDialogSize"]));
            }
            set {
                this["IssueUpdateDialogSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.Forms.FormWindowState IssueUpdateDialogWindowState {
            get {
                return ((global::System.Windows.Forms.FormWindowState)(this["IssueUpdateDialogWindowState"]));
            }
            set {
                this["IssueUpdateDialogWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool HideIssueUpdateTip {
            get {
                return ((bool)(this["HideIssueUpdateTip"]));
            }
            set {
                this["HideIssueUpdateTip"] = value;
            }
        }
    }
}
