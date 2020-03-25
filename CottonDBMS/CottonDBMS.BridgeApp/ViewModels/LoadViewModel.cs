using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.BridgeApp.Navigation;
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
using CottonDBMS.Bridges.Shared.Messages;

namespace CottonDBMS.BridgeApp.ViewModels
{


    public class LoadViewModel : BasePageViewModel
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

        private string _GinTicketLoadNumberErrorMessage;
        public string GinTicketLoadNumberErrorMessage
        {
            get
            {
                return _GinTicketLoadNumberErrorMessage;
            }
            set
            {
                Set<string>(() => GinTicketLoadNumberErrorMessage, ref _GinTicketLoadNumberErrorMessage, value);
            }
        }

        private bool _HasGinTicketLoadNumberError;
        public bool HasGinTicketLoadNumberError
        {
            get
            {
                return _HasGinTicketLoadNumberError;
            }
            set
            {
                Set<bool>(() => HasGinTicketLoadNumberError, ref _HasGinTicketLoadNumberError, value);
            }
        }

        private int _BridgeLoadNumber;
        public int BridgeLoadNumber
        {
            get
            {
                return _BridgeLoadNumber;
            }
            set
            {
                Set<int>(() => BridgeLoadNumber, ref _BridgeLoadNumber, value);
            }
        }

        private bool _attendantChecked;
        public bool AttendantChecked
        {
            get
            {
                return _attendantChecked;
            }
            set
            {
                Set<bool>(() => AttendantChecked, ref _attendantChecked, value);
            }
        }

        private bool _driverChecked;
        public bool DriverChecked
        {
            get
            {
                return _driverChecked;
            }
            set
            {
                Set<bool>(() => DriverChecked, ref _driverChecked, value);
            }
        }

