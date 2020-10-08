using Amqp.Listener;

namespace Mystikweb.Amqp.Server.Abstractions
{
    public interface IMessageExchange
    {
        string Name { get; }
        void AttachClient(AttachContext attachContext);
    }
}
