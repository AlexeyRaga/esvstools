﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EventStore.VSTools {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EventStore.VSTools.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Cannot create EventStore Queries window..
        /// </summary>
        internal static string CanNotCreateWindow {
            get {
                return ResourceManager.GetString("CanNotCreateWindow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EventStore Projections Project.
        /// </summary>
        internal static string ProjectionsProjectTitle {
            get {
                return ResourceManager.GetString("ProjectionsProjectTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Icon similar to (Icon).
        /// </summary>
        internal static System.Drawing.Icon projectNodeIcon {
            get {
                object obj = ResourceManager.GetObject("projectNodeIcon", resourceCulture);
                return ((System.Drawing.Icon)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized resource of type System.Drawing.Bitmap.
        /// </summary>
        internal static System.Drawing.Bitmap script_16xLG {
            get {
                object obj = ResourceManager.GetObject("script_16xLG", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EventStore Queries.
        /// </summary>
        internal static string ToolWindowTitle {
            get {
                return ResourceManager.GetString("ToolWindowTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Pull Down Projections From the EventStore.
        /// </summary>
        internal static string Wizard_DownloadProjections {
            get {
                return ResourceManager.GetString("Wizard_DownloadProjections", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to EventStore Connection.
        /// </summary>
        internal static string Wizard_EventStoreConnection {
            get {
                return ResourceManager.GetString("Wizard_EventStoreConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No projections will be imported.
        /// </summary>
        internal static string Wizard_FinishPage_NoImport {
            get {
                return ResourceManager.GetString("Wizard_FinishPage_NoImport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import projections from the EventStore.
        /// </summary>
        internal static string Wizard_FinishPageTitle {
            get {
                return ResourceManager.GetString("Wizard_FinishPageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import projections into the project.
        /// </summary>
        internal static string Wizard_ImportProjectionsCheck {
            get {
                return ResourceManager.GetString("Wizard_ImportProjectionsCheck", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The following projections will be imported:.
        /// </summary>
        internal static string Wizard_ProjectionsToImport {
            get {
                return ResourceManager.GetString("Wizard_ProjectionsToImport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This wizard will help you to import the existing projections from the EventStore into this project. Click &quot;Next&quot; if you want to proceed, or cancel this wizard if you want to start with clean project..
        /// </summary>
        internal static string Wizard_StartPageContent {
            get {
                return ResourceManager.GetString("Wizard_StartPageContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Import projections from the EventStore.
        /// </summary>
        internal static string Wizard_StartPageTitle {
            get {
                return ResourceManager.GetString("Wizard_StartPageTitle", resourceCulture);
            }
        }
    }
}
