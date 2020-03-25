using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CottonDBMS.Interfaces;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.DataModels;
using CottonDBMS.RFID;
using Impinj.OctaneSdk;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.Bridges.Shared.Tasks;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Cloud;
using System.IO;
using System.IO.Ports;

namespace CottonDBMS.Bridges.Shared.ViewModels
{
    public class SettingsPageViewModel : BasePageViewModel
    {
        private string _originalHost { get; set; }

        public bool IsFirstLaunch { get; set; }

        public bool AllowPortSelection { get; set; }

        public bool AllowPortBarCodeSelection { get; set; }

        public bool AllowWeighInPageTimeOutSelection { get; set; }

        private string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                Set<string>(() => Password, ref _password, value);
            }
        }

        private string _PasswordErrorMessage;
        public string PasswordErrorMessage
        {
            get
            {
                return _PasswordErrorMessage;
            }
            set
            {
                Set<string>(() => PasswordErrorMessage, ref _PasswordErrorMessage, value);
            }
        }

        private bool _HasPasswordErrorMessage;
        public bool HasPasswordErrorMessage
        {
            get
            {
                return _HasPasswordErrorMessage;
            }
            set
            {
                Set<bool>(() => HasPasswordErrorMessage, ref _HasPasswordErrorMessage, value);
            }
        }

        private bool _IsScreenLocked;
        public bool IsScreenLocked
        {
            get
            {
                return _IsScreenLocked;
            }
            set
            {
                Set<bool>(() => IsScreenLocked, ref _IsScreenLocked, value);
            }
        }

        private string _bridgeId;
        public string BridgeID
        {
            get
            {
                return _bridgeId;
            }
            set
            {
                Set<string>(() => BridgeID, ref _bridgeId, value);
            }
        }

        private string _BridgeIDErrorMessage;
        public string BridgeIDErrorMessage
        {
            get
            {
                return _BridgeIDErrorMessage;
            }
            set
            {
                Set<string>(() => BridgeIDErrorMessage, ref _BridgeIDErrorMessage, value);
            }
        }

        private bool _HasBridgeIDErrorMessage;
        public bool HasBridgeIDErrorMessage
        {
            get
            {
                return _HasBridgeIDErrorMessage;
            }
            set
            {
                Set<bool>(() => HasBridgeIDErrorMessage, ref _HasBridgeIDErrorMessage, value);
            }
        }

        private string _screenTitle;
        public string ScreenTitle
        {
            get
            {
                return _screenTitle;
            }
            set
            {
                Set<string>(() => ScreenTitle, ref _screenTitle, value);
            }
        }

        private string _ScreenTitleErrorMessage;
        public string ScreenTitleErrorMessage
        {
            get
            {
                return _ScreenTitleErrorMessage;
            }
            set
            {
                Set<string>(() => ScreenTitleErrorMessage, ref _ScreenTitleErrorMessage, value);
            }
        }

        private bool _HasScreenTitleErrorMessage;
        public bool HasScreenTitleErrorMessage
        {
            get
            {
                return _HasScreenTitleErrorMessage;
            }
            set
            {
                Set<bool>(() => HasScreenTitleErrorMessage, ref _HasScreenTitleErrorMessage, value);
            }
        }

        private string _readerHostName;
        public string ReaderHostname
        {
            get
            {
                return _readerHostName;
            }
            set
            {
                Set<string>(() => ReaderHostname, ref _readerHostName, value);
            }
        }

        private string _ReaderHostnameErrorMessage;
        public string ReaderHostnameErrorMessage
        {
            get
            {
                return _ReaderHostnameErrorMessage;
            }
            set
            {
                Set<string>(() => ReaderHostnameErrorMessage, ref _ReaderHostnameErrorMessage, value);
            }
        }

        private bool _HasReaderHostnameErrorMessage;
        public bool HasReaderHostnameErrorMessage
        {
            get
            {
                return _HasReaderHostnameErrorMessage;
            }
            set
            {
                Set<bool>(() => HasReaderHostnameErrorMessage, ref _HasReaderHostnameErrorMessage, value);
            }
        }

        private string _documentDbConnection;
        public string DocumentDbConnection
        {
            get
            {
                return _documentDbConnection;
            }
            set
            {
                Set<string>(() => DocumentDbConnection, ref _documentDbConnection, value);
            }
        }

        private string _DocumentDbConnectionErrorMessage;
        public string DocumentDbConnectionErrorMessage
        {
            get
            {
                return _DocumentDbConnectionErrorMessage;
            }
            set
            {
                Set<string>(() => DocumentDbConnectionErrorMessage, ref _DocumentDbConnectionErrorMessage, value);
            }
        }

        private bool _HasDocumentDbConnectionErrorMessage;
        public bool HasDocumentDbConnectionErrorMessage
        {
            get
            {
                return _HasDocumentDbConnectionErrorMessage;
            }
            set
            {
                Set<bool>(() => HasDocumentDbConnectionErrorMessage, ref _HasDocumentDbConnectionErrorMessage, value);
            }
        }

        private string _documentDbEndpoint;
        public string DocumentDbEndpoint
        {
            get
            {
                return _documentDbEndpoint;
            }
            set
            {
                Set<string>(() => DocumentDbEndpoint, ref _documentDbEndpoint, value);
            }
        }

        private string _DocumentDbEndpointErrorMessage;
        public string DocumentDbEndpointErrorMessage
        {
            get
            {
                return _DocumentDbEndpointErrorMessage;
            }
            set
            {
                Set<string>(() => DocumentDbEndpointErrorMessage, ref _DocumentDbEndpointErrorMessage, value);
            }
        }

        private bool _HasDocumentDbEndpointErrorMessage;
        public bool HasDocumentDbEndpointErrorMessage
        {
            get
            {
                return _HasDocumentDbEndpointErrorMessage;
            }
            set
            {
                Set<bool>(() => HasDocumentDbEndpointErrorMessage, ref _HasDocumentDbEndpointErrorMessage, value);
            }
        }

        private int _dataSyncInterval;
        public int DataSyncInterval
        {
            get
            {
                return _dataSyncInterval;
            }
            set
            {
                Set<int>(() => DataSyncInterval, ref _dataSyncInterval, value);
            }
        }

        private int _weighInTimeOut;
        public int WeighInTimeout
        {
            get
            {
                return _weighInTimeOut;
            }
            set
            {
                Set<int>(() => WeighInTimeout, ref _weighInTimeOut, value);
            }
        }

        private string _targetStatus;
        public string TargetStatus
        {
            get
            {
                return _targetStatus;
            }
            set
            {
                Set<string>(() => TargetStatus, ref _targetStatus, value);
            }
        }

        private string _portName;
        public string PortName
        {
            get
            {
                return _portName;
            }
            set
            {
                Set<string>(() => PortName, ref _portName, value);
            }
        }

        private string _BarcodePortName;
        public string BarcodePortName
        {
            get
            {
                return _BarcodePortName;
            }
            set
            {
                Set<string>(() => BarcodePortName, ref _BarcodePortName, value);
            }
        }

        private bool _HasBarcodePortErrorMessage;
        public bool HasBarcodePortErrorMessage
        {
            get
            {
                return _HasBarcodePortErrorMessage;
            }
            set
            {
                Set<bool>(() => HasBarcodePortErrorMessage, ref _HasBarcodePortErrorMessage, value);
            }
        }

        private int _stableWeightSeconds;
        public int StableWeightSeconds
        {
            get
            {
                return _stableWeightSeconds;
            }
            set
            {
                Set<int>(() => StableWeightSeconds, ref _stableWeightSeconds, value);
            }
        }

        public ObservableCollection<string> AvailablePorts { get; private set; }

        public ObservableCollection<string> AvailableStatuses { get; private set; }

        private string _latitude;
        public string Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                Set<string>(() => Latitude, ref _latitude, value);
            }
        }

        private string _LatitudeErrorMessage;
        public string LatitudeErrorMessage
        {
            get
            {
                return _LatitudeErrorMessage;
            }
            set
            {
                Set<string>(() => LatitudeErrorMessage, ref _LatitudeErrorMessage, value);
            }
        }

        private bool _HasLatitudeErrorMessage;
        public bool HasLatitudeErrorMessage
        {
            get
            {
                return _HasLatitudeErrorMessage;
            }
            set
            {
                Set<bool>(() => HasLatitudeErrorMessage, ref _HasLatitudeErrorMessage, value);
            }
        }

        private string _longitude;
        public string Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                Set<string>(() => Longitude, ref _longitude, value);
            }
        }

        private string _LongitudeErrorMessage;
        public string LongitudeErrorMessage
        {
            get
            {
                return _LongitudeErrorMessage;
            }
            set
            {
                Set<string>(() => LongitudeErrorMessage, ref _LongitudeErrorMessage, value);
            }
        }

        private bool _HasLongitudeErrorMessage;
        public bool HasLongitudeErrorMessage
        {
            get
            {
                return _HasLongitudeErrorMessage;
            }
            set
            {
                Set<bool>(() => HasLongitudeErrorMessage, ref _HasLongitudeErrorMessage, value);
            }
        }

        private int _antenna1TransmitPower;
        public int Antenna1TransmitPower
        {
            get
            {
                return _antenna1TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna1TransmitPower, ref _antenna1TransmitPower, value);
            }
        }

        private int _antenna1ReceivePower;
        public int Antenna1ReceivePower
        {
            get
            {
                return _antenna1ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna1ReceivePower, ref _antenna1ReceivePower, value);
            }
        }

        private int _antenna2TransmitPower;
        public int Antenna2TransmitPower
        {
            get
            {
                return _antenna2TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna2TransmitPower, ref _antenna2TransmitPower, value);
            }
        }
        
        private int _antenna2ReceivePower;
        public int Antenna2ReceivePower
        {
            get
            {
                return _antenna2ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna2ReceivePower, ref _antenna2ReceivePower, value);
            }
        }

        private int _antenna3TransmitPower;
        public int Antenna3TransmitPower
        {
            get
            {
                return _antenna3TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna3TransmitPower, ref _antenna3TransmitPower, value);
            }
        }

        private int _antenna3ReceivePower;
        public int Antenna3ReceivePower
        {
            get
            {
                return _antenna3ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna3ReceivePower, ref _antenna3ReceivePower, value);
            }
        }

        private int _antenna4TransmitPower;
        public int Antenna4TransmitPower
        {
            get
            {
                return _antenna4TransmitPower;
            }
            set
            {
                Set<int>(() => Antenna4TransmitPower, ref _antenna4TransmitPower, value);
            }
        }

        private int _antenna4ReceivePower;
        public int Antenna4ReceivePower
        {
            get
            {
                return _antenna4ReceivePower;
            }
            set
            {
                Set<int>(() => Antenna4ReceivePower, ref _antenna4ReceivePower, value);
            }
        }

        private bool _unattendedMode;
        public bool UnattendedMode
        {
            get
            {
                return _unattendedMode;
            }
            set
            {
                Set<bool>(() => UnattendedMode, ref _unattendedMode, value);
            }
        }

        private int _weightAutoSaveTimeout;
        public int WeightAutoSaveTimeout
        {
            get
            {
                return _weightAutoSaveTimeout;
            }
            set
            {
                Set<int>(() => WeightAutoSaveTimeout, ref _weightAutoSaveTimeout, value);
            }
        }

        public SettingsPageViewModel(INavigationService navService) : base(navService)
        {
            SaveCommand       = new RelayCommand(this.ExecuteSave);
            SubmitPasswordCommand = new RelayCommand(this.ExecuteSubmitPassword);
            ClearCommand = new RelayCommand(this.ExecuteClearCommand);
            SyncCommand = new RelayCommand(this.ExecuteSyncCommand);
            CancelCommand     = new RelayCommand(this.ExecuteCancel);
            IsScreenLocked = true;
            HasPasswordErrorMessage = false;
            AllowWeighInPageTimeOutSelection = false;
            AvailablePorts = new ObservableCollection<string>();
            AvailableStatuses = new ObservableCollection<string>();
        }        

        public void Initialize()
        {
            if (IsFirstLaunch) IsScreenLocked = false;

            if (IsScreenLocked)
                return;

            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Loading..." });

            Task.Run(() =>
            {
                try
                {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        loadForm();
                    }));

                    Settings settings = TagDataProvider.GetSettings();
                    if (settings != null)
                    {
                        Antenna1ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(1).RxSensitivityInDbm);
                        Antenna1TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(1).TxPowerInDbm);

                        Antenna2ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(2).RxSensitivityInDbm);
                        Antenna2TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(2).TxPowerInDbm);

                        Antenna3ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(3).RxSensitivityInDbm);
                        Antenna3TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(3).TxPowerInDbm);

                        Antenna4ReceivePower = Convert.ToInt32(settings.Antennas.GetAntenna(4).RxSensitivityInDbm);
                        Antenna4TransmitPower = Convert.ToInt32(settings.Antennas.GetAntenna(4).TxPowerInDbm);
                    }

                    if (Antenna1ReceivePower == 0) Antenna1ReceivePower = -80;
                    if (Antenna2ReceivePower == 0) Antenna2ReceivePower = -80;
                    if (Antenna3ReceivePower == 0) Antenna3ReceivePower = -80;
                    if (Antenna4ReceivePower == 0) Antenna4ReceivePower = -80;

                    if (Antenna1TransmitPower == 0) Antenna1TransmitPower = 30;
                    if (Antenna2TransmitPower == 0) Antenna2TransmitPower = 30;
                    if (Antenna3TransmitPower == 0) Antenna3TransmitPower = 30;
                    if (Antenna4TransmitPower == 0) Antenna4TransmitPower = 30;
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
                Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
            });

        }

        private bool trySaveDbKeysFromInstallDrive()
        {
            bool savedKeys = false;
            try
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var endpoint = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.DOCUMENTDB_ENDPOINT, "");
                    var key = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.DOCUMENT_DB_KEY, "");

                    //look for removable drive to get connection settings from it
                    DriveInfo[] allDrives = DriveInfo.GetDrives();

                    foreach (var d in allDrives.Where(x => x.DriveType == DriveType.Removable))
                    {
                        try
                        {
                            var fileName = d.RootDirectory + "settings.txt";

                            if (System.IO.File.Exists(fileName))
                            {
                                string encryptedString = System.IO.File.ReadAllText(fileName);
                                string decryptedString = CottonDBMS.Helpers.EncryptionHelper.Decrypt(encryptedString);
                                var parms = Newtonsoft.Json.JsonConvert.DeserializeObject<BridgeInstallParams>(decryptedString);

                                endpoint = parms.EndPoint;
                                key = parms.Key;
                                dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.DOCUMENTDB_ENDPOINT, parms.EndPoint);
                                dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.DOCUMENT_DB_KEY, parms.Key);
                                dp.SaveChanges();
                                savedKeys = true;
                                break;
                            }

                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }

            return savedKeys;
        }

        private void loadForm()
        {
            try
            {
                clearErrors();

                if (IsFirstLaunch)
                    trySaveDbKeysFromInstallDrive();

                AvailablePorts.Clear();
                AvailableStatuses.Clear();
                AvailableStatuses.Add("AT GIN");
                AvailableStatuses.Add("ON FEEDER");

                this.TargetStatus = "AT GIN";

                AvailablePorts.Add("NONE");
                foreach (var p in SerialPort.GetPortNames().OrderBy(s => s))
                {
                    AvailablePorts.Add(p);
                }

                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var repo = dp.SettingsRepository;

                    var portSetting = repo.FindSingle(x => x.Key == BridgeSettingKeys.SCALE_COM_PORT);
                    if (portSetting != null && !string.IsNullOrWhiteSpace(portSetting.Value))
                    {
                        PortName = portSetting.Value;
                    }
                    else
                    {
                        if (AvailablePorts.Count > 0)
                        {
                            PortName = AvailablePorts.First();
                        }
                        else
                        {
                            PortName = "";
                        }
                    }

                    portSetting = repo.FindSingle(x => x.Key == BridgeSettingKeys.BARCODE_COM_PORT);
                    if (portSetting != null && !string.IsNullOrWhiteSpace(portSetting.Value))
                    {
                        BarcodePortName = portSetting.Value;
                    }
                    else
                    {
                        if (AvailablePorts.Count > 0)
                        {
                            BarcodePortName = AvailablePorts.First();
                        }
                        else
                        {
                            BarcodePortName = "";
                        }
                    }

                    ReaderHostname = repo.GetSettingWithDefault(BridgeSettingKeys.READER_HOST_NAME, "169.254.1.1");
                    _originalHost = ReaderHostname;

                    ScreenTitle = repo.GetSettingWithDefault(BridgeSettingKeys.GIN_NAME, "");
                    BridgeID = repo.GetSettingWithDefault(BridgeSettingKeys.BRIDGE_ID, "");
                    Latitude = repo.GetSettingWithDefault(BridgeSettingKeys.LATITUDE, "");
                    Longitude = repo.GetSettingWithDefault(BridgeSettingKeys.LONGITUDE, "");
                    TargetStatus = repo.GetSettingWithDefault(BridgeSettingKeys.TARGET_STATUS, "AT GIN");
                    DocumentDbConnection = repo.GetSettingWithDefault(BridgeSettingKeys.DOCUMENT_DB_KEY, "");
                    DocumentDbEndpoint = repo.GetSettingWithDefault(BridgeSettingKeys.DOCUMENTDB_ENDPOINT, "");
                    DataSyncInterval = int.Parse(repo.GetSettingWithDefault(BridgeSettingKeys.DATA_SYNC_INTERVAL, "1"));
                    WeighInTimeout = int.Parse(repo.GetSettingWithDefault(BridgeSettingKeys.WEIGH_IN_TIMEOUT, "30"));
                    StableWeightSeconds = int.Parse(repo.GetSettingWithDefault(BridgeSettingKeys.STABLE_WEIGHT_SECONDS, "10"));

                    WeightAutoSaveTimeout = int.Parse(repo.GetSettingWithDefault(BridgeSettingKeys.WEIGHT_AUTO_SAVE_TIMEOUT, "10"));
                    UnattendedMode = bool.Parse(repo.GetSettingWithDefault(BridgeSettingKeys.UNATTENDED_MODE, "FALSE"));
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void clearErrors()
        {
            HasDocumentDbConnectionErrorMessage = false;
            HasDocumentDbEndpointErrorMessage = false;
            HasScreenTitleErrorMessage = false;
            HasBridgeIDErrorMessage = false;
            HasLatitudeErrorMessage = false;
            HasLongitudeErrorMessage = false;
            HasBarcodePortErrorMessage = false;
        }

        private async Task<bool> ValidateForm()
        {
            bool isValid = true;
            try
            {
              

                clearErrors();
                if (string.IsNullOrWhiteSpace(_documentDbConnection))
                {
                    isValid = false;
                    HasDocumentDbConnectionErrorMessage = true;
                    DocumentDbConnectionErrorMessage = "required";
                }

                if (string.IsNullOrWhiteSpace(_readerHostName))
                {
                    isValid = false;
                    HasReaderHostnameErrorMessage = true;
                    ReaderHostnameErrorMessage = "required";
                }

                if (string.IsNullOrWhiteSpace(DocumentDbEndpoint))
                {
                    isValid = false;
                    HasDocumentDbEndpointErrorMessage = true;
                    DocumentDbEndpointErrorMessage = "required";
                }
               /* else if (!ValidationHelper.ValidUrl(DocumentDbEndpoint))
                {
                    isValid = false;
                    HasDocumentDbEndpointErrorMessage = true;
                    DocumentDbEndpointErrorMessage = "Invalid URL";
                }*/


                /*if (!HasDocumentDbConnectionErrorMessage && !HasDocumentDbEndpointErrorMessage)
                {
                    try
                    {
                        DocumentDBContext.Initialize(_documentDbEndpoint, _documentDbConnection);
                        var trucks = await DocumentDBContext.GetAllItemsAsync<TruckEntity>(p => p.EntityType == EntityType.TRUCK);
                    }
                    catch (Exception exc)
                    {
                        isValid = false;
                        HasDocumentDbEndpointErrorMessage = true;
                        DocumentDbEndpointErrorMessage = "Unable to connect.";
                        Logging.Logger.Log(exc);
                    }
                }*/

                if (string.IsNullOrWhiteSpace(ScreenTitle))
                {
                    isValid = false;
                    HasScreenTitleErrorMessage = true;
                    ScreenTitleErrorMessage = "required";
                }

                if (string.IsNullOrWhiteSpace(BridgeID))
                {
                    isValid = false;
                    HasBridgeIDErrorMessage = true;
                    BridgeIDErrorMessage = "required";
                }

                if (string.IsNullOrWhiteSpace(Latitude))
                {
                    isValid = false;
                    HasLatitudeErrorMessage = true;
                    LatitudeErrorMessage = "required";
                }
                else if (!ValidationHelper.ValidateLatLong(Latitude))
                {
                    isValid = false;
                    HasLatitudeErrorMessage = true;
                    LatitudeErrorMessage = "Invalid latitude";
                }

                if (string.IsNullOrWhiteSpace(Longitude))
                {
                    isValid = false;
                    HasLongitudeErrorMessage = true;
                    LongitudeErrorMessage = "required";
                }
                else if (!ValidationHelper.ValidateLatLong(Longitude))
                {
                    isValid = false;
                    HasLongitudeErrorMessage = true;
                    LongitudeErrorMessage = "Invalid latitude";
                }

                if (BarcodePortName != "NONE" && BarcodePortName == PortName)
                {
                    isValid = false;
                    HasBarcodePortErrorMessage = true;
                }

                return isValid;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
                return false;
            }
        }

        public RelayCommand SaveCommand { get; private set; }
               

        private async void ExecuteSave()
        {
            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Validating settings..." });
            if (!await ValidateForm())
            {
                Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                return;
            }

            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Saving..." });

            await Task.Run(() =>
            {
                         
                try
                {
                    Settings settings = TagDataProvider.GetDefaultSettings();

                    if (settings != null)
                    {
                        settings.Antennas.TxPowerMax = false;
                        settings.Antennas.RxSensitivityMax = false;
                        settings.HoldReportsOnDisconnect = false;
                        settings.Report.Mode = ReportMode.Individual;                        
                        settings.SearchMode = SearchMode.DualTarget;
                        settings.ReaderMode = ReaderMode.AutoSetStaticDRM;
                        settings.Report.IncludeFirstSeenTime = true;
                        settings.Report.IncludeLastSeenTime = true;
                        settings.Report.IncludeSeenCount = true;
                        settings.Keepalives.Enabled = true;
                        settings.Report.IncludePeakRssi = true;
                        settings.Report.IncludePhaseAngle = true;
                        settings.Report.IncludeAntennaPortNumber = true;
                        settings.Keepalives.EnableLinkMonitorMode = true;
                        settings.Keepalives.LinkDownThreshold = 5;
                        settings.Keepalives.PeriodInMs = 3000;

                        settings.Antennas.GetAntenna(1).RxSensitivityInDbm = (double)_antenna1ReceivePower;
                        settings.Antennas.GetAntenna(1).TxPowerInDbm = (double)_antenna1TransmitPower;

                        settings.Antennas.GetAntenna(2).RxSensitivityInDbm = (double)_antenna2ReceivePower;
                        settings.Antennas.GetAntenna(2).TxPowerInDbm = (double)_antenna2TransmitPower;

                        settings.Antennas.GetAntenna(3).RxSensitivityInDbm = (double)_antenna3ReceivePower;
                        settings.Antennas.GetAntenna(3).TxPowerInDbm = (double)_antenna3TransmitPower;

                        settings.Antennas.GetAntenna(4).RxSensitivityInDbm = (double)_antenna4ReceivePower;
                        settings.Antennas.GetAntenna(4).TxPowerInDbm = (double)_antenna4TransmitPower;

                        TagDataProvider.ApplySettings(settings);

                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Settings saved." });
                        System.Threading.Thread.Sleep(3000);
                    }
                    else
                    {
                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Unable to save reader settings. Reader disconnected?" });
                        System.Threading.Thread.Sleep(3000);
                    }

                    
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        if (PortName == "NONE") PortName = "";
                        if (BarcodePortName == "NONE") BarcodePortName = "";

                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.SCALE_COM_PORT, PortName);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.BARCODE_COM_PORT, BarcodePortName);

                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.BRIDGE_ID, BridgeID);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.GIN_NAME, ScreenTitle);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.DOCUMENTDB_ENDPOINT, DocumentDbEndpoint);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.DOCUMENT_DB_KEY, DocumentDbConnection);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.LATITUDE, Latitude);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.LONGITUDE, Longitude);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.TARGET_STATUS, TargetStatus);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.DATA_SYNC_INTERVAL, DataSyncInterval.ToString());
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.WEIGH_IN_TIMEOUT, WeighInTimeout.ToString());
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.STABLE_WEIGHT_SECONDS, StableWeightSeconds.ToString());

                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.WEIGHT_AUTO_SAVE_TIMEOUT, WeightAutoSaveTimeout.ToString());
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.UNATTENDED_MODE, UnattendedMode.ToString());

                        bool needsReaderDisconnect = false;
                        if (_originalHost != ReaderHostname)
                        {
                            needsReaderDisconnect = true;
                            dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.READER_HOST_NAME, ReaderHostname);                            
                        }

                        dp.SaveChanges();    
                        
                        if (needsReaderDisconnect)
                        {
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Reconnecting to " + ReaderHostname });
                            System.Threading.Thread.Sleep(1000);
                            TagDataProvider.Disconnect();
                            TagDataProvider.SetHostName(ReaderHostname);
                            TagDataProvider.Connect();
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "Reader connected to " + ReaderHostname });
                            //shell should start the reader back up in 15-30 seconds
                        }
                    }

                    /*if (portChanged)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            System.Windows.MessageBox.Show("Please close and restart the application for new scale setting to take effect");
                        }));
                    }*/
                }
                catch (Exception exc)
                {

                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        System.Windows.MessageBox.Show("An error occurred saving settings. " + exc.Message);
                    }));
                    Logging.Logger.Log(exc);
                }
                finally
                {
                    Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                    Messenger.Default.Send<SettingsSavedMessage>(new SettingsSavedMessage { GinName = ScreenTitle, IsFirstLaunch = IsFirstLaunch });
                }              
            });
        }

        public RelayCommand CancelCommand { get; private set; }

        private void ExecuteCancel()
        {
            if (IsFirstLaunch)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                NavService.Pop();
            }
        }

        public RelayCommand SyncCommand { get; private set; }

        private void ExecuteSyncCommand()
        {
            Task.Run(() =>
            {
                try
                {
                    if (BridgeSyncProcessHelper.SyncProcessRunning())
                    {
                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Performing data sync." });
                        CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                    }
                    else
                    {
                        if (!CottonDBMS.DataModels.Helpers.NetworkHelper.HasNetwork())
                        {
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "No network.  Unable to sync." });
                            System.Threading.Thread.Sleep(4000);
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                            return;
                        }

                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Performing data sync." });
                        BridgeSyncProcessHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                    }
                    Messenger.Default.Send<DataRefreshedMessage>(new DataRefreshedMessage());
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                    Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                }
            });
        }

        public RelayCommand ClearCommand { get; private set; }

        private void ExecuteClearCommand()
        {
            Task.Run(() =>
            {
                System.Windows.MessageBoxResult msgResult = System.Windows.MessageBoxResult.No;
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    msgResult = System.Windows.MessageBox.Show("Are you sure you want to clear all data stored on the bridge?", "Confirm", System.Windows.MessageBoxButton.YesNo);
                }));

                if (msgResult == System.Windows.MessageBoxResult.Yes)
                {
                    if (CottonDBMS.DataModels.Helpers.NetworkHelper.HasNetwork())
                    {
                        if (BridgeSyncProcessHelper.SyncProcessRunning())
                        {
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Waiting for background sync to complete." });
                            BridgeSyncProcessHelper.WaitForSyncToStop();
                        }
                        else
                        {
                            //run the sync to send data collected this also ensure after it completes
                            //it will not start again during the reset operation
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Sending collected data." });
                            BridgeSyncProcessHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                            BridgeSyncProcessHelper.WaitForSyncToStop();
                        }

                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Clearing data..." });
                        using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                        {
                            dp.TruckRepository.ClearTruckData();
                            dp.LoadScanRepository.ClearBridgeScanData();
                            dp.TruckRepository.ClearClientFarmFieldData();
                            dp.FeederScanRepository.ClearBridgeScanData();
                            dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.LAST_SYNC_TIME, DateTime.Now.AddYears(-1).ToString());
                            dp.SaveChanges();                            
                            TagDataProvider.ClearBuffer();
                                                    
                        }

                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                        Messenger.Default.Send<DataRefreshedMessage>(new DataRefreshedMessage());
                    }
                    else
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            msgResult = System.Windows.MessageBox.Show("Cannot clear data no network connection");
                        }));
                    }
                }
            });
        }

        public RelayCommand SubmitPasswordCommand { get; private set; }

        private void ExecuteSubmitPassword()
        {
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var bridgeID = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.BRIDGE_ID, "");
                if (string.IsNullOrWhiteSpace(Password))
                {
                    HasPasswordErrorMessage = true;
                    PasswordErrorMessage = "required";
                }
                else if (Password.ToLower() != bridgeID.ToLower())
                {
                    HasPasswordErrorMessage = true;
                    PasswordErrorMessage = "Incorrect password.";
                }
                else
                {
                    IsScreenLocked = false;
                    HasPasswordErrorMessage = false;
                    Initialize();
                }
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();


        }
    }
}
