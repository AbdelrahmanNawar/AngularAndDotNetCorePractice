using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AzureSeviceBus
{
    class Program
    {
        private static readonly string AzureConnectionString = "Endpoint=sb://orderproccessing.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LhMcaOTnOmqE7nWFLb1JTxho9zetWQneZvukx69vqmc=";
        private static readonly string QueuePath = "orderqueue";

        static void Main(string[] args)
        {
            var queueClient = new QueueClient(AzureConnectionString, QueuePath);
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, HandleExceptionAsync);

            Console.ReadLine();
            queueClient.CloseAsync().Wait();
        }


        private static Task HandleExceptionAsync(ExceptionReceivedEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var content = Encoding.UTF8.GetString(message.Body);
            Console.WriteLine($"Received content: {content}");

            var myObj = JsonConvert.DeserializeObject<MessageOrderStatus>(content);
            myObj.Status = new Random().Next(1, 3);
            HttpService service = new HttpService();
            service.SendUpdateRequest(myObj.Id, myObj.Status);
        }
        public class MessageOrderStatus
        {
            public int Id { get; set; }
            public int Status { get; set; }
        }
    }
}
