using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CottonDBMS.Bridges.Shared.Tasks
{
    public class FeederBridgeSyncTask : BridgeSyncTaskBase
    {
        public FeederBridgeSyncTask(CancellationToken token) : base(token)
        {

        }

        public override async Task ExecuteSteps(UnitOfWork uow, DateTime lastSyncTime)
        {            
            await SyncHelper.SyncModuleOwnershipTable(uow, lastSyncTime, this.Token);
            await SyncHelper.SyncFeederScans(uow, lastSyncTime, this.Token);
        }
    }
}
