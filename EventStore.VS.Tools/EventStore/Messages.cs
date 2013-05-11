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
        public string Name { get; private set; }
        public string Content { get; private set; }

        public ProjectionMessage(string name, string content)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            Name = name;
            Content = content;
        }
    }

    public sealed class DeployProjection : ProjectionMessage
    {
        public DeployProjection(string name, string content) : base(name, content) { }
    }
}
