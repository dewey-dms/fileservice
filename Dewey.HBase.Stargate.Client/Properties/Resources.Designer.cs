﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18213
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dewey.HBase.Stargate.Client.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HBase.Stargate.Client.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to All columns must have a name..
        /// </summary>
        internal static string ErrorProvider_ColumnNameMissing {
            get {
                return ResourceManager.GetString("ErrorProvider_ColumnNameMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table name must be specified at a minimum for batch insert..
        /// </summary>
        internal static string ResourceBuilder_MinimumForBatchInsertNotMet {
            get {
                return ResourceManager.GetString("ResourceBuilder_MinimumForBatchInsertNotMet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table name must be specified at a minimum for Cell or Row queries..
        /// </summary>
        internal static string ResourceBuilder_MinimumForCellOrRowQueryNotMet {
            get {
                return ResourceManager.GetString("ResourceBuilder_MinimumForCellOrRowQueryNotMet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table name and row key must be specified at a minimum for item deletion..
        /// </summary>
        internal static string ResourceBuilder_MinimumForDeleteItemNotMet {
            get {
                return ResourceManager.GetString("ResourceBuilder_MinimumForDeleteItemNotMet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table name must be specified for scanner access..
        /// </summary>
        internal static string ResourceBuilder_MinimumForScannerNotMet {
            get {
                return ResourceManager.GetString("ResourceBuilder_MinimumForScannerNotMet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table name must be specified at a minimum for schema update/creation..
        /// </summary>
        internal static string ResourceBuilder_MinimumForSchemaUpdateNotMet {
            get {
                return ResourceManager.GetString("ResourceBuilder_MinimumForSchemaUpdateNotMet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Table name, row key, and column name must be specified at a minimum for single value access..
        /// </summary>
        internal static string ResourceBuilder_MinimumForSingleValueAccessNotMet {
            get {
                return ResourceManager.GetString("ResourceBuilder_MinimumForSingleValueAccessNotMet", resourceCulture);
            }
        }
    }
}