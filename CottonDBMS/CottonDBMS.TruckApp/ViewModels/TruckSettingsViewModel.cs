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


namespace CottonDBMS.TruckApp.ViewModels
{
    public class TruckSettingsViewModel : ViewModelBase
    {
        private IUnitOfWork _unitOfWork = null;
        private Timer timer = null;

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

        private int _gpsOffset;
        public int GPSOffset
        {
            get
            {
                return _gpsOffset;
            }
            set
            {
                Set<int>(() => GPSOffset, ref _gpsOffset, value);
            }
        }

        private ComboBoxItemViewModel _truck = null;
        public ComboBoxItemViewModel Truck
        {
            get
            {
                return _truck;
            }
            set
            {
                Set<ComboBoxItemViewModel>(() => Truck, ref _truck, value);              
            }
        }

        private ObservableCollection<ComboBoxItemViewModel> _AvailableTrucks;
        public ObservableCollection<ComboBoxItemViewModel> AvailableTrucks
        {
            get { return this._AvailableTrucks; }
            private set { Set<ObservableCollection<ComboBoxItemViewModel>>(() => AvailableTrucks, ref _AvailableTrucks, value); }
        }

        public RelayCommand SaveCommand { get; private set; }
        private void ExecuteSave()
        {
            Task.Run(async () =>
            {
                timer.Stop();
                if (_truck == null)
                {
                    ShowErrorMessage = true;
                    ErrorMessage = "Truck ID is required.";
                }
                else
                {
                    ErrorMessage = "";
                    ShowErrorMessage = false;                    
                    
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        try
                        {
                            var repo = dp.SettingsRepository;
                            
                            repo.UpsertSetting(TruckClientSettingKeys.GPS_OFFSET_FEET, _gpsOffset.ToString());
                            CottonDBMS.TruckApp.DataProviders.GPSDataProvider.SetGpsOffsetFeet(Convert.ToDouble(_gpsOffset));
                            dp.SaveChanges();

                            var truckIDSetting = repo.FindSingle(x => x.Key == TruckClientSettingKeys.TRUCK_ID);
                            string originalTruckID = truckIDSetting.Value;
                            string newId = _truck.ID;
                            if (originalTruckID != newId)
                            {
                                //check for network 
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

                                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Setting new truck ID." });
                                    dp.TruckRepository.ClearTruckData();

                                    //remove the list of pickup lists this truck has effectively releasing them.
                                    await CottonDBMS.Cloud.DocumentDBContext.DeleteItemAsync<TruckListsDownloaded>("TRUCKDOWNLOADS_" + originalTruckID);

                                    truckIDSetting.Value = newId;
                                    dp.SaveChanges();
                                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "New truck ID saved." });
                                    System.Threading.Thread.Sleep(2000);
                                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Syncing data for new truck assignment." });
                                    CottonDBMS.DataModels.Helpers.NetworkHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Sync completed." });
                                    System.Threading.Thread.Sleep(2000);
                                    Messenger.Default.Send<DataRefreshedMessage>(new DataRefreshedMessage());
                                }
                                else
                                {
                                    await Refresh();
                                    System.Windows.Application.Current.Dispatcher.Invoke((Action)(() =>
                                    {
                                        System.Windows.MessageBox.Show("No network connection present.  A network connection is need to change truck ID assigments.");
                                    }));
                                }
                            }
                            else {
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Truck ID already saved." });
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                        catch(Exception exc)
                        {
                            Logging.Logger.Log(exc);
                            System.Windows.MessageBox.Show("An error occurred trying to change truck id.");
                        }                     
                    }

                    CottonDBMS.TruckApp.Helpers.SettingsHelper.PersistSettingsToAppData();
                    Messenger.Default.Send<DataRefreshedMessage>(new DataRefreshedMessage());
                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = false, Message = "" });
                }
                timer.Start();
            });
        }

        public TruckSettingsViewModel()
        {            
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSave);
        }

        private async Task Refresh()
        {
            await Task.Run(() =>
            {
                try
                {
                    using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        var repo = dp.SettingsRepository;
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {


                            AvailableTrucks.Clear();
                            foreach (var t in dp.TruckRepository.GetAll().OrderBy(t => t.Name))
                            {
                                AvailableTrucks.Add(new ComboBoxItemViewModel { ID = t.Id, DisplayText = t.Name });
                            }
                            var truckIDSetting = repo.FindSingle(x => x.Key == TruckClientSettingKeys.TRUCK_ID);
                            if (truckIDSetting != null)
                            {
                                var truck = dp.TruckRepository.GetById(truckIDSetting.Value);

                                if (truck != null)
                                {
                                    Truck = AvailableTrucks.SingleOrDefault(t => t.ID == truck.Id);
                                }
                                else
                                {
                                    Truck = null;
                                }
                            }
                        }));
                    }
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
            });
        }

        public async Task InitializeAsync()
        {
            AvailableTrucks = new ObservableCollection<ComboBoxItemViewModel>();

            using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                var setting = dp.SettingsRepository.FindSingle(s => s.Key == TruckClientSettingKeys.GPS_OFFSET_FEET);

                if (setting != null)
                {
                    GPSOffset = int.Parse(setting.Value);
                }
            }



            await Refresh();

            timer = new Timer(35000);
            timer.Elapsed += async (sender, e) => await HandleTimer();
            timer.Start();
        }

        private async Task HandleTimer()
        {
            await Refresh();
        }
    }
}
