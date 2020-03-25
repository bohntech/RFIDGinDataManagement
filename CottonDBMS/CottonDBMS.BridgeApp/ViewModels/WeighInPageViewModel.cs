using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.BridgeApp.Navigation;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.RFID;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.ViewModels;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges.Shared.Messages;
using System.Text.RegularExpressions;

namespace CottonDBMS.BridgeApp.ViewModels
{

    public class WeighInPageViewModel : BasePageViewModel
    {
        //private object _dataLocker = new object();
        private bool _canceled = false;
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

        private bool _loadCreated = false;

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

        private bool _isSplitWeigh;
        public bool IsSplitWeight
        {
            get
            {
                return _isSplitWeigh;
            }
            set
            {
                Set<bool>(() => IsSplitWeight, ref _isSplitWeigh, value);
            }
        }

        private bool _isSplitCanceled;
        public bool IsSplitCanceled
        {
            get
            {
                return _isSplitCanceled;
            }
            set
            {
                Set<bool>(() => IsSplitCanceled, ref _isSplitCanceled, value);
            }
        }

        private bool _HasLookupError;
        public bool HasLookupError
        {
            get
            {
                return _HasLookupError;
            }
            set
            {
                Set<bool>(() => HasLookupError, ref _HasLookupError, value);
            }
        }


        private decimal _grossWeight;
        public decimal GrossWeight
        {
            get
            {
                return _grossWeight;
            }
            set
            {                
                Set<decimal>(() => GrossWeight, ref _grossWeight, value);
            }
        }

        private decimal _weight1;
        public decimal Weight1
        {
            get
            {
                return _weight1;
            }
            set
            {
                Set<decimal>(() => Weight1, ref _weight1, value);
            }
        }

        private decimal _weight2;
        public decimal Weight2
        {
            get
            {
                return _weight2;
            }
            set
            {
                Set<decimal>(() => Weight2, ref _weight2, value);
            }
        }

        private TruckEntity _activeTruck = null;        
        private bool _weight1Acquired = false;
        private bool _weight2Acquired = false;
        private bool _waitingForMotion = false;
        private DateTime _readerWindowStartTime = DateTime.Now;
        private List<TruckEntity> _allTrucks = new List<TruckEntity>();
        private bool _yellowLightOn = false;
        private int zeroWeightCount = 0;
        private bool waitingForExit = false;
        private System.Timers.Timer timer = null;
        private System.Timers.Timer autoSaveTimer = null;
        private bool isUnattendedMode = false;
        private bool _initialized = false;

        public WeighInPageViewModel(INavigationService navService) : base(navService)
        {
            _initialized = false;

            LookupLoadCommand = new RelayCommand(this.ExecuteLookupLoad);
            CancelSplitWeightCommand = new RelayCommand(this.ExecuteCancelSplit);
            CancelCommand = new RelayCommand(this.ExecuteCancelCommand);
            Messenger.Default.Register<ScaleWeightReportMessage>(this, handleWeightReported);
            Messenger.Default.Register<WeightAcquiredMessage>(this, handleWeightAcquired);
            Messenger.Default.Register<InMotionMessage>(this, handleInMotion);
            Messenger.Default.Register<BarcodeScannedMessage>(this, handleBarCodeScanned);
            Messenger.Default.Register<List<TagItem>>(this, handleTagsReported);
            _readerWindowStartTime = DateTime.UtcNow.AddSeconds(-15);
            Weight1 = Weight2 = GrossWeight = 0.00M;
            IsSplitCanceled = false;
        }

        public void Initialize(TruckEntity selectedTruck)
        {
            Logging.Logger.Log("INFO", "Entering Weighin Initialize");
            //lock (_dataLocker)
            //{
                _activeTruck = selectedTruck;
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    isUnattendedMode = bool.Parse(dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.UNATTENDED_MODE, "FALSE"));

