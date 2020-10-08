using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mystikweb.Amqp.Server.Abstractions;
using Mystikweb.Amqp.Server.Connections;
using Mystikweb.Amqp.Server.Exchanges;
using Mystikweb.Amqp.Server.Queues;
using System;

namespace Mystikweb.Amqp.Server
{
    public static class HostingExtensions
    {
        public static IServiceCollection AddAmqpServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddAmqpServices(options => configuration.GetSection(ServerOptions.SERVER_OPTIONS).Bind(options));
        }

        public static IServiceCollection AddAmqpServices(this IServiceCollection services,
            Action<OptionsBuilder<ServerOptions>> configureOptions = null)
        {
            services.AddOptions();

            if (configureOptions != null)
            {
                configureOptions?.Invoke(services.AddOptions<ServerOptions>());
            }
            else
            {
                services.AddOptions<ServerOptions>();
            }

            services.AddTransient<ClientQueue>();
            services.AddSingleton<IClientQueueFactory, ClientQueueFactory>();

            services.AddTransient<MessageExchangeOptions>();
            services.AddTransient<IMessageExchange, MessageExchange>();
            services.AddSingleton<Func<MessageExchangeOptions, IMessageExchange>>(services =>
                options => ActivatorUtilities.CreateInstance<MessageExchange>(services, options));
            services.AddSingleton<IMessageExchangeFactory, MessageExchangeFactory>();
            services.AddSingleton<IMessageExchangeManager, MessageExchangeManager>();

            services.AddSingleton<ClientConnectionProcessor>();
            services.AddHostedService<ServerHost>();

            return services;
        }
    }
}
