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
using CottonDBMS.TruckApp.DataProviders;
using System.Diagnostics;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private IWindowService _windowService = null;


        private string lastProducerID = "-1";
        private string lastFarmID = "-1";
        private string lastFieldID = "-1";


        private object lockingObj = new object();

        /*private List<ComboBoxItemViewModel> allProducers = null;
        private List<FarmComboBoxItemViewModel> allFarms = null;
        private List<FieldComboBoxItemViewModel> allFields = null;*/

        public delegate void OnFilterChangedHandler(object sender, EventArgs e);
        public event OnFilterChangedHandler OnFilterChanged;

        private void fireFilterChanged()
        {
            if (OnFilterChanged != null)
            {
                OnFilterChanged(this, new EventArgs());
            }
        }

        private ObservableCollection<PickupListGridItem> _lists;
        public ObservableCollection<PickupListGridItem> Lists
        {
            get { return this._lists; }
            private set { Set<ObservableCollection<PickupListGridItem>>(() => Lists, ref _lists, value); }
        }

        private ObservableCollection<ComboBoxItemViewModel> _producers;
        public ObservableCollection<ComboBoxItemViewModel> Producers
        {
            get { return this._producers; }
            private set
            {
                Set<ObservableCollection<ComboBoxItemViewModel>>(() => Producers, ref _producers, value);
            }
        }

        private ComboBoxItemViewModel _selectedProducer;
        public ComboBoxItemViewModel SelectedProducer
        {
            get
            {
                return _selectedProducer;
            }
            set
            {
                if (value == null) lastProducerID = "";
                else //if (value.ID != lastProducerID)
                {
                    using (var _unitOfWork = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        //do producer changed
                        lastProducerID = value.ID;

                        Farms.Clear();
                        Farms.Add(new FarmComboBoxItemViewModel { ID = "", DisplayText = "All Farms" });
                        foreach (var f in _unitOfWork.FarmRepository.GetAll().OrderBy(f => f.Name))
                        {
                            if (f.ClientId == value.ID)
                            {
                                Farms.Add(new FarmComboBoxItemViewModel { ClientID = f.ClientId, ID = f.Id, DisplayText = f.Name });
                            }
                        }
                        SelectedFarm = Farms[0];
                    }
                }
                Set<ComboBoxItemViewModel>(() => SelectedProducer, ref _selectedProducer, value);
                fireFilterChanged();
            }
        }

        private FarmComboBoxItemViewModel _selectedFarm;
        public FarmComboBoxItemViewModel SelectedFarm
        {
            get
            {
                return _selectedFarm;
            }
            set
            {
                if (value == null)
                {
                    lastFarmID = "";
                    lastFieldID = "";
                    Fields.Clear();
                    Fields.Add(new FieldComboBoxItemViewModel { ID = "", DisplayText = "All Fields" });
                    SelectedField = Fields[0];
                }
                else //if (value != null && value.ID != lastFarmID)
                {
                    using (var _unitOfWork = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        //do farm changed
                        lastFarmID = value.ID;
                        Fields.Clear();
                        Fields.Add(new FieldComboBoxItemViewModel { ID = "", DisplayText = "All Fields" });
                        SelectedField = Fields[0];
                        foreach (var f in _unitOfWork.FieldRepository.GetAll(new string[] { "Farm.Client" }).OrderBy(f => f.Name))
                        {
                            if (f.Id != GUIDS.UNASSIGNED_FIELD_ID)
                            {
                                if (f.FarmId == value.ID && f.Client.Id == value.ClientID)
                                {
                                    Fields.Add(new FieldComboBoxItemViewModel { ClientID = f.Client.Id, ID = f.Id, FarmID = f.FarmId, DisplayText = f.Name });
                                }
                            }
                        }
                    }
                }
                Set<FarmComboBoxItemViewModel>(() => SelectedFarm, ref _selectedFarm, value);
                fireFilterChanged();


            }
        }

        private FieldComboBoxItemViewModel _selectedField;
        public FieldComboBoxItemViewModel SelectedField
        {
            get
            {
                return _selectedField;
            }
            set
            {
                if (value == null)
                {
                    lastFieldID = "";
                }
                else //if (value != null && value.ID != lastFieldID)
                {
                    //do field changed
                    lastFieldID = value.ID;
                }
                Set<FieldComboBoxItemViewModel>(() => SelectedField, ref _selectedField, value);
                fireFilterChanged();
            }
        }

        private ObservableCollection<FarmComboBoxItemViewModel> _farms;
        public ObservableCollection<FarmComboBoxItemViewModel> Farms
        {
            get { return this._farms; }
            private set { Set<ObservableCollection<FarmComboBoxItemViewModel>>(() => Farms, ref _farms, value); }
        }

        private ObservableCollection<FieldComboBoxItemViewModel> _fields;
        public ObservableCollection<FieldComboBoxItemViewModel> Fields
        {
            get { return this._fields; }
            private set { Set<ObservableCollection<FieldComboBoxItemViewModel>>(() => Fields, ref _fields, value); }
        }


        private int _selectedListId;
        public int SelectedListId
        {
            get
            {
                return _selectedListId;
            }
            set
            {
                Set<int>(() => SelectedListId, ref _selectedListId, value);
            }
        }

        private PickupListGridItem _selectedItem;
        public PickupListGridItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                Set<PickupListGridItem>(() => SelectedItem, ref _selectedItem, value);
            }
        }

        public RelayCommand AddFieldCommand { get; private set; }
        private void ExecuteAddFieldCommand()
        {
            _windowService.ShowModalWindow(WindowType.AddFieldWindow, new AddPickUpListViewModel(_windowService));
        }

        public RelayCommand OpenFieldCommand { get; private set; }
        private void ExecuteOpenFieldCommand()
        {
            if (!_windowService.IsWindowOpen(WindowType.PickupWindow) && this.SelectedItem != null && !string.IsNullOrWhiteSpace(this.SelectedItem.ListID))
            {
                PickUpListViewModel vm = SimpleIoc.Default.GetInstance<PickUpListViewModel>();
                vm.ListID = this.SelectedItem.ListID;
                vm.InitiatingEvent = null;
                //DataProviders.AggregateDataProvider.ClearBuffer();
                //DataProviders.QuadratureEncoderDataProvider.ClearBuffer();
                //DataProviders.GPSDataProvider.ClearBuffer();
                _windowService.ShowModalWindow(WindowType.PickupWindow, vm);
            }
        }

        public RelayCommand DeleteCheckedCommand { get; private set; }
        private void ExecuteDeleteCheckedCommand()
        {
            Task.Run(() =>
            {
                try
                {
                    using (var _unitOfWork = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        bool deleteConfirmed = false;

                        if (CottonDBMS.DataModels.Helpers.NetworkHelper.HasNetwork())
                        {
                            var selectedCount = Lists.Count(x => x.Checked);

                            if (selectedCount == 0)
                            {
                                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                {
                                    System.Windows.MessageBox.Show("No lists checked for delete.  Please check off at least one list to delete.");
                                }));
                                return;
                            }

                            var snsOnTruck = AggregateDataProvider.SerialNumbersOnTruck;


                            var moduleEntitiesOnTruck = _unitOfWork.ModuleRepository.FindMatching(x => snsOnTruck.Contains(x.Name));
                            var listIDsOnTruck = moduleEntitiesOnTruck.Select(x => x.PickupListId).ToList();

                            var listsOnTruckCount = Lists.Count(x => listIDsOnTruck.Contains(x.ListID) && x.Checked);

                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                if (listsOnTruckCount > 0)
                                {
                                    deleteConfirmed = false;
                                    System.Windows.MessageBox.Show("Cannot perform delete. There are currently modules on the truck that belong to a list you are trying to delete.", "Confirmation");
                                    return;
                                }

                                if (System.Windows.MessageBox.Show("Are you sure you want to delete the " + selectedCount.ToString() + " checked list(s).", "Confirmation", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                                {
                                    deleteConfirmed = true;
                                }
                            }));

                            if (deleteConfirmed)
                            {
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Processing..." });
                                if (CottonDBMS.DataModels.Helpers.NetworkHelper.SyncProcessRunning())
                                {
                                    Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Waiting for background sync to complete." });
                                    CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                                }
                                else
                                 {
                                     //run the sync to send data collected this also ensure after it completes
                                     //it will not start again during the release
                                     Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Sending collected data." });
                                     CottonDBMS.DataModels.Helpers.NetworkHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                                     CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                                 }

                                var selectedListIds = Lists.Where(x => x.Checked && !listIDsOnTruck.Contains(x.ListID)).Select(x => x.ListID);
                                var listsToDelete = _unitOfWork.PickupListRepository.FindMatching(x => selectedListIds.Contains(x.Id)).ToList();
                                _unitOfWork.PickupListRepository.BulkDeleteListAndModules(listsToDelete);

                                //run the sync again to ensure released lists go immediately back to cloud                          
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Releasing lists." });

                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = true, Message = "Syncing..." });
                                CottonDBMS.DataModels.Helpers.NetworkHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
                                CottonDBMS.DataModels.Helpers.NetworkHelper.WaitForSyncToStop();
                                Messenger.Default.Send<BusyMessage>(new Messages.BusyMessage { IsBusy = false, Message = "" });

                                Messenger.Default.Send<DataRefreshedMessage>(new DataRefreshedMessage());
                            }
                        }
                        else
                        {
                            //no network
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                System.Windows.MessageBox.Show("No network.  A network connection is needed to delete lists.");
                            }));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }

            });
        }

        public RelayCommand ShutdownCommand { get; private set; }
        private void ExecuteShutdownCommand()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (System.Windows.MessageBox.Show("Are you sure you want to shutdown the system?", "Confirmation", System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                {
                    App.Current.Shutdown();
                }
            }));
        }

        public RelayCommand SyncCommand { get; private set; }
        private void ExecuteSyncCommand()
        {
            Task.Run(() =>
            {
                try
                {    
                    if (CottonDBMS.DataModels.Helpers.NetworkHelper.SyncProcessRunning())
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
                            System.Threading.Thread.Sleep(5000);
                            Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = false, Message = "" });                            
                            return;
                        }

                        Messenger.Default.Send<BusyMessage>(new BusyMessage { IsBusy = true, Message = "Performing data sync." });
                        CottonDBMS.DataModels.Helpers.NetworkHelper.RunSync(System.Reflection.Assembly.GetExecutingAssembly().Location, false);
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

        private void refresh()
        {
            using (var _unitOfWork = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                Producers.Clear();
                Farms.Clear();
                Fields.Clear();

                Lists = new ObservableCollection<PickupListGridItem>();
                Producers = new ObservableCollection<ComboBoxItemViewModel>();
                Producers.Add(new ComboBoxItemViewModel { ID = "", DisplayText = "All Clients" });
                Farms = new ObservableCollection<FarmComboBoxItemViewModel>();
                Farms.Add(new FarmComboBoxItemViewModel { ID = "", DisplayText = "All Farms" });
                Fields = new ObservableCollection<FieldComboBoxItemViewModel>();
                Fields.Add(new FieldComboBoxItemViewModel { ID = "", DisplayText = "All Fields" });

                var clients = _unitOfWork.ClientRepository.GetAll(new string[] { "Farms.Fields" }).OrderBy(c => c.Name);
                foreach (var c in clients.Where(x => x.Id != GUIDS.UNASSIGNED_CLIENT_ID))
                {
                    var producer = new ComboBoxItemViewModel { ID = c.Id, DisplayText = c.Name };
                    Producers.Add(producer);

                    foreach (var farm in c.Farms.OrderBy(f => f.Name))
                    {
                        var item = new FarmComboBoxItemViewModel { ClientID = c.Id, ID = farm.Id, DisplayText = farm.Name };
                        Farms.Add(item);

                        foreach (var field in farm.Fields.OrderBy(f => f.Name))
                        {
                            var fieldItem = new FieldComboBoxItemViewModel { ClientID = farm.ClientId, FarmID = farm.Id, ID = field.Id, DisplayText = field.Name };
                            Fields.Add(fieldItem);
                        }
                    }
                }

                var truck = _unitOfWork.SettingsRepository.GetCurrentTruck();
                string truckID = "";
                if (truck != null)
                {
                    truckID = truck.Id;
                }

                foreach (var list in _unitOfWork.PickupListRepository.FindMatching(p => p.Id != GUIDS.UNASSIGNED_LIST_ID && p.AssignedTrucks.Any(t => t.Id == truckID), new string[] { "Field.Farm.Client", "AssignedTrucks" }).OrderBy(p => p.Name))
                {
                    Lists.Add(new PickupListGridItem { Client = list.Field.Client.Name, Farm = list.Field.Farm.Name, Field = list.Field.Name, ListID = list.Id, ListName = list.Name });
                }
            }
        }

        private void HandleTimer()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                lastFarmID = "-1";
                lastProducerID = "-1";
                lastFieldID = "-1";

                var selProducerID = (SelectedProducer != null) ? SelectedProducer.ID : "";
                var selFarmID = (SelectedFarm != null) ? SelectedFarm.ID : "";
                var selFieldID = (SelectedField != null) ? SelectedField.ID : "";
                var selItemID = "";

                if (SelectedItem != null) selItemID = SelectedItem.ListID;

                refresh();

                //restore selections
                int producerIndex = 0;
                for (producerIndex = 0; producerIndex < Producers.Count(); producerIndex++) if (Producers[producerIndex].ID == selProducerID) break;
                if (producerIndex >= Producers.Count()) producerIndex = 0;

                if (Producers.Count() > 0)
                    SelectedProducer = Producers[producerIndex];

                int farmIndex = 0;
                for (farmIndex = 0; farmIndex < Farms.Count(); farmIndex++) if (Farms[farmIndex].ID == selFarmID) break;
                if (farmIndex >= Farms.Count()) farmIndex = 0;

                if (Farms.Count() > 0)
                    SelectedFarm = Farms[farmIndex];

                int fieldIndex = 0;
                for (fieldIndex = 0; fieldIndex < Fields.Count(); fieldIndex++) if (Fields[fieldIndex].ID == selFieldID) break;
                if (fieldIndex >= Fields.Count()) fieldIndex = 0;

                if (Fields.Count() > 0)
                    SelectedField = Fields[fieldIndex];

                var item = Lists.SingleOrDefault(l => l.ListID == selItemID);
                if (item != null) SelectedItem = item;
                else if (Lists.Count() > 0)
                {
                    SelectedItem = Lists[0];
                }
                else
                {
                    SelectedItem = null;
                }
            }));
        }

        public HomeViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            AddFieldCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteAddFieldCommand);
            OpenFieldCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteOpenFieldCommand);
            ShutdownCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteShutdownCommand);
            SyncCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSyncCommand);
            DeleteCheckedCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteDeleteCheckedCommand);

            Producers = new ObservableCollection<ComboBoxItemViewModel>();
            Farms = new ObservableCollection<FarmComboBoxItemViewModel>();
            Fields = new ObservableCollection<FieldComboBoxItemViewModel>();
            Lists = new ObservableCollection<PickupListGridItem>();

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<PickupListAddedMessage>(this, action => ProcessListAddedMessage(action));
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TagLoadingMessage>(this, action => ProcessTagLoadingMessage(action));
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<TagUnloadingMessage>(this, action => ProcessTagUnloadingMessage(action));
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<DataRefreshedMessage>(this, action => ProcessDataRefreshedMessage(action));
            Messenger.Default.Register<AllAggEventsProcessComplete>(this, (action) => ProcessLoadUnloadComplete(action));

            refresh();
            SelectedProducer = Producers[0];
            SelectedFarm = Farms[0];
            SelectedField = Fields[0];

            if (Lists.Count() > 0)
                SelectedItem = Lists[0];

            /*Timer timer = new Timer(30000);
            timer.Elapsed += (sender, e) => HandleTimer();
            timer.Start();*/
        }

        private void ProcessDataRefreshedMessage(DataRefreshedMessage msg)
        {
            Task.Run(() =>
            {
                HandleTimer();
            });
        }

        private void ProcessListAddedMessage(PickupListAddedMessage msg)
        {
            Task.Run(() =>
            {
                HandleTimer();
            });
        }

        private void ProcessTagLoadingMessage(TagLoadingMessage msg)
        {
            Task.Run(() =>
            {
                using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    DateTime gpsStart = DateTime.Now.ToUniversalTime().AddSeconds(-4);
                    DateTime gpsEnd = DateTime.Now.ToUniversalTime();

                    double latitude = GPSDataProvider.GetAverageLatitude(gpsStart, gpsEnd);
                    double longitude = GPSDataProvider.GetAverageLongitude(gpsStart, gpsEnd);

                    //PickupListEntity pickupList = uow.PickupListRepository.GetListIDWithSerialNumber(msg.SerialNumber);

                    //IF LOADING WINDOW NOT OPEN AND WE ARE ON THE GIN YARD
                    /*if (!_windowService.IsWindowOpen(WindowType.LoadingWindow) && (uow.SettingsRepository.CoordsAtFeeder(latitude, longitude) || uow.SettingsRepository.CoordsOnGinYard(latitude, longitude)))
                    {
                        if (!_windowService.IsWindowOpen(WindowType.LoadingAtGin))
                        {
                            Logging.Logger.Log("DEBUG", "Processing load message from home view model for gin load.");
                            //we are at the gin yard or feeder so just show loading window with no list checks
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                LoadingAtGinViewModel vm = SimpleIoc.Default.GetInstance<LoadingAtGinViewModel>();
                                vm.SerialNumber = msg.SerialNumber;
                                vm.Initialize();
                                _windowService.ShowModalWindow(WindowType.LoadingAtGin, vm);
                            }));
                        }
                        else
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                LoadingAtGinViewModel vm = SimpleIoc.Default.GetInstance<LoadingAtGinViewModel>();                                
                                vm.AddModule(msg.SerialNumber);                                
                            }));
                        }
                    }
                    else
                    {*/
                        Logging.Logger.Log("DEBUG", "Processing load message from home view model.");
                        //if we're in the field then            
                        if (!_windowService.IsWindowOpen(WindowType.PickupWindow))
                        {
                            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                PickUpListViewModel vm = SimpleIoc.Default.GetInstance<PickUpListViewModel>();

                                if (this.SelectedItem != null)
                                    vm.ListID = this.SelectedItem.ListID;
                                else
                                    vm.ListID = GUIDS.UNASSIGNED_LIST_ID;

                                vm.InitiatingEvent = msg;
                                _windowService.ShowModalWindow(WindowType.PickupWindow, vm);
                            }));
                        }
                    //}
                }
            });
        }

        private void ProcessLoadUnloadComplete(AllAggEventsProcessComplete data)
        {
            //if (!data.IsLoading)  //finished processing all modules so reload pickup list window to update totals and push pins
            //{
            if (_windowService.IsWindowOpen(WindowType.LoadingWindow)) _windowService.CloseModalWindow(WindowType.LoadingWindow);

            if (!_windowService.IsWindowOpen(WindowType.LoadingIncorrectModuleWindow))
            {
                if (_windowService.IsWindowOpen(WindowType.PickupWindow) && this.SelectedItem != null && !string.IsNullOrEmpty(this.SelectedItem.ListID))
                {
                    _windowService.CloseModalWindow(WindowType.PickupWindow);
                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        PickUpListViewModel vm = SimpleIoc.Default.GetInstance<PickUpListViewModel>();
                        vm.ListID = this.SelectedItem.ListID;
                        vm.InitiatingEvent = null;
                        _windowService.ShowModalWindow(WindowType.PickupWindow, vm);
                    }));
                }
            }
            //}
        }

        private void ProcessTagUnloadingMessage(TagUnloadingMessage msg)
        {
            Task.Run(() =>
            {
                if (!_windowService.IsWindowOpen(WindowType.WaitingForUnloadWindow))
                {

                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                    {
                        Logging.Logger.Log("DEBUG", "Processing unload message from home view model.");
                        DateTime gpsStart = DateTime.Now.ToUniversalTime().AddSeconds(-3);
                        DateTime gpsEnd = DateTime.Now.ToUniversalTime();

                        double latitude = GPSDataProvider.GetAverageLatitude(gpsStart, gpsEnd);
                        double longitude = GPSDataProvider.GetAverageLongitude(gpsStart, gpsEnd);


                        /*if (_windowService.IsWindowOpen(WindowType.PickupWindow))
                        {
                            _windowService.CloseModalWindow(WindowType.PickupWindow);
                        }*/

                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            UnloadingAtGinViewModel vm = SimpleIoc.Default.GetInstance<UnloadingAtGinViewModel>();
                            vm.SerialNumber = msg.SerialNumber;
                            vm.Refresh();
                            if (!_windowService.IsWindowOpen(WindowType.UnloadingAtGin))
                            {
                                vm.Initialize();
                                _windowService.ShowModalWindow(WindowType.UnloadingAtGin, vm);
                            }
                            else
                            {
                                vm.Refresh();
                            }
                        }));
                    }
                }
            });
        }
    }    

    public class ComboBoxItemViewModel
    {
        public string ID { get; set; }
        public string DisplayText { get; set; }
    }

    public class FarmComboBoxItemViewModel : ComboBoxItemViewModel
    {
        public string ClientID { get; set; }
    }

    public class FieldComboBoxItemViewModel : ComboBoxItemViewModel
    {
        public string ClientID { get; set; }
        public string FarmID { get; set; }
    }

    public class PickupListComboBoxItemViewModel : ComboBoxItemViewModel
    {
        public string ProducerID { get; set; }
        public string FarmID { get; set; }
        public string FieldID { get; set; }
    }

    public class PickupListGridItem : ViewModelBase
    {
        private string _client;
        public string Client
        {
            get
            {
                return _client;
            }
            set
            {
                Set<string>(() => Client, ref _client, value);
            }
        }

        private string _farm;
        public string Farm
        {
            get
            {
                return _farm;
            }
            set
            {
                Set<string>(() => Farm, ref _farm, value);
            }
        }

        private string _field;
        public string Field
        {
            get
            {
                return _field;
            }
            set
            {
                Set<string>(() => Field, ref _field, value);
            }
        }

        private string _listName;
        public string ListName
        {
            get
            {
                return _listName;
            }
            set
            {
                Set<string>(() => ListName, ref _listName, value);
            }
        }

        private string _listID;
        public string ListID
        {
            get
            {
                return _listID;
            }
            set
            {
                Set<string>(() => ListID, ref _listID, value);
            }
        }

        private bool _checked;
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                Set<bool>(() => Checked, ref _checked, value);
            }
        }
    }
}
