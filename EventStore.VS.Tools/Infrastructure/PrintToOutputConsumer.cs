using System;

namespace EventStore.VS.Tools.Infrastructure
{
    public sealed class PrintToOutputConsumer<T> : IConsume<T>
    {
        private readonly Func<T, string> _formatter;

        public PrintToOutputConsumer(Func<T, string> formatter)
        {
            _formatter = formatter;
        }

        public void Consume(T message)
        {
            Output.Pane.OutputStringThreadSafe(_formatter(message));
            Output.Pane.OutputStringThreadSafe(Environment.NewLine);
        }
    }
}