                    GinName = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.GIN_NAME, "");
                    _allTrucks = dp.TruckRepository.GetAll().ToList();
                    WelcomeMessage = selectedTruck.Name + " PULLING ONTO SCALE";
                    IsSplitWeight = _activeTruck.IsSemi;
                    IsSplitCanceled = !_activeTruck.IsSemi;
                    _yellowLightOn = false;
                    zeroWeightCount = 0;
                    if (TagDataProvider.TagsInBuffer() >= 1)
                    {
                        TagDataProvider.SetGPOState(1, true);
                        _yellowLightOn = true;
                        TagDataProvider.SetGPOState(2, false);
                        TagDataProvider.SetGPOState(3, false);
                    }

                    timer = new System.Timers.Timer();
                    timer.Interval = int.Parse(dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.WEIGH_IN_TIMEOUT, "30")) * 1000;
                    timer.AutoReset = false;
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();

                    autoSaveTimer = new System.Timers.Timer();
                    autoSaveTimer.Interval = int.Parse(dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.WEIGHT_AUTO_SAVE_TIMEOUT, "10")) * 60 * 1000;
                    autoSaveTimer.AutoReset = false;
                    autoSaveTimer.Elapsed += AutoSaveTimer_Elapsed; ;
                    autoSaveTimer.Start();
                }

                _initialized = true;
            //}
        }

        private void AutoSaveTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (_canceled || _loadCreated) return;
                
                autoSaveTimer.Enabled = false;
                autoSaveTimer.Stop();

                Logging.Logger.Log("INFO", "Entering AutoSave");
                //lock (_dataLocker)
                //{
                    bool newLoad = true;

                    if (!_loadCreated)
                    {
                        setGinTicketToAuto();
                        Logging.Logger.Log("INFO", "CREATE LOAD FROM AUTO SAVE");
                        CreateLoad(ref newLoad);
                    }

                    TagDataProvider.SetGPOState(1, false);
                    TagDataProvider.SetGPOState(2, false);
                    TagDataProvider.SetGPOState(3, false);

                    ExitToIdlePage();
                //}
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Enabled = false;
            Logging.Logger.Log("INFO", "Cancelling from weigh in time out.  No motion seen soon enough.");
            ExecuteCancelCommand();
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<ScaleWeightReportMessage>(this);
            Messenger.Default.Unregister<WeightAcquiredMessage>(this);
            Messenger.Default.Unregister<InMotionMessage>(this);
            Messenger.Default.Unregister<BarcodeScannedMessage>(this);
            Messenger.Default.Unregister<List<TagItem>>(this, handleTagsReported);

            TagDataProvider.SetGPOState(1, false);
            TagDataProvider.SetGPOState(2, false);
            TagDataProvider.SetGPOState(3, false);

            base.Cleanup();
        }

        public RelayCommand LookupLoadCommand { get; private set; }

        private void ExecuteLookupLoad()
        {
            try
            {
                if (_loadCreated) return;

                _canceled = true;
                timer.Stop();
                autoSaveTimer.Stop();

                Logging.Logger.Log("INFO", "Entering ExecuteLookupLoad");
                //lock (_dataLocker)
                //{
                    bool newLoad = false;

                    if (string.IsNullOrWhiteSpace(GinTicketLoadNumber))
                    {
                        HasLookupError = true;
                        return;
                    }
                    else
                    {
                        HasLookupError = false;
                    }
                    Logging.Logger.Log("INFO", "CREATE LOAD FROM LOAD LOOKUP");

                    if (!_loadCreated)
                    {
                        CreateLoad(ref newLoad);
                    }

                    TagDataProvider.SetGPOState(1, false);
                    TagDataProvider.SetGPOState(2, false);
                    TagDataProvider.SetGPOState(3, false);

                    var vm = new LoadViewModel(NavService);
                    NavService.ShowPage(PageType.LOAD_PAGE, false, (BasePageViewModel)vm);
                    vm.Initialize(GinTicketLoadNumber, newLoad);
                //}
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        private void CreateLoad(ref bool newLoad)
        {
            _loadCreated = true;
            timer.Stop();
            autoSaveTimer.Stop();
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                Logging.Logger.Log("INFO", "CREATING LOAD FROM WEIGH IN: LOAD " + GinTicketLoadNumber);

                //find module ownerships
                var bridgeID = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.BRIDGE_ID, "");
                var ownerships = dp.ModuleOwnershipRepository.FindMatching(m => m.GinTagLoadNumber == GinTicketLoadNumber).ToList().OrderBy(t => t.LastCreatedOrUpdated);
                var lastOwnership = ownerships.LastOrDefault();
                var lastLoadScan = dp.LoadScanRepository.FindMatching(l => l.GinTagLoadNumber == GinTicketLoadNumber).OrderBy(t => t.Created).LastOrDefault();
                var lastLoadScanTime = (lastLoadScan != null) ? lastLoadScan.LastCreatedOrUpdated : DateTime.Now.AddYears(-1);

                ModuleScanData scanData = new ModuleScanData();
                scanData.Scans = new List<LoadModuleScan>();
                Logging.Logger.Log("INFO", "GET TAGS SEEN BETWEEN " + _readerWindowStartTime.ToLocalTime().ToString() + " AND " + DateTime.Now.ToString());
                var tags = TagDataProvider.GetTagsFirstSeenInTimeRange(_readerWindowStartTime, DateTime.UtcNow);
                var tagSerials = tags.Select(t => t.SerialNumber).ToArray();
                var ginnedTags = dp.ModuleOwnershipRepository.FindMatching(m => tagSerials.Contains(m.Name) && (m.Status == "Ginned" || m.Status == "On feeder"));

                if (ginnedTags.Count() > 0)
                {
                    Logging.Logger.Log("WARNING", "ONE OR MORE TAGS READ WITH GINNED STATUS.  GINNED COUNT: " + ginnedTags.Count().ToString());
                }

                var trucks = dp.TruckRepository.GetAll();

                foreach (var t in tags)
                {
                    if (!ginnedTags.Any(g => g.Name == t.SerialNumber) && !trucks.Any(x => x.RFIDTagId == t.SerialNumber || x.RFIDTagId == t.Epc))
                    {

                        Logging.Logger.Log("INFO", "RECORDING MODULE SN " + t.SerialNumber + " FIRST SEEN AT LOCAL TIME" + t.Firstseen.ToLocalTime().ToString());
                        scanData.Scans.Add(new LoadModuleScan
                        {
                            EPC = t.Epc,
                            SerialNumber = t.SerialNumber,
                            ScanTime = t.Firstseen
                        });
                    }
                }

                var serialNumbers = tags.Select(t => t.SerialNumber).ToArray();
                var moduleOwnershipsForTags = dp.ModuleOwnershipRepository.FindMatching(m => serialNumbers.Contains(m.Name)).ToList();

                if (moduleOwnershipsForTags.Count() > 0 && lastOwnership == null)
                {
                    lastOwnership = moduleOwnershipsForTags[0];
                    Logging.Logger.Log("INFO", "Found ownership from module.");
                }

                if (lastOwnership != null)
                {
                    Logging.Logger.Log("INFO", "Using ownership " + lastOwnership.Client + "/" + lastOwnership.Farm + "/" + lastOwnership.Field);
                }

                bool populateFromOwnership = (lastOwnership != null && lastLoadScan == null);

                //if ((lastOwnership != null && lastLoadScan == null)/* || (lastOwnership != null && lastOwnership.LastCreatedOrUpdated > lastLoadScan.LastCreatedOrUpdated)*/)
                //    populateFromOwnership = true;

                if (lastLoadScan == null)
                {
                    Logging.Logger.Log("INFO", "CREATING NEW LOAD SCAN");
                    newLoad = true;
                    lastLoadScan = new LoadScanEntity();
                    lastLoadScan.SubmittedBy = "attendant";

                    //always use a new load number otherwise a previous load
                    //could get overwritten
                    lastLoadScan.BridgeLoadNumber = dp.LoadScanRepository.LastLoadNumber() + 1;

                    /*if (populateFromOwnership)
                        if (lastOwnership.BridgeLoadNumber > 0) lastLoadScan.BridgeLoadNumber = lastOwnership.BridgeLoadNumber;
                        else lastLoadScan.BridgeLoadNumber = dp.LoadScanRepository.LastLoadNumber() + 1;
                    else
                        lastLoadScan.BridgeLoadNumber = dp.LoadScanRepository.LastLoadNumber() + 1;
                        */
                }

                lastLoadScan.BridgeID = bridgeID;
                lastLoadScan.TruckID = _activeTruck.Name;
                lastLoadScan.GinTagLoadNumber = GinTicketLoadNumber;
                lastLoadScan.GrossWeight = GrossWeight;
                lastLoadScan.NetWeight = GrossWeight - _activeTruck.TareWeight;
                lastLoadScan.SplitWeight1 = Weight1;
                lastLoadScan.SplitWeight2 = Weight2;
                lastLoadScan.Latitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LATITUDE);
                lastLoadScan.Longitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LONGITUDE);

                if (populateFromOwnership && lastOwnership != null)
                {
                    lastLoadScan.TrailerNumber = lastOwnership.TrailerNumber;
                    lastLoadScan.Variety = lastOwnership.Variety;
                    lastLoadScan.YardRow = lastOwnership.Location;
                    lastLoadScan.PickedBy = lastOwnership.PickedBy;
                }

                if (populateFromOwnership && lastOwnership != null)
                {
                    lastLoadScan.Client = lastOwnership.Client;
                    lastLoadScan.Farm = lastOwnership.Farm;
                    lastLoadScan.Field = lastOwnership.Field;
                }

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
                lastLoadScan.SetSerializedModuleScanData(scanData);
                dp.LoadScanRepository.Save(lastLoadScan);
                dp.SaveChanges();

                Logging.Logger.WriteBuffer();
            }
        }

        public RelayCommand CancelSplitWeightCommand { get; private set; }

        private void ExecuteCancelCommand()
        {          
                TagDataProvider.SetGPOState(1, false);
                TagDataProvider.SetGPOState(2, false);
                TagDataProvider.SetGPOState(3, false);
                ExitToIdlePage();           
        }

        private void ExitToIdlePage()
        {
            if (!_canceled)
            {
                _canceled = true;
                var vm = new IdlePageViewModel(NavService);
                NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
                vm.Initialize();
            }
        }

        public RelayCommand CancelCommand { get; private set; }

        private void ExecuteCancelSplit()
        {
            Logging.Logger.Log("INFO", "Entering ExecuteCancelSplit");
            //lock (_dataLocker)
            //{
                IsSplitWeight = false;
                GrossWeight = Weight1;
                Weight2 = 0.00M;
                IsSplitCanceled = true;

                if (WelcomeMessage.Contains("OF 2"))
                    WelcomeMessage = "WEIGHT RECORDED";
            //}
        }

        private void handleWeightReported(ScaleWeightReportMessage msg)
        {
            try
            {
                if (!_initialized || _canceled || _loadCreated) return;  //load was created so we need to ignore events that may have gotten queued
                Logging.Logger.Log("INFO", "Entering handleWeightReported: " + msg.Weight.ToString());
                //lock (_dataLocker)
                //{                   

                    if (!_weight1Acquired)
                    {
                        Weight1 = msg.Weight;
                    }
                    else if (!_weight2Acquired && !_waitingForMotion && IsSplitWeight)
                    {
                        Weight2 = msg.Weight;
                    }

                    if (waitingForExit)
                    {
                        if (msg.Weight < 200) //changed from 100
                        {
                            zeroWeightCount++;
                            Logging.Logger.Log("INFO", "Zero weight count " + zeroWeightCount.ToString());
                        }
                        else
                        {
                            zeroWeightCount = 0;
                        }

                        if (zeroWeightCount >= 5) //changed from 20
                        {
                            TagDataProvider.SetGPOState(2, false);
                            TagDataProvider.SetGPOState(3, false);

                            if (isUnattendedMode && !_loadCreated)
                            {
                                bool newLoad = true;
                                setGinTicketToAuto();
                                Logging.Logger.Log("INFO", "CREATE LOAD WEIGHT REPORTED EVENT");

                                if (!_loadCreated)
                                    CreateLoad(ref newLoad);

                                ExitToIdlePage();
                            }
                        }
                    }                    

                    GrossWeight = Weight1 + Weight2;
                //}
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void handleTagsReported(List<TagItem> tags)
        {
            try
            {
                if (!_initialized || _canceled || _loadCreated) return;

                if (!_yellowLightOn)
                {
                    if (tags.Any(t => !_allTrucks.Any(truck => truck.RFIDTagId == t.SerialNumber || truck.RFIDTagId == t.Epc)))
                    {
                        TagDataProvider.SetGPOState(1, true);
                        _yellowLightOn = true;
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void handleBarCodeScanned(BarcodeScannedMessage msg)
        {
            string value = Regex.Replace(msg.Data, "[A-Za-z]", "");
            GinTicketLoadNumber = value;
            this.ExecuteLookupLoad();
        }

        private void setGinTicketToAuto()
        {          
               GinTicketLoadNumber = "AUTO-" + DateTime.Now.ToString("yyyyMMddHHmmss");          
        }

        private void handleWeightAcquired(WeightAcquiredMessage msg)
        {
            try
            {
                if (!_initialized || _canceled || _loadCreated) return;
                                
                Logging.Logger.Log("INFO", "Entering handleWeightAcquired " + msg.Weight.ToString());
                //lock (_dataLocker)
                //{
                    if (_canceled) return;                    

                    if (!_weight1Acquired)
                    {
                        Weight1 = msg.Weight;
                        _weight1Acquired = true;

                        //if not semi signal weight acquired
                        if (!IsSplitWeight)
                        {
                            WelcomeMessage = "WEIGHT RECORDED";
                            Logging.Logger.Log("INFO", "WEIGHT RECORDED " + msg.Weight.ToString());
                            waitingForExit = true;
                            TagDataProvider.SetGPOState(2, true);
                            TagDataProvider.SetGPOState(3, false);
                        }
                        else
                        {
                            WelcomeMessage = "WEIGHT 1 OF 2 RECORDED";
                            Logging.Logger.Log("INFO", "WEIGHT 1 OF 2 RECORDED " + msg.Weight.ToString());
                            TagDataProvider.SetGPOState(2, true);
                            TagDataProvider.SetGPOState(3, false);
                            _waitingForMotion = true;
                        }
                    }
                    else if (_isSplitWeigh && _weight1Acquired && !_waitingForMotion && !_weight2Acquired)
                    {
                        Weight2 = msg.Weight;
                        _weight2Acquired = true;
                        waitingForExit = true;
                        //signal pull forward
                        WelcomeMessage = "WEIGHT 2 OF 2 RECORDED ";
                        Logging.Logger.Log("INFO", "WEIGHT 2 OF 2 RECORDED " + msg.Weight.ToString());
                        TagDataProvider.SetGPOState(3, false);
                        TagDataProvider.SetGPOState(2, true);
                    }
                //}
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        private void handleInMotion(InMotionMessage msg)
        {
            try
            {

                if (!_initialized || _canceled || _loadCreated) return;

                if (timer != null)
                    timer.Enabled = false;                                

                Logging.Logger.Log("INFO", "Entering handleInMotion");
                //lock (_dataLocker)
                //{

                    if (_canceled) return;

                    TagDataProvider.SetGPOState(2, false);

                    if (zeroWeightCount < 20) TagDataProvider.SetGPOState(3, true);
                    //TagDataProvider.SetGPOState(3, true);

                    if (!_weight1Acquired)
                    {
                        Weight1 = msg.Weight;
                    }
                    else if (_weight1Acquired && _waitingForMotion)
                    {
                        _waitingForMotion = false;
                    }
                    else if (!_weight2Acquired && IsSplitWeight)
                    {
                        Weight2 = msg.Weight;
                    }
                //}
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }
    }
}
