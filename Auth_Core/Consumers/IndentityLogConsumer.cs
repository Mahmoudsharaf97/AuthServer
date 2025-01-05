using Auth_Core.Entity;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Auth_Core.Consumers
{
    public class IndentityLogConsumer : IndentityLog, IConsumer<IndentityLog>
    {
        private readonly ILogger<IndentityLogConsumer> _logger;

        public IndentityLogConsumer(ILogger<IndentityLogConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<IndentityLog> context)
        {
            _logger.LogInformation("IndentityLog: {@log}", context.Message);
            return Task.CompletedTask;
        }
    }
}