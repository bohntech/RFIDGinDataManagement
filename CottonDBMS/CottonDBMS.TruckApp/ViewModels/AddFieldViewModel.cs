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
using GalaSoft.MvvmLight.Ioc;
using CottonDBMS.Interfaces;
using CottonDBMS.Cloud;
using CottonDBMS.TruckApp.DataProviders;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class AddPickUpListViewModel : ViewModelBase
    {
        private IWindowService _windowService = null;
        
        public  ObservableCollection<ComboBoxItemViewModel> Producers { get; private set; }
        public ObservableCollection<FarmComboBoxItemViewModel> Farms { get; private set; }
        public ObservableCollection<FieldComboBoxItemViewModel> Fields { get; private set; }

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

            string clientId = "";
            string farmId = "";
            

            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString())) {

                if (String.IsNullOrWhiteSpace(ListName))
                {
                    ErrorMessages.Add("List name is required.");
                }
                else if (!uow.PickupListRepository.CanSavePickupList("", _listName))
                {
                    ErrorMessages.Add("List name already in use.");
                }

                if (_selectedProducer == null || _selectedProducer.ID == "-1")
                {
                    ErrorMessages.Add("Client is required.");
                }
                else if (ShowNewProducerText)
                {
                    if (string.IsNullOrWhiteSpace(_NewProducerText))
                    {
                        ErrorMessages.Add("Client is required.");
                    }
                    else if (!uow.ClientRepository.CanSaveClient("", _NewProducerText))
                    {
                        ErrorMessages.Add("Client name already exists.");
                    }
                }
                else
                {
                    clientId = _selectedProducer.ID;
                }

                if (!ShowNewFarmText && (_selectedFarm == null || _selectedFarm.ID == "-1"))
                {
                    ErrorMessages.Add("Farm is required.");
                }
                else if (ShowNewFarmText)
                {
                    if (string.IsNullOrWhiteSpace(_NewFarmText))
                    {
                        ErrorMessages.Add("Farm is required.");
                    }
                    else if (!uow.FarmRepository.CanSaveFarm(clientId, "", _NewFarmText.Trim(), false))
                    {
                        ErrorMessages.Add("Farm name already exists.");
                    }
                }
                else
                {
                    farmId = _selectedFarm.ID;
                }

                if (!ShowFieldText && (_selectedField == null || _selectedField.ID == "-1"))
                {
                    ErrorMessages.Add("Field is required.");
                }
                else if (ShowFieldText)
                {
                    if (string.IsNullOrWhiteSpace(_NewFieldText))
                    {
                        ErrorMessages.Add("Field is required.");
                    }
                    else if (!uow.FieldRepository.CanSaveField(clientId, farmId, "", _NewFieldText.Trim(), false))
                    {
                        ErrorMessages.Add("Field name already exists.");
                    }
                }                               

                HasErrors = (ErrorMessages.Count > 0);
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
                if (value == null || value.ID == "-1")
                {
                    ShowFarmCombo = false;
                    ShowNewFarmText = false;
                    ShowNewProducerText = false;
                    ShowFieldText = false;
                }
                else if (value.ID == "")
                {
                    ShowFarmCombo = false;
                    ShowNewFarmText = true;
                    ShowNewProducerText = true;
                    ShowFieldText = true;
                }
                else
                {
                    //load farms for producer and default to select one
                    Farms.Clear();
                    Farms.Add(new FarmComboBoxItemViewModel { ID = "-1", DisplayText = "-- Select One --" });
                    Farms.Add(new FarmComboBoxItemViewModel { ID = "", DisplayText = "-- Add New --" });

                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        foreach(var farm in uow.FarmRepository.FindMatching(x => x.ClientId == value.ID && x.Id != GUIDS.UNASSIGNED_FARM_ID).OrderBy(x => x.Name))
                        {
                            Farms.Add(new FarmComboBoxItemViewModel { DisplayText = farm.Name, ID = farm.Id, ClientID = farm.ClientId });
                        }
                    }

                    ShowFieldText = false;
                    ShowFarmCombo = true;
                    ShowNewProducerText = false;
                    ShowNewFarmText = false;

                    SelectedFarm = Farms[0];
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
                    ShowNewFarmText = false;
                }
                else if (value.ID == "")
                {
                    ShowFieldCombo = false;
                    ShowFieldText = true;
                    ShowNewFarmText = true;
                }
                else
                {
                    //load farms for producer and default to select one
                    Fields.Clear();
                    Fields.Add(new FieldComboBoxItemViewModel { ID = "-1", DisplayText = "-- Select One --" });
                    Fields.Add(new FieldComboBoxItemViewModel { ID = "", DisplayText = "-- Add New --" });

                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        foreach (var field in uow.FieldRepository.FindMatching(x => x.FarmId == value.ID && x.Id != GUIDS.UNASSIGNED_FIELD_ID, new string[] {"Farm.Client"}).OrderBy(x => x.Name))
                        {
                            Fields.Add(new FieldComboBoxItemViewModel { DisplayText = field.Name, ID = field.Id, ClientID = field.Client.Id, FarmID = field.FarmId });
                        }
                    }

                    ShowFieldText = false;
                    ShowFieldCombo = true;
                    ShowNewFarmText = false;
                    SelectedField = Fields[0];
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
                    ShowFieldText = false;
                }
                else if (value.ID == "")
                {                    
                    ShowFieldText = true;
                }
                else
                {
                    ShowFieldText = false;                    
                }

                Set<FieldComboBoxItemViewModel>(() => SelectedField, ref _selectedField, value);
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        private void ExecuteSaveCommand()
        {
            refreshErrorState();
            if (!HasErrors)
            {
                try
                {
                    using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
                    {
                        //get this truck                         
                        var syncedSetting = uow.SyncedSettingsRepo.GetAll().FirstOrDefault();
                        TruckEntity thisTruck = uow.SettingsRepository.GetCurrentTruck();

                        string clientName = "";
                        if (_selectedProducer.ID == "") clientName = _NewProducerText.Trim();
                        else clientName = _selectedProducer.DisplayText.Trim();

                        string farmName = "";
                        if (ShowNewFarmText) farmName = _NewFarmText.Trim();
                        else farmName = _selectedFarm.DisplayText.Trim();

                        string fieldName = "";
                        if (ShowFieldText) fieldName = _NewFieldText.Trim();
                        else fieldName = _selectedField.DisplayText.Trim();

                        ClientEntity client = uow.ClientRepository.EnsureClientCreated(clientName, InputSource.TRUCK);
                        uow.SaveChanges();                        
                        FarmEntity farm = uow.FarmRepository.EnsureFarmCreated(client, farmName, InputSource.TRUCK);
                        uow.SaveChanges();
                        FieldEntity field = uow.FieldRepository.EnsureFieldCreated(farm, fieldName, InputSource.TRUCK);
                        uow.SaveChanges();

                        var coords = GPSDataProvider.GetLastCoords();
                        double lat = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLat(coords);
                        double lng = CottonDBMS.DataModels.Helpers.GPSHelper.SafeLong(coords);

                        PickupListEntity list = new PickupListEntity();
                        list.Id = Guid.NewGuid().ToString();
                        list.Name = _listName.Trim();
                        list.Source = InputSource.TRUCK;
                        list.AssignedModules = new List<ModuleEntity>();
                        list.AssignedTrucks = new List<TruckEntity>();
                        list.DownloadedToTrucks = new List<TruckEntity>();
                        list.FieldId = field.Id;
                        list.PickupListStatus = PickupListStatus.OPEN;
                        list.ModulesPerLoad = (syncedSetting != null) ? syncedSetting.ModulesPerLoad : 4;

                        if (uow.SettingsRepository.CoordsOnGinYard(lat, lng) || uow.SettingsRepository.CoordsAtFeeder(lat, lng))
                        {
                            list.Destination = PickupListDestination.GIN_FEEDER;
                        }
                        else
                        {
                            list.Destination = PickupListDestination.GIN_YARD;
                        }

                        if (thisTruck != null)
                        {
                            list.AssignedTrucks.Add(thisTruck);
                            list.DownloadedToTrucks.Add(thisTruck);
                        }

                        uow.PickupListRepository.Add(list);
                        uow.SaveChanges();

                        GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<PickupListAddedMessage>(new PickupListAddedMessage { Id = list.Id });
                    }
                    _windowService.CloseModalWindow(WindowType.AddFieldWindow);                    
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                    System.Windows.MessageBox.Show("An error occured trying to save list. " + exc.Message);                   
                }
            }              
        }

        public RelayCommand CancelCommand { get; private set; }
        private void ExecuteCancelCommand()
        {
            _windowService.CloseModalWindow(WindowType.AddFieldWindow);
        }

        public AddPickUpListViewModel(IWindowService windowService)
        {
            _windowService = windowService;
            SaveCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteSaveCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteCancelCommand);

            NewFarmText = "";
            ListName = "";
            NewProducerText = "";
            NewFieldText = "";

            ErrorMessages = new ObservableCollection<string>();

            Producers = new ObservableCollection<ComboBoxItemViewModel>();
            Producers.Add(new ComboBoxItemViewModel { ID = "-1", DisplayText = "-- Select One --" });
            Producers.Add(new ComboBoxItemViewModel { ID = "", DisplayText = "-- Add New --" });

            Farms = new ObservableCollection<FarmComboBoxItemViewModel>();
            Farms.Add(new FarmComboBoxItemViewModel { ID = "-1", ClientID="", DisplayText = "-- Select One --" });
            Farms.Add(new FarmComboBoxItemViewModel { ID = "",  ClientID="", DisplayText = "-- Add New --" });

            Fields = new ObservableCollection<FieldComboBoxItemViewModel>();
            Fields.Add(new FieldComboBoxItemViewModel { ID = "-1", ClientID = "", DisplayText = "-- Select One --" });
            Fields.Add(new FieldComboBoxItemViewModel { ID = "", ClientID = "", DisplayText = "-- Add New --" });

            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                foreach(var p in uow.ClientRepository.GetAll().OrderBy(x => x.Name))
                {
                    if (p.Id != GUIDS.UNASSIGNED_CLIENT_ID)
                        Producers.Add(new ComboBoxItemViewModel { DisplayText = p.Name, ID = p.Id });
                }
            }

            SelectedProducer = Producers[0];            
        }
    }
}
