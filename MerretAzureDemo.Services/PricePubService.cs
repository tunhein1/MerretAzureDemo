using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerretAzureDemo.MerretDataAccess.Models;
using MerretAzureDemo.MerretDataAccess;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace MerretAzureDemo.Services
{
    public class PricePubService : IPricePubService
    {
        private readonly IMerretRepository _merretRepository;
        const string ServiceBusConnectionString = "Endpoint=sb://hnproductfeed.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ChNika/E2ypWggEBibcw6GvyIaMUT2N6SHSs+WfTn7E=";
        const string TopicName = "pricefeedtopic";
        static ITopicClient topicClient;
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public PricePubService(IMerretRepository merretRepository)
        {
            _merretRepository = merretRepository;
        }

        public PricePubService()
        {
            _merretRepository = new MerretRepository();
        }

        public void DoGreenChannelProcess()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        async Task MainAsync()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            var updateFrom = DateTime.Now.AddDays(-7);
            var result = GetGrennChannelPriceList(updateFrom);

            await SendMessageAsync(result);
        }

        async Task SendMessageAsync(List<PDLPRCP> priceList)
        {
            foreach(var item in priceList)
            {
                var msgBody = JsonConvert.SerializeObject(item);
                var message = new Message(Encoding.UTF8.GetBytes(msgBody));

                log.Info($"Sending message: {msgBody}");

                await topicClient.SendAsync(message);
            }
        }

        private List<PDLPRCP> GetGrennChannelPriceList(DateTime updateFrom)
        {
            return _merretRepository.GetLinePriceFile(updateFrom);
        }
    }
}
