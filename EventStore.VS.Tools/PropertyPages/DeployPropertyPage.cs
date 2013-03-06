//-------------------------------------------------------------------------------------------------
// <copyright file="WixPathsPropertyPage.cs" company="Outercurve Foundation">
//   Copyright (c) 2004, Outercurve Foundation.
//   This software is released under Microsoft Reciprocal License (MS-RL).
//   The license and further copyright text can be found in the file
//   LICENSE.TXT at the root directory of the distribution.
// </copyright>
// 
// <summary>
// Contains the WixPathsPropertyPage class.
// </summary>
//-------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace EventStore.VS.Tools.PropertyPages
{
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Project;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Property page for the include and reference paths
    /// </summary>
    [ComVisible(true)]
    [Guid(GuidList.guidDeployPropertyPage)]
    internal class DeployPropertyPage : PropertyPageBase
    {
        private string _connectionString;

        [RefreshProperties(RefreshProperties.All)]
		[CategoryAttribute("Event Store")]
		[DisplayName("Connection string")]
		[DescriptionAttribute("Connection string to the Event Store")]
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; IsDirty = true; }
        }

        public DeployPropertyPage()
        {
            this.Name = "Event Store Connection";
        }
        protected override void BindProperties()
        {
            this._connectionString = this.GetConfigProperty("ESConnectionString");
        }

        protected override int ApplyChanges()
        {
            this.SetConfigProperty("ESConnectionString", _connectionString);
            return VSConstants.S_OK;
        }
    }
}
