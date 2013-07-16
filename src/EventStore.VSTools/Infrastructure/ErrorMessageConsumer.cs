using System;
using EventStore.VSTools.EventStore;

namespace EventStore.VSTools.Infrastructure
{
    public sealed class ErrorMessageConsumer : IConsume<ErrorEvent>
    {
        private readonly IOutputErrorMessages _errorOutput;

        public ErrorMessageConsumer(IOutputErrorMessages errorOutput)
        {
            _errorOutput = errorOutput;
        }

        public void Consume(ErrorEvent message)
        {
            var text = String.IsNullOrEmpty(message.Message) ? message.Title : message.Message;
            _errorOutput.WriteError(text);
        }
    }
}
