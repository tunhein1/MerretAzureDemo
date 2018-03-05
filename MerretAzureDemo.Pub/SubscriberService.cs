using MerretAzureDemo.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.Pub
{
    partial class SubscriberService : ServiceBase
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPriceSubService _priceSubService;
        public SubscriberService()
        {
            InitializeComponent();
            _priceSubService = new PriceSubService();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            log.Info("Grenn channel price sub process started.");
            _priceSubService.DoPriceSubProcess();
            log.Info("Grenn channel price sub process finished.");
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        public void TestService()
        {
            log.Info("Grenn channel price sub process started.");
            _priceSubService.DoPriceSubProcess();
            log.Info("Grenn channel price sub process finished.");
        }
    }
}
