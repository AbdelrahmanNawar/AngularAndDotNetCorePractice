using BackendApi.Models;
using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackendApi.Controllers
{
    public class ServiceMessageSender : IServiceMessage
    {
        private static readonly string AzureConnectionString = "Endpoint=sb://orderproccessing.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LhMcaOTnOmqE7nWFLb1JTxho9zetWQneZvukx69vqmc=";
        private static readonly string QueuePath = "orderqueue";
        private QueueClient queueClient;

        public ServiceMessageSender()
        {
            queueClient = new QueueClient(AzureConnectionString, QueuePath);
        }

        public void SendMessageAsync(string message)
        {
            var sendMessage = new Message((Encoding.UTF8.GetBytes(message)));
            queueClient.SendAsync(sendMessage).Wait();
            queueClient.CloseAsync().Wait();
        }
    }
}
