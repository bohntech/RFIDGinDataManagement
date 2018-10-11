//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.Data.EF;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.DataModels;
using System.Collections.ObjectModel;
using CottonDBMS.TruckApp.Navigation;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Interfaces;
using CottonDBMS.Cloud;
using CottonDBMS.TruckApp.DataProviders;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class TruckSetupViewModel : ViewModelBase
    {
        private IWindowService _windowService = null;

        private string endpoint = "";
        private string key = "";

        public ObservableCollection<ComboBoxItemViewModel> Drivers { get; private set; }
        public ObservableCollection<ComboBoxItemViewModel> Trucks { get; private set; }

        private ComboBoxItemViewModel _selectedTruck = null;
        public ComboBoxItemViewModel SelectedTruck
        {
            get
            {
                return _selectedTruck;
            }
            set
            {
                Set<ComboBoxItemViewModel>(() => SelectedTruck, ref _selectedTruck, value);                
            }
        }


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

        private bool _hasBlockingError;
        public bool HasBlockingError
        {
            get
            {
                return _hasBlockingError;
            }
            set
            {
                Set<bool>(() => HasBlockingError, ref _hasBlockingError, value);
            }
        }

        private string _blockingErrorMessage;
        public string BlockingErrorMessage
        {
            get
            {
                return _blockingErrorMessage;
            }
            set
            {
                Set<string>(() => BlockingErrorMessage, ref _blockingErrorMessage, value);
            }
        }

        private bool _showStep1;
        public bool ShowStep1
        {
            get
            {
                return _showStep1;
            }
            set
            {
                Set<bool>(() => ShowStep1, ref _showStep1, value);
            }
        }

        private bool _showStep2;
        public bool ShowStep2
        {
            get
            {
                return _showStep2;
            }
            set
            {
                Set<bool>(() => ShowStep2, ref _showStep2, value);
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

        private string _passwordErrorMessage;
        public string PasswordErrorMessage
        {
            get
            {
                return _passwordErrorMessage;
            }
            set
            {
                Set<string>(() => PasswordErrorMessage, ref _passwordErrorMessage, value);
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

        public RelayCommand CloseCommand { get; private set; }
        private void ExecuteCloseCommand()
        {
            App.Current.Shutdown();
        }

        public RelayCommand ContinueCommand { get; private set; }
        private void ExecuteContinueCommand()
        {
            ErrorMessage = "";
            if (SelectedDriver.ID == "")
            {
                ErrorMessage = "Please select a driver.";
            }
            else if (SelectedTruck.ID == "")
            {
                ErrorMessage = "Please select a truck.";
            }

            if (string.IsNullOrEmpty(ErrorMessage))
            {
                //no errors so save settings
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {
                    dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.DRIVER_ID, _selectedDriver.ID);
                    dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.TRUCK_ID, _selectedTruck.ID);
                    dp.SaveChanges();
                    
                    ShowStep1 = false;
                    ShowStep2 = true;
                }
                CottonDBMS.TruckApp.Helpers.SettingsHelper.PersistSettingsToAppData();
            }
            else
            {
                ShowStep1 = true;
                ShowStep2 = false;
            }
        }

        public RelayCommand SavePasswordCommand { get; private set; }
        private void ExecuteSavePasswordCommand()
        {

            if (string.IsNullOrWhiteSpace(Password))
            {
                PasswordErrorMessage = "Password is required.";
            }
            else if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                PasswordErrorMessage = "Confirm password is required";
            }
            else if (Password.Trim() != ConfirmPassword.Trim())
            {
                PasswordErrorMessage = "Passwords do not match.";
            }
            else
            {
                //error checking passed so we can save the lock code
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {
                    dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.ADMIN_PASSWORD, Password.Trim());
                    dp.SaveChanges();

                    _windowService.CloseModalWindow(WindowType.TruckSetupWindow);
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<FirstSetupWizardComplete>(new FirstSetupWizardComplete());
                }
            }
        }

        public RelayCommand SkipPasswordCommand { get; private set; }
        private void ExecuteSkipPasswordCommand()
        {
            _windowService.CloseModalWindow(WindowType.TruckSetupWindow);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<FirstSetupWizardComplete>(new FirstSetupWizardComplete());
        }

        private bool trySaveDbKeysFromInstallDrive()
        {
            bool savedKeys = false;
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                endpoint = dp.SettingsRepository.GetSettingWithDefault(TruckClientSettingKeys.DOCUMENTDB_ENDPOINT, "");
                key = dp.SettingsRepository.GetSettingWithDefault(TruckClientSettingKeys.DOCUMENT_DB_KEY, "");

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
                            var parms = Newtonsoft.Json.JsonConvert.DeserializeObject<TruckAppInstallParams>(decryptedString);

                            endpoint = parms.EndPoint;
                            key = parms.Key;
                            dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.DOCUMENTDB_ENDPOINT, parms.EndPoint);
                            dp.SettingsRepository.UpsertSetting(TruckClientSettingKeys.DOCUMENT_DB_KEY, parms.Key);
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

            return savedKeys;
        }

        private async Task<bool> tryDownloadTrucksFromCloud()
        {
            bool trucksDownloaded = false;
            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                try
                {
                    Logging.Logger.Log("INFO", "Fetch local trucks.");
                    var localTrucks = dp.TruckRepository.GetAll();

                    Logging.Logger.Log("INFO", "Fetch local Drivers.");
                    var localDrivers = dp.DriverRepository.GetAll();

                    Logging.Logger.Log("INFO", "Fetch current truck.");
                    var currentTruck = dp.SettingsRepository.GetCurrentTruck();

                    Logging.Logger.Log("INFO", "Fetch current driver.");
                    var currentDriver = dp.SettingsRepository.GetCurrentDriver();
                     
                    if (currentDriver == null || currentTruck == null) //if no truck or driver set try to pull down from cloud and set a truck/driver
                    {
                        Logging.Logger.Log("INFO", "Initialize document db context.");
                        CottonDBMS.Cloud.DocumentDBContext.Initialize(endpoint, key);

                        Logging.Logger.Log("INFO", "Download trucks from cloud.");
                        var trucks = await DocumentDBContext.GetAllItemsAsync<TruckEntity>(p => p.EntityType == EntityType.TRUCK);

                        Logging.Logger.Log("INFO", "Download drivers from cloud.");
                        var drivers = await DocumentDBContext.GetAllItemsAsync<DriverEntity>(p => p.EntityType == EntityType.DRIVER);

                        if (localTrucks.Count() == 0)
                        {
                            Logging.Logger.Log("INFO", "Adding local trucks.");
                            Trucks.Add(new ComboBoxItemViewModel { DisplayText = "-- Choose Truck --", ID = "" });
                            foreach (var t in trucks)
                            {
                                Logging.Logger.Log("INFO", "Adding truck " + t.Name);
                                dp.TruckRepository.Add(t);
                                Trucks.Add(new ComboBoxItemViewModel { DisplayText = t.Name, ID = t.Id.ToString() });
                            }
                        }

                        if (localDrivers.Count() == 0)
                        {
                            Drivers.Add(new ComboBoxItemViewModel { DisplayText = "-- Choose Driver --", ID = "" });
                            foreach (var d in drivers)
                            {
                                Logging.Logger.Log("INFO", "Adding driver " + d.Name);
                                Drivers.Add(new ComboBoxItemViewModel { DisplayText = d.Name, ID = d.Id.ToString() });
                                dp.DriverRepository.Add(d);
                            }
                        }

                        Logging.Logger.Log("INFO", "Saving db changes");
                        dp.SaveChanges();
                        Logging.Logger.Log("INFO", "Db changes saved");
                        trucksDownloaded = true;
                    }
                    else
                    {
                        Logging.Logger.Log("INFO", "Driver and truck already set.");
                    }
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }

                return trucksDownloaded;
            }            
        }

        private async Task<bool> tryDownloadSettings()
        {
            bool savedSettings = false;
            try
            {
                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {                    
                    var syncedSettings = await DocumentDBContext.GetAllItemsAsync<SyncedSettings>(p => p.EntityType == EntityType.SETTING_SUMMARY);
                    var settingsToSave = syncedSettings.FirstOrDefault();
                    var localSyncedSetting = dp.SyncedSettingsRepo.GetAll().FirstOrDefault();

                    settingsToSave.SyncedToCloud = true;
                    if (localSyncedSetting == null)
                    {
                        dp.SyncedSettingsRepo.Add(settingsToSave);                        
                    }
                    else
                    {
                        dp.SyncedSettingsRepo.Update(settingsToSave);
                    }
                    dp.SaveChanges();
                    savedSettings = true;
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }

            return savedSettings;
        }

      
        public TruckSetupViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public async Task Initialize()
        {
            ContinueCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteContinueCommand);
            SavePasswordCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSavePasswordCommand);
            SkipPasswordCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSkipPasswordCommand);
            CloseCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteCloseCommand);
            Trucks = new ObservableCollection<ComboBoxItemViewModel>();
            Drivers = new ObservableCollection<ComboBoxItemViewModel>();

            HasBlockingError = false;
            BlockingErrorMessage = "";
            ShowStep2 = false;
            ShowStep1 = false;
            ErrorMessage = "";

            if (!CottonDBMS.DataModels.Helpers.NetworkHelper.HasNetwork())
            {
                BlockingErrorMessage = "No network.  Truck must have a data connection to complete setup";
                HasBlockingError = true;
                return;
            }

            var savedKeys = trySaveDbKeysFromInstallDrive();
            bool savedTrucks = false;
            bool savedSettings = false;
            if (!savedKeys)
            {
                BlockingErrorMessage = "Database keys could not be found or are invalid.";
                HasBlockingError = true;
                return;
            }
           
            savedTrucks = await tryDownloadTrucksFromCloud();
            if (!savedTrucks)
            {
                BlockingErrorMessage = "Unabled to download list of truck ids.";
                HasBlockingError = true;
                return;
            }
            SelectedTruck = Trucks[0];
            SelectedDriver = Drivers[0];
            
          
            savedSettings = await tryDownloadSettings();
            if (!savedSettings)
            {
                BlockingErrorMessage = "Unabled to download settings.";
                HasBlockingError = true;
                return;
            }           

            if (!HasBlockingError)
            {
                ShowStep1 = true;

                
            }
        }
    }
}

