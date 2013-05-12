using System.ComponentModel;

namespace EventStore.VSTools.PropertyPages
{
    using Microsoft.VisualStudio;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Property page for the include and reference paths
    /// </summary>
    [ComVisible(true)]
    [Guid(GuidList.guidDeployPropertyPage)]
    internal class DeployPropertyPage : PropertyPageBase
    {
        private string _connectionString;

        [RefreshProperties(RefreshProperties.All)]
		[Category("Event Store")]
		[DisplayName("Connection string")]
		[Description("Connection string to the Event Store")]
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; IsDirty = true; }
        }

        public DeployPropertyPage()
        {
            Name = "Event Store Connection";
        }
        protected override void BindProperties()
        {
            _connectionString = GetConfigProperty(Constants.EventStore.ConnectionString);
        }

        protected override int ApplyChanges()
        {
            SetConfigProperty(Constants.EventStore.ConnectionString, _connectionString);
            return VSConstants.S_OK;
        }
    }
}
