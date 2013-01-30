using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.EventStore_VS_Tools.Commands
{
    public interface IVsCommand
    {
        uint CmdId { get; }
        void Execute(object sender, EventArgs eventArgs);
    }
}
