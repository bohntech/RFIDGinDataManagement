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
    public class UnloadingModuleViewModel : ViewModelBase
    {
        IWindowService _windowService = null;
        System.Timers.Timer t = null;
        private DateTime lastIntervalStart;

        private PickupListEntity activeList = null;

        private List<string> modulesScanned = new List<string>();

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

        public UnloadingModuleViewModel(IWindowService windowService) : base()
        {
            _windowService = windowService;

            OverrideCommand = new GalaSoft.MvvmLight.CommandWpf.RelayCommand(this.ExecuteOverrideCommand);            
        }

        private void ProcessQuadratureMessage(QuadratureStateChangeMessage e)
        {
            Task.Run(() =>
            {
                //if stopped after unload
                var prevEvent = QuadratureEncoderDataProvider.PreviousEvent;
                if (e.DirectionOfRotation == Enums.DirectionOfRotation.Stopped && prevEvent != null && prevEvent.DirectionOfRotation == Enums.DirectionOfRotation.RotatingCounterClockwise)
                {
                    t.Stop();  //stop refresh timer

                    HandleRefreshTimer(); //run refresh to see if bad serials are still on truck

                    if (BadSerials.Count() > 0)
                    {
                        //show correction window
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            UnloadCorrectionViewModel vm = SimpleIoc.Default.GetInstance<UnloadCorrectionViewModel>();
                            vm.ActiveListID = this.ActiveListID;
                            vm.Initialize();
                            _windowService.ShowModalWindow(WindowType.UnloadCorrectionWindow, vm);
                        }));
                    }
                }               
                else if (e.DirectionOfRotation == Enums.DirectionOfRotation.RotatingClockwise) //loading
                {
                    if (!t.Enabled)
                        t.Start();

                    if (_windowService.IsWindowOpen(WindowType.UnloadCorrectionWindow))
                    {
                        _windowService.CloseModalWindow(WindowType.UnloadCorrectionWindow);
                    }                   
                }
                else //we started unloading again so monitor for unloads
                {
                    if (_windowService.IsWindowOpen(WindowType.UnloadCorrectionWindow))
                    {
                        _windowService.CloseModalWindow(WindowType.UnloadCorrectionWindow);
                    }
                    _windowService.FocusLast(WindowType.WaitingForUnloadWindow);
                    t.Start();
                }
            });
        }


        private void ProcessManualCorrection(ManualUnloadCorrectionMessage action)
        {
            if (!t.Enabled)  //restart timer
            {
                t.Start();
            }
        }

        public RelayCommand OverrideCommand { get; private set; }
        private void ExecuteOverrideCommand()
        {
            t.Stop();  //stop refresh timer

            HandleRefreshTimer(); //run refresh to see if bad serials are still on truck

            if (BadSerials.Count() > 0)
            {
                //show correction window
                UnloadCorrectionViewModel vm = SimpleIoc.Default.GetInstance<UnloadCorrectionViewModel>();
                vm.ActiveListID = this.ActiveListID;
                vm.Initialize();
                _windowService.ShowModalWindow(WindowType.UnloadCorrectionWindow, vm);

            }
        }

        public void Initialize()
        {
            //initialize using ActiveListID to lookup info
            ModulesOnTruck = new ObservableCollection<ModuleViewModel>();           
            BadSerials = new ObservableCollection<ModuleViewModel>();
            modulesScanned = new List<string>();
            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                activeList = uow.PickupListRepository.GetById(ActiveListID, new string[] { "AssignedModules" });
            }

            var snsOnTruck = AggregateDataProvider.SerialNumbersOnTruck;
            foreach (var sn in snsOnTruck)
            {
                if (!ModulesOnTruck.Any(m => m.SerialNumber == sn))
                {
                    ModulesOnTruck.Add(new ViewModels.ModuleViewModel { SerialNumber = sn });
                }
            }

            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<QuadratureStateChangeMessage>(this, action => ProcessQuadratureMessage(action));
            Messenger.Default.Register<ManualUnloadCorrectionMessage>(this, (action) => ProcessManualCorrection(action));

            t = new System.Timers.Timer(250);
            t.Elapsed += (sender, e) => this.HandleRefreshTimer();
            lastIntervalStart = DateTime.UtcNow;
            t.Start();
        }

        public void HandleRefreshTimer()
        {
            if (!_windowService.IsWindowOpen(WindowType.WaitingForUnloadWindow)) return;

            _windowService.FocusLast(WindowType.WaitingForUnloadWindow);

            lock (activeList)
            {
                var snsOnTruck = AggregateDataProvider.SerialNumbersOnTruck;
                var snsInBuffer = AggregateDataProvider.SerialNumbersInBuffer;
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    var modulesToRemove = ModulesOnTruck.Where(m => !snsOnTruck.Any(t => t == m.SerialNumber)).ToList();
                    foreach (var m in modulesToRemove) ModulesOnTruck.Remove(m);

                    modulesToRemove = BadSerials.Where(m => !snsOnTruck.Any(t => t == m.SerialNumber)).ToList();
                    foreach (var m in modulesToRemove) BadSerials.Remove(m);

                    //add all modules on truck
                    foreach (var sn in snsOnTruck)
                    {
                        if (!ModulesOnTruck.Any(m => m.SerialNumber == sn))
                        {
                            ModulesOnTruck.Add(new ViewModels.ModuleViewModel { SerialNumber = sn });
                        }
                    }

                    //determine modules that shouldn't be on truck          
                    foreach (var sn in snsOnTruck)
                    {
                        if (!activeList.AssignedModules.Any(m => m.Name == sn) && !BadSerials.Any(m => m.SerialNumber == sn))
                        {                          
                             BadSerials.Add(new ModuleViewModel { SerialNumber = sn, BackgroundColor = "Red", ForegroundColor = "White" });                            
                        }
                    }
                                       
                    //set background color of modules seen
                    //TagDataProvider.GetTagsFirstSeenInTimeRange
                    foreach(var badSerial in BadSerials)
                    {
                        if (snsInBuffer.Any(m => m == badSerial.SerialNumber))
                        {
                            badSerial.BackgroundColor = "PaleVioletRed";
                            badSerial.ForegroundColor = "White";
                        }
                        else
                        {
                            badSerial.BackgroundColor = "Red";
                            badSerial.ForegroundColor = "White";
                        }
                    }
                    
                    if (BadSerials.Count() == 0) //no incorrect serials were found
                    {
                        t.Stop();
                        _windowService.CloseModalWindow(WindowType.WaitingForUnloadWindow);
                    }
                }));
            }
            lastIntervalStart = DateTime.UtcNow;
        }          

        public override void Cleanup()
        {            
            Messenger.Default.Unregister<QuadratureStateChangeMessage>(this);
            Messenger.Default.Unregister<ManualUnloadCorrectionMessage>(this);
            base.Cleanup();
        }
    }
}
