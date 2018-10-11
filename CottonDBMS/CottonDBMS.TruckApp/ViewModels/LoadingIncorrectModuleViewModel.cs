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
using CottonDBMS.TruckApp.DataProviders;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class LoadingIncorrectModuleViewModel : ViewModelBase
    {
        private IWindowService _windowService = null;
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

        private bool _loadStopped;
        public bool LoadStopped
        {
            get
            {
                return _loadStopped;
            }
            set
            {
                Set<bool>(() => LoadStopped, ref _loadStopped, value);
            }
        }

        private ObservableCollection<ModuleViewModel> _badSerials = null;
        public ObservableCollection<ModuleViewModel> BadSerials
        {
            get
            {
                return _badSerials;
            }
            set
            {
                Set<ObservableCollection<ModuleViewModel>>(() => BadSerials, ref _badSerials, value);
            }
        }
    

        public string ActiveListID { get; set; }

        private ObservableCollection<ModuleViewModel> _modulesOnTruck = null;
        public ObservableCollection<ModuleViewModel> ModulesOnTruck
        {
            get
            {
                return _modulesOnTruck;
            }
            set
            {
                Set<ObservableCollection<ModuleViewModel>>(() => ModulesOnTruck, ref _modulesOnTruck, value);
            }
        }

        public RelayCommand UnloadCommand { get; private set; }
        private void ExecuteUnloadCommand()
        {
            //_windowService.ClosePickupWindow();
            _windowService.CloseModalWindow(WindowType.LoadingIncorrectModuleWindow);

            if (_windowService.IsWindowOpen(WindowType.UnloadCorrectionWindow))
                _windowService.CloseModalWindow(WindowType.UnloadCorrectionWindow);

            if (_windowService.IsWindowOpen(WindowType.WaitingForUnloadWindow))
                _windowService.CloseModalWindow(WindowType.WaitingForUnloadWindow);

            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                UnloadingModuleViewModel vm = SimpleIoc.Default.GetInstance<UnloadingModuleViewModel>();
                vm.ActiveListID = ActiveListID;
                vm.Client = this.Client;
                vm.Farm = this.Farm;
                vm.Field = this.Field;
                vm.ListName = this.ListName;
                vm.Initialize();                
                _windowService.ShowModalWindow(WindowType.WaitingForUnloadWindow, vm);
            }));

        }

        public RelayCommand ContinueCommand { get; private set; }
        private void ExecuteContinueCommand()
        {            
            ChangeListViewModel vm = SimpleIoc.Default.GetInstance<ChangeListViewModel>();
            vm.ActiveListID = ActiveListID;            
            vm.Initialize();
            _windowService.ShowModalWindow(WindowType.ChangeListWindow, vm);
        }

        public LoadingIncorrectModuleViewModel(IWindowService windowService) : base()
        {
            _windowService = windowService;
            UnloadCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteUnloadCommand);
            ContinueCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteContinueCommand);
        }

        public void Initialize(bool loadStopped)
        {
            //initialize using ActiveListID to lookup info
            ModulesOnTruck = new ObservableCollection<ModuleViewModel>();
           
            if (ActiveListID == GUIDS.UNASSIGNED_LIST_ID)
            {
                Client = "Unassigned";
                Farm = "";
                Field = "";
                ListName = "";
            }
            LoadStopped = loadStopped;
            BadSerials = new ObservableCollection<ModuleViewModel>();
            Messenger.Default.Register<QuadratureStateChangeMessage>(this, (action) => ProcessQuadratureEvent(action));
            
        }

        public void NewModuleLoaded(string serialNumber, bool onList)
        {
            try
            {
                foreach (var sn in AggregateDataProvider.SerialNumbersOnTruckIncludingBuffered)
                {
                    if (sn != serialNumber && !ModulesOnTruck.Any(t => t.SerialNumber == sn))
                    {
                        ModulesOnTruck.Add(new ModuleViewModel { SerialNumber = sn });
                    }
                }

                if (!ModulesOnTruck.Any(t => t.SerialNumber == serialNumber))
                {
                    ModulesOnTruck.Add(new ModuleViewModel { SerialNumber = serialNumber });
                }

                if (!onList && !BadSerials.Any(t => t.SerialNumber == serialNumber))
                {
                    BadSerials.Add(new ModuleViewModel { SerialNumber = serialNumber });
                }
            }
            catch(Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private void ProcessQuadratureEvent(QuadratureStateChangeMessage action)
        {            
            if (action.DirectionOfRotation == Enums.DirectionOfRotation.RotatingCounterClockwise)
            {
                LoadStopped = true;
                ExecuteUnloadCommand();
            }
            else if (action.DirectionOfRotation == Enums.DirectionOfRotation.Stopped)
            {
                LoadStopped = true;
            }
        }

        

        public void DoCleanup()
        {
            Messenger.Default.Unregister<QuadratureStateChangeMessage>(this);
        }

    }
}
