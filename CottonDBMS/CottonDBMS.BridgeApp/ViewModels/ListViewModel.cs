using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Bridges.Shared.Messages;
using CottonDBMS.BridgeApp.Navigation;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.RFID;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using CottonDBMS.Bridges.Shared;
using CottonDBMS.Bridges.Shared.ViewModels;
using CottonDBMS.Bridges.Shared.Navigation;



namespace CottonDBMS.BridgeApp.ViewModels
{

    public class GinLoadScanViewModel : ViewModelBase
    {

        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {                
                Set<string>(() => Id, ref _id, value);
            }
        }

        private string _GinTicketLoadNumber;
        public string GinTicketLoadNumber
        {
            get
            {
                return _GinTicketLoadNumber;
            }
            set
            {
                
                string temp = value.Trim().TrimStart('0');
                Set<string>(() => GinTicketLoadNumber, ref _GinTicketLoadNumber, temp);
            }
        }

        private string _Client;
        public string Client
        {
            get
            {
                return _Client;
            }
            set
            {
                Set<string>(() => Client, ref _Client, value);
            }
        }

        private string _Farm;
        public string Farm
        {
            get
            {
                return _Farm;
            }
            set
            {
                Set<string>(() => Farm, ref _Farm, value);
            }
        }

        private string _Field;
        public string Field
        {
            get
            {
                return _Field;
            }
            set
            {
                Set<string>(() => Field, ref _Field, value);
            }
        }

        private string _GrossWeight;
        public string GrossWeight
        {
            get
            {
                return _GrossWeight;
            }
            set
            {
                Set<string>(() => GrossWeight, ref _GrossWeight, value);
            }
        }

