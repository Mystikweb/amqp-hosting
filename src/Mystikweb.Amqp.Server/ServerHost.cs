using Amqp.Listener;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mystikweb.Amqp.Server.Connections;
using System.Threading;
using System.Threading.Tasks;

namespace Mystikweb.Amqp.Server
{
    public class ServerHost : IHostedService
    {
        private readonly ILogger logger;
        private readonly ContainerHost containerHost;

        public ServerHost(ILogger<ServerHost> logger,
            IOptions<ServerOptions> serverOptions,
            ClientConnectionProcessor clientConnectionProcessor)
        {
            this.logger = logger;

            var serverAddress = serverOptions.Value.GetOptionsAddress();
            containerHost = new ContainerHost(serverAddress);
            containerHost.RegisterLinkProcessor(clientConnectionProcessor);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Starting AMQP server");

            containerHost.Open();

            foreach (var listener in containerHost.Listeners)
            {
                logger.LogInformation($"AMQP server listening at {listener.Address.Scheme}://{listener.Address.Host}:{listener.Address.Port}");
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stopping AMQP server");

            containerHost.Close();

            return Task.CompletedTask;
        }
    }
}
