using Amqp;
using Amqp.Framing;
using Amqp.Listener;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mystikweb.Amqp.Server.Queues
{
    public class ClientQueue : LinkEndpoint
    {
        private readonly ILogger logger;
        private ListenerLink clientLink;
        private Func<string, Message, Task> onMessagePublished;

        public ClientQueue(ILogger<ClientQueue> logger)
        {
            this.logger = logger;
        }

        public string Name => clientLink.Name;
        public bool IsConsumer => !clientLink.Role;

        public void InitializeEndpoint(ListenerLink clientLink, Func<string, Message, Task> onMessagePublished)
        {
            this.clientLink = clientLink;
            this.onMessagePublished = onMessagePublished;
        }

        public void SendMessage(Message message)
        {
            clientLink.SendMessage(message);
        }

        public override void OnMessage(MessageContext messageContext)
        {
            logger.LogInformation($"OnMessage called from {Name}");

            onMessagePublished?.Invoke(clientLink.Name, Message.Decode(messageContext.Message.Encode()));

            messageContext.Complete();
        }

        public override void OnDisposition(DispositionContext dispositionContext)
        {
            logger.LogInformation($"OnDisposition called from {Name}");

            dispositionContext.Complete();
        }

        public override void OnFlow(FlowContext flowContext)
        {
            logger.LogInformation($"OnFlow called from {Name}");
        }

        public override void OnLinkClosed(ListenerLink link, Error error)
        {
            logger.LogInformation($"OnLinkClosed called from {Name}");

            base.OnLinkClosed(link, error);
        }
    }
}
