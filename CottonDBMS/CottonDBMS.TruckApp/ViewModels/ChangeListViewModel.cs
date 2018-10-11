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
    public class ChangeListViewModel : ViewModelBase
    {
        private IWindowService _windowService = null;
        
        public ObservableCollection<ComboBoxItemViewModel> Producers { get; private set; }
        public ObservableCollection<FarmComboBoxItemViewModel> Farms { get; private set; }
        public ObservableCollection<FieldComboBoxItemViewModel> Fields { get; private set; }
        public ObservableCollection<PickupListComboBoxItemViewModel> PickupLists { get; set; }

        public string ActiveListID { get; set; }

        public ObservableCollection<ModuleViewModel> ModulesOnTruck { get; set; }

        private string _currentClient;
        public string CurrentClient
        {
            get
            {
                return _currentClient;
            }
            set
            {
                Set<string>(() => CurrentClient, ref _currentClient, value);
            }
        }

        private string _currentFarm;
        public string CurrentFarm
        {
            get
            {
                return _currentFarm;
            }
            set
            {
                Set<string>(() => CurrentFarm, ref _currentFarm, value);
            }
        }

        private string _currentField;
        public string CurrentField
        {
            get
            {
                return _currentField;
            }
            set
            {
                Set<string>(() => CurrentField, ref _currentField, value);
            }
        }

        private string _currentList;
        public string CurrentList
        {
            get
            {
                return _currentList;
            }
            set
            {
                Set<string>(() => CurrentList, ref _currentList, value);
            }
        }

        private bool _ShowNewProducerText;
        public bool ShowNewProducerText
        {
            get
            {
                return _ShowNewProducerText;
            }
            set
            {
                Set<bool>(() => ShowNewProducerText, ref _ShowNewProducerText, value);
            }
        }

        private string _NewProducerText;
        public string NewProducerText
        {
            get
            {
                return _NewProducerText;
            }
            set
            {
                Set<string>(() => NewProducerText, ref _NewProducerText, value);
            }
        }

        private bool _ShowFarmCombo;
        public bool ShowFarmCombo
        {
            get
            {
                return _ShowFarmCombo;
            }
            set
            {
                Set<bool>(() => ShowFarmCombo, ref _ShowFarmCombo, value);
            }
        }

        private bool _ShowNewFarmText;
        public bool ShowNewFarmText
        {
            get
            {
                return _ShowNewFarmText;
            }
            set
            {
                Set<bool>(() => ShowNewFarmText, ref _ShowNewFarmText, value);
            }
        }

        private string _NewFarmText;
        public string NewFarmText
        {
            get
            {
                return _NewFarmText;
            }
            set
            {
                Set<string>(() => NewFarmText, ref _NewFarmText, value);
            }
        }

        private bool _showFieldCombo;
        public bool ShowFieldCombo
        {
            get
            {
                return _showFieldCombo;
            }
            set
            {
                Set<bool>(() => ShowFieldCombo, ref _showFieldCombo, value);
            }
        }

        private bool _showFieldText;
        public bool ShowFieldText
        {
            get
            {
                return _showFieldText;
            }
            set
            {
                Set<bool>(() => ShowFieldText, ref _showFieldText, value);
            }
        }

        private string _NewFieldText;
        public string NewFieldText
        {
            get
            {
                return _NewFieldText;
            }
            set
            {
                Set<string>(() => NewFieldText, ref _NewFieldText, value);
            }
        }

        private bool _showListCombo;
        public bool ShowListCombo
        {
            get
            {
                return _showListCombo;
            }
            set
            {
                Set<bool>(() => ShowListCombo, ref _showListCombo, value);
            }
        }

        private bool _showListText;
        public bool ShowListText
        {
            get
            {
                return _showListText;
            }
            set
            {
                Set<bool>(() => ShowListText, ref _showListText, value);
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

        public ObservableCollection<string> ErrorMessages { get; set; }

        private bool _hasErrors;
        public bool HasErrors
        {
            get
            {
                return _hasErrors;
            }
            set
            {
                Set<bool>(() => HasErrors, ref _hasErrors, value);
            }
        }

        private void refreshErrorState()
        {
            if (ErrorMessages == null) ErrorMessages = new ObservableCollection<string>();

            ErrorMessages.Clear();

            if (String.IsNullOrWhiteSpace(ListName) && ShowListText)
            {
                ErrorMessages.Add("List name is required.");
            }
            else if (!ShowListText && (_selectedPickupList == null || _selectedPickupList.ID == "-1"))
            {
                ErrorMessages.Add("List is required.");
            }

            if (!(ShowNewProducerText) && (_selectedProducer == null || _selectedProducer.ID  == "-1"))
            {
                ErrorMessages.Add("Client is required.");
            }
            else if (ShowNewProducerText && string.IsNullOrWhiteSpace(_NewProducerText))
            {
                ErrorMessages.Add("Client is required.");
            }

            if (!(ShowNewFarmText) && (_selectedFarm == null || _selectedFarm.ID == "-1"))
            {
                ErrorMessages.Add("Farm is required.");
            }
            else if (ShowNewFarmText && string.IsNullOrWhiteSpace(_NewFarmText))
            {
                ErrorMessages.Add("Farm is required.");
            }

            if (!ShowFieldText && (_selectedField == null || _selectedField.ID == "-1"))
            {
                ErrorMessages.Add("Field is required.");
            }
            else if (ShowFieldText && string.IsNullOrWhiteSpace(_NewFieldText))
            {
                ErrorMessages.Add("Field is required.");
            }

            HasErrors = (ErrorMessages.Count > 0);
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
                if (value == null || value.ID == "-1")
                {
                    ShowFarmCombo = false;
                    ShowNewFarmText = false;
                    ShowNewProducerText = false;
                    ShowFieldText = false;
                    ShowListText = false;
                }
                else if (value.ID == "")
                {
                    ShowFarmCombo = false;
                    ShowNewFarmText = true;
                    ShowNewProducerText = true;
                    ShowFieldText = true;
                    ShowListText = true;
                    ShowListCombo = false;
                    ShowFieldCombo = false;
                }
                else
                {
                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        //load farms for producer and default to select one
                        Farms.Clear();
                        Farms.Add(new FarmComboBoxItemViewModel { ID = "-1", DisplayText = "-- Select One --" });
                        Farms.Add(new FarmComboBoxItemViewModel { ID = "", DisplayText = "-- Add New --" });
                        foreach (var f in uow.FarmRepository.GetAll().OrderBy(f => f.Name))
                        {
                            if (f.ClientId == value.ID && f.Id != GUIDS.UNASSIGNED_FARM_ID)
                            {
                                Farms.Add(new FarmComboBoxItemViewModel { ClientID = f.ClientId, ID = f.Id, DisplayText = f.Name });
                            }
                        }

                        ShowFieldText = false;
                        ShowListText = false;
                        ShowFarmCombo = true;
                        ShowNewProducerText = false;
                        ShowNewFarmText = false;

                        SelectedFarm = Farms[0];
                    }
                }

                Set<ComboBoxItemViewModel>(() => SelectedProducer, ref _selectedProducer, value);
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
                if (value == null || value.ID == "-1")
                {
                    ShowFieldCombo = false;
                    ShowFieldText = false;
                    ShowListText = false;
                    ShowNewFarmText = false;
                }
                else if (value.ID == "")
                {
                    ShowFieldCombo = false;
                    ShowFieldText = true;
                    ShowNewFarmText = true;
                    ShowListText = true;
                    ShowListCombo = false;
                }
                else
                {
                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        //load farms for producer and default to select one
                        Fields.Clear();
                        Fields.Add(new FieldComboBoxItemViewModel { ID = "-1", DisplayText = "-- Select One --" });
                        Fields.Add(new FieldComboBoxItemViewModel { ID = "", DisplayText = "-- Add New --" });

                        foreach (var f in uow.FieldRepository.GetAll(new string[] { "Farm.Client" }).OrderBy(f => f.Name))
                        {
                            if (f.FarmId == value.ID && f.Id != GUIDS.UNASSIGNED_FIELD_ID)
                            {
                                Fields.Add(new FieldComboBoxItemViewModel { ClientID = f.Farm.Client.Name, FarmID = f.FarmId, ID = f.Id, DisplayText = f.Name });
                            }
                        }

                        ShowFieldText = false;
                        ShowFieldCombo = true;
                        ShowNewFarmText = false;
                        ShowListText = false;
                        ShowListCombo = false;

                        SelectedField = Fields[0];
                    }
                }

                Set<FarmComboBoxItemViewModel>(() => SelectedFarm, ref _selectedFarm, value);

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
                if (value == null || value.ID == "-1")
                {
                    ShowListCombo = false;
                    ShowListText = false;
                    ShowFieldText = false;
                }
                else if (value.ID == "")
                {
                    ShowListCombo = false;
                    ShowListText = true;
                    ShowFieldText = true;
                }
                else
                {
                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        //load farms for producer and default to select one
                        PickupLists.Clear();
                        PickupLists.Add(new PickupListComboBoxItemViewModel { ID = "-1", DisplayText = "-- Select One --" });
                        PickupLists.Add(new PickupListComboBoxItemViewModel { ID = "", DisplayText = "-- Add New --" });

                        var lastCoords = GPSDataProvider.GetLastCoords();
                        double lat = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLat(lastCoords);
                        double lon = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLong(lastCoords);

                        bool atGin = uow.SettingsRepository.CoordsOnGinYard(lat, lon);

                        foreach (var f in uow.PickupListRepository.GetAll(new string[] { "Field.Farm" }).OrderBy(p => p.Name))
                        {
                            if (f.FieldId == value.ID && f.Id != GUIDS.UNASSIGNED_LIST_ID)
                            {
                                if (f.Destination == PickupListDestination.GIN_YARD && !atGin ||
                                    f.Destination == PickupListDestination.GIN_FEEDER && atGin)
                                {
                                    PickupLists.Add(new PickupListComboBoxItemViewModel { DisplayText = f.Name, ID = f.Id, FarmID = f.Field.FarmId, FieldID = f.FieldId, ProducerID = f.Field.Farm.ClientId });
                                }
                            }
                        }

                        ShowListText = false;
                        ShowListCombo = true;
                        ShowFieldText = false;

                        SelectedPickupList = PickupLists[0];
                    }
                }

                Set<FieldComboBoxItemViewModel>(() => SelectedField, ref _selectedField, value);
            }
        }

        private PickupListComboBoxItemViewModel _selectedPickupList;
        public PickupListComboBoxItemViewModel SelectedPickupList
        {
            get
            {
                return _selectedPickupList;
            }
            set
            {
                if (value == null || value.ID == "-1")
                {                    
                    ShowListText = false;                   
                }
                else if (value.ID == "")
                {
                    ShowListText = true;
                }
                else
                {
                    ShowListText = false;
                }

                Set<PickupListComboBoxItemViewModel>(() => SelectedPickupList, ref _selectedPickupList, value);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        private void ExecuteSaveCommand()
        {
            refreshErrorState();

            if (!HasErrors)
            {
                using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                {
                    string clientName = "";
                    if (ShowNewProducerText) clientName = NewProducerText.Trim();
                    else clientName = SelectedProducer.DisplayText;

                    string farmName = "";
                    if (ShowNewFarmText) farmName = NewFarmText.Trim();
                    else farmName = SelectedFarm.DisplayText;

                    string fieldName = "";
                    if (ShowFieldText) fieldName = NewFieldText.Trim();
                    else fieldName = SelectedField.DisplayText.Trim();

                    var coords = GPSDataProvider.GetLastCoords();
                    double lat = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLat(coords);
                    double lng = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLong(coords);

                    var client = uow.ClientRepository.EnsureClientCreated(clientName, InputSource.TRUCK);
                    uow.SaveChanges();
                    var farm = uow.FarmRepository.EnsureFarmCreated(client, farmName, InputSource.TRUCK);
                    uow.SaveChanges();
                    var field = uow.FieldRepository.EnsureFieldCreated(farm, fieldName, InputSource.TRUCK);
                    field.Latitude = lat;
                    field.Longitude = lng;
                    uow.SaveChanges();

                    var currentTruck = uow.SettingsRepository.GetCurrentTruck();
                    PickupListEntity list = null;
                   
                    if (ShowListText)
                    {
                        list = new PickupListEntity();
                        list.Id = Guid.NewGuid().ToString();
                        list.Created = DateTime.UtcNow;
                        list.FieldId = field.Id;
                        list.Latitude = lat;
                        list.Longitude = lng;
                        list.Name = ListName;
                        list.PickupListStatus = PickupListStatus.OPEN;
                        list.Source = InputSource.TRUCK;
                        list.SyncedToCloud = false;
                        list.AssignedModules = new List<ModuleEntity>();
                        list.AssignedTrucks = new List<TruckEntity>();
                        list.DownloadedToTrucks = new List<TruckEntity>();  

                        if (uow.SettingsRepository.CoordsOnGinYard(lat, lng) || uow.SettingsRepository.CoordsAtFeeder(lat, lng))
                        {
                            list.Destination = PickupListDestination.GIN_FEEDER;
                        }
                        else
                        {
                            list.Destination = PickupListDestination.GIN_YARD;
                        }
                        
                        if (currentTruck != null) {
                            list.AssignedTrucks.Add(currentTruck);
                            list.DownloadedToTrucks.Add(currentTruck);
                        }
                        uow.PickupListRepository.Add(list);
                        uow.SaveChanges();
                    }
                    else
                    {
                        list = uow.PickupListRepository.GetById(SelectedPickupList.ID,"Field.Farm.Client", "AssignedModules", "DownloadedToTrucks", "AssignedTrucks");                                                          
                    }

                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<PickupListAddedMessage>(new PickupListAddedMessage { Id = list.Id });

                    //assign all serial numbers on truck to the new list
                    uow.PickupListRepository.MoveModulesToList(list, AggregateDataProvider.SerialNumbersOnTruck, lat, lng);
                    _windowService.CloseModalWindow(WindowType.ChangeListWindow);
                    _windowService.CloseModalWindow(WindowType.LoadingIncorrectModuleWindow);
                    _windowService.CloseModalWindow(WindowType.PickupWindow);

                    //re-open pickup window with new list assignment
                    PickUpListViewModel vm = SimpleIoc.Default.GetInstance<PickUpListViewModel>();
                    vm.ListID = list.Id;
                    vm.InitiatingEvent = null;                  
                    _windowService.ShowModalWindow(WindowType.PickupWindow, vm);
                }
            }
        }

        public RelayCommand CancelCommand { get; private set; }
        private void ExecuteCancelCommand()
        {
            _windowService.CloseModalWindow(WindowType.ChangeListWindow);
        }

        public ChangeListViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSaveCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteCancelCommand);

            ModulesOnTruck = new ObservableCollection<ModuleViewModel>();
        }

        public void Initialize()
        {
            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                NewFarmText = "";
                ListName = "";
                NewProducerText = "";
                NewFieldText = "";

                ShowFarmCombo = false;                
                ShowFieldCombo = false;
                ShowListCombo = false;

                var list = uow.PickupListRepository.GetById(ActiveListID, "Field.Farm.Client");


                var clients = uow.ClientRepository.GetAll(new string[] {"Farms.Fields"}).OrderBy(t => t.Name);
                var lists = uow.PickupListRepository.GetAll();

                ErrorMessages = new ObservableCollection<string>();
                Producers = new ObservableCollection<ComboBoxItemViewModel>();
                Producers.Add(new ComboBoxItemViewModel { ID = "-1", DisplayText = "-- Select One --" });
                Producers.Add(new ComboBoxItemViewModel { ID = "", DisplayText = "-- Add New --" });

                Farms = new ObservableCollection<FarmComboBoxItemViewModel>();
                Farms.Add(new FarmComboBoxItemViewModel { ID = "-1", ClientID = "", DisplayText = "-- Select One --" });
                Farms.Add(new FarmComboBoxItemViewModel { ID = "", ClientID = "", DisplayText = "-- Add New --" });

                Fields = new ObservableCollection<FieldComboBoxItemViewModel>();
                Fields.Add(new FieldComboBoxItemViewModel { ID = "-1", ClientID = "", DisplayText = "-- Select One --" });
                Fields.Add(new FieldComboBoxItemViewModel { ID = "", ClientID = "", DisplayText = "-- Add New --" });

                PickupLists = new ObservableCollection<PickupListComboBoxItemViewModel>();
                CurrentClient = list.Field.Farm.Client.Name;
                CurrentFarm = list.Field.Farm.Name;
                CurrentField = list.Field.Name;
                CurrentList = list.Name;

                if (list.Id == GUIDS.UNASSIGNED_LIST_ID)
                {
                    CurrentClient = "Unassigned";
                    CurrentFarm = "";
                    CurrentField = "";
                    CurrentList = "";
                }
                                             
                foreach(var client in clients.Where(c => c.Id != GUIDS.UNASSIGNED_CLIENT_ID))
                {
                    var producer = new ComboBoxItemViewModel { ID = client.Id, DisplayText = client.Name  };
                    Producers.Add(producer);                    
                }
                
                SelectedProducer = Producers[0];
              
                ModulesOnTruck.Clear();

                foreach (var sn in AggregateDataProvider.SerialNumbersOnTruck)
                {
                    ModulesOnTruck.Add(new ViewModels.ModuleViewModel { SerialNumber = sn });                    
                }
            }
        }
    }
}
