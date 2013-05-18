using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Project;

namespace EventStore.VSTools.PropertyPages
{
    [ComVisible(true)]
    [Guid(GuidList.guidGeneralPropertyPage)]
    public sealed class GeneralPropertyPage : PropertyPageBase
    {
        public GeneralPropertyPage()
        {
            Name = "General";
        }

 

        protected override void BindProperties()
        {
 
        }

        protected override int ApplyChanges()
        {
            IsDirty = false;

            return VSConstants.S_OK;
        }
    }
}