        private bool _HasAttendantDriverError;
        public bool HasAttendantDriverError
        {
            get
            {
                return _HasAttendantDriverError;
            }
            set
            {
                Set<bool>(() => HasAttendantDriverError, ref _HasAttendantDriverError, value);
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

        private string _PickedBy;
        public string PickedBy
        {
            get
            {
                return _PickedBy;
            }
            set
            {
                Set<string>(() => PickedBy, ref _PickedBy, value);
            }
        }

        private string _YardLocation;
        public string YardLocation
        {
            get
            {
                return _YardLocation;
            }
            set
            {
                Set<string>(() => YardLocation, ref _YardLocation, value);
            }
        }

        private string _YardLocationErrorMessage;
        public string YardLocationErrorMessage
        {
            get
            {
                return _YardLocationErrorMessage;
            }
            set
            {
                Set<string>(() => YardLocationErrorMessage, ref _YardLocationErrorMessage, value);
            }
        }

        private bool _HasYardLocationError;
        public bool HasYardLocationError
        {
            get
            {
                return _HasYardLocationError;
            }
            set
            {
                Set<bool>(() => HasYardLocationError, ref _HasYardLocationError, value);
            }
        }

        private string _TrailerNumber;
        public string TrailerNumber
        {
            get
            {
                return _TrailerNumber;
            }
            set
            {
                Set<string>(() => TrailerNumber, ref _TrailerNumber, value);
            }
        }

        private string _TruckID;
        public string TruckID
        {
            get
            {
                return _TruckID;
            }
            set
            {
                Set<string>(() => TruckID, ref _TruckID, value);

                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var truck = allTrucks.SingleOrDefault(t => t.Name == _TruckID);

                    if (truck != null)
                    {
                        _tareWeight = truck.TareWeight;
                    }
                    else
                    {
                        _tareWeight = 0.00M;
                    }

                    decimal temp = 0.00M;
                    if (decimal.TryParse(GrossWeight, out temp))
                        NetWeight = temp - _tareWeight;
                    else
                    {
                        NetWeight = temp - _tareWeight;
                    }
                }
            }
        }

        private string _TruckIDErrorMessage;
        public string TruckIDErrorMessage
        {
            get
            {
                return _TruckIDErrorMessage;
            }
            set
            {
                Set<string>(() => TruckIDErrorMessage, ref _TruckIDErrorMessage, value);
            }
        }

        private bool _HasTruckIDError;
        public bool HasTruckIDError
        {
            get
            {
                return _HasTruckIDError;
            }
            set
            {
                Set<bool>(() => HasTruckIDError, ref _HasTruckIDError, value);
            }
        }

        public ObservableCollection<string> AvailableTrucks { get; private set; }

        private string _GrossWeight;
        public string GrossWeight
        {
            get
            {
                return _GrossWeight;
            }
            set
            {
                Set<string>(() => GrossWeight, ref _GrossWeight, value);

                decimal temp = 0.00M;
                if (decimal.TryParse(value, out temp))
                    NetWeight = temp - _tareWeight;
                else
                {
                    NetWeight = temp - _tareWeight;
                }
            }
        }
              


        private decimal _NetWeight;
        public decimal NetWeight
        {
            get
            {
                return _NetWeight;
            }
            set
            {
                Set<decimal>(() => NetWeight, ref _NetWeight, value);


            }
        }

        private string _GrossWeightErrorMessage;
        public string GrossWeightErrorMessage
        {
            get
            {
                return _GrossWeightErrorMessage;
            }
            set
            {
                if (ValidationHelper.ValidDecimal(value))
                {
                    NetWeight = decimal.Parse(value) - _tareWeight;
                }
                else
                {
                    NetWeight = 0.00M;
                }

                Set<string>(() => GrossWeightErrorMessage, ref _GrossWeightErrorMessage, value);
            }
        }

        private bool _HasGrossWeightError;
        public bool HasGrossWeightError
        {
            get
            {
                return _HasGrossWeightError;
            }
            set
            {
                Set<bool>(() => HasGrossWeightError, ref _HasGrossWeightError, value);
            }
        }

        private string _SplitWeight1;
        public string SplitWeight1
        {
            get
            {
                return _SplitWeight1;
            }
            set
            {
                Set<string>(() => SplitWeight1, ref _SplitWeight1, value);

                decimal w1 = 0.00M;
                decimal w2 = 0.00M;

                if (!decimal.TryParse(_SplitWeight1, out w1)) w1 = 0.00M;
                if (!decimal.TryParse(_SplitWeight2, out w2)) w2 = 0.00M;

                GrossWeight = (w1 + w2).ToString();
            }
        }

        private string _SplitWeight1ErrorMessage;
        public string SplitWeight1ErrorMessage
        {
            get
            {
                return _SplitWeight1ErrorMessage;
            }
            set
            {
                Set<string>(() => SplitWeight1ErrorMessage, ref _SplitWeight1ErrorMessage, value);
            }
        }

        private bool _HasSplitWeight1Error;
        public bool HasSplitWeight1Error
        {
            get
            {
                return _HasSplitWeight1Error;
            }
            set
            {
                Set<bool>(() => HasSplitWeight1Error, ref _HasSplitWeight1Error, value);
            }
        }

        private string _SplitWeight2;
        public string SplitWeight2
        {
            get
            {
                return _SplitWeight2;
            }
            set
            {   
                Set<string>(() => SplitWeight2, ref _SplitWeight2, value);

                decimal w1 = 0.00M;
                decimal w2 = 0.00M;

                if (!decimal.TryParse(_SplitWeight1, out w1)) w1 = 0.00M;
                if (!decimal.TryParse(_SplitWeight2, out w2)) w2 = 0.00M;

                GrossWeight = (w1 + w2).ToString();
            }
        }

        private string _SplitWeight2ErrorMessage;
        public string SplitWeight2ErrorMessage
        {
            get
            {
                return _SplitWeight2ErrorMessage;
            }
            set
            {
                Set<string>(() => SplitWeight2ErrorMessage, ref _SplitWeight2ErrorMessage, value);
            }
        }

        private bool _HasSplitWeight2Error;
        public bool HasSplitWeight2Error
        {
            get
            {
                return _HasSplitWeight2Error;
            }
            set
            {
                Set<bool>(() => HasSplitWeight2Error, ref _HasSplitWeight2Error, value);
            }
        }

        private string _ModuleHeader;
        public string ModuleHeader
        {
            get
            {
                return _ModuleHeader;
            }
            set
            {
                Set<string>(() => ModuleHeader, ref _ModuleHeader, value);
            }
        }

        private string _SerialNumberToAdd;
        public string SerialNumberToAdd
        {
            get
            {
                return _SerialNumberToAdd;
            }
            set
            {
                Set<string>(() => SerialNumberToAdd, ref _SerialNumberToAdd, value);
            }
        }

        private string _AddSerialErrorMessage;
        public string AddSerialErrorMessage
        {
            get
            {
                return _AddSerialErrorMessage;
            }
            set
            {
                Set<string>(() => AddSerialErrorMessage, ref _AddSerialErrorMessage, value);
            }
        }

        private bool _HasAddSerialError;
        public bool HasAddSerialError
        {
            get
            {
                return _HasAddSerialError;
            }
            set
            {
                Set<bool>(() => HasAddSerialError, ref _HasAddSerialError, value);
            }
        }

        private ObservableCollection<LoadModuleScan> _Modules;
        public ObservableCollection<LoadModuleScan> Modules
        {
            get { return _Modules; }
            set
            {
                _Modules = value;
                RaisePropertyChanged("Modules");
            }
        }
        #endregion

        private System.Timers.Timer timer = null;
        private bool executingTimer = false;
        private LoadScanEntity _scan = null;
        
        private bool _isManualEdit = false;

        private string _loadScanId = string.Empty;

        private bool _initialized = false;
        private bool _popPageOnSave = false;

        private List<TruckEntity> allTrucks = new List<TruckEntity>();
        private decimal _tareWeight = 0.00M;

        private void ClearErrors()
        {
            HasAttendantDriverError = false;
            HasGinTicketLoadNumberError = false;
            HasGrossWeightError = false;
            HasSplitWeight1Error = false;
            HasSplitWeight2Error = false;
            HasYardLocationError = false;
            HasTruckIDError = false;
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            ClearErrors();

            if (!AttendantChecked && !DriverChecked)
            {
                HasAttendantDriverError = true;
                isValid = false;
            }

            if (string.IsNullOrEmpty(YardLocation))
            {
                HasYardLocationError = true;
                YardLocationErrorMessage = "required";
                isValid = false;
            }

            if (string.IsNullOrEmpty(GrossWeight))
            {
                HasGrossWeightError = true;
                GrossWeightErrorMessage = "required";
                isValid = false;
            }
            else if (!ValidationHelper.ValidDecimal(GrossWeight))
            {
                HasGrossWeightError = true;
                GrossWeightErrorMessage = "Invalid weight.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(SplitWeight1) || !ValidationHelper.ValidDecimal(SplitWeight1))
            {
                HasSplitWeight1Error = true;
                SplitWeight1ErrorMessage = "Invalid weight.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(SplitWeight2) || !ValidationHelper.ValidDecimal(SplitWeight2))
            {
                HasSplitWeight2Error = true;
                SplitWeight2ErrorMessage = "Invalid weight.";
                isValid = false;
            }

            if (string.IsNullOrEmpty(TruckID) || TruckID == "-- Select One --")
            {
                HasTruckIDError = true;
                TruckIDErrorMessage = "required";
                isValid = false;
            }

            return isValid;
        }

        public LoadViewModel(INavigationService navService) : base(navService)
        {
            SaveCommand = new RelayCommand(this.ExecuteSaveCommand);
            CancelCommand = new RelayCommand(this.ExecuteCancelCommand);
            RemoveModuleCommand = new RelayCommand<string>(this.ExecuteRemoveModuleCommand);
            AddSerialNumberCommand = new RelayCommand(this.ExecuteAddSerialNumber);
            Modules = new ObservableCollection<LoadModuleScan>();
            AvailableTrucks = new ObservableCollection<string>();

            HasAddSerialError = false;
            Messenger.Default.Register<InactiveMessage>(this, handleInactiveMessage);

            _initialized = false;

            //Remove autosave
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = 30000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!executingTimer && _initialized)
            {
                executingTimer = true;
                //SaveForm(true);
                executingTimer = false;
            }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<InactiveMessage>(this, handleInactiveMessage);
            timer.Enabled = false;
            base.Cleanup();
        }

        public void Initialize(string ginTicketLoadNumber, bool newLoad, bool isManualEdit = false, bool popPageOnSave=false, string loadGuid = null)
        {
            _isManualEdit = isManualEdit;
            _initialized = false;
            _popPageOnSave = popPageOnSave;
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                GinTicketLoadNumber = ginTicketLoadNumber;
                var loads = dp.LoadScanRepository.FindMatching(t => t.GinTagLoadNumber == ginTicketLoadNumber);

                if (!string.IsNullOrEmpty(loadGuid))
                {
                    loads = dp.LoadScanRepository.FindMatching(t => t.Id == loadGuid);
                }

                var trucks = dp.TruckRepository.GetAll();
                allTrucks = trucks.ToList();
                var ownerships = dp.ModuleOwnershipRepository.FindMatching(m => m.GinTagLoadNumber == GinTicketLoadNumber).ToList().OrderBy(t => t.LastCreatedOrUpdated);
                //var lastOwnership = ownerships.LastOrDefault();
                _scan = null;

                AvailableTrucks.Clear();
                AvailableTrucks.Add("-- Select One --");
                foreach (var t in trucks)
                    AvailableTrucks.Add(t.Name);
                
                if (loads.Count() > 0)
                {
                    _scan = loads.Last();
                    _loadScanId = _scan.Id;

                    TruckID = _scan.TruckID;
                    var truck = dp.TruckRepository.FindSingle(t => t.Name == TruckID);
                                        
                    if (truck != null)
                    {
                        _tareWeight = truck.TareWeight;
                    }
                    else
                    {
                        _tareWeight = 0.00M;
                        TruckID = "-- Select One --";
                    }

                    BridgeLoadNumber = _scan.BridgeLoadNumber;
                    GrossWeight = _scan.GrossWeight.ToString();
                    NetWeight =  _scan.NetWeight;                    
                    PickedBy = _scan.PickedBy;
                    Variety = _scan.Variety;
                    YardLocation = _scan.YardRow;
                    TrailerNumber = _scan.TrailerNumber;
                    Client = _scan.Client;
                    Farm = _scan.Farm;
                    Field = _scan.Field;
                    SplitWeight1 = _scan.SplitWeight1.ToString();
                    SplitWeight2 = _scan.SplitWeight2.ToString();

                    /*if (isManualEdit && lastOwnership != null && lastOwnership.LastCreatedOrUpdated > _scan.LastCreatedOrUpdated.AddMinutes(10))
                        
                    {
                        TrailerNumber = lastOwnership.TrailerNumber;
                        Variety = lastOwnership.Variety;
                        YardLocation = lastOwnership.Location;
                        Client = lastOwnership.Client;
                        Farm = lastOwnership.Farm;
                        Field = lastOwnership.Field;
                        GrossWeight = lastOwnership.LoadGrossWeight.ToString();
                        NetWeight = lastOwnership.LoadNetWeight;
                        TruckID = lastOwnership.TruckID;
                        PickedBy = lastOwnership.PickedBy;
                        SplitWeight1 = lastOwnership.LoadSplitWeight1.ToString();
                        SplitWeight2 = lastOwnership.LoadSplitWeight2.ToString();
                    }*/

                    if (!newLoad)
                    {
                        AttendantChecked = (_scan.SubmittedBy == "attendant");
                        DriverChecked = (_scan.SubmittedBy == "driver");
                    }
                    else
                    {
                        DriverChecked = false;
                        AttendantChecked = true;
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                    Modules = new ObservableCollection<LoadModuleScan>();
                    foreach (var m in _scan.ScanData.Scans)
                    {
                        //filter out truck tags
                        if (!trucks.Any(t => t.RFIDTagId == m.SerialNumber || t.RFIDTagId == m.EPC))
                            Modules.Add(m);
                    }
                    }));

                    ModuleHeader = Modules.Count().ToString() + " MODULES IN LOAD";
                    
                }
                else
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        System.Windows.MessageBox.Show("Load not found or not saved.");                        
                    }));
                    throw new Exception("No load found for ticket number.");
                }
            }

            _initialized = true;
        }

        public RelayCommand SaveCommand { get; private set; }

        private void ExecuteSaveCommand()
        {
            ClearErrors();
            if (ValidateForm())
            {
                if (_isManualEdit)
                {
                    Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Saving..." });
                }
                SaveForm(false);
                if (_isManualEdit)
                {
                    Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "Saving..." });
                    
                    if (!_popPageOnSave)
                    {                        
                        var vm = new IdlePageViewModel(NavService);
                        NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
                        vm.Initialize();
                    }
                    else
                    {
                        NavService.Pop();
                    }
                }
                else
                {
                    var vm = new ExitScalePageViewModel(NavService);
                    NavService.ShowPage(PageType.EXIT_SCALE_PAGE, false, vm);
                    vm.Initialize();
                }
            }
        }

        public RelayCommand CancelCommand { get; private set; }

        private void ExecuteCancelCommand()
        {
            if (!_popPageOnSave)
            {
                var vm = new IdlePageViewModel(NavService);
                NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
                vm.Initialize();
            }
            else
            {
                NavService.Pop();
            }
        }

        private void SaveForm(bool isAutoSave)
        {
            try
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    LoadScanEntity loadScan = null;
                    var bridgeID = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.BRIDGE_ID, "");

                    //try to look load up by gin tag number to update matching load
                   // if (!isAutoSave)
                   //     loadScan = dp.LoadScanRepository.FindSingle(l => l.GinTagLoadNumber == GinTicketLoadNumber);

                    if (loadScan == null)
                    {
                        loadScan = dp.LoadScanRepository.GetById(_loadScanId);
                        loadScan.GinTagLoadNumber = GinTicketLoadNumber;
                    }

                    loadScan.BridgeID = bridgeID;

                    if (TruckID == "-- Select One --")
                        _TruckID = "";

                    loadScan.TruckID = TruckID;

                    if (!isAutoSave)
                        loadScan.GinTagLoadNumber = GinTicketLoadNumber;

                    var grossWeight = 0.00M;
                    var split1 = 0.00M;
                    var split2 = 0.00M;

                    if (decimal.TryParse(GrossWeight, out grossWeight))
                        loadScan.GrossWeight = grossWeight;

                    if (decimal.TryParse(SplitWeight1, out split1))
                        loadScan.SplitWeight1 = split1;

                    if (decimal.TryParse(SplitWeight2, out split2))
                        loadScan.SplitWeight2 = split2;

                    loadScan.NetWeight = loadScan.GrossWeight - _tareWeight;
                    loadScan.Latitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LATITUDE);
                    loadScan.Longitude = dp.SettingsRepository.GetSettingDoubleValue(BridgeSettingKeys.LONGITUDE);

                    loadScan.PickedBy = PickedBy;
                    loadScan.Processed = false;

                    var status = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.TARGET_STATUS, "AT GIN");
                    if (status == "AT GIN")
                        loadScan.TargetStatus = ModuleStatus.AT_GIN;
                    else
                        loadScan.TargetStatus = ModuleStatus.ON_FEEDER;

                    loadScan.TrailerNumber = TrailerNumber;
                    loadScan.Variety = Variety;
                    loadScan.YardRow = YardLocation;
                    if (AttendantChecked) loadScan.SubmittedBy = "attendant";
                    if (DriverChecked) loadScan.SubmittedBy = "driver";
                    loadScan.Client = Client;
                    loadScan.Farm = Farm;
                    loadScan.Field = Field;

                    var moduleScanData = new ModuleScanData();
                    moduleScanData.Scans = new List<LoadModuleScan>();
                    moduleScanData.Scans.AddRange(Modules.ToList());
                    loadScan.SetSerializedModuleScanData(moduleScanData);

                    dp.LoadScanRepository.Save(loadScan);
                    dp.SaveChanges();

                    Messenger.Default.Send<LoadSavedMessage>(new LoadSavedMessage { Scan = loadScan });
                }
            }
            catch (Exception exc)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (!isAutoSave)
                    {
                        System.Windows.MessageBox.Show("An error occured saving load. " + exc.Message);
                    }
                }));
                Logging.Logger.Log(exc);
            }
        }

        public RelayCommand<string> RemoveModuleCommand { get; private set; }

        private void ExecuteRemoveModuleCommand(string serialNumber)
        {
            var moduleToRemove = Modules.FirstOrDefault(m => m.SerialNumber == serialNumber);

            if (moduleToRemove != null)
                Modules.Remove(moduleToRemove);

            ModuleHeader = Modules.Count().ToString() + " MODULES IN LOAD";
        }

        public RelayCommand AddSerialNumberCommand { get; private set; }

        private void ExecuteAddSerialNumber()
        {
            if (string.IsNullOrWhiteSpace(SerialNumberToAdd))
            {
                HasAddSerialError = true;
                AddSerialErrorMessage = "Please enter a serial number.";
            }
            else if (Modules.Any(t => t.SerialNumber == SerialNumberToAdd.Trim()))
            {
                AddSerialErrorMessage = "Modules is already in list.";
                HasAddSerialError = true;
            }
            else
            {
                HasAddSerialError = false;
                LoadModuleScan scan = new LoadModuleScan
                {
                    EPC = SerialNumberToAdd,
                    SerialNumber = SerialNumberToAdd,
                    ScanTime = DateTime.UtcNow
                };

                Modules.Add(scan);
                ModuleHeader = Modules.Count().ToString() + " IN LOAD";
            }
            SerialNumberToAdd = "";
        }

        private void handleInactiveMessage(InactiveMessage msg)
        {
            if (!_popPageOnSave)
            {
                SaveForm(true);
                var vm = new IdlePageViewModel(NavService);
                NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
                vm.Initialize();
            }
            else
            {
                NavService.Pop();
            }
        }
    }
}
