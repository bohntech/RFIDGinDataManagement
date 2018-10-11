//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.Data.EF;

namespace CottonDBMS.EF.Repositories
{
    public class SyncedSettingRepository : EntityRepository<SyncedSettings>, IEntityRepository<SyncedSettings>
    {
        public SyncedSettingRepository(AppDBContext context) : base(context)
        {

        }
    }
}
