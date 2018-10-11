//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IFarmRepository : IEntityRepository<FarmEntity>
    {
        bool CanSaveFarm(string clientId, string farmId, string Name, bool validateAllIds);
        IEnumerable<string> GetFarmIdsLinkedToFields();
        FarmEntity EnsureFarmCreated(ClientEntity existingClient, string farmName, InputSource source);
        IEnumerable<FarmEntity> GetAllMatchingFarms(string clientName, string farmName);
    }
}
