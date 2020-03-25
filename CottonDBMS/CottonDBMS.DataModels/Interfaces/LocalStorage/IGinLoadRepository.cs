//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IGinLoadRepository : IEntityRepository<GinLoadEntity>
    {
        PagedResult<GinLoadEntity> GetLoads(GinLoadsFilter filter, int pageSize, int pageNo);
        bool CanSaveLoadNumber(string id, string loadNumber);
        bool CanGinTagLoadNumber(string id, string loadNumber);
        IEnumerable<string> GetUndeletableIds();
        int LastLoadNumber();
        string GetLoadIdForGinTicketNumber(string ginTicketLoadNumber);
    }
}
