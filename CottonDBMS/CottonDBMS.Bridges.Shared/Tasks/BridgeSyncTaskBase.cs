using CottonDBMS.Bridges.Shared.Helpers;
using CottonDBMS.Cloud;
using CottonDBMS.DataModels;
using CottonDBMS.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CottonDBMS.Bridges.Shared.Tasks
{
    public abstract class BridgeSyncTaskBase
    {
        protected CancellationToken Token;

        public BridgeSyncTaskBase(CancellationToken ctoken)
        {
            Token = ctoken;
        }

        public abstract Task ExecuteSteps(UnitOfWork uow, DateTime lastSyncTime);

        public async Task DoSync()
        {
            var newSyncTime = DateTime.UtcNow;

            try
            {
                if (!SyncHelper.HasConnection()) return;

                using (var uow = new UnitOfWork())
                {
                    var documentDbSetting = uow.SettingsRepository.FindSingle(x => x.Key == BridgeSettingKeys.DOCUMENT_DB_KEY);
                    var endpointSetting = uow.SettingsRepository.FindSingle(x => x.Key == BridgeSettingKeys.DOCUMENTDB_ENDPOINT);
                    DocumentDBContext.Initialize(endpointSetting.Value, documentDbSetting.Value);
                    if (DocumentDBContext.Initialized)
                    {
                        try
                        {
                            var lastSyncTime = DateTime.Parse(uow.SettingsRepository.GetSettingWithDefault(BridgeSettingKeys.LAST_SYNC_TIME, "01/01/2019"));
                            lastSyncTime = lastSyncTime.AddMinutes(-1);

                            await ExecuteSteps(uow, lastSyncTime);

                            uow.SettingsRepository.UpsertSetting(BridgeSettingKeys.LAST_SYNC_TIME, newSyncTime.ToString());
                            uow.SaveChanges();
                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }
                }
            }
            catch (Exception outerExc)
            {
                Logging.Logger.Log(outerExc);
            }
        }
    }
}
