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
using CottonDBMS.Data.EF;
using CottonDBMS.TruckApp.Messages;
using CottonDBMS.DataModels;
using System.Collections.ObjectModel;
using CottonDBMS.TruckApp.Navigation;
using CottonDBMS.TruckApp.DataProviders;
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Interfaces;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class PickUpListViewModel : ViewModelBase
    {
        private IWindowService _windowService = null;
        private System.Threading.Timer timer = null;        
        LoadingIncorrectModuleViewModel incorrectModuleVM = SimpleIoc.Default.GetInstance<LoadingIncorrectModuleViewModel>();
        private PickupListEntity pickupList = null;
        private SyncedSettings syncedSettings = null;
        private bool defaultToMap = true;

        private bool _hasNetwork;
        public bool HasNetwork
        {
            get
            {
                return _hasNetwork;
            }
            set
            {
                Set<bool>(() => HasNetwork, ref _hasNetwork, value);
            }
        }

        private bool _mapVisible;
        public bool MapVisible
        {
            get
            {
                return _mapVisible;
            }
            set
            {             
                Set<bool>(() => MapVisible, ref _mapVisible, value);
            }
        }

        private bool _hasCheckedModules;
        public bool HasCheckedModules
        {
            get
            {
                return _hasCheckedModules;
            }
            set
            {
                Set<bool>(() => HasCheckedModules, ref _hasCheckedModules, value);
            }
        }


        private double _fieldLat;
        public double FieldLat
        {
            get
            {
                return _fieldLat;
            }
            set
            {
                Set<double>(() => FieldLat, ref _fieldLat, value);
            }
        }

        private double _fieldLong;
        public double FieldLong
        {
            get
            {
                return _fieldLong;
            }
            set
            {
                Set<double>(() => FieldLong, ref _fieldLong, value);
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

        public TagLoadingMessage InitiatingEvent { get; set; }

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

        private int _modulesInField;
        public int ModulesInField
        {
            get
            {
                return _modulesInField;
            }
            set
            {
                Set<int>(() => ModulesInField, ref _modulesInField, value);
            }
        }

        private int _modulesLoaded;
        public int ModulesLoaded
        {
            get
            {
                return _modulesLoaded;
            }
            set
            {
                Set<int>(() => ModulesLoaded, ref _modulesLoaded, value);
            }
        }

        private int _modulesOnList;
        public int ModulesOnList
        {
            get
            {
                return _modulesOnList;
            }
            set
            {
                Set<int>(() => ModulesOnList, ref _modulesOnList, value);
            }
        }

        private int _loadsRemaining;
        public int LoadsRemaining
        {
            get
            {
                return _loadsRemaining;
            }
            set
            {
                Set<int>(() => LoadsRemaining, ref _loadsRemaining, value);
            }
        }

        private int _loadsCompleted;
        public int LoadsCompleted
        {
            get
            {
                return _loadsCompleted;
            }
            set
            {
                Set<int>(() => LoadsCompleted, ref _loadsCompleted, value);
            }
        }

        private string _destinationName;
        public string DestinationName
        {
            get
            {
                return _destinationName;
            }
            set
            {
                Set<string>(() => DestinationName, ref _destinationName, value);
            }
        }

        private double _currentLat;
        public double CurrentLat
        {
            get
            {
                return _currentLat;
            }
            set
            {
                Set<double>(() => CurrentLat, ref _currentLat, value);
            }
        }

        private double _currentLong;
        public double CurrentLong
        {
            get
            {
                return _currentLong;
            }
            set
            {
                Set<double>(() => CurrentLong, ref _currentLong, value);
            }
        }

        public ObservableCollection<ModuleViewModel> Modules { get; set; }
        public ObservableCollection<ModuleViewModel> ListModules { get; set; }
        
        public RelayCommand ForceUnloadCommand { get; private set; }
        private void ExecuteForceUnloadCommand()
        {
            //iterate through modules and execute force unload
            var snsOnTruck = AggregateDataProvider.SerialNumbersOnTruck.ToArray();
            foreach (var moduleVM in ListModules.Where(m => m.Selected && snsOnTruck.Any(s => s == m.SerialNumber)))
            {
                AggregateDataProvider.ForceUnload(moduleVM.SerialNumber);
            }

            LoadData(ListID);          
        }

        public RelayCommand ForceLoadCommand { get; private set; }
        private void ExecuteLoadCommand()
        {
            //iterate through modules and execute force unload
            foreach (var moduleVM in ListModules.Where(m => m.Selected))
            {
                AggregateDataProvider.ForceLoad(moduleVM.SerialNumber);
            }

            LoadData(ListID);            
        }

        public RelayCommand CloseCommand { get; private set; }
        private void ExecuteCloseCommand()
        {
            _windowService.CloseModalWindow(WindowType.PickupWindow);
        }

        public RelayCommand ShowMapCommand { get; private set; }
        private void ExecuteShowMapCommand()
        {
            MapVisible = true;
            defaultToMap = true;
            Messenger.Default.Send<MapViewRequested>(new MapViewRequested());
        }

        public RelayCommand SetGinCommand { get; private set; }
        private void ExecuteGinCommand()
        {
            CottonDBMS.TruckApp.Windows.OverrideGPS overrideWindow = new CottonDBMS.TruckApp.Windows.OverrideGPS();
            overrideWindow.Show();
        }

        public RelayCommand SetYardCommand { get; private set; }
        private void ExecuteYardCommand()
        {
            MapVisible = true;
        }
        
        public RelayCommand ShowListCommand { get; private set; }
        private void ExecuteShowListCommand()
        {
            MapVisible = false;
            defaultToMap = false;
        }

        public RelayCommand DirectionsCommand { get; private set; }
        private void ExecuteDirectionsCommand()
        {
            var currentCoords = GPSDataProvider.GetLastCoords();

            double? destLat = null;
            double? destLng = null;
            double? myLat = null;
            double? myLong = null;

            if (Modules.Count() > 0)
            {
                destLat = Modules[0].Latitude;
                destLng = Modules[0].Longitude;
            }
            else
            {
                destLat = FieldLat;
                destLng = FieldLong;
            }

            if (currentCoords != null)
            {
                myLat = currentCoords.Latitude;
                myLong = currentCoords.Longitude;
            }
            else
            {
                myLat = 33.5779;
                myLong = -101.8552;
            }            

            _windowService.LaunchDirections(destLat, destLng, myLat, myLong);
        }

        public PickUpListViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            CloseCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteCloseCommand);
            DirectionsCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteDirectionsCommand);
            ShowMapCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteShowMapCommand);
            ShowListCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteShowListCommand);
            SetGinCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteGinCommand);
            SetYardCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteYardCommand);
            ForceUnloadCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteForceUnloadCommand);
            ForceLoadCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteLoadCommand);
            Modules = new ObservableCollection<ModuleViewModel>();
            ListModules = new ObservableCollection<ModuleViewModel>();
        }               

        public void Initialize()
        {
            MapVisible = defaultToMap;
            var lastCoords = GPSDataProvider.GetLastCoords(DateTime.Now.ToUniversalTime().AddSeconds(-5), DateTime.Now.ToUniversalTime());
            CurrentLat = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLat(lastCoords);
            CurrentLong = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLong(lastCoords);

            FieldLat = 0.000;
            FieldLong = 0.000;
            HasCheckedModules = false;

            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                if (InitiatingEvent != null)  //find the list based on serial number
                {
                    PickupListDestination dest = PickupListDestination.GIN_YARD;
                    if (uow.SettingsRepository.CoordsOnGinYard(CurrentLat, CurrentLong) || uow.SettingsRepository.CoordsAtFeeder(CurrentLat, CurrentLong))
                    {
                        dest = PickupListDestination.GIN_FEEDER;
                    }
                    ListID = uow.PickupListRepository.GetListIDWithSerialNumber(InitiatingEvent.SerialNumber, CurrentLat, CurrentLong, dest);
                }
            }
            LoadData(ListID);
                        
            Messenger.Default.Register<GPSEventMessage>(this, (action) => ProcessGPSMessage(action));
            Messenger.Default.Register<TagLoadingMessage>(this, (action) => ProcessTagLoadingMessage(action));
            Messenger.Default.Register<AggregateEvent>(this, (action) => ProcessAggregatedEvent(action));            

            if (InitiatingEvent != null)
            {
                ProcessTagLoadingMessage(InitiatingEvent);
            }
            
           Task.Run(() =>
           {
               checkForInternet();
               timer = new System.Threading.Timer(timerCallback, null, 500, 1000);
           });
        }

        public void LoadData(string listId)
        {

            try
            {
                //LOAD DATA BASED ON LIST ID    
                using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {
                    pickupList = uow.PickupListRepository.GetById(listId, "Field.Farm.Client", "AssignedModules");
                    syncedSettings = uow.SyncedSettingsRepo.GetAll().FirstOrDefault();

                    if (pickupList.Destination == PickupListDestination.GIN_YARD && pickupList.AssignedModules.Count(m => m.ModuleStatus == ModuleStatus.IN_FIELD) > 0)
                    {
                        var firstModule = pickupList.AssignedModules.First();
                        FieldLat = firstModule.Latitude;
                        FieldLong = firstModule.Longitude;
                    }
                    else if (pickupList.Destination == PickupListDestination.GIN_FEEDER && pickupList.AssignedModules.Count(m => m.ModuleStatus == ModuleStatus.AT_GIN) > 0)
                    {
                        var firstModule = pickupList.AssignedModules.First();
                        FieldLat = firstModule.Latitude;
                        FieldLong = firstModule.Longitude;
                    }

                    if (FieldLat == 0.000 || FieldLong == 0.000)
                    {
                        FieldLat = pickupList.Latitude;
                        FieldLong = pickupList.Longitude;
                    }

                    ListName = pickupList.Name;
                    Client = pickupList.ClientName;
                    Farm = pickupList.FarmName;
                    Field = pickupList.Field.Name;

                    if (syncedSettings == null) pickupList.ModulesPerLoad = 4;
                    else pickupList.ModulesPerLoad = syncedSettings.ModulesPerLoad;

                    ModulesInField = pickupList.ModulesRemaining;
                    ModulesLoaded = pickupList.AssignedModules.Count(c => AggregateDataProvider.SerialNumbersOnTruck.Contains(c.Name));

                    //adjust modules in field by modules on truck
                    if (pickupList.Destination == PickupListDestination.GIN_YARD)
                    {
                        foreach (var module in pickupList.AssignedModules.Where(s => s.ModuleStatus == ModuleStatus.IN_FIELD && AggregateDataProvider.SerialNumbersOnTruck.Any(x => x == s.Name)))
                        {
                            ModulesInField--;
                        }
                    }
                    else if (pickupList.Destination == PickupListDestination.GIN_FEEDER)
                    {
                        foreach (var module in pickupList.AssignedModules.Where(s => s.ModuleStatus == ModuleStatus.AT_GIN && AggregateDataProvider.SerialNumbersOnTruck.Any(x => x == s.Name)))
                        {
                            ModulesInField--;
                        }
                    }

                    ModulesOnList = pickupList.TotalModules;
                    LoadsCompleted = pickupList.LoadsCompleted;
                    LoadsRemaining = pickupList.LoadsRemaining;
                    DestinationName = pickupList.DestinationName;
                                        
                    setLoadsRemaining();

                    HasCheckedModules = false;

                    System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        foreach(var m in ListModules)
                        {
                            m.PropertyChanged -= MVm_PropertyChanged;
                        }

                        Modules.Clear();
                        ListModules.Clear();
                        foreach (var m in pickupList.AssignedModules.OrderBy(s => s.Name))
                        {
                            //using not thread safe version of serial numbers on truck because this event gets fired 
                            //inside a lock on the _aggregateEvents in the provider
                            //using the threadsafe property will cause a thread lock on the same property
                            var mVm = new ModuleViewModel { Loaded = AggregateDataProvider.SerialNumbersOnTruckNotThreadSafe.Contains(m.Name), Latitude = m.Latitude, Longitude = m.Longitude, SerialNumber = m.Name };
                            mVm.ShowOnMap = (m.ModuleStatus == ModuleStatus.IN_FIELD && pickupList.Destination == PickupListDestination.GIN_YARD) ||
                                            (m.ModuleStatus == ModuleStatus.AT_GIN && pickupList.Destination == PickupListDestination.GIN_FEEDER);
                            Modules.Add(mVm);

                            if (mVm.ShowOnMap || mVm.Loaded)
                            {
                                mVm.PropertyChanged += MVm_PropertyChanged;
                                ListModules.Add(mVm);
                            }
                        }
                    }));
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void MVm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            HasCheckedModules = ListModules.Any(x => x.Selected) && !MapVisible;
        }

        private void setLoadsRemaining()
        {
            if (syncedSettings != null)
            {
                int loadsInField = ModulesInField / syncedSettings.ModulesPerLoad;

                if (ModulesInField % syncedSettings.ModulesPerLoad > 0 && ModulesInField > 0)
                {
                    loadsInField++;
                }
                LoadsRemaining = loadsInField;
            }
        }

        private void checkForInternet()
        {
            var temp = _hasNetwork;
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    using (webClient.OpenRead("http://google.com"))
                    {
                        HasNetwork = true;
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                HasNetwork = false;
            }

            if (temp != _hasNetwork)
            {
                Messenger.Default.Send<NetworkStatus>(new NetworkStatus { HasInternet = _hasNetwork });
            }
        }

        private void timerCallback(object state)
        {
            //check network connection            
            checkForInternet();
        }

        private void ProcessGPSMessage(GPSEventMessage e)
        {
            CurrentLat = e.Latitude;
            CurrentLong = e.Longitude;
        }

        private void ProcessTagLoadingMessage(TagLoadingMessage msg)
        {
            if (!_windowService.IsWindowOpen(WindowType.LoadingAtGin))
            {
                Task.Run(() =>
                {
                    Logging.Logger.Log("DEBUG", "Processing tag loading message from pickup list " + msg.SerialNumber);
                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            DateTime gpsStart = DateTime.Now.ToUniversalTime().AddSeconds(-3);
                            DateTime gpsEnd = DateTime.Now.ToUniversalTime();

                            double latitude = GPSDataProvider.GetAverageLatitude(gpsStart, gpsEnd);
                            double longitude = GPSDataProvider.GetAverageLongitude(gpsStart, gpsEnd);

                        //if we're not at the gin yard or feeder - we must be loading in the field
                        //if (!uow.SettingsRepository.CoordsAtFeeder(latitude, longitude) && !uow.SettingsRepository.CoordsOnGinYard(latitude, longitude))
                           // {
                                bool moduleOnList = this.Modules.Any(m => m.SerialNumber.Trim().ToLower() == msg.SerialNumber.Trim().ToLower());

                            if (moduleOnList && !_windowService.IsWindowOpen(WindowType.LoadingIncorrectModuleWindow) && !_windowService.IsWindowOpen(WindowType.WaitingForUnloadWindow))
                            {
                                if (!_windowService.IsWindowOpen(WindowType.LoadingWindow))
                                {
                                    LoadingWindowViewModel vm = SimpleIoc.Default.GetInstance<LoadingWindowViewModel>();
                                    vm.SerialNumber = msg.SerialNumber;
                                    vm.Initialize(this.ListID);
                                    _windowService.ShowModalWindow(WindowType.LoadingWindow, vm);
                                }
                                else //window is already open but another module passed under antenna view
                                {
                                    LoadingWindowViewModel vm = SimpleIoc.Default.GetInstance<LoadingWindowViewModel>();
                                    vm.NewModuleDetected(msg.SerialNumber);
                                }
                            }
                            else //we're in the incorrect module sequence or we detected an incorrect module previously
                            {
                                if (_windowService.IsWindowOpen(WindowType.LoadingWindow)) _windowService.CloseModalWindow(WindowType.LoadingWindow);

                                if (!_windowService.IsWindowOpen(WindowType.LoadingIncorrectModuleWindow))
                                {

                                    if (_windowService.IsWindowOpen(WindowType.WaitingForUnloadWindow))
                                        _windowService.CloseModalWindow(WindowType.WaitingForUnloadWindow);

                                    if (_windowService.IsWindowOpen(WindowType.UnloadCorrectionWindow))
                                        _windowService.CloseModalWindow(WindowType.UnloadCorrectionWindow);

                                    incorrectModuleVM.ActiveListID = this.ListID;
                                    incorrectModuleVM.ListName = this.ListName;
                                    incorrectModuleVM.Client = this.Client;
                                    incorrectModuleVM.Farm = this.Farm;
                                    incorrectModuleVM.Field = this.Field;
                                    incorrectModuleVM.Initialize(false);
                                    incorrectModuleVM.NewModuleLoaded(msg.SerialNumber, moduleOnList);
                                    _windowService.ShowModalWindow(WindowType.LoadingIncorrectModuleWindow, incorrectModuleVM);
                                }
                                else //window is already open
                                {
                                    incorrectModuleVM.NewModuleLoaded(msg.SerialNumber, moduleOnList);
                                }
                            }

                            
                            //}

                            Logging.Logger.Log("DEBUG", "Done Processing tag loading message from pickup list " + msg.SerialNumber);
                        }));
                    }
                });
            }
        }     

        private void ProcessAggregatedEvent(AggregateEvent data)
        {
            Logging.Logger.Log("DEBUG", "Processing load/unload event in pickup list.");
            if (pickupList != null)
            {
                LoadData(ListID);
                if (data.EventType == EventType.LOADED)
                {
                    if (_windowService.IsWindowOpen(WindowType.LoadingWindow)) _windowService.CloseModalWindow(WindowType.LoadingWindow);
                }
            }           
        }        

        private void ProcessTagUnloadingMessage()
        {
            //show dialog for unloading tag
        }

        public void DoCleanUp()
        {            
            Messenger.Default.Unregister<GPSEventMessage>(this);            
            Messenger.Default.Unregister<TagLoadingMessage>(this);
            Messenger.Default.Unregister<AggregateEvent>(this);            
            timer.Dispose();
        }        
    }        
}
