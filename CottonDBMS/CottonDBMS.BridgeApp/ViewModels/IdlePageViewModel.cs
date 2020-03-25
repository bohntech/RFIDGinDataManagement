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
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.ViewModels;
using System.Text.RegularExpressions;

namespace CottonDBMS.BridgeApp.ViewModels
{
    

    public class IdlePageViewModel : BasePageViewModel
    {
        private System.Timers.Timer timer = null;        
        private bool executingTimer = false;

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

        private bool _ShowInputError;
        public bool ShowInputError
        {
            get
            {
                return _ShowInputError;
            }
            set
            {                
                Set<bool>(() => ShowInputError, ref _ShowInputError, value);
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
            LookupLoadCommand = new RelayCommand(this.ExecuteLookupLoad);
            CopyLoadCommand = new RelayCommand(this.ExecuteCopyLoad);
            CreateLoadCommand = new RelayCommand(this.ExecuteCreateLoad);
            ViewLoadListCommand = new RelayCommand(this.ExecuteViewLoadList);
            Messenger.Default.Register<List<TagItem>>(this, handleTagsReported);
            Messenger.Default.Register<BarcodeScannedMessage>(this, handleBarCodeScanned);
            Messenger.Default.Register<InMotionMessage>(this, handleInMotionMessage);
            Messenger.Default.Register<WeightAcquiredMessage>(this, handleWeightAcquired);
            
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 10000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
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
        }

        public void Initialize()
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                TagDataProvider.SetGPOState(1, false);
                TagDataProvider.SetGPOState(2, false);
                TagDataProvider.SetGPOState(3, false);

                GinName = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.GIN_NAME, "");
                WelcomeMessage = "WELCOME, TO " + GinName;
                TagDataProvider.ClearBuffer();
                Logging.Logger.WriteBuffer();
                /*Task.Run(() =>
                {
                    TagDataProvider.DisconnectIfUptimeLimitReached(1);
                });*/