        private DateTime _created;
        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {                
                Set<DateTime>(() => Created, ref _created, value);
            }
        }

        private DateTime? _updated;
        public DateTime? Updated
        {
            get
            {
                return _updated;
            }
            set
            {
                Set<DateTime?>(() => Updated, ref _updated, value);
            }
        }

        private int _bridgeLoadNumber;
        public int BridgeLoadNumber
        {
            get
            {
                return _bridgeLoadNumber;
            }
            set
            {
                Set<int>(() => BridgeLoadNumber, ref _bridgeLoadNumber, value);
            }
        }

        private DateTime _lastCreatedOrUpdated;
        public DateTime LastCreatedOrUpdated
        {
            get
            {
                return _lastCreatedOrUpdated;
            }
            set
            {
                Set<DateTime>(() => LastCreatedOrUpdated, ref _lastCreatedOrUpdated, value);
            }
        }

        private string _TruckID;
        public string TruckID
        {
            get
            {
                return _TruckID;
            }
            set
            {
                Set<string>(() => TruckID, ref _TruckID, value);
            }
        }

        private string _serialNumbers;
        public string SerialNumbers
        {
            get
            {
                return _serialNumbers;
            }
            set
            {
                Set<string>(() => SerialNumbers, ref _serialNumbers, value);
            }
        }

        private string _location;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                Set<string>(() => Location, ref _location, value);
            }
        }

        private string _picker;
        public string Picker
        {
            get
            {
                return _picker;
            }
            set
            {
                Set<string>(() => Picker, ref _picker, value);
            }
        }

        private string _variety;
        public string Variety
        {
            get
            {
                return _variety;
            }
            set
            {
                Set<string>(() => Variety, ref _variety, value);
            }
        }

        private string _trailer;
        public string Trailer
        {
            get
            {
                return _trailer;
            }
            set
            {
                Set<string>(() => Trailer, ref _trailer, value);
            }
        }
    }

    public class ListViewModel : BasePageViewModel
    {

        private ObservableCollection<GinLoadScanViewModel> _GinLoads;
        public ObservableCollection<GinLoadScanViewModel> GinLoads
        {
            get
            {
                return _GinLoads;
            }
            set
            {
                
                Set<ObservableCollection<GinLoadScanViewModel>>(() => GinLoads, ref _GinLoads, value);
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                Set<DateTime>(() => StartDate, ref _startDate, value);
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                Set<DateTime>(() =>  EndDate, ref _endDate, value);
            }
        }

        private bool _showOnlyAuto;
        public bool ShowOnlyAuto
        {
            get
            {
                return _showOnlyAuto;
            }
            set
            {
                Set<bool>(() => ShowOnlyAuto, ref _showOnlyAuto, value);
            }
        }

        private GinLoadScanViewModel _selectedItem;
        public GinLoadScanViewModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                Set<GinLoadScanViewModel>(() => SelectedItem, ref _selectedItem, value);
            }
        }

        public ListViewModel(INavigationService navService) : base(navService)
        {
            CloseCommand = new RelayCommand(this.ExecuteCloseCommand);
            OpenCommand = new RelayCommand(this.ExecuteOpenCommand);            
            GinLoads = new ObservableCollection<GinLoadScanViewModel>();
            Messenger.Default.Register<LoadSavedMessage>(this, handleLoadSavedMessage);
        }

        private string normalizeName(string name)
        {
            return !string.IsNullOrEmpty(name) ? name : "UNKNOWN";
        }
        

        public void Initialize()
        {
            try
            {
                ShowOnlyAuto = true;
                StartDate = DateTime.Now.AddDays(-7);
                EndDate = DateTime.Now;

                GinLoads.Clear();

                using (var dp = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
                {
                    foreach (var g in dp.LoadScanRepository.GetAll().OrderByDescending(i => i.LastCreatedOrUpdated))
                    {
                        GinLoadScanViewModel vm = new GinLoadScanViewModel();
                        vm.Client = normalizeName(g.Client);
                        vm.Id = g.Id;
                        vm.BridgeLoadNumber = g.BridgeLoadNumber;
                        vm.Farm = normalizeName(g.Farm);
                        vm.Field = normalizeName(g.Field);
                        vm.Created = g.Created.ToLocalTime();
                        vm.Updated = (g.Updated.HasValue) ? (DateTime?)g.Updated.Value.ToLocalTime() : null;
                        vm.LastCreatedOrUpdated = g.LastCreatedOrUpdated.ToLocalTime();
                        vm.GinTicketLoadNumber = (string.IsNullOrEmpty(g.GinTagLoadNumber)) ? "" : g.GinTagLoadNumber;
                        vm.GrossWeight = g.GrossWeight.ToString("N2");
                        vm.TruckID = g.TruckID;
                        vm.SerialNumbers = "";
                        vm.Location = g.YardRow;
                        vm.Trailer = g.TrailerNumber;
                        vm.Variety = g.Variety;
                        vm.Picker = g.PickedBy;
                        foreach (var sn in g.ScanData.Scans)
                        {
                            vm.SerialNumbers += sn.SerialNumber + ", ";
                        }
                        vm.SerialNumbers = vm.SerialNumbers.TrimEnd(", ".ToCharArray());

                        GinLoads.Add(vm);
                    }
                }
                Logging.Logger.WriteBuffer();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        public override void Cleanup()
        {
            Messenger.Default.Unregister<LoadSavedMessage>(this);         
           
            base.Cleanup();
        }

        public RelayCommand CloseCommand { get; private set; }        


        private void ExecuteCloseCommand()
        {
            var vm = new IdlePageViewModel(NavService);
            NavService.ShowPage(PageType.IDLE_PAGE, false, vm);
            vm.Initialize();
        }


        public RelayCommand OpenCommand { get; private set; }
        private void ExecuteOpenCommand()
        {
            var vm = new LoadViewModel(NavService);
            NavService.ShowPage(PageType.LOAD_PAGE, true, (BasePageViewModel)vm);
            vm.Initialize(SelectedItem.GinTicketLoadNumber, false, true, true, SelectedItem.Id);
            
        }

        private void handleLoadSavedMessage(LoadSavedMessage msg)
        {
            try
            {
                var vm = GinLoads.SingleOrDefault(i => i.Id == msg.Scan.Id);
                var g = msg.Scan;
                if (vm != null)
                {
                    vm.Client = normalizeName(g.Client);
                    vm.Id = g.Id;
                    vm.BridgeLoadNumber = g.BridgeLoadNumber;
                    vm.Farm = normalizeName(g.Farm);
                    vm.Field = normalizeName(g.Field);
                    vm.Created = g.Created.ToLocalTime();
                    vm.Updated = (g.Updated.HasValue) ? (DateTime?)g.Updated.Value.ToLocalTime() : null;
                    vm.LastCreatedOrUpdated = g.LastCreatedOrUpdated.ToLocalTime();
                    vm.GinTicketLoadNumber = (string.IsNullOrEmpty(g.GinTagLoadNumber)) ? "" : g.GinTagLoadNumber;
                    vm.GrossWeight = g.GrossWeight.ToString("N2");
                    vm.TruckID = g.TruckID;
                    vm.SerialNumbers = "";
                    vm.Location = g.YardRow;
                    vm.Trailer = g.TrailerNumber;
                    vm.Variety = g.Variety;
                    vm.Picker = g.PickedBy;
                    foreach (var sn in g.ScanData.Scans)
                    {
                        vm.SerialNumbers += sn.SerialNumber + ", ";
                    }
                    vm.SerialNumbers = vm.SerialNumbers.TrimEnd(", ".ToCharArray());
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                Logging.Logger.WriteBuffer();
            }
        }

        private void handleBarCodeScanned(BarcodeScannedMessage msg)
        {
            
        }

        private void handleInMotionMessage(InMotionMessage msg)
        {
           
        }

        private void handleWeightAcquired(WeightAcquiredMessage msg)
        {
            
        }

        private void handleTagsReported(List<TagItem> tagsReported)
        {
            
        }
    }
}
