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
using System.Timers;

namespace MerretAzureDemo.Ws
{
    public partial class PubService : ServiceBase
    {
        private Timer timer1 = null;
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPricePubService _priceService;

        public PubService()
        {
            InitializeComponent();
            _priceService = new PricePubService();
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }

        protected override void OnStart(string[] args)
        {
            timer1 = new Timer();
            this.timer1.Interval = 1000 * 60 * 1; // every 10 minutes 
            this.timer1.Elapsed += new ElapsedEventHandler(this.timer1_Tick);
            timer1.Enabled = true;
        }

        protected override void OnStop()
        {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                log.Info("Grenn channel price process started.");
                _priceService.DoGreenChannelProcess();
                log.Info("Grenn channel price process finished.");
            }
            catch (Exception ex)
            {
                log.Error("Green channel error:", ex);
            }
        }

        public void TestService()
        {
            log.Info("Grenn channel price process started.");
            _priceService.DoGreenChannelProcess();
            log.Info("Grenn channel price process finished.");
        }
    }
}
