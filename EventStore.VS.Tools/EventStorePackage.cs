using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using EventStore.VS.Tools.Commands;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Project;
using EventStore.VS.Tools.PropertyPages;
using System;

namespace EventStore.VS.Tools
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(MyToolWindow))]
    [ProvideProjectFactory(
        typeof(ProjectionsProjectFactory),
        null,
        "EventStore Projections Project Files (*.espproj);*.espproj",
        "espproj", "espproj",
        @".\\NullPath",
        LanguageVsTemplate = "EventStore")]

    [ProvideObject(typeof(DeployPropertyPage))]
    [ProvideObject(typeof(GeneralPropertyPage))]
    [Guid(GuidList.guidEventStore_VS_ToolsPkgString)]
    public sealed class EventStorePackage : ProjectPackage
    {
        private readonly IDictionary<uint, IVsCommand> _commands = new Dictionary<uint, IVsCommand>(); 

        public IVsCommand FindCommand(uint commandId)
        {
            IVsCommand command;
            return !_commands.TryGetValue(commandId, out command) ? null : command;
        }

        public EventStorePackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();
            RegisterProjectFactory(new ProjectionsProjectFactory(this));
            RegisterCommands();
            
        }

        private IEnumerable<IVsCommand> BuildCommands()
        {
            yield return new DeployCommand(this);
            yield return new ToolWindowCommand(this);
            yield return new RunProjectionCommand(this);
        }

        private void RegisterCommands()
        {
            // Add our command handlers for menu (commands must exist in the .vsct file)
            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null == mcs) return;

            var commands = BuildCommands();
            foreach (var vsCommand in commands)
            {
                _commands.Add(vsCommand.CmdId, vsCommand);
            }
        }

        //private static void RegisterCommandsInService(IMenuCommandService commandService, IEnumerable<IVsCommand> commands)
        //{
        //    if (commands == null) return;

        //    foreach (var vsCommand in commands)
        //    {
        //        var commandId = new CommandID(GuidList.guidEventStore_VS_ToolsCmdSet, (int) vsCommand.CmdId);
        //        var menuItem = new MenuCommand(vsCommand.Execute, commandId);
        //        commandService.AddCommand(menuItem);
        //    }
        //}


        public override string ProductUserContext
        {
            get { return "Projections"; }
        }
    }
}
