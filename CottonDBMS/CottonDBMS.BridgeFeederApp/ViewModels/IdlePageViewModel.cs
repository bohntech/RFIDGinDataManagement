using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CottonDBMS.BridgeFeederApp.Navigation;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.RFID;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Timers;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.ViewModels;
using CottonDBMS.BridgeApp.ViewModels;

namespace CottonDBMS.BridgeFeederApp.ViewModels
{


    public class IdlePageViewModel : BasePageViewModel
    {
        //private System.Timers.Timer timer = null;        
        //private bool executingTimer = false;

        private string _GinName;
        public string GinName
        {
            get
            {
                return _GinName;
            }
            set
            {
                Set<string>(() => GinName, ref _GinName, value);
            }
        }

        private string _GinTicketLoadNumber;
        public string GinTicketLoadNumber
        {
            get
            {
                return _GinTicketLoadNumber;
            }
            set
            {
                string temp = value.Trim().TrimStart('0');
                Set<string>(() => GinTicketLoadNumber, ref _GinTicketLoadNumber, temp);
            }
        }

        private string _InputErrorMessage;
        public string InputErrorMessage
        {
            get
            {
                return _InputErrorMessage;
            }
            set
            {
                string temp = value.Trim().TrimStart('0');
                Set<string>(() => InputErrorMessage, ref _InputErrorMessage, temp);
            }
        }


        private string _WelcomeMessage;
        public string WelcomeMessage
        {
            get
            {
                return _WelcomeMessage;
            }
            set
            {
                Set<string>(() => WelcomeMessage, ref _WelcomeMessage, value);
            }
        }

        private List<TruckEntity> _trucks = new List<TruckEntity>();

        public IdlePageViewModel(INavigationService navService) : base(navService)
        {
            //LookupLoadCommand = new RelayCommand(this.ExecuteLookupLoad);
            Messenger.Default.Register<List<TagItem>>(this, handleTagsReported);
            //Messenger.Default.Register<BarcodeScannedMessage>(this, handleBarCodeScanned);

            /*timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;*/
        }

        /* private void Timer_Elapsed(object sender, ElapsedEventArgs e)
         {
             if (!executingTimer) //prevent long execution of handler from overlapping later callbacks
             {
                 executingTimer = true;                
                 using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                 {
                     lock (_trucks)
                     {
                         _trucks = dp.TruckRepository.GetAll().ToList();
                     }
                 }
                 executingTimer = false;
             }
         }*/

        public void Initialize()
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                GinName = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.GIN_NAME, "");
                WelcomeMessage = "WELCOME, TO " + GinName;
                TagDataProvider.ClearBuffer();
            }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<List<TagItem>>(this);
            base.Cleanup();
        }

        private void handleTagsReported(List<TagItem> tagsReported)
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {

                foreach (var t in tagsReported)
                {
                    var existingScan = dp.FeederScanRepository.FindSingle(s => s.Name == t.SerialNumber);
                    if (existingScan == null)
                    {
                        var vm = new ModuleScanViewModel(NavService);
                        NavService.ShowPage(PageType.FEEDER_MODULE_PAGE, false, vm);
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            vm.Initialize(t.SerialNumber, t.Epc, true);
                        }));
                    }
                }              
                TagDataProvider.ClearBuffer();               
            }
        }
    }
}
