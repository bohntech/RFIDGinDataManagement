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
using CottonDBMS.BridgePBIApp.Helpers;

namespace CottonDBMS.Bridges.Shared.ViewModels
{
    public class PBISettingsPageViewModel : BasePageViewModel
    {
        public bool IsFirstLaunch { get; set; }
        
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

        public ObservableCollection<string> AvailablePorts { get; private set; }

        public ObservableCollection<string> AvailableStatuses { get; private set; }

        private string _TareWeight;
        public string TareWeight
        {
            get
            {
                return _TareWeight;
            }
            set
            {
                Set<string>(() => TareWeight, ref _TareWeight, value);
            }
        }

        private string _TareWeightErrorMessage;
        public string TareWeightErrorMessage
        {
            get
            {
                return _TareWeightErrorMessage;
            }
            set
            {
                Set<string>(() => TareWeightErrorMessage, ref _TareWeightErrorMessage, value);
            }
        }

        private bool _HasTareWeightErrorMessage;
        public bool HasTareWeightErrorMessage
        {
            get
            {
                return _HasTareWeightErrorMessage;
            }
            set
            {
                Set<bool>(() => HasTareWeightErrorMessage, ref _HasTareWeightErrorMessage, value);
            }
        }

        

        public PBISettingsPageViewModel(INavigationService navService) : base(navService)
        {
            SaveCommand       = new RelayCommand(this.ExecuteSave);            
            ClearCommand = new RelayCommand(this.ExecuteClearCommand);
            SyncCommand = new RelayCommand(this.ExecuteSyncCommand);
            CancelCommand     = new RelayCommand(this.ExecuteCancel);          
            AvailablePorts = new ObservableCollection<string>();            
        }        

        public void Initialize()
        {
          
            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Loading..." });

            Task.Run(() =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    loadForm();
                    Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                }));                
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
                                 
                    DocumentDbConnection = repo.GetSettingWithDefault(BridgeSettingKeys.DOCUMENT_DB_KEY, "");
                    DocumentDbEndpoint = repo.GetSettingWithDefault(BridgeSettingKeys.DOCUMENTDB_ENDPOINT, "");
                    TareWeight = repo.GetSettingWithDefault(BridgeSettingKeys.TARE_WEIGHT, "");
                    StableWeightSeconds = int.Parse(repo.GetSettingWithDefault(BridgeSettingKeys.STABLE_WEIGHT_SECONDS, "5"));
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
            HasTareWeightErrorMessage = false;            
            HasBarcodePortErrorMessage = false;
        }

        private async Task<bool> ValidateForm()
        {
            bool isValid = true;

            clearErrors();
            if (string.IsNullOrWhiteSpace(_documentDbConnection))
            {
                isValid = false;
                HasDocumentDbConnectionErrorMessage = true;
                DocumentDbConnectionErrorMessage = "required";
            }

            if (string.IsNullOrWhiteSpace(DocumentDbEndpoint))
            {
                isValid = false;
                HasDocumentDbEndpointErrorMessage = true;
                DocumentDbEndpointErrorMessage = "required";
            }
            else if (!ValidationHelper.ValidUrl(DocumentDbEndpoint))
            {
                isValid = false;
                HasDocumentDbEndpointErrorMessage = true;
                DocumentDbEndpointErrorMessage = "Invalid URL";
            }


            if (!HasDocumentDbConnectionErrorMessage && !HasDocumentDbEndpointErrorMessage)
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
            }
            
            if (string.IsNullOrWhiteSpace(TareWeight))
            {
                isValid = false;
                HasTareWeightErrorMessage = true;
                TareWeightErrorMessage = "required";
            }
            else if (!ValidationHelper.ValidDecimal(TareWeight))
            {
                isValid = false;
                HasTareWeightErrorMessage = true;
                TareWeightErrorMessage = "Invalid decimal number.";
            }
                       

            if (BarcodePortName != "NONE" && BarcodePortName == PortName)
            {
                isValid = false;
                HasBarcodePortErrorMessage = true;
            }

            return isValid;
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
                                      
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        if (PortName == "NONE") PortName = "";
                        if (BarcodePortName == "NONE") BarcodePortName = "";

                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.SCALE_COM_PORT, PortName);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.BARCODE_COM_PORT, BarcodePortName);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.TARE_WEIGHT, TareWeight);

                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.DOCUMENTDB_ENDPOINT, DocumentDbEndpoint);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.DOCUMENT_DB_KEY, DocumentDbConnection);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.TARE_WEIGHT, TareWeight);
                        dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.STABLE_WEIGHT_SECONDS, StableWeightSeconds.ToString());
                        dp.SaveChanges();
                    }
                  
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
                    Messenger.Default.Send<SettingsSavedMessage>(new SettingsSavedMessage { GinName = "", IsFirstLaunch = IsFirstLaunch });
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
                if (!SyncHelper.HasConnection())
                {
                    Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "No network.  Unable to sync." });
                    System.Threading.Thread.Sleep(3000);
                    Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
                    return;
                }

                Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Syncing..." });

                if (PBISyncHelper.IsRunning)
                {
                    PBISyncHelper.WaitForCompletion();
                }
                PBISyncHelper.RunSync();
                PBISyncHelper.WaitForCompletion();
                Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });
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
                    msgResult = System.Windows.MessageBox.Show("Are you sure you want to clear all PBI Logger data?", "Confirm", System.Windows.MessageBoxButton.YesNo);
                }));

                if (msgResult == System.Windows.MessageBoxResult.Yes)
                {
                    if (CottonDBMS.DataModels.Helpers.NetworkHelper.HasNetwork())
                    {
                       if (PBISyncHelper.IsRunning)
                        {
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Waiting for background sync to complete." });
                            BridgeSyncProcessHelper.WaitForSyncToStop();
                        }
                        else
                        {
                            //run the sync to send data collected this also ensure after it completes
                            //it will not start again during the reset operation
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Sending collected data." });
                            PBISyncHelper.RunSync();
                            PBISyncHelper.WaitForCompletion();
                        }

                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Clearing data..." });
                        using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                        {
                            dp.BaleScanRepository.ClearScanData();                            
                            dp.SettingsRepository.UpsertSetting(BridgeSettingKeys.LAST_SYNC_TIME, DateTime.Now.AddYears(-1).ToString());
                            dp.SaveChanges();                            
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
        
        public override void Cleanup()
        {
            base.Cleanup();
        }
    }
}
