using Mystikweb.Amqp.Server.Abstractions;
using System;

namespace Mystikweb.Amqp.Server.Exchanges
{
    public class MessageExchangeFactory : IMessageExchangeFactory
    {
        private readonly Func<MessageExchangeOptions, IMessageExchange> initFunc;

        public MessageExchangeFactory(Func<MessageExchangeOptions, IMessageExchange> initFunc)
        {
            this.initFunc = initFunc;
        }

        public IMessageExchange CreateExchange(MessageExchangeOptions options)
        {
            return initFunc(options);
        }
    }
}
