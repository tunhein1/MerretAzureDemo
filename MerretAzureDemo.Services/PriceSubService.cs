using MerretAzureDemo.MerretDataAccess.Models;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;

namespace MerretAzureDemo.Services
{
    public class PriceSubService : IPriceSubService
    {
        const string ServiceBusConnectionString = "Endpoint=sb://hnproductfeed.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ChNika/E2ypWggEBibcw6GvyIaMUT2N6SHSs+WfTn7E=";
        const string TopicName = "pricefeedtopic";
        const string SubscriptionName = "PriceFeedSub";
        static ISubscriptionClient subscriptionClient;
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected static readonly CsvHelper.Configuration.Configuration _csvConfig =
            new CsvHelper.Configuration.Configuration()
            {
                Delimiter = ",",
                HasHeaderRecord = false,
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                UseNewObjectForNullReferenceMembers = false,
            };


        public void DoPriceSubProcess()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        async Task MainAsync()
        {
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            // register sub message handler and recive message in a loop
            RegisterOnMessageHandlerAndReceiveMessages();

            Console.ReadKey();

            await subscriptionClient.CloseAsync();

        }

        void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,

                // Indicates whether the message pump should automatically complete the messages after returning from user callback.
                // False below indicates the complete operation is handled by the user callback as in ProcessMessagesAsync().
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            log.Info($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            if (ExportPrices(message))
            {
                await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

                log.Info($"Processed message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");
            }
        }

        Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            log.Error($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            log.Error("Exception context for troubleshooting:");
            log.Error($"- Endpoint: {context.Endpoint}");
            log.Error($"- Entity Path: {context.EntityPath}");
            log.Error($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        bool ExportPrices(Message message)
        {
            try
            {
                string fileName = $"C:\\temp\\price_{DateTime.Now.ToString("ddMMyyyyhh")}.csv";

                //if(!File.Exists(fileName))
                //{
                using (var sw = new StreamWriter(fileName))
                {
                    var writer = new CsvHelper.CsvWriter(sw, _csvConfig);
                    var result = JsonConvert.DeserializeObject<PDLPRCP>(Encoding.UTF8.GetString(message.Body));
                    writer.WriteField(result.StyleNumber);
                    writer.WriteField(result.ColourCode);
                    writer.WriteField(result.SizeCode);
                    writer.WriteField(result.CountryCode);
                    writer.WriteField(result.BaseCcyCurrentRet);
                    writer.WriteField(result.BaseCcyOriginalRet);
                    

                }
                //}
                // else
                //{

                // }


                return true;
            }
            catch(Exception ex)
            {
                log.Error("Export prices: ", ex);
            }
            return false;
        }


    }
}
