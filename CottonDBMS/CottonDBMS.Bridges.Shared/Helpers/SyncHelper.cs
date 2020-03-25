using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CottonDBMS.Cloud;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.EF;
using System.Diagnostics;

namespace CottonDBMS.Bridges.Shared.Helpers
{
    public class SyncHelper
    {
        public static bool HasConnection()
        {
            var temp = false;
            try
            {
                using (var webClient = new System.Net.WebClient())
                {
                    using (webClient.OpenRead("http://google.com"))
                    {
                        temp = true;
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
            return temp;
        }
       

        public static async Task SyncTrucksTable(CancellationToken cToken, UnitOfWork uow)
        {
            var trucks = await DocumentDBContext.GetAllItemsAsync<TruckEntity>(p => p.EntityType == EntityType.TRUCK);
            var localTruckIds = uow.TruckRepository.GetAllIds();

            if (cToken.IsCancellationRequested) return;
            using (var addWork = new UnitOfWork())
            {
                addWork.DisableChangeTracking();
                AddRemoteEntitiesToLocalOfType<TruckEntity>(addWork, addWork.TruckRepository, cToken, trucks, localTruckIds);
            }

            if (cToken.IsCancellationRequested) return;

            using (var updateWork = new UnitOfWork())
            {
                updateWork.DisableChangeTracking();
                SyncHelper.UpdateRemoteEntitiesOnLocalOfType<TruckEntity>(updateWork, updateWork.TruckRepository, cToken, trucks, localTruckIds, true);
            }

            if (cToken.IsCancellationRequested) return;
            SyncHelper.DeleteEntitiesOfTypeNoSourceCheck<TruckEntity>(uow, uow.TruckRepository, cToken, trucks);
        }

        public static async Task SyncLoadScans(UnitOfWork uow, CancellationToken token)
        {

            try
            {
                if (token.IsCancellationRequested) return;

                Logging.Logger.Log("INFO", "Push load scans to cloud.");
                //var cutOffTime = DateTime.UtcNow.AddMinutes(-30);

                var newEntities = uow.LoadScanRepository.FindMatching(t => t.Source == InputSource.TRUCK && t.SyncedToCloud == false && t.Updated.HasValue && !t.GinTagLoadNumber.StartsWith("AUTO"));
                foreach (var entity in newEntities)
                {
                    try
                    {
                        if (token.IsCancellationRequested) return;
                                                
                        await DocumentDBContext.UpsertItemAsync<LoadScanEntity>(entity);
                        uow.LoadScanRepository.Update(entity, true);
                        uow.SaveChanges();
                    }
                    catch (Exception excInner)
                    {
                        Logging.Logger.Log(excInner);
                    }
                }

                if (token.IsCancellationRequested) return;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public static async Task SyncModuleOwnershipTable(UnitOfWork uow, DateTime lastSyncTime, CancellationToken token)
        {
            //Sync module ownership table          
            //get only modules updated or created 
            var remoteModuleOwnerships = await DocumentDBContext.GetAllItemsAsync<ModuleOwnershipEntity>(p => p.EntityType == EntityType.MODULE_OWNERSHIP && (p.Created > lastSyncTime ||
            (p.Updated.HasValue && p.Updated > lastSyncTime)));

            if (token.IsCancellationRequested) return;

            var localModuleOwnershipIds = uow.ModuleOwnershipRepository.GetAllIds();

            if (token.IsCancellationRequested) return;

            using (var addWork = new UnitOfWork())
            {
                addWork.DisableChangeTracking();
                AddRemoteEntitiesToLocalOfType<ModuleOwnershipEntity>(addWork, addWork.ModuleOwnershipRepository, token, remoteModuleOwnerships, localModuleOwnershipIds);
            }

            if (token.IsCancellationRequested) return;

            using (var updateWork = new UnitOfWork())
            {
                updateWork.DisableChangeTracking();
                UpdateRemoteEntitiesOnLocalOfType<ModuleOwnershipEntity>(updateWork, updateWork.ModuleOwnershipRepository, token, remoteModuleOwnerships, localModuleOwnershipIds, false);
            }
        }

        public static async Task SyncFeederScans(UnitOfWork uow, DateTime lastSyncTime, CancellationToken token)
        {
            try
            {
                if (token.IsCancellationRequested) return;

                Logging.Logger.Log("INFO", "Push feeder scans to cloud.");

                var newEntities = uow.FeederScanRepository.FindMatching(t => t.Source == InputSource.TRUCK && t.SyncedToCloud == false);
                foreach (var entity in newEntities)
                {
                    try
                    {
                        if (token.IsCancellationRequested) return;
                        await DocumentDBContext.UpsertItemAsync<FeederScanEntity>(entity);
                        uow.FeederScanRepository.Update(entity, true);
                        uow.SaveChanges();
                    }
                    catch (Exception excInner)
                    {
                        Logging.Logger.Log(excInner);
                    }
                }

                if (token.IsCancellationRequested) return;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public static async Task SyncPBIScans(UnitOfWork uow, DateTime lastSyncTime, CancellationToken token)
        {
            try
            {
                if (token.IsCancellationRequested) return;

                Logging.Logger.Log("INFO", "Push PBI scans to cloud.");

                var newEntities = uow.BaleScanRepository.FindMatching(t => t.Source == InputSource.TRUCK && t.SyncedToCloud == false);
                foreach (var entity in newEntities)
                {
                    try
                    {
                        if (token.IsCancellationRequested) return;
                        await DocumentDBContext.UpsertItemAsync<BaleScanEntity>(entity);
                        uow.BaleScanRepository.Update(entity, true);
                        uow.SaveChanges();
                    }
                    catch (Exception excInner)
                    {
                        Logging.Logger.Log(excInner);
                    }
                }

                if (token.IsCancellationRequested) return;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public static void SetEnvironmentPaths(string appFolder, string appSyncFolder)
        {
            string rootDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            rootDir = rootDir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;

            if (!System.IO.Directory.Exists(rootDir))
            {
                System.IO.Directory.CreateDirectory(rootDir);
            }
            string dataBaseDir = rootDir + "\\" + appFolder;
            if (!System.IO.Directory.Exists(dataBaseDir))
            {
                System.IO.Directory.CreateDirectory(dataBaseDir);
            }

            string appLogDir = rootDir + "\\" + appSyncFolder + "\\";
            if (!System.IO.Directory.Exists(appLogDir))
            {
                System.IO.Directory.CreateDirectory(appLogDir);
            }

            CottonDBMS.Logging.Logger.SetLogPath(appLogDir);
            AppDomain.CurrentDomain.SetData("DataDirectory", dataBaseDir.TrimEnd('\\'));
        }

        /*TODO THESE METHODS COULD BE MOVED TO COMMON ASSEMBLY AND SHARED WITH TRUCK SYNC */
        public static void AddRemoteEntitiesToLocalOfType<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities, IEnumerable<string> localIds) where TObject : BaseEntity, new()
        {
            try
            {
                //add entities in remote source but not local
                Logging.Logger.Log("INFO", "Add entities of type " + typeof(TObject).ToString());
                int count = 1;
                foreach (var e in remoteEntities.Where(t => !localIds.Contains(t.Id)))
                {
                    try
                    {
                        if (cToken.IsCancellationRequested) return;
                        //var localEntity = localEntities.SingleOrDefault(t => t.Id == e.Id);

                        //if (localEntity == null)
                        //{
                        Console.WriteLine("Adding entity: " + e.Name);
                        e.SyncedToCloud = true;
                        repo.Add(e);
                        count++;

                        if (count == 100)
                        {
                            count = 1;
                            uow.SaveChanges();
                        }
                        //}
                    }
                    catch (Exception exc)
                    {
                        Logging.Logger.Log(exc);
                    }
                }
                uow.SaveChanges();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public static void UpdateRemoteEntitiesOnLocalOfType<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities, IEnumerable<string> localIds, bool detachIfAttached) where TObject : BaseEntity, new()
        {
            try
            {
                Logging.Logger.Log("INFO", "Update entities of type " + typeof(TObject).ToString());

                //update entities that have same id
                int i = 1;
                foreach (var remoteEntity in remoteEntities.Where(t => localIds.Contains(t.Id)))
                {
                    try
                    {
                        Console.WriteLine("Updating entity: " + remoteEntity.Name);
                        if (cToken.IsCancellationRequested) return;
                        remoteEntity.SyncedToCloud = true;
                        repo.QuickUpdate(remoteEntity, detachIfAttached);
                        i++;
                        if (i == 200)
                        {
                            i = 1;
                            Console.WriteLine("Saving changes");
                            uow.SaveChanges();
                        }
                    }
                    catch (Exception exc)
                    {
                        Logging.Logger.Log(exc);
                    }
                }
                Console.WriteLine("Saving changes");
                uow.SaveChanges();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        public static void DeleteEntitiesOfTypeNoSourceCheck<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities) where TObject : BaseEntity, new()
        {
            Logging.Logger.Log("INFO", "Remove entities of type " + typeof(TObject).ToString());

            var localEntities = repo.GetAll();
            var entitiesToDelete = new List<TObject>();

            //delete entities that are no longer in gin list and that originated from gin
            foreach (var e in localEntities.Where(l => !remoteEntities.Any(r => r.Id == l.Id)))
            {
                try
                {
                    if (cToken.IsCancellationRequested) return;
                    entitiesToDelete.Add(e);
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
            }

            foreach (var item in entitiesToDelete)
            {
                try
                {
                    if (cToken.IsCancellationRequested) return;
                    repo.Delete(item);
                    uow.SaveChanges();
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
            }
        }
    }
}
