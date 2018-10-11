//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface ISettingsRepository : IGenericRepository<Setting>
    {
        void UpsertSetting(string key, string value);
        string GetSettingWithDefault(string key, string defaultValue);
        double GetSettingDoubleValue(string key);
        TruckEntity GetCurrentTruck();
        DriverEntity GetCurrentDriver();
        bool EventOnGinYard(AggregateEvent e);
        bool EventAtFeeder(AggregateEvent e);
        bool CoordsAtFeeder(double lat, double lng);
        bool CoordsOnGinYard(double lat, double lng);
    }
}
