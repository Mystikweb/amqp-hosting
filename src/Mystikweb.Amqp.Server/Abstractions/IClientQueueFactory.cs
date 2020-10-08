using Amqp;
using Amqp.Listener;
using Mystikweb.Amqp.Server.Queues;
using System;
using System.Threading.Tasks;

namespace Mystikweb.Amqp.Server.Abstractions
{
    public interface IClientQueueFactory
    {
        ClientQueue CreateQueue(ListenerLink listener, Func<string, Message, Task> onMessagePublished);
    }
}
