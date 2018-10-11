//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface ITruckRepository : IEntityRepository<TruckEntity>
    {
        bool CanSaveTruckId(string id, string truckid);
        bool CanSaveTruckPrefix(string id, string prefix);
        IEnumerable<string> GetUndeletableIds(IEnumerable<TruckEntity> itemsToDelete);
        string GetNextLoadNumber();
        string GetLastLoadNumber();
        void ClearTruckData();
        void ClearClientFarmFieldData();
        void ClearLoadNumber(string serialNumber);
    }
}
