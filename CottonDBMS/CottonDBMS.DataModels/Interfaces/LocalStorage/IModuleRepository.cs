using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;

namespace CottonDBMS.Interfaces
{
    public interface IModuleRepository : IEntityRepository<ModuleEntity>
    {
        PagedResult<ModuleEntity> GetModules(ModuleFilter filter, int pageSize, int pageNo);
        bool CanSaveModule(string id, string serialNumber);
        string GetNextManualLoadNumber(string prefix, int pad);
        int GetModuleCountByStatus(ModuleStatus status);
        int GetLoadCountByStatus(ModuleStatus status, int modulesPerLoad);
        int GetTotalModuleCount();
        List<string> GetAllSerialNumbers();
        IEnumerable<ModuleEntity> GetAllMatchingModules(ModuleFilter filter);
        IEnumerable<ModuleHistoryEntity> GetAllMatchingLocations(ModuleFilter filter);
        IEnumerable<ModulePoint> GetModulePoints(ModuleFilter filter);
        IEnumerable<ModulePoint> GetModulePointHistory(ModuleFilter filter);
        void ClearGinModuleData();
        bool ModuleIDExists(string id, string moduleID);
    }
}
