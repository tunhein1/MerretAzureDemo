using MerretAzureDemo.MerretDataAccess.Models;
using MerretAzureDemo.Services;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.Pub
{
    class Program_temp
    {
        const string ServiceBusConnectionString = "Endpoint=sb://hnproductfeed.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ChNika/E2ypWggEBibcw6GvyIaMUT2N6SHSs+WfTn7E=";
        //const string TopicName = "demotopic";
        const string TopicName = "pricefeedtopic";
        static ITopicClient topicClient;

        public IPricePubService _priceService = new PricePubService();

        void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            const int numberOfMessages = 10;
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send messages.
            //await SendMessagesAsync(numberOfMessages);


            //await SendMessagesAsync(result);

            Console.ReadKey();

            await topicClient.CloseAsync();
        }

        async Task SendMessagesAsync(List<PDLPRCP> priceList)
        {
            foreach(var item in priceList)
            {
                var msgBody = JsonConvert.SerializeObject(item);
                var message = new Message(Encoding.UTF8.GetBytes(msgBody));

                Console.WriteLine($"Sending message: {msgBody}");

                await topicClient.SendAsync(message);
            }
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the topic.
                    string messageBody = $"Message - more {i}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the topic.
                    await topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }


    }
}
