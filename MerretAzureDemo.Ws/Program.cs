using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.Ws
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            if (ConfigurationManager.AppSettings["Environment"] == "Debug")
            {
                //PubService pubService = new PubService();
                //pubService.TestService();

                SubService subService = new SubService();
                subService.TestService();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new PubService(),
                new SubService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
