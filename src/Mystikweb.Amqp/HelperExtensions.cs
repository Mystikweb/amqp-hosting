using Amqp;

namespace Mystikweb.Amqp
{
    public static class HelperExtensions
    {
        public static Address GetOptionsAddress(this ConnectionOptions options)
        {
            return new Address(options.Address,
                options.Port,
                options.UserName,
                options.Password,
                scheme: options.UseSecured ? "AMQPS" : "AMQP");
        }
    }
}
