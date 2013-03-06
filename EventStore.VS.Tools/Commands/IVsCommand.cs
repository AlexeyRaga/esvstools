﻿using Microsoft.VisualStudio.Project;

namespace EventStore.VS.Tools.Commands
{
    public interface IVsCommand
    {
        uint CmdId { get; }
        void Execute(HierarchyNode node);
    }
}