                lock (_trucks)
                {
                    _trucks = dp.TruckRepository.GetAll().ToList();
                }
            }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<List<TagItem>>(this);
            Messenger.Default.Unregister<BarcodeScannedMessage>(this);
            Messenger.Default.Unregister<InMotionMessage>(this);
            Messenger.Default.Unregister<WeightAcquiredMessage>(this);
            timer.Enabled = false;
            base.Cleanup();
        }

        public RelayCommand LookupLoadCommand { get; private set; }

        public RelayCommand CopyLoadCommand {get; private set; }

        public RelayCommand CreateLoadCommand { get; private set; }

        public RelayCommand ViewLoadListCommand { get; private set; }

        private void ExecuteLookupLoad()
        {
            ShowInputError = false;

            //TODO VALIDATE TICKET LOAD NUMBER EXISTS
            if (string.IsNullOrEmpty(GinTicketLoadNumber))
            {
                ShowInputError = true;
                InputErrorMessage = "Please enter a gin ticket load number.";
            }
            else
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    if (!dp.LoadScanRepository.FindMatching(m => m.GinTagLoadNumber == GinTicketLoadNumber).Any())
                    {
                        ShowInputError = true;
                        InputErrorMessage = "Load not found.";
                    }
                }
            }

            if (!ShowInputError)
            {
                var vm = new LoadViewModel(NavService);
                NavService.ShowPage(PageType.LOAD_PAGE, false, (BasePageViewModel)vm);
                vm.Initialize(GinTicketLoadNumber.Trim().TrimStart('0'), false, true);
            }
        }

        private void ExecuteCopyLoad()
        {
            ShowInputError = false;

            //TODO VALIDATE TICKET LOAD NUMBER EXISTS
            if (string.IsNullOrEmpty(GinTicketLoadNumber))
            {
                ShowInputError = true;
                InputErrorMessage = "Please enter a gin ticket load number.";
            }
            else
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    if (!dp.LoadScanRepository.FindMatching(m => m.GinTagLoadNumber == GinTicketLoadNumber).Any())
                    {
                        ShowInputError = true;
                        InputErrorMessage = "Load not found.";
                    }
                }
            }

            if (!ShowInputError)
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    dp.LoadScanRepository.DisableChangeTracking();
                    var existingLoad = dp.LoadScanRepository.FindSingle(l => l.GinTagLoadNumber == GinTicketLoadNumber);

                    existingLoad.Id = Guid.NewGuid().ToString();
                    existingLoad.BridgeLoadNumber = dp.LoadScanRepository.LastLoadNumber() + 1;
                    existingLoad.GinTagLoadNumber = GinTicketLoadNumber + "COPY";
                    existingLoad.Created = DateTime.UtcNow;
                    existingLoad.Updated = null;
                    dp.LoadScanRepository.Add(existingLoad);
                    dp.SaveChanges();

                    var vm = new LoadViewModel(NavService);
                    NavService.ShowPage(PageType.LOAD_PAGE, false, (BasePageViewModel)vm);
                    vm.Initialize(existingLoad.GinTagLoadNumber.Trim().TrimStart('0'), false, true);
                }
                
            }
        }

        private void ExecuteCreateLoad()
        {
            ShowInputError = false;

            //TODO VALIDATE TICKET LOAD NUMBER EXISTS
            if (string.IsNullOrEmpty(GinTicketLoadNumber))
            {
                ShowInputError = true;
                InputErrorMessage = "Please enter a gin ticket load number.";
            }
           
            if (!ShowInputError)
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var hasLoad = dp.LoadScanRepository.FindMatching(x => x.GinTagLoadNumber == GinTicketLoadNumber).Any();

                    if (hasLoad)
                    {
                        this.ExecuteLookupLoad();
                        return;
                    }

                    var bridgeID = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.BRIDGE_ID, "");
                    var lastLoadScan = new LoadScanEntity();

                    lastLoadScan.SubmittedBy = "attendant";

                    //always use a new load number otherwise a previous load
                    //could get overwritten
                    lastLoadScan.BridgeLoadNumber = dp.LoadScanRepository.LastLoadNumber() + 1;
                    var scanData = new ModuleScanData();
                    scanData.Scans = new List<LoadModuleScan>();
                    lastLoadScan.SetSerializedModuleScanData(scanData);
                    lastLoadScan.BridgeID = bridgeID;
                    lastLoadScan.TruckID = "";
                    lastLoadScan.GinTagLoadNumber = GinTicketLoadNumber;
                    lastLoadScan.GrossWeight = 0.00M;
                    lastLoadScan.NetWeight = 0.00M;
                    lastLoadScan.SplitWeight1 = 0.00M;
                    lastLoadScan.SplitWeight2 = 0.00M;
                    lastLoadScan.Latitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LATITUDE);
                    lastLoadScan.Longitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LONGITUDE);

                    lastLoadScan.TrailerNumber = "";
                    lastLoadScan.Variety = "";
                    lastLoadScan.YardRow = "IN FIELD";
                    lastLoadScan.PickedBy = "";

                    lastLoadScan.Client = "";
                    lastLoadScan.Farm = "";
                    lastLoadScan.Field = "";

                    var status = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.TARGET_STATUS, "AT GIN");

                    if (status == "AT GIN")
                    {
                        lastLoadScan.TargetStatus = ModuleStatus.AT_GIN;
                    }
                    else
                    {
                        lastLoadScan.TargetStatus = ModuleStatus.ON_FEEDER;
                    }
                    lastLoadScan.SubmittedBy = "attendant";
                    //lastLoadScan.SetSerializedModuleScanData(scanData);
                    dp.LoadScanRepository.Save(lastLoadScan);
                    dp.SaveChanges();

                    var vm = new LoadViewModel(NavService);
                    NavService.ShowPage(PageType.LOAD_PAGE, false, (BasePageViewModel)vm);
                    vm.Initialize(GinTicketLoadNumber, false, true);
                }
            }
        }

        private void ExecuteViewLoadList()
        {
            var vm = new ListViewModel(NavService);
            NavService.ShowPage(PageType.LOAD_LIST_PAGE, false, (BasePageViewModel)vm);
            vm.Initialize();
        }

        private void handleBarCodeScanned(BarcodeScannedMessage msg)
        {
            string value = Regex.Replace(msg.Data, "[A-Za-z]", "");
            GinTicketLoadNumber = value;
            this.ExecuteLookupLoad();
        }

        private void handleInMotionMessage(InMotionMessage msg)
        {
            try
            {
                TagDataProvider.SetGPOState(3, true);
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        private void handleWeightAcquired(WeightAcquiredMessage msg)
        {
            try
            {
                TagDataProvider.SetGPOState(3, false);
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        private void handleTagsReported(List<TagItem> tagsReported)
        {
            try
            {
                TagItem truckTag = null;
                TruckEntity selectedTruck = null;

                lock (_trucks)
                {
                    /*foreach (var tag in tagsReported)
                    {
                        Logging.Logger.Log("INFO", "EPC: " + tag.Epc + " SN: " + tag.SerialNumber + " ANTENNA PORT: " + tag.AntennaePort.ToString() + " FIRST SEEN LOCAL TIME: " + tag.Firstseen.ToLocalTime().ToString() + " PEAK RSSI: " + tag.PeakRSSI.ToString() + " PHASE ANGLE: " + tag.PhaseAngle.ToString());
                    }*/

                    var truckTagIds = _trucks.Select(t => t.RFIDTagId).ToArray();
                    truckTag = tagsReported.FirstOrDefault(t => truckTagIds.Contains(t.SerialNumber) || truckTagIds.Contains(t.Epc));

                    if (truckTag != null)
                    {
                        using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                        {
                            selectedTruck = dp.TruckRepository.FindSingle(t => t.RFIDTagId == truckTag.SerialNumber || t.RFIDTagId == truckTag.Epc);
                            var lastLoad = dp.LoadScanRepository.LastLoad();
                            if (lastLoad != null && lastLoad.TruckID == selectedTruck.Name && lastLoad.Created.AddMinutes(15) > DateTime.UtcNow)
                            {
                                selectedTruck = null;
                                Logging.Logger.Log("INFO", "Truck Tag ignored: " + truckTag.SerialNumber);
                                TagDataProvider.ClearBuffer();
                            }
                        }
                    }

                    if (selectedTruck != null)
                    {
                        Logging.Logger.Log("INFO", "Truck Tag for truck: " + selectedTruck.Name + " scanned.  Starting weigh in.");
                        //check to see if this truck
                        var vm = new WeighInPageViewModel(NavService);
                        //TagDataProvider.ClearBuffer();
                        NavService.ShowPage(PageType.WEIGHT_IN_PAGE, false, (BasePageViewModel)vm);
                        vm.Initialize(selectedTruck);
                    }
                    else
                    {
                        //TagDataProvider.ClearBuffer();
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }
    }
}
