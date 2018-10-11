//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using System.Device.Location;

namespace CottonDBMS.EF.Repositories
{
    public class SettingsRepository : GenericRepository<Setting>, ISettingsRepository
    {
        public static double yardNorthwestCornerLatitude = 0.00;
        public static double yardNorthWestCornerLongitude = 0.0;
        public static double yardSouthEastCornerLatitude = 0.00;
        public static double yardSouthEastCornerLongitude = 0.00;
        public static bool hasYardCoords = false;

        public static double feederNorthLatitude = 0.00;
        public static double feederWestLongitude = 0.00;
        public static double feederDetectionRadiusYards = 0.00;
        public static bool hasFeederCoords = false;

        public SettingsRepository(AppDBContext context) : base(context)
        {

        }

        public void UpsertSetting(string key, string value)
        {
            hasFeederCoords = false;
            hasYardCoords = false;
            var setting = this.FindSingle(s => s.Key == key);

            if (setting == null)
            {
                setting = new Setting { Key = key, Value = value };
                this.Add(setting);
            }
            else
            {
                setting.Value = value;
                this.Update(setting);
            }
        }

        public string GetSettingWithDefault(string key, string defaultValue)
        {
            var setting = this.FindSingle(s => s.Key == key);

            if (setting == null)
            {
                setting = new Setting { Key = key, Value = defaultValue };
                this.Add(setting);
                _context.SaveChanges();
                return defaultValue;
            }
            else
            {
                return setting.Value;
            }
        }

        public double GetSettingDoubleValue(string key)
        {
            var setting = this.FindSingle(s => s.Key == key);
            double temp = 0.000;
            if (setting == null)
            {
                return 0.00;
            }
            else if (double.TryParse(setting.Value, out temp))
            {
                return temp;
            }
            else return 0.00;
        }

        public TruckEntity GetCurrentTruck()
        {
            var truckID = GetSettingWithDefault(TruckClientSettingKeys.TRUCK_ID, "");

            if (truckID != "")
            {
                return _context.Truck.SingleOrDefault(t => t.Id == truckID);
            }
            else
            {
                return null;
            }
        }

        public DriverEntity GetCurrentDriver()
        {
            var driverID = GetSettingWithDefault(TruckClientSettingKeys.DRIVER_ID, "");

            if (driverID != "")
            {
                return _context.Driver.SingleOrDefault(t => t.Id == driverID);
            }
            else
            {
                return null;
            }
        }

        public bool EventOnGinYard(AggregateEvent e)
        {
            return CoordsOnGinYard(e.AverageLat, e.AverageLong);
        }

        public bool CoordsOnGinYard(double lat, double lng)
        {
            SettingsRepository settingsRepo = new SettingsRepository(_context);
            SyncedSettingRepository syncedSettingsRepo = new SyncedSettingRepository(_context);
            if (!hasYardCoords)
            {
                var syncedSettings = syncedSettingsRepo.GetAll().FirstOrDefault();
                if (syncedSettings != null)
                {
                    yardNorthwestCornerLatitude = syncedSettings.GinYardNWLat;
                    yardNorthWestCornerLongitude = syncedSettings.GinYardNWLong;
                    yardSouthEastCornerLatitude = syncedSettings.GinYardSELat;
                    yardSouthEastCornerLongitude = syncedSettings.GinYardSELong;
                }
                else
                {
                    yardNorthwestCornerLatitude = settingsRepo.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_NW_CORNER_NORTH);
                    yardNorthWestCornerLongitude = settingsRepo.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_NW_CORNER_WEST);
                    yardSouthEastCornerLatitude = settingsRepo.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_SE_CORNER_NORTH);
                    yardSouthEastCornerLongitude = settingsRepo.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_SE_CORNER_WEST);
                }
                hasYardCoords = true;
            }

            if (lat == 0.000 || lng == 0.000) return false;
            else if (lat <= yardNorthwestCornerLatitude && lat >= yardSouthEastCornerLatitude &&
                                   lng >= yardNorthWestCornerLongitude && lng <= yardSouthEastCornerLongitude)
            {
                return true;
            }
            else return false;
        }

        public bool EventAtFeeder(AggregateEvent e)
        {
            return CoordsAtFeeder(e.AverageLat, e.AverageLong);
        }

        public bool CoordsAtFeeder(double lat, double lng)
        {
            if (!hasFeederCoords)
            {
                SettingsRepository settingsRepo = new SettingsRepository(_context);
                SyncedSettingRepository syncedSettingsRepo = new SyncedSettingRepository(_context);
                var syncedSettings = syncedSettingsRepo.GetAll().FirstOrDefault();
                if (syncedSettings != null)
                {
                    feederNorthLatitude = syncedSettings.FeederLatitude;
                    feederWestLongitude = syncedSettings.FeederLongitude;
                    feederDetectionRadiusYards = syncedSettings.FeederDetectionRadius;                    
                }
                else
                {
                    feederNorthLatitude = settingsRepo.GetSettingDoubleValue(GinAppSettingKeys.GIN_FEEDER_NORTH);
                    feederWestLongitude = settingsRepo.GetSettingDoubleValue(GinAppSettingKeys.GIN_FEEDER_WEST);
                    feederDetectionRadiusYards = settingsRepo.GetSettingDoubleValue(GinAppSettingKeys.GIN_FEEDER_DETECTION_RADIUS);
                }
                hasFeederCoords = true;
            }

            var feederPoint = new GeoCoordinate(feederNorthLatitude, feederWestLongitude);
            var dropPoint = new GeoCoordinate(lat, lng);
            var distanceBetween = feederPoint.GetDistanceTo(dropPoint);
            var distanceInYards = distanceBetween * 0.9144;     // convert to yards

            if (lat == 0.000 || lng == 0.000) return false;
            else if (distanceInYards <= feederDetectionRadiusYards)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

