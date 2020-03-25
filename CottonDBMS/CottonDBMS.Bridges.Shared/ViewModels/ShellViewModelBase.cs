using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using System.Timers;
using System.Reflection;
using GalaSoft.MvvmLight.Command;
using CottonDBMS.RFID;
using CottonDBMS.Bridges.Shared.Tasks;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.ViewModels;
using CottonDBMS.Bridges.Shared.Navigation;
using CottonDBMS.Bridges;

namespace CottonDBMS.Bridges.Shared.ViewModels
{
    public class ShellViewModelBase : BasePageViewModel
    {
        private Timer timer = null;
        private bool readerTimeSet = false;
        private bool executingTimer = false;
        public bool AllowScaleConnect { get; set; }
        public bool AllowBarcoderConnect { get; set; }
        public bool AllowRFIDReaderConnect { get; set; }
        public bool AllowWeighInTimeOut { get; set; }
        public string ReaderHostName { get; set; }

        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                Set<bool>(() => IsBusy, ref _IsBusy, value);
            }
        }

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

        private string _ScaleStatus;
        public string ScaleStatus
        {
            get
            {
                return _ScaleStatus;
            }
            set
            {
                Set<string>(() => ScaleStatus, ref _ScaleStatus, value);
            }
        }

        private string _BarCodeStatus;
        public string BarCodeStatus
        {
            get
            {
                return _BarCodeStatus;
            }
            set
            {
                Set<string>(() => BarCodeStatus, ref _BarCodeStatus, value);
            }
        }

        private string _ReaderStatus;
        public string ReaderStatus
        {
            get
            {
                return _ReaderStatus;
            }
            set
            {
                Set<string>(() => ReaderStatus, ref _ReaderStatus, value);
            }
        }

        private string _BusyMessage;
        public string BusyMessage
        {
            get
            {
                return _BusyMessage;
            }
            set
            {
                Set<string>(() => BusyMessage, ref _BusyMessage, value);
            }
        }

        private string _Version;
        public string Version
        {
            get
            {
                return _Version;
            }
            set
            {
                Set<string>(() => Version, ref _Version, value);
            }
        }

        private string _CurrentTime;
        public string CurrentTime
        {
            get
            {
                return _CurrentTime;
            }
            set
            {
                Set<string>(() => CurrentTime, ref _CurrentTime, value);
            }
        }

        private string _SyncStatus;
        public string SyncStatus
        {
            get
            {
                return _SyncStatus;
            }
            set
            {
                Set<string>(() => SyncStatus, ref _SyncStatus, value);
            }
        }

        private string _LastSerialNumber;
        public string LastSerialNumber
        {
            get
            {
                return _LastSerialNumber;
            }
            set
            {
                Set<string>(() => LastSerialNumber, ref _LastSerialNumber, value);
            }
        }

        private int _TagsInBuffer;
        public int TagsInBuffer
        {
            get
            {
                return _TagsInBuffer;
            }
            set
            {
                Set<int>(() => TagsInBuffer, ref _TagsInBuffer, value);
            }
        }

        private ScalePortReader _scaleReader = null;
        private BarcodeReader _barCodeReader = null;

        private ScaleWeightReportMessage _lastScaleMessage = null;

        private DateTime lastActivity = DateTime.Now;

        protected bool EnableProcessSync = true;

        public ShellViewModelBase(INavigationService navService) : base(navService)
        {
            Messenger.Default.Register<SettingsSavedMessage>(this, HandleSettingsSavedMessage);
            Messenger.Default.Register<BarcodeScannedMessage>(this, handleBarCodeScanned);
            Messenger.Default.Register<BusyMessage>(this, handleBusyMessage);
            Messenger.Default.Register<ScaleWeightReportMessage>(this, handleScaleMessage);
            Messenger.Default.Register<List<TagItem>>(this, handleTagsReported);
            Messenger.Default.Register<KeyDownMessage>(this, handleKeyDownMessage);

            OpenSettingsCommand = new RelayCommand(this.ExecuteOpenSettings);
            IsBusy = false;
            AllowBarcoderConnect = true;
            AllowScaleConnect = true;
            AllowRFIDReaderConnect = true;
            AllowWeighInTimeOut = false;
            timer = new Timer();
            timer.AutoReset = true;
            timer.Interval = 25000;
            timer.Start();
            timer.Elapsed += Timer_Elapsed;            
           
        }

        private void connectToReader()
        {
            try
            {
                if (!TagDataProvider.IsConnected)
                {
                    Logging.Logger.Log("INFO", "READER NOT CONNECTED");

                    if (ReaderStatus == "CONNECTED")
                    {
                        Logging.Logger.Log("INFO", "READER CHANGE FROM CONNECTED TO DISCONNECTED");
                    }

                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        var setting = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.READER_HOST_NAME, "169.254.1.1");
                        TagDataProvider.SetHostName(setting);
                        TagDataProvider.Connect();
                        //System.Threading.Thread.Sleep(1000);
                    }
                }

                if (!TagDataProvider.IsConnected)
                {
                    ReaderStatus = "DISCONNECTED";
                }
                else
                {
                    if (ReaderStatus == "DISCONNECTED")
                    {
                        Logging.Logger.Log("INFO", "READER RE-CONNECTED");
                    }


                    ReaderStatus = "CONNECTED";
                    if (!readerTimeSet)
                    {
                        readerTimeSet = true;
                        TagDataProvider.Stop();
                        TagDataProvider.SyncReaderTime();
                        //TagDataProvider.Disconnect();
                    }
                }

                if (TagDataProvider.IsConnected && !TagDataProvider.IsSingulating())
                {
                    System.Threading.Thread.Sleep(500);

                    TagDataProvider.Start(0);

                    if (!TagDataProvider.IsSingulating())
                    {
                        TagDataProvider.Stop();
                        TagDataProvider.Disconnect();
                    }
                }
            }
            catch (Exception readerExc)
            {
                ReaderStatus = "ERROR";
                Logging.Logger.Log(readerExc);
            }
        }

        private void connectBarCodeReader(IUnitOfWork dp)
        {
            //try connect to serial port
            var barCodePort = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.BARCODE_COM_PORT, "");
            if (!string.IsNullOrEmpty(barCodePort))
            {
                if (_barCodeReader == null)
                {
                    _barCodeReader = new BarcodeReader(barCodePort);
                    _barCodeReader.Start();
                }
                else if (!_barCodeReader.IsOpen)
                {
                    _barCodeReader.Start();
                }
            }

            if (_barCodeReader != null && _barCodeReader.IsOpen)
            {
                BarCodeStatus = "CONNECTED";
            }
            else
            {
                BarCodeStatus = "DISCONNECTED";
            }
        }

        private void connectScaleReader(IUnitOfWork dp)
        {
            //try connect to serial port
            var scalePort = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.SCALE_COM_PORT, "");
            var stableWeightSeconds = int.Parse(dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.STABLE_WEIGHT_SECONDS, "5"));            

            if (!string.IsNullOrEmpty(scalePort))
            {
                if (_scaleReader == null)
                {
                    _scaleReader = new ScalePortReader(scalePort);
                    _scaleReader.Start();
                    _scaleReader.SetStableWeightSeconds(stableWeightSeconds);
                }
                else if (!_scaleReader.IsOpen)
                {
                    _scaleReader.Start();
                }
            }

            if (_scaleReader != null && _scaleReader.IsOpen && _lastScaleMessage == null)
            {
                ScaleStatus = "CONNECTED";
            }
            else if (_scaleReader == null || !_scaleReader.IsOpen)
            {
                ScaleStatus = "DISCONNECTED";
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            if (!executingTimer) //prevent long execution of handler from overlapping later callbacks
            {
                executingTimer = true;

                if (lastActivity.AddMinutes(2) < DateTime.Now)                
                    Messenger.Default.Send<InactiveMessage>(new InactiveMessage());                

                
                CurrentTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");

                if (String.IsNullOrEmpty(Version))
                    Version = "VERSION " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

                //try to connect to reader
                if (AllowRFIDReaderConnect)
                {
                    connectToReader();
                }
                
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    if (AllowBarcoderConnect)
                        connectBarCodeReader(dp);

                    if (AllowScaleConnect)
                        connectScaleReader(dp);                    
                }

                if (EnableProcessSync)
                {
                    if (!BridgeSyncProcessHelper.Initialized && !NavService.IsOpen(PageType.SETTINGS_PAGE)) //make sure sync timer is running but not if in first setup
                    {
                        BridgeSyncProcessHelper.Init();
                    }

                    if (BridgeSyncProcessHelper.SyncProcessRunning())
                        SyncStatus = "SYNCING";
                    else
                        SyncStatus = "WAITING";
                }

                if (AllowRFIDReaderConnect)
                    TagsInBuffer = TagDataProvider.TagsInBuffer();

                executingTimer = false;
            }
        }

        public void Initialize()
        {
            CurrentTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
            ReaderStatus = "DISCONNECTED";
            BusyMessage = "Loading...";
            IsBusy = false;
            TagsInBuffer = 0;
            LastSerialNumber = "";

            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                GinName = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.GIN_NAME, "");

                var endpoint = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.DOCUMENTDB_ENDPOINT, "");
                var key = dp.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.DOCUMENT_DB_KEY, "");


                if (string.IsNullOrEmpty(endpoint) || string.IsNullOrEmpty(key))
                {
                    
                    OpenFirstRunPage();
                   
                }
                else
                {
                    OpenStartPage();
                    
                }
            }
        }

        protected virtual void OpenStartPage()
        {
            throw new NotImplementedException();   
        }

        protected virtual void OpenFirstRunPage()
        {
            throw new NotImplementedException();
        }

        public RelayCommand OpenSettingsCommand { get; private set; }

        protected virtual void ExecuteOpenSettings()
        {
            if (!NavService.IsOpen(PageType.SETTINGS_PAGE))
            {
                INavigationService navService = SimpleIoc.Default.GetInstance<INavigationService>();
                var vm = new SettingsPageViewModel(navService);
                vm.AllowPortSelection = AllowScaleConnect;
                vm.AllowPortBarCodeSelection = AllowBarcoderConnect;
                vm.AllowWeighInPageTimeOutSelection = AllowWeighInTimeOut;
                vm.IsFirstLaunch = false;                
                NavService.ShowPage(PageType.SETTINGS_PAGE, true, vm);
                vm.Initialize();
            }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<SettingsSavedMessage>(this);
            Messenger.Default.Unregister<BusyMessage>(this);
            Messenger.Default.Unregister<ScaleWeightReportMessage>(this);
            Messenger.Default.Unregister<KeyDownMessage>(this);
            Messenger.Default.Unregister<List<TagItem>>(this);
            Messenger.Default.Unregister<BarcodeScannedMessage>(this);
            timer.Enabled = false;
            TagDataProvider.Disconnect();

            if (_scaleReader != null)
                _scaleReader.Dispose();

            if (_barCodeReader != null)
                _barCodeReader.Dispose();
            base.Cleanup();            
        }

        protected virtual void HandleSettingsSavedMessage(SettingsSavedMessage msg)
        {
            GinName = msg.GinName;
            if (_scaleReader != null)
            {
                _scaleReader.Stop();
                _scaleReader.Dispose();
                _scaleReader = null;
            }

            if (_barCodeReader != null)
            {
                _barCodeReader.Stop();
                _barCodeReader.Dispose();
                _barCodeReader = null;
            }            
        }

        private void handleScaleMessage(ScaleWeightReportMessage msg)
        {
            try
            {
                if (_lastScaleMessage == null || (_lastScaleMessage.Status != msg.Status || _lastScaleMessage.Weight != msg.Weight))
                {
                    if (_scaleReader.IsOpen)
                        ScaleStatus = "CONNECTED READING " + msg.Weight.ToString() + " " + msg.StatusName;
                    else
                        ScaleStatus = "DISCONNECTED";
                }
                _lastScaleMessage = msg;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }
                
        private void handleBusyMessage(BusyMessage msg)
        {
            IsBusy = msg.IsBusy;
            BusyMessage = msg.Message;
        }

        private void handleKeyDownMessage(KeyDownMessage msg)
        {
            lastActivity = DateTime.Now;
        }

        private void handleTagsReported(List<TagItem> tagsReported)
        {
            try
            {
                TagsInBuffer = TagDataProvider.TagsInBuffer();
                LastSerialNumber = tagsReported.Last().SerialNumber;

                foreach (var tag in tagsReported)
                {
                    Logging.Logger.Log("INFO", "EPC: " + tag.Epc + " SN: " + tag.SerialNumber + " ANTENNA PORT: " + tag.AntennaePort.ToString() + " FIRST SEEN LOCAL TIME: " + tag.Firstseen.ToLocalTime().ToString() + " PEAK RSSI: " + tag.PeakRSSI.ToString() + " PHASE ANGLE: " + tag.PhaseAngle.ToString());
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        private void handleBarCodeScanned(BarcodeScannedMessage msg)
        {
            lastActivity = DateTime.Now;
        }
    }
}
