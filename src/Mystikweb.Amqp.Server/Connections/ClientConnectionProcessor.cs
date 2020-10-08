using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using Microsoft.Extensions.Logging;
using Mystikweb.Amqp.Server.Abstractions;

namespace Mystikweb.Amqp.Server.Connections
{
    public class ClientConnectionProcessor : ILinkProcessor
    {
        private readonly ILogger logger;
        private readonly IMessageExchangeManager exchangeManager;

        public ClientConnectionProcessor(ILogger<ClientConnectionProcessor> logger,
            IMessageExchangeManager exchangeManager)
        {
            this.logger = logger;
            this.exchangeManager = exchangeManager;
        }

        public void Process(AttachContext attachContext)
        {
            var source = attachContext.Attach.Source as Source;

            string queueName = string.Empty;
            if (attachContext.Attach.Role)
            {
                queueName = source.Address;
            }
            else
            {
                if (attachContext.Attach.Target is Target target)
                {
                    queueName = target.Address;
                }
            }

            if (string.IsNullOrEmpty(queueName))
            {
                logger.LogWarning($"Attempt to connect without an exchange name.");
                attachContext.Complete(new Error(ErrorCode.InvalidField) { Description = "Unable to connect without exchange name." });
            }

            exchangeManager.AttachToExchange(queueName, attachContext);
        }
    }
}
