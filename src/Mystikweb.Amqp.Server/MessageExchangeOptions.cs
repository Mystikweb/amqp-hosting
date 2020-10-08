namespace Mystikweb.Amqp.Server
{
    public class MessageExchangeOptions
    {
        public string Name { get; set; }
        public int MaxConnections { get; set; } = 5;
        public int InitialCredit { get; set; } = 200;
    }
}
