using Amqp.Listener;

namespace Mystikweb.Amqp.Server.Abstractions
{
    public interface IMessageExchangeManager
    {
        void AttachToExchange(string name, AttachContext attachContext);
    }
}
