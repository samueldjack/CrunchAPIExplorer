﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CrunchApiExplorer.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ConsumerKey {
            get {
                return ((string)(this["ConsumerKey"]));
            }
            set {
                this["ConsumerKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SharedSecret {
            get {
                return ((string)(this["SharedSecret"]));
            }
            set {
                this["SharedSecret"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string AccessToken {
            get {
                return ((string)(this["AccessToken"]));
            }
            set {
                this["AccessToken"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://testing.crunch.co.uk/crunch-core/oauth/access_token")]
        public string AccessTokenEndpoint {
            get {
                return ((string)(this["AccessTokenEndpoint"]));
            }
            set {
                this["AccessTokenEndpoint"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://testing.crunch.co.uk/crunch-core/login/oauth-login.seam")]
        public string UserAuthorizationEndpoint {
            get {
                return ((string)(this["UserAuthorizationEndpoint"]));
            }
            set {
                this["UserAuthorizationEndpoint"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://app.crunch.co.uk/")]
        public global::System.Uri LiveServer {
            get {
                return ((global::System.Uri)(this["LiveServer"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://testing.crunch.co.uk/crunch-core/oauth/request_token")]
        public string RequestTokenEndpoint {
            get {
                return ((string)(this["RequestTokenEndpoint"]));
            }
            set {
                this["RequestTokenEndpoint"] = value;
            }
        }
    }
}
