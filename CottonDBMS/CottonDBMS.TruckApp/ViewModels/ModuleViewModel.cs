//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace CottonDBMS.TruckApp.ViewModels
{
    public class ModuleViewModel : ViewModelBase
    {

        private bool _showOnMap;
        public bool ShowOnMap
        {
            get
            {
                return _showOnMap;
            }
            set
            {
                Set<bool>(() => ShowOnMap, ref _showOnMap, value);
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

        private double _latitude;
        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                Set<double>(() => Latitude, ref _latitude, value);
            }
        }

        private double _longitude;
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                Set<double>(() => Longitude, ref _longitude, value);
            }
        }

        private bool _loaded;
        public bool Loaded
        {
            get
            {
                return _loaded;
            }
            set
            {
                Set<bool>(() => Loaded, ref _loaded, value);
            }
        }

        private string _backgroundColor;
        public string BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                Set<string>(() => BackgroundColor, ref _backgroundColor, value);
            }
        }

        private string _foregroundColor;
        public string ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }
            set
            {
                Set<string>(() => ForegroundColor, ref _foregroundColor, value);
            }
        }

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                Set<bool>(() => Selected, ref _selected, value);
            }
        }
    }
}
