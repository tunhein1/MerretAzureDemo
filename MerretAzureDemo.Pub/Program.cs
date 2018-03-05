using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.Pub
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main()
        {

            if (ConfigurationManager.AppSettings["Environment"] == "Debug")
            {
                //GreenChannelAzureService service1 = new GreenChannelAzureService();
                //service1.TestService();

                SubscriberService subService = new SubscriberService();
                subService.TestService();
            }
            else
            {
                ServiceBase[] ServicesToRun;

                ServicesToRun = new ServiceBase[] {
                new GreenChannelAzureService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
