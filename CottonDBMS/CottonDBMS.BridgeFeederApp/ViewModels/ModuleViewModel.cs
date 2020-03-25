using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.BridgeFeederApp.Navigation;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.RFID;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.ViewModels;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.BridgeFeederApp.ViewModels;

namespace CottonDBMS.BridgeApp.ViewModels
{

    public class ModuleScanViewModel : BasePageViewModel
    {
        #region Observable Properties
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

        private string _BridgeLoadNumber;
        public string BridgeLoadNumber
        {
            get
            {
                return _BridgeLoadNumber;
            }
            set
            {
                Set<string>(() => BridgeLoadNumber, ref _BridgeLoadNumber, value);
            }
        }

        private string _Client;
        public string Client
        {
            get
            {
                return _Client;
            }
            set
            {
                Set<string>(() => Client, ref _Client, value);
            }
        }

        private string _Farm;
        public string Farm
        {
            get
            {
                return _Farm;
            }
            set
            {
                Set<string>(() => Farm, ref _Farm, value);
            }
        }

        private string _Field;
        public string Field
        {
            get
            {
                return _Field;
            }
            set
            {
                Set<string>(() => Field, ref _Field, value);
            }
        }

        private string _Variety;
        public string Variety
        {
            get
            {
                return _Variety;
            }
            set
            {
                Set<string>(() => Variety, ref _Variety, value);
            }
        }

        private string _SerialNumber;
        public string SerialNumber
        {
            get
            {
                return _SerialNumber;
            }
            set
            {
                Set<string>(() => SerialNumber, ref _SerialNumber, value);
            }
        }

        private string _scanTime;
        public string ScanTime
        {
            get
            {
                return _scanTime;
            }
            set
            {
                Set<string>(() => ScanTime, ref _scanTime, value);
            }
        }

        #endregion

        public ModuleScanViewModel(INavigationService navService) : base(navService)
        {            
            Messenger.Default.Register<List<TagItem>>(this, handleTagsReported);
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<List<TagItem>>(this, handleTagsReported);            
            base.Cleanup();
        }

        public void Initialize(string serialNumber, string epc, bool goToIdleOnExistingSerial)
        {
            try
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var ownerships = dp.ModuleOwnershipRepository.FindMatching(m => m.Name == serialNumber).ToList().OrderBy(t => t.LastCreatedOrUpdated);
                    var lastOwnership = ownerships.LastOrDefault();
                    var bridgeID = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.BRIDGE_ID, "");
                    var existingScan = dp.FeederScanRepository.FindSingle(s => s.Name == serialNumber);

                    if (existingScan != null) //ignore serial numbers already scanned
                    {
                        if (goToIdleOnExistingSerial)
                        {
                            var vm = new IdlePageViewModel(NavService);
                            NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
                            vm.Initialize();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }

                    FeederScanEntity newScan = new FeederScanEntity();
                    newScan.BridgeID = bridgeID;
                    newScan.Latitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LATITUDE);
                    newScan.Longitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LONGITUDE);

                    var status = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.TARGET_STATUS, "AT GIN");
                    if (status == "AT GIN")
                        newScan.TargetStatus = ModuleStatus.AT_GIN;
                    else
                        newScan.TargetStatus = ModuleStatus.ON_FEEDER;

                    newScan.EPC = epc;
                    newScan.Processed = false;
                    newScan.SyncedToCloud = false;
                    newScan.Source = InputSource.TRUCK;
                    newScan.Name = serialNumber;
                    dp.FeederScanRepository.Save(newScan);
                    dp.SaveChanges();
                    ScanTime = newScan.Created.ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt");
                    
                        //load view model
                        if (lastOwnership != null)
                        {
                            BridgeLoadNumber = lastOwnership.BridgeLoadNumber > 0 ? lastOwnership.BridgeLoadNumber.ToString() : "";
                            GinTicketLoadNumber = lastOwnership.GinTagLoadNumber;
                            Client = lastOwnership.Client;
                            Farm = lastOwnership.Farm;
                            Field = lastOwnership.Field;
                            Variety = lastOwnership.Variety;
                        }
                        else
                        {
                            BridgeLoadNumber = "";
                            GinTicketLoadNumber = "";
                            Variety = Client = Farm = Field = "";
                        }

                        SerialNumber = serialNumber;                   
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }

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
                        Initialize(t.SerialNumber, t.Epc, false);
                    }
                }

                TagDataProvider.ClearBuffer();                
            }
        }
    }
}