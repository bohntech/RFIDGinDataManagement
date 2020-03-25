//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.CommandWpf;
using CottonDBMS.TruckApp.DataProviders;
using Impinj.OctaneSdk;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.TruckApp.Messages;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Cloud;


namespace CottonDBMS.TruckApp.ViewModels
{
    public class DataSyncSettingsViewModel : ViewModelBase
    {

        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                Set<string>(() => ErrorMessage, ref _errorMessage, value);
            }
        }

        private bool _showErrorMessage;
        public bool ShowErrorMessage
        {
            get
            {
                return _showErrorMessage;
            }
            set
            {
                Set<bool>(() => ShowErrorMessage, ref _showErrorMessage, value);
            }
        }

        private bool _showClearButtons;
        public bool ShowClearButtons
        {
            get
            {
                return _showClearButtons;
            }
            set
            {
                Set<bool>(() => ShowClearButtons, ref _showClearButtons, value);
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

        public RelayCommand SaveCommand { get; private set; }

        private void ExecuteSave()
        {
            Task.Run(async () =>
            {
                string key = "";
                string endpoint = "";
                bool shutDown = false;
                
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var documentDbSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENT_DB_KEY);                    
                    var endpointSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENTDB_ENDPOINT);

                    if (documentDbSetting != null)
                        key = documentDbSetting.Value;

                    if (endpointSetting != null)
                        endpoint = endpointSetting.Value;

                    if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(endpoint))
                    {
                        shutDown = true;
                    }                   
                }

                if (string.IsNullOrWhiteSpace(_documentDbConnection))
                {
                    ShowErrorMessage = true;
                    ErrorMessage = "Document db key is required.";
                }
                else if (string.IsNullOrWhiteSpace(_documentDbEndpoint))
                {
                    ShowErrorMessage = true;
                    ErrorMessage = "Document db endpoint is required.";
                }
                else
                {
                    ErrorMessage = "";
                    ShowErrorMessage = false;
                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Saving..." });
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        var documentDbEndpointSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENTDB_ENDPOINT);
                        var documentDbKeySetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENT_DB_KEY);
                        var syncIntervalSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DATA_SYNC_INTERVAL);

                        var localTrucks = dp.TruckRepository.GetAll();
                        var localDrivers = dp.DriverRepository.GetAll();

                        if (documentDbKeySetting != null)
                        {
                            documentDbKeySetting.Value = _documentDbConnection;
                        }
                        else
                        {
                            documentDbKeySetting = new Setting();
                            documentDbKeySetting.Key = TruckClientSettingKeys.DOCUMENT_DB_KEY;
                            documentDbKeySetting.Value = _documentDbConnection;
                            dp.SettingsRepository.Add(documentDbKeySetting);
                        }

                        if (documentDbEndpointSetting != null)
                        {
                            documentDbEndpointSetting.Value = _documentDbEndpoint;
                        }
                        else
                        {
                            documentDbEndpointSetting = new Setting();
                            documentDbEndpointSetting.Key = TruckClientSettingKeys.DOCUMENTDB_ENDPOINT;
                            documentDbEndpointSetting.Value = _documentDbEndpoint;
                            dp.SettingsRepository.Add(documentDbEndpointSetting);
                        }

                        if (syncIntervalSetting != null)
                        {
                            syncIntervalSetting.Value = _dataSyncInterval.ToString();
                        }
                        else
                        {
                            syncIntervalSetting = new Setting();
                            syncIntervalSetting.Key = TruckClientSettingKeys.DATA_SYNC_INTERVAL;
                            syncIntervalSetting.Value = _dataSyncInterval.ToString();
                            dp.SettingsRepository.Add(syncIntervalSetting);
                        }

                        AggregateDataProvider.UpdateReadDelay(_dataSyncInterval);
                        dp.SaveChanges();
                        
                        try
                        {
                            var currentTruck = dp.SettingsRepository.GetCurrentTruck();
                            var currentDriver = dp.SettingsRepository.GetCurrentDriver();

                            if (currentDriver == null || currentTruck == null) //if no truck or driver set try to pull down from cloud and set a truck/driver
                            {
                                CottonDBMS.Cloud.DocumentDBContext.Initialize(_documentDbEndpoint, _documentDbConnection);
                                var trucks = await DocumentDBContext.GetAllItemsAsync<TruckEntity>(p => p.EntityType == EntityType.TRUCK);
                                var drivers = await DocumentDBContext.GetAllItemsAsync<DriverEntity>(p => p.EntityType == EntityType.DRIVER);

                                if (localTrucks.Count() == 0)
                                    foreach (var t in trucks)
                                        dp.TruckRepository.Add(t);
                                
                                if (localDrivers.Count() == 0)
                                    foreach (var d in drivers) dp.DriverRepository.Add(d);

                                dp.SaveChanges();

                                localTrucks = dp.TruckRepository.GetAll();
                                localDrivers = dp.DriverRepository.GetAll();

                                if (localTrucks.Count() > 0)
                                    dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.TRUCK_ID, localTrucks.First().Id);

                                if (localDrivers.Count() > 0)
                                    dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.DRIVER_ID, localDrivers.First().Id);

                                dp.SaveChanges();
                            }                           

                        }
                        catch(Exception exc)
                        {
                            ErrorMessage = "Unable to connect to cloud.";
                            ShowErrorMessage = true;
                        }

                        try
                        {
                            var syncedSettings = await DocumentDBContext.GetAllItemsAsync<SyncedSettings>(p => p.EntityType == EntityType.SETTING_SUMMARY);
                            var settingsToSave = syncedSettings.FirstOrDefault();

                            var localSyncedSetting = dp.SyncedSettingsRepo.GetAll().FirstOrDefault();

                            settingsToSave.SyncedToCloud = true;
                            if (localSyncedSetting == null)
                            {
                                dp.SyncedSettingsRepo.Add(settingsToSave);
                                dp.SaveChanges();
                            }      
                        }
                        catch (Exception exc)
                        {
                            ErrorMessage = "Unable to retrieve system settings.";
                            ShowErrorMessage = true;
                            Logging.Logger.Log(exc);
                        }
                    }
                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Settings Saved." });
                    System.Threading.Thread.Sleep(2000);
                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = false, Message = "" });

                    if (shutDown)
                    {

                        Environment.Exit(0);
                    }
                }
                CottonDBMS.TruckApp.Helpers.SettingsHelper.PersistSettingsToAppData();
            });
        }

        public RelayCommand ClearAllCommand { get; private set; }

        private void ExecuteClearAllCommand()
        {
            Task.Run(async () =>
            {
                System.Windows.MessageBoxResult msgResult = System.Windows.MessageBoxResult.No;
                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    msgResult = System.Windows.MessageBox.Show("Are you sure you want to clear all data stored on the truck including module history, pickup lists, clients, farms, and fields?  Data already sent to gin will not be deleted from the gin database.", "Confirm", System.Windows.MessageBoxButton.YesNo);
                }));

                if (msgResult == System.Windows.MessageBoxResult.Yes)
                {
                    if (CottonDBMS.DataModels.Helpers.NetworkHelper.HasNetwork())
                    {
                        if (CottonDBMS.DataModels.Helpers.NetworkHelper.SyncProcessRunning())
                        {
                            Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Waiting for background sync to complete." });
                            CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                        }
                        else
                        {
                            //run the sync to send data collected this also ensure after it completes
                            //it will not start again during the reset operation
                            Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Sending collected data." });
                            CottonDBMS.DataModels.Helpers.NetworkHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                            CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                        }

                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Clearing data..." });
                        using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                        {
                            dp.TruckRepository.ClearTruckData();
                            dp.TruckRepository.ClearClientFarmFieldData();
                            AggregateDataProvider.Reset();
                            TagDataProvider.ClearBuffer();

                            var truck = dp.SettingsRepository.GetCurrentTruck();
                            if (truck != null)
                            {
                                await DocumentDBContext.DeleteItemAsync<TruckListsDownloaded>("TRUCKDOWNLOADS_" + truck.Id);
                            }
                        }

                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = false, Message = "" });
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

        public RelayCommand ClearModuleDataCommand { get; private set; }

        private void ExecuteClearModuleDataCommand()
        {

            Task.Run(async () => {
                System.Windows.MessageBoxResult msgResult = System.Windows.MessageBoxResult.No;

                System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                {
                    msgResult = System.Windows.MessageBox.Show("Are you sure you want to clear all module history and pickup lists from this truck?  Data already sent to gin will not be deleted from the gin database.", "Confirm", System.Windows.MessageBoxButton.YesNo);
                }));

                if (msgResult == System.Windows.MessageBoxResult.Yes)
                {
                    if (CottonDBMS.DataModels.Helpers.NetworkHelper.HasNetwork())
                    {
                        if (CottonDBMS.DataModels.Helpers.NetworkHelper.SyncProcessRunning())
                        {
                            Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Waiting for background sync to complete." });
                            CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                        }
                        else
                        {
                            //run the sync to send data collected this also ensure after it completes
                            //it will not start again during the reset operation
                            Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Sending collected data." });
                            CottonDBMS.DataModels.Helpers.NetworkHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                            CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                        }

                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Clearing data..." });
                        using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                        {
                            dp.TruckRepository.ClearTruckData();
                            AggregateDataProvider.Reset();
                            var truck = dp.SettingsRepository.GetCurrentTruck();

                            if (truck != null)
                            {
                                await DocumentDBContext.DeleteItemAsync<TruckListsDownloaded>("TRUCKDOWNLOADS_" + truck.Id);
                            }
                        }

                        Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = false, Message = "" });
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

        

        public DataSyncSettingsViewModel()
        {
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSave);
            ClearAllCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteClearAllCommand);
            ClearModuleDataCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteClearModuleDataCommand);

            ShowClearButtons = false;
        }

        public async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    var documentDbEndpointSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENTDB_ENDPOINT);
                    var documentDbSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENT_DB_KEY);
                    var syncIntervalSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DATA_SYNC_INTERVAL);

                    if (documentDbSetting != null)                  
                        DocumentDbConnection = documentDbSetting.Value;

                    if (documentDbEndpointSetting != null)
                        DocumentDbEndpoint = documentDbEndpointSetting.Value;

                    if (!string.IsNullOrEmpty(DocumentDbEndpoint)) ShowClearButtons = true;
                    else ShowClearButtons = false;

                    int temp = 0;
                    if (syncIntervalSetting != null && int.TryParse(syncIntervalSetting.Value, out temp))
                    {
                        DataSyncInterval = temp;
                    }
                    else
                    {
                        DataSyncInterval = 5;
                    }
                }
            });
        }
    }
}
