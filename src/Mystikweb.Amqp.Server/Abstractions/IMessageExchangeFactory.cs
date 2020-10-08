namespace Mystikweb.Amqp.Server.Abstractions
{
    public interface IMessageExchangeFactory
    {
        IMessageExchange CreateExchange(MessageExchangeOptions options);
    }
}
