using Microsoft.VisualStudio.Project;

namespace EventStore.VSTools.Commands
{
    public interface IVsCommand
    {
        uint CmdId { get; }
        void Execute(HierarchyNode node);
    }
}
