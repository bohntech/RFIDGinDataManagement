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
    public class LoadingWindowViewModel : ViewModelBase
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

        private string _serialNumber;
        public string SerialNumber
        {
            get
            {
                return _serialNumber;
            }
            set
            {
                Set<string>(() => SerialNumber, ref _serialNumber, value);
            }
        }

        public string ActiveListID { get; set; }

        public ObservableCollection<ModuleViewModel> ModulesOnTruck { get; set; }

        public LoadingWindowViewModel() : base()
        {

        }

        public void Initialize(string listId)
        {            
            ModulesOnTruck = new ObservableCollection<ModuleViewModel>();

            //determine modules on truck
            foreach(var sn in AggregateDataProvider.SerialNumbersOnTruck) {
                ModulesOnTruck.Add(new ViewModels.ModuleViewModel { SerialNumber = sn });
            }

            ActiveListID = listId;

            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var list = uow.PickupListRepository.GetById(listId, "Field.Farm.Client");
                Client = list.Field.Farm.Client.Name; ;
                Farm = list.Field.Farm.Name;
                Field = list.Field.Name;
                ListName = list.Name;
            }
        }

        public void NewModuleDetected(string serialNumber)
        {
            var lat = GPSDataProvider.GetAverageLatitude(DateTime.Now.ToUniversalTime().AddSeconds(-3), DateTime.Now.ToUniversalTime());
            var lng = GPSDataProvider.GetAverageLongitude(DateTime.Now.ToUniversalTime().AddSeconds(-3), DateTime.Now.ToUniversalTime());
            foreach (var sn in AggregateDataProvider.SerialNumbersOnTruckIncludingBuffered)
            {
                if (sn != serialNumber && !ModulesOnTruck.Any(t => t.SerialNumber == sn))
                {
                    ModulesOnTruck.Add(new ModuleViewModel { SerialNumber = sn, Latitude = lat, Longitude = lng, Loaded = true });
                }
            }
            SerialNumber = serialNumber;
        }
    }
}
