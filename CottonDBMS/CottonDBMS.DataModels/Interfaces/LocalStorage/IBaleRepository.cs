//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IBaleRepository : IEntityRepository<BaleEntity>
    {
        PagedResult<BaleEntity> GetBales(BalesFilter filter, int pageSize, int pageNo);
      
        int LastScanNumber();
      
    }
}
