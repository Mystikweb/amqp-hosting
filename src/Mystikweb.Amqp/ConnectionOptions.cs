namespace Mystikweb.Amqp
{
    public abstract class ConnectionOptions
    {
        public string Address { get; set; } = "localhost";
        public int Port { get; set; } = 7500;
        public bool UseSecured { get; set; } = false;
        public string UserName { get; set; } = "admin";
        public string Password { get; set; } = "admin";
    }
}
