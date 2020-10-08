using Amqp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mystikweb.Amqp.Client.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mystikweb.Amqp.Client
{
    public class ClientConnection : IClientConnection
    {
        private readonly ILogger logger;
        private readonly ClientOptions clientOptions;

        private readonly Connection connection;
        private readonly Session session;

        public ClientConnection(ILogger logger,
            IOptions<ClientOptions> options)
        {
            this.logger = logger;
            clientOptions = options.Value;

            connection = new Connection(clientOptions.GetOptionsAddress());
            session = new Session(connection);
        }


    }
}
