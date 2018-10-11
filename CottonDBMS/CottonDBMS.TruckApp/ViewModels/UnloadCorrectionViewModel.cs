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
    public class UnloadCorrectionViewModel : ViewModelBase
    {
        IWindowService _windowService = null;

        private PickupListEntity activeList = null;
        
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

        public UnloadCorrectionViewModel(IWindowService windowService) : base()
        {
            _windowService = windowService;
            ForceUnloadCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteForceUnloadCommand);
            RetryCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteRetryCommand);
            CancelCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteCancelCommand);
        }       

        public void Initialize()
        {
            //initialize using ActiveListID to lookup info
            ModulesOnTruck = new ObservableCollection<ModuleViewModel>();
            BadSerials = new ObservableCollection<ModuleViewModel>();
            
            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                activeList = uow.PickupListRepository.GetById(ActiveListID, new string[] { "AssignedModules", "Field.Farm.Client" });
                Farm = activeList.FarmName;
                Field = activeList.FieldName;
                Client = activeList.ClientName;
            }

            var snsOnTruck = AggregateDataProvider.SerialNumbersOnTruck;

            foreach (var sn in AggregateDataProvider.SerialNumbersOnTruck)
            {
                if (!ModulesOnTruck.Any(m => m.SerialNumber == sn))
                {
                    var moduleViewModel = new ViewModels.ModuleViewModel { SerialNumber = sn, Selected =false };

                    if (!activeList.AssignedModules.Any(l => l.Name == sn))
                    {
                        moduleViewModel.BackgroundColor = "Red";
                        moduleViewModel.ForegroundColor = "White";
                    }
                    else
                    {
                        moduleViewModel.BackgroundColor = "Green";
                        moduleViewModel.ForegroundColor = "White";
                    }
                    ModulesOnTruck.Add(moduleViewModel);
                }
            }
          
        }

        public RelayCommand ForceUnloadCommand { get; private set; }
        private void ExecuteForceUnloadCommand()
        {
            //iterate through modules and execute force unload

            foreach(var moduleVM in ModulesOnTruck.Where(m => m.Selected))
            {
                AggregateDataProvider.ForceUnload(moduleVM.SerialNumber);
            }

            
            _windowService.CloseModalWindow(WindowType.UnloadCorrectionWindow);
            Messenger.Default.Send<ManualUnloadCorrectionMessage>(new ManualUnloadCorrectionMessage());
        }

        public RelayCommand RetryCommand { get; private set; }
        private void ExecuteRetryCommand()
        {        
            _windowService.CloseModalWindow(WindowType.UnloadCorrectionWindow);
            Messenger.Default.Send<ManualUnloadCorrectionMessage>(new ManualUnloadCorrectionMessage());
        }

        public RelayCommand CancelCommand { get; private set; }
        private void ExecuteCancelCommand()
        {
            _windowService.CloseModalWindow(WindowType.UnloadCorrectionWindow);
            _windowService.CloseModalWindow(WindowType.WaitingForUnloadWindow);

            LoadingIncorrectModuleViewModel incorrectModuleVM = SimpleIoc.Default.GetInstance<LoadingIncorrectModuleViewModel>();
            incorrectModuleVM.ActiveListID = this.ActiveListID;
            incorrectModuleVM.ListName = this.ListName;
            incorrectModuleVM.Client = this.Client;
            incorrectModuleVM.Farm = this.Farm;
            incorrectModuleVM.Field = this.Field;

            var module = BadSerials.LastOrDefault();
            string serial = module != null ? module.SerialNumber : "--";

            incorrectModuleVM.Initialize(true);
            foreach (var s in AggregateDataProvider.SerialNumbersOnTruck)
            {
                incorrectModuleVM.NewModuleLoaded(s, false);
            }            
            //incorrectModuleVM.NewModuleLoaded(msg.SerialNumber, moduleOnList);
            _windowService.ShowModalWindow(WindowType.LoadingIncorrectModuleWindow, incorrectModuleVM);
            //Messenger.Default.Send<ManualUnloadCorrectionMessage>(new ManualUnloadCorrectionMessage());
        }

        public override void Cleanup()
        {
            //GalaSoft.MvvmLight.Messaging.Messenger.Default.Unregister<TagUnloadingMessage>(this);
            base.Cleanup();
        }        
    }
}
