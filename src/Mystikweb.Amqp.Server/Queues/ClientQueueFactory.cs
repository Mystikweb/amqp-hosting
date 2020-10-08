using Amqp;
using Amqp.Listener;
using Microsoft.Extensions.DependencyInjection;
using Mystikweb.Amqp.Server.Abstractions;
using System;
using System.Threading.Tasks;

namespace Mystikweb.Amqp.Server.Queues
{
    public class ClientQueueFactory : IClientQueueFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ClientQueueFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ClientQueue CreateQueue(ListenerLink listener, Func<string, Message, Task> onMessagePublished)
        {
            var result = serviceProvider.GetRequiredService<ClientQueue>();
            result.InitializeEndpoint(listener, onMessagePublished);

            return result;
        }
    }
}
