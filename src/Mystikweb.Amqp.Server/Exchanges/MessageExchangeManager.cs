using Amqp.Listener;
using Microsoft.Extensions.Logging;
using Mystikweb.Amqp.Server.Abstractions;
using System.Collections.Concurrent;
using System.Linq;

namespace Mystikweb.Amqp.Server.Exchanges
{
    public class MessageExchangeManager : IMessageExchangeManager
    {
        private readonly ILogger logger;
        private readonly IMessageExchangeFactory messageExchangeFactory;
        private readonly ConcurrentBag<IMessageExchange> exchanges = new ConcurrentBag<IMessageExchange>();

        public MessageExchangeManager(ILogger<MessageExchangeManager> logger,
            IMessageExchangeFactory messageExchangeFactory)
        {
            this.logger = logger;
            this.messageExchangeFactory = messageExchangeFactory;
        }

        public void AttachToExchange(string name, AttachContext attachContext)
        {
            var exchange = exchanges.FirstOrDefault(x => x.Name == name);
            if (exchange == null)
            {
                logger.LogInformation($"Exchange {name} not found creating new instance.");
                exchange = messageExchangeFactory.CreateExchange(new MessageExchangeOptions { Name = name });
                exchanges.Add(exchange);
            }

            exchange.AttachClient(attachContext);
        }
    }
}
