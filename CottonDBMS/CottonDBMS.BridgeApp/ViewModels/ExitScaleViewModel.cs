using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CottonDBMS.BridgeApp.Navigation;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.RFID;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Timers;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.ViewModels;

namespace CottonDBMS.BridgeApp.ViewModels
{
    public class ExitScalePageViewModel : BasePageViewModel
    {
        private System.Timers.Timer timer = null;
        
        public ExitScalePageViewModel(INavigationService navService) : base(navService)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = false;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var vm = new IdlePageViewModel(NavService);
            NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
            vm.Initialize();
        }

        public void Initialize()
        {
           
        }

        public override void Cleanup()
        {            
            timer.Enabled = false;
            base.Cleanup();
        }        
    }
}
