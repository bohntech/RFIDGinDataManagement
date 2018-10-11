//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Data.EF;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.DataModels;
using System.Collections.ObjectModel;
using CottonDBMS.TruckApp.Navigation;
using CottonDBMS.Interfaces;
using System.Timers;
using CottonDBMS.Cloud;
using System.IO;
using CottonDBMS.Helpers;
using Newtonsoft;
using Newtonsoft.Json;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class NavViewModel : ViewModelBase
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                Set<bool>(() => IsBusy, ref _isBusy, value);
            }
        }

        private bool _isDialogOpen;
        public bool DialogOpen
        {
            get
            {
                return _isDialogOpen;
            }
            set
            {
                Set<bool>(() => DialogOpen, ref _isDialogOpen, value);
            }
        }
        

        private string _busyMessage;
        public string BusyMessage
        {
            get
            {
                return _busyMessage;
            }
            set
            {
                Set<string>(() => BusyMessage, ref _busyMessage, value);
            }
        }

        private bool _settingsLocked;
        public bool SettingsLocked
        {
            get
            {
                return _settingsLocked;
            }
            set
            {                
                Set<bool>(() => SettingsLocked, ref _settingsLocked, value);
                refreshVisibilities();
            }
        }

        private bool _truckTabVisible;
        public bool TruckTabVisible
        {
            get
            {
                return _truckTabVisible;
            }
            private set
            {
                Set<bool>(() => TruckTabVisible, ref _truckTabVisible, value);
            }
        }

        private bool _dataTabVisible;
        public bool DataTabVisible
        {
            get
            {
                return _dataTabVisible;
            }
            private set
            {
                Set<bool>(() => DataTabVisible, ref _dataTabVisible, value);
            }
        }

        private bool _settingsTabVisible;
        public bool SettingsTabVisible
        {
            get
            {
                return _settingsTabVisible;
            }
            private set
            {
                Set<bool>(() => SettingsTabVisible, ref _settingsTabVisible, value);
            }
        }

        private bool _lockButtonVisible;
        public bool LockButtonVisible
        {
            get
            {
                return _lockButtonVisible;
            }
            private set
            {
                Set<bool>(() => LockButtonVisible, ref _lockButtonVisible, value);
            }
        }

        private bool _unlockButtonVisible;
        public bool UnlockButtonVisible
        {
            get
            {
                return _unlockButtonVisible;
            }
            private set
            {
                Set<bool>(() => UnlockButtonVisible, ref _unlockButtonVisible, value);
            }
        }

        private bool _cloudDBInitialized;
        public bool CloudDBInitialized
        {
            get
            {
                return _cloudDBInitialized;
            }
            set
            {                
                Set<bool>(() => CloudDBInitialized, ref _cloudDBInitialized, value);
            }
        }       

        private bool _hasCloudSettings;
        public bool HasCloudSettings
        {
            get
            {
                return _hasCloudSettings;
            }
            set
            {
                Set<bool>(() => HasCloudSettings, ref _hasCloudSettings, value);
                refreshVisibilities();
            }
        }

        private bool _passwordModalVisible;
        public bool PasswordModalVisible
        {
            get
            {
                return _passwordModalVisible;
            }
            set
            {
                Set<bool>(() => PasswordModalVisible, ref _passwordModalVisible, value);
            }
        }       

        private bool _unlockPasswordModalVisible;
        public bool UnlockPasswordModalVisible
        {
            get
            {
                return _unlockPasswordModalVisible;
            }
            set
            {
                Set<bool>(() => UnlockPasswordModalVisible, ref _unlockPasswordModalVisible, value);
            }
        }

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                Set<string>(() => ConfirmPassword, ref _confirmPassword, value);
            }
        }

        private void refreshVisibilities()
        {
            if (SettingsLocked || !HasCloudSettings) TruckTabVisible = false;
            else TruckTabVisible = true;

            if (SettingsLocked || !HasCloudSettings) SettingsTabVisible = false;
            else SettingsTabVisible = true;

            if (SettingsLocked || !HasCloudSettings) LockButtonVisible = false;
            else LockButtonVisible = true;

            if (!SettingsLocked || !HasCloudSettings) UnlockButtonVisible = false;
            else UnlockButtonVisible = true;

            if (SettingsLocked || !HasCloudSettings) DataTabVisible = false;
            else DataTabVisible = true;
        }

        public RelayCommand LockSettingsCommand { get; private set; }
        private void ExecuteLockSettings()
        {
            //if (!CloudDBInitialized)
            ErrorMessage = "";
            PasswordModalVisible = true;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            DialogOpen = true;
        }

        public RelayCommand SavePasswordCommand { get; private set; }
        private void ExecuteSavePasswordCommand()
        {
            if (String.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Password is required.";
            }
            else if (Password.Trim() != ConfirmPassword.Trim())
            {
                ErrorMessage = "Passwords do not match.";
            }
            else
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {
                    var repo = dp.SettingsRepository;
                    var setting = repo.FindSingle(k => k.Key == CottonDBMS.DataModels.TruckClientSettingKeys.ADMIN_PASSWORD);
                    setting.Value = _password.Trim();
                    repo.Update(setting);
                    dp.SaveChanges();
                }
                PasswordModalVisible = false;
                SettingsLocked = true;
                DialogOpen = false;
            }
        }

        public RelayCommand CancelCommand { get; private set; }
        private void ExecuteCancelCommand()
        {           
            PasswordModalVisible = false;
            UnlockPasswordModalVisible = false;
            DialogOpen = false;
        }

        public RelayCommand UnlockCommand { get; private set; }
        private void ExecuteUnlockCommand()
        {
            Password = "";
            ErrorMessage = "";
            PasswordModalVisible = false;
            UnlockPasswordModalVisible = true;
            DialogOpen = true;
        }

        public RelayCommand SaveUnlockCommand { get; private set; }
        private void ExecuteSaveUnlockCommand()
        {
            if (String.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Password is required.";
            }           
            else
            {

                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {
                    var repo = dp.SettingsRepository;
                    var setting = repo.FindSingle(k => k.Key == CottonDBMS.DataModels.TruckClientSettingKeys.ADMIN_PASSWORD);

                    if (setting.Value.Trim() != Password.Trim())
                    {
                        ErrorMessage = "Invalid password.";
                    }
                    else
                    {
                        PasswordModalVisible = false;
                        SettingsLocked = false;
                        DialogOpen = false;
                        UnlockPasswordModalVisible = false;
                        Password = string.Empty;
                        ConfirmPassword = string.Empty;

                        setting.Value = null;
                        repo.Update(setting);
                        dp.SaveChanges();
                    }
                }                
            }
        }

        public ObservableCollection<ComboBoxItemViewModel> Drivers { get; private set; }

        private ComboBoxItemViewModel _selectedDriver = null;
        public ComboBoxItemViewModel SelectedDriver
        {
            get
            {
                return _selectedDriver;
            }
            set
            {
                Set<ComboBoxItemViewModel>(() => SelectedDriver, ref _selectedDriver, value);

                if (value != null)
                {
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.DRIVER_ID, (value != null) ? value.ID : "");
                        dp.SaveChanges();
                    }
                }
            }
        }

        private void refresh()
        {
            Task.Run(() => {
                try
                {
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        var drivers = dp.DriverRepository.GetAll().OrderBy(t => t.Name);
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {                            
                            Drivers.Clear();                            
                            foreach (var driver in drivers)
                            {
                                Drivers.Add(new ViewModels.ComboBoxItemViewModel { DisplayText = driver.Name, ID = driver.Id });
                            }

                            var driverSetting = dp.SettingsRepository.FindSingle(s => s.Key == TruckClientSettingKeys.DRIVER_ID);
                            if (driverSetting != null)
                            {
                                var savedDriver = Drivers.FirstOrDefault(d => d.ID == driverSetting.Value);
                                if (savedDriver != null)
                                {
                                    SelectedDriver = savedDriver;
                                }
                                else
                                {
                                    SelectedDriver = Drivers[0];
                                }
                            }
                            else
                            {
                                SelectedDriver = Drivers[0];
                            }
                        }));                        
                    }
                }
                catch(Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
            });
        }

       

        public NavViewModel()
        {
            LockSettingsCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteLockSettings);
            SavePasswordCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSavePasswordCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteCancelCommand);
            UnlockCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteUnlockCommand);
            SaveUnlockCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSaveUnlockCommand);
            Drivers = new ObservableCollection<ComboBoxItemViewModel>();
            PasswordModalVisible = false;
            UnlockPasswordModalVisible = false;

            
                      
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                var setting = dp.SettingsRepository.FindSingle(k => k.Key == TruckClientSettingKeys.ADMIN_PASSWORD);

                if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
                {
                    SettingsLocked = true;
                }
                else
                {
                    SettingsLocked = false;
                }                               
                                
                var documentDbSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENT_DB_KEY);
                var syncIntervalSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DATA_SYNC_INTERVAL);
                var endpointSetting = dp.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENTDB_ENDPOINT);

                string key = "";
                string endpoint = "";

                if (documentDbSetting != null)
                    key = documentDbSetting.Value;

                if (endpointSetting != null)
                    endpoint = endpointSetting.Value;                              

                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(endpoint))
                {
                    CloudDBInitialized = false;
                    HasCloudSettings = false;
                }
                else
                {
                    HasCloudSettings = true;
                    try
                    {
                        Task.Run(async () => { 
                            DocumentDBContext.Initialize(endpoint, key);
                            if (!await DocumentDBContext.DatabaseExistsAsync())
                            {
                                CloudDBInitialized = true;           
                            }
                        });
                    }
                    catch (Exception exc)
                    {                        
                        Logging.Logger.Log(exc);
                    }
                }
            }
            Messenger.Default.Register<BusyMessage>(this,(action) => ReceiveBusyMessage(action));
            Messenger.Default.Register<DialogClosedMessage>(this, (action => ReceiveDialogClosed(action)));
            Messenger.Default.Register<DialogOpenedMessage>(this, (action => ReceiveDialogOpened(action)));

            refresh();
            Timer timer = new Timer(25000);
            timer.Elapsed += (sender, e) => refresh();
            timer.Start();
        }

        private object ReceiveDialogClosed(DialogClosedMessage msg)
        {
            DialogOpen = false;
            return null;
        }

        private object ReceiveDialogOpened(DialogOpenedMessage msg)
        {
            DialogOpen = true;
            return null;
        }

        private object ReceiveBusyMessage(BusyMessage msg)
        {
            BusyMessage = msg.Message;
            IsBusy = msg.IsBusy;
            DialogOpen = msg.IsBusy;
            return null;
        }
    }
}

