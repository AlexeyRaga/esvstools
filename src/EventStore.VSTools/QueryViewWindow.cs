﻿using System.Runtime.InteropServices;
using EventStore.VSTools.Views;
using EventStore.VSTools.Views.QueryTool;
using Microsoft.VisualStudio.Shell;

namespace EventStore.VSTools
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("f0a336be-32fe-4e02-bb4d-bb455632d8b0")]
    public class QueryViewWindow : ToolWindowPane
    {
        private readonly QueryToolWindowViewModel _queryTool = new QueryToolWindowViewModel();

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public QueryViewWindow() :
            base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = Resources.ToolWindowTitle;
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.

            var toolWindowViewModel = new QueryToolWindowViewModel();
            base.Content = new QueryViewToolWindow(toolWindowViewModel);
            _queryTool = toolWindowViewModel;
        }

        public void ShowQueryResult(string name, string queryUri, string queryResult)
        {
            _queryTool.ShowQueryResult(name, queryUri, queryResult);
        }
    }
}
