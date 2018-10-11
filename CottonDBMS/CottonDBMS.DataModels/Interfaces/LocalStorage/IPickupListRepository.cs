//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IPickupListRepository : IEntityRepository<PickupListEntity>
    {
        IEnumerable<string> GetModuleIdsOnPickupList();
        PagedResult<PickupListEntity> GetLists(PickupListFilter filter, int pageSize, int pageNo, int modulesPerLoad);
        IEnumerable<string> GetDownloadedPickupListIds();
        bool CanSavePickupList(string id, string name);
        IEnumerable<ModuleEntity> GetAvailableModulesForPickupList(string listId, string fieldId, PickupListDestination destination);
        void Update(PickupListEntity entity, List<string> AssignedModuleIds, List<string> AssignedTruckIds);
        string GetListIDWithSerialNumber(string serial, double lat, double lng, PickupListDestination dest);
        void MoveModulesToList(PickupListEntity list, List<string> serialNumbersToMove, double lat, double lng);
        FieldEntity FindFieldWithSerialNumber(string serial);
        void ReleaseListsFromTruck(IEnumerable<TruckPickupListRelease> releaseDocuments);

        void BulkDeleteListAndModules(IEnumerable<PickupListEntity> entities);
    }
}
