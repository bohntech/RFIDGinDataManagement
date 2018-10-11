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
    public class UnloadingAtGinViewModel : ViewModelBase
    {
        private IWindowService _windowService = null;
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



        public ObservableCollection<ModuleViewModel> ModulesOnTruck { get; set; }

        public UnloadingAtGinViewModel(IWindowService windowService) : base()
        {
            _windowService = windowService;
            
        }

        private void ProcessQuadratureMessage(QuadratureStateChangeMessage e)
        {
            if (e.DirectionOfRotation == Enums.DirectionOfRotation.Stopped)
            {
                Cleanup();
                _windowService.CloseModalWindow(WindowType.UnloadingAtGin);
            }
        }

        public void Refresh()
        {
            //initialize using ActiveListID to lookup info
            ModulesOnTruck = new ObservableCollection<ModuleViewModel>();

            foreach (var sn in AggregateDataProvider.SerialNumbersOnTruck)
            {
                ModulesOnTruck.Add(new ViewModels.ModuleViewModel { SerialNumber = sn });
            }


            var coords = GPSDataProvider.GetLastCoords();
            double lat = 0.00;
            double lng = 0.00;

            if (coords != null)
            {
                lat = coords.NonNullLatitude;
                lng = coords.NonNullLongitude;
            }

            using (var uow = SimpleIoc.Default.GetInstance<IUnitOfWork>(Guid.NewGuid().ToString()))
            {
                if (uow.SettingsRepository.CoordsAtFeeder(lat, lng))
                {
                    Location = "Gin Feeder";
                }
                else if (uow.SettingsRepository.CoordsOnGinYard(lat, lng))
                {
                    Location = "Gin Yard";
                }
                else
                {
                    Location = "Field";
                }
            }

        }

        public void Initialize()
        {
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<QuadratureStateChangeMessage>(this, action => ProcessQuadratureMessage(action));
            Refresh();
        }

        public override void Cleanup() 
        {
            base.Cleanup();
            Messenger.Default.Unregister<QuadratureStateChangeMessage>(this);            
        }
    }
}
