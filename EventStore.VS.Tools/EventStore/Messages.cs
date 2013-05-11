using EventStore.VS.Tools.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.VS.Tools
{
    public abstract class ProjectionMessage : IMessage
    {
        public string FilePath { get; private set; }

        public ProjectionMessage(string filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException("filePath");
            FilePath = filePath;
        }
    }

    public sealed class DeployProjection : ProjectionMessage
    {
        public DeployProjection(string filePath) : base(filePath) { }
    }
}
