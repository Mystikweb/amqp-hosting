using Amqp;
using Amqp.Listener;
using Microsoft.Extensions.Logging;
using Mystikweb.Amqp.Server.Abstractions;
using Mystikweb.Amqp.Server.Queues;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mystikweb.Amqp.Server.Exchanges
{
    public class MessageExchange : IMessageExchange
    {
        private readonly ILogger logger;
        private readonly MessageExchangeOptions options;
        private readonly IClientQueueFactory clientQueueFactory;
        
        private readonly SemaphoreSlim semaphore;
        private readonly ConcurrentBag<ClientQueue> queues = new ConcurrentBag<ClientQueue>();

        public MessageExchange(ILogger<MessageExchange> logger,
            IClientQueueFactory clientQueueFactory,
            MessageExchangeOptions options)
        {
            this.logger = logger;
            this.clientQueueFactory = clientQueueFactory;
            this.options = options;

            semaphore = new SemaphoreSlim(this.options.MaxConnections);
        }

        public string Name => options.Name;

        public void AttachClient(AttachContext attachContext)
        {
            logger.LogInformation($"{Name} received connection from {attachContext.Link.Name}");

            semaphore.Wait();

            ClientQueue clientQueue;
            try
            {
                clientQueue = clientQueueFactory.CreateQueue(attachContext.Link, MessagePublished);
                queues.Add(clientQueue);
            }
            finally
            {
                semaphore.Release();
            }

            attachContext.Complete(clientQueue, options.InitialCredit);
        }

        private Task MessagePublished(string publisherName, Message message)
        {
            foreach (var queue in queues.Where(c => c.Name != publisherName && c.IsConsumer))
            {
                queue.SendMessage(message);
            }

            return Task.CompletedTask;
        }
    }
}
