//Licensed under MIT License see LICENSE.TXT in project root folder
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

namespace CottonDBMS.TruckApp.Sync
{
    class Program
    {
        static CancellationTokenSource tokenSource = null;
        static CancellationToken token;        
        public static int SyncIntervalMinutes { get; set; }

        private static void updateSyncInterval()
        {
            using (var uow = new UnitOfWork())
            {
                var synIntervalSetting = uow.SettingsRepository.FindSingle(s => s.Key == TruckClientSettingKeys.DATA_SYNC_INTERVAL);
                if (synIntervalSetting != null)
                {
                    int temp = 5;
                    if (int.TryParse(synIntervalSetting.Value, out temp))
                    {
                        SyncIntervalMinutes = temp;
                    }
                }
            }
        }

        private static bool hasConnection()
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

        public static bool alreadyRunningProcess()       
        {
            Process thisProcess = Process.GetCurrentProcess();
            Process[] matchingProcs = Process.GetProcessesByName(thisProcess.ProcessName);
            foreach (Process p in matchingProcs)
            {
                if ((p.Id != thisProcess.Id) &&
                    (p.MainModule.FileName == thisProcess.MainModule.FileName))
                    return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            if (alreadyRunningProcess()) Environment.Exit(0);

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            dir = dir.TrimEnd('\\') + "\\" + FolderConstants.ROOT_APP_DATA_FOLDER;

            if (!System.IO.Directory.Exists(dir))
            {
                System.IO.Directory.CreateDirectory(dir);
            }

            string truckLogDir = dir + "\\"+FolderConstants.TRUCK_SYNC_APP_DATA_FOLDER+"\\";
            if (!System.IO.Directory.Exists(truckLogDir))
            {
                System.IO.Directory.CreateDirectory(truckLogDir);
            }

            CottonDBMS.Logging.Logger.SetLogPath(truckLogDir);
            AppDomain.CurrentDomain.SetData("DataDirectory", dir.TrimEnd('\\'));

            Logging.Logger.Log("INFO", "SYNC LAUNCHED");            
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;            
            updateSyncInterval();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;

            Task callTask = Task.Run(async () =>
            {
                System.Threading.Thread.CurrentThread.Priority = ThreadPriority.Lowest;                
                await DoSync();
            });

            callTask.Wait();                        
            Environment.Exit(0);            
        }

        public static async Task DoSync()
        {
            try
            {                
                if (!hasConnection()) return;

                using (var uow = new UnitOfWork())
                {
                    var documentDbSetting = uow.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENT_DB_KEY);
                    var endpointSetting = uow.SettingsRepository.FindSingle(x => x.Key == TruckClientSettingKeys.DOCUMENTDB_ENDPOINT);
                    DocumentDBContext.Initialize(endpointSetting.Value, documentDbSetting.Value);

                    //return;                      
                    if (DocumentDBContext.Initialized)
                    {
                        try
                        {
                            var truck = uow.SettingsRepository.GetCurrentTruck();
                            
                            List<PickupListEntity> pickupLists = new List<PickupListEntity>();                            

                            Logging.Logger.Log("INFO", "Fetching pickuplists.");
                            if (truck != null)
                            {
                                //process deleted documents - this mainly cleans out the Docs to process table
                                //except for PickupLists if the truck deleted a pickup list a Release request doc is put in the cloud
                                //so the gin can remove the truck assignment       
                                List<TruckPickupListRelease> releaseDocs = new List<TruckPickupListRelease>();
                                
                                //get all lists that have been released by this truck previously
                                await processDeletedDocuments(uow, token, truck.Id, releaseDocs);
                                var cloudReleaseDocs = await DocumentDBContext.GetAllItemsAsync<TruckPickupListRelease>(x => x.TruckID == truck.Id);
                                releaseDocs.AddRange(cloudReleaseDocs);
                                var tempLists = await DocumentDBContext.GetAllItemsAsync<PickupListEntity>(p => p.EntityType == EntityType.PICKUPLIST && p.Source == InputSource.GIN && p.AssignedTruckIDs.Contains(truck.Id), 100);

                                foreach(var list in tempLists)  //only add lists that have not been released by truck
                                {
                                    if (!releaseDocs.Any(x => x.PickupListID == list.Id))
                                    {
                                        pickupLists.Add(list);
                                    }
                                }
                            }

                            //immediately mark lists that have been downloaded                            
                            TruckListsDownloaded document = new TruckListsDownloaded();
                            document.Id = "TRUCKDOWNLOADS_" + truck.Id;
                            document.Name = truck.Id;
                            document.Source = InputSource.TRUCK;
                            document.SyncedToCloud = true;
                            document.Created = DateTime.UtcNow;
                            document.PickupListsDownloaded = new List<string>();
                            document.PickupListsDownloaded.AddRange(pickupLists.Select(x => x.Id).ToArray());
                            await DocumentDBContext.UpsertItemAsync<TruckListsDownloaded>(document);

                            
                            var driver = uow.SettingsRepository.GetCurrentDriver();

                            if (truck == null || driver == null) return;  //only sync if a truck and driver is configured

                            //pull down clients/farms/fields/trucks and pickuplists 
                            Logging.Logger.Log("INFO", "Downloading  data.");
                            var clients = await DocumentDBContext.GetAllItemsAsync<ClientEntity>(p => p.EntityType == EntityType.CLIENT && p.Source == InputSource.GIN, 500);
                            var farms = await DocumentDBContext.GetAllItemsAsync<FarmEntity>(p => p.EntityType == EntityType.FARM && p.Source == InputSource.GIN, 500);
                            var fields = await DocumentDBContext.GetAllItemsAsync<FieldEntity>(p => p.EntityType == EntityType.FIELD && p.Source == InputSource.GIN, 500);

                            var trucks = await DocumentDBContext.GetAllItemsAsync<TruckEntity>(p => p.EntityType == EntityType.TRUCK);
                            var drivers = await DocumentDBContext.GetAllItemsAsync<DriverEntity>(p => p.EntityType == EntityType.DRIVER);
                            var syncedSettings = await DocumentDBContext.GetAllItemsAsync<SyncedSettings>(p => p.EntityType == EntityType.SETTING_SUMMARY);

                            var clientIdsToIgnore = new List<string>();
                            clientIdsToIgnore.Add(GUIDS.UNASSIGNED_CLIENT_ID);

                            var farmIdsToIgnore = new List<string>();
                            farmIdsToIgnore.Add(GUIDS.UNASSIGNED_FARM_ID);

                            var fieldIdsToIgnore = new List<string>();
                            fieldIdsToIgnore.Add(GUIDS.UNASSIGNED_FIELD_ID);

                            var listIdsToIgnore = new List<string>();
                            listIdsToIgnore.Add(GUIDS.UNASSIGNED_LIST_ID);


                            Logging.Logger.Log("INFO", "Fetching local  data.");
                            var localClients = uow.ClientRepository.GetAll().Where(c => c.Id != GUIDS.UNASSIGNED_CLIENT_ID);
                            var localFarms = uow.FarmRepository.GetAll().Where(c => c.Id != GUIDS.UNASSIGNED_FARM_ID);
                            var localFields = uow.FieldRepository.GetAll().Where(c => c.Id != GUIDS.UNASSIGNED_FIELD_ID);
                            var localSyncedSettings = uow.SyncedSettingsRepo.GetAll();
                            var localPickupLists = uow.PickupListRepository.GetAll(new string[] { "DownloadedToTrucks", "AssignedTrucks", "Field.Farm.Client" }).Where(c => c.Id != GUIDS.UNASSIGNED_LIST_ID);
                            var localModules = uow.ModuleRepository.GetAll(new string[] {"Field.Farm.Client", "PickupList" });
                            var localDrivers = uow.DriverRepository.GetAll();
                            var localTrucks = uow.TruckRepository.GetAll();
                            
                            var pickUpListIds = pickupLists.Select(t => t.Id).Distinct().ToArray();

                            Logging.Logger.Log("INFO", "Fetching modules");

                            //TODO PICKUPLIST ID
                            var modules = await DocumentDBContext.GetAllItemsAsync<ModuleEntity>(p => p.PickupListId != null && pickUpListIds.Contains(p.PickupListId) && p.EntityType == EntityType.MODULE, 50000);

                            Logging.Logger.Log("INFO", "Computing records to delete.");
                            var fieldsToRemove = localFields.Where(f => !fields.Any(x => x.Id == f.Id)).ToList();
                            var farmsToRemove = localFarms.Where(f => !farms.Any(x => x.Id == f.Id)).ToList();
                            var clientsToRemove = localClients.Where(f => !clients.Any(x => x.Id == f.Id)).ToList();
                            var trucksToRemove = localTrucks.Where(t => !trucks.Any(x => x.Id == t.Id)).ToList();
                            var driversToRemove = localDrivers.Where(d => !drivers.Any(x => x.Id == d.Id)).ToList();
                            var listsToRemove = localPickupLists.Where(p => !pickupLists.Any(x => x.Id == p.Id)).ToList();

                            if (token.IsCancellationRequested) return;

                            //if there are items that were deleted at gin but got linked to truck entered data before
                            //truck data was synced then mark records to be sent to gin again to preserve
                            //data integrity
                            if (truck != null && trucksToRemove.Any(t => t.Id == truck.Id))
                            {
                                truck.Source = InputSource.TRUCK;
                                truck.Updated = DateTime.UtcNow;
                                uow.TruckRepository.Update(truck);
                                uow.SaveChanges();
                            }

                            if (token.IsCancellationRequested) return;

                            if (driver != null && driversToRemove.Any(d => d.Id == driver.Id))
                            {
                                driver.Source = InputSource.TRUCK;
                                driver.Updated = DateTime.UtcNow;
                                uow.DriverRepository.Update(driver);
                                uow.SaveChanges();
                            }

                            ///////////// THIS SECTION COULD PROBABLY BE REMOVED SINCE WE ARE PREVENTING DELETE OF LISTS ONCE DOWNLOADED //////////////////////
                            foreach (var list in listsToRemove) //if list was removed at gin but had modules added, force it to be re-added
                            {
                                bool canDelete = true;
                                var cloudDocument = await DocumentDBContext.GetItemAsync<PickupListEntity>(list.Id);

                                //make sure modules get put back in cloud for reassignment
                                //TODO PICKUPLISTID
                                foreach (var m in localModules.Where(m => m.PickupListId == list.Id))
                                {
                                    var eventsWithSerial = uow.AggregateEventRepository.FindMatching(evt => evt.SerialNumber == m.Name).ToList();
                                    if (eventsWithSerial.Count() > 0)
                                    {
                                        canDelete = false;
                                        m.Source = InputSource.TRUCK;
                                        m.Updated = DateTime.UtcNow;
                                        uow.ModuleRepository.Update(m);
                                    }
                                }

                                //TODO PICKUPLISTID
                                if (localModules.Any(m => m.PickupListId == list.Id) && !canDelete)
                                {

                                    list.Source = InputSource.TRUCK;
                                    list.Updated = DateTime.UtcNow;
                                    uow.PickupListRepository.Update(list);

                                    if (cloudDocument != null) //list is in cloud but this truck was removed - add to ignored list to prevent overwriting
                                    {
                                        listIdsToIgnore.Add(list.Id);
                                    }
                                }                          
                            }
                            uow.SaveChanges();

                            foreach (var list in localPickupLists.Where(p => p.Source == InputSource.TRUCK))
                            {
                                if (token.IsCancellationRequested) return;

                                var field = localFields.SingleOrDefault(x => x.Id == list.FieldId);
                                if (field != null && fieldsToRemove.Any(x => x.Id == field.Id))
                                {
                                    field.Source = InputSource.TRUCK;
                                    field.Updated = DateTime.UtcNow;
                                    uow.FieldRepository.Update(field);

                                    if (farmsToRemove.Any(x => x.Id == field.FarmId))
                                    {
                                        field.Farm.Source = InputSource.TRUCK;
                                        field.Updated = DateTime.UtcNow;
                                        uow.FarmRepository.Update(field.Farm);
                                    }

                                    if (clientsToRemove.Any(x => x.Id == field.Farm.Client.Id))
                                    {
                                        field.Farm.Client.Source = InputSource.TRUCK;
                                        field.Farm.Client.Updated = DateTime.UtcNow;
                                    }
                                }
                            }
                            uow.SaveChanges();

                            ///////////// THE ABOVE SECTION COULD PROBABLY BE REMOVED SINCE WE ARE PREVENTING DELETE OF LISTS ONCE DOWNLOADED //////////////////////

                            if (token.IsCancellationRequested) return;

                            await pushEntitiesOfType<DriverEntity>(uow, uow.DriverRepository, token, drivers, new List<string>());
                            await pushEntitiesOfType<TruckEntity>(uow, uow.TruckRepository, token, trucks, new List<string>());
                            
                            await pushEntitiesOfType<ClientEntity>(uow, uow.ClientRepository, token, clients, clientIdsToIgnore);
                            await pushEntitiesOfType<FarmEntity>(uow, uow.FarmRepository, token, farms, farmIdsToIgnore);
                            await pushEntitiesOfType<FieldEntity>(uow, uow.FieldRepository, token, fields, fieldIdsToIgnore);
                            await pushEntitiesOfType<PickupListEntity>(uow, uow.PickupListRepository, token, pickupLists, listIdsToIgnore);

                            await pushModules(uow, uow.ModuleRepository, token, modules);

                            if (token.IsCancellationRequested) return;

                            using (var addWork = new UnitOfWork())
                            {
                                addWork.DisableChangeTracking();
                                addEntitiesOfType<DriverEntity>(addWork, addWork.DriverRepository, token, drivers, localDrivers);
                                addEntitiesOfType<TruckEntity>(addWork, addWork.TruckRepository, token, trucks, localTrucks);
                                addEntitiesOfType<ClientEntity>(addWork, addWork.ClientRepository, token, clients, localClients);
                                addEntitiesOfType<FarmEntity>(addWork, addWork.FarmRepository, token, farms, localFarms);
                                addEntitiesOfType<FieldEntity>(addWork, addWork.FieldRepository, token, fields, localFields);
                                addEntitiesOfType<PickupListEntity>(addWork, addWork.PickupListRepository, token, pickupLists, localPickupLists);
                                addEntitiesOfType<SyncedSettings>(addWork, addWork.SyncedSettingsRepo, token, syncedSettings, localSyncedSettings);
                                addEntitiesOfType<ModuleEntity>(addWork, addWork.ModuleRepository, token, modules, localModules);
                            }

                            if (token.IsCancellationRequested) return;

                            using (var updateWork = new UnitOfWork())
                            {
                                updateWork.DisableChangeTracking();
                                updateEntitiesOfType<DriverEntity>(updateWork, updateWork.DriverRepository, token, drivers, localDrivers, false);
                                updateEntitiesOfType<TruckEntity>(updateWork, updateWork.TruckRepository, token, trucks, localTrucks, true);
                                updateEntitiesOfType<ClientEntity>(updateWork, updateWork.ClientRepository, token, clients, localClients, false);
                                updateEntitiesOfType<FarmEntity>(updateWork, updateWork.FarmRepository, token, farms, localFarms, false);
                                updateEntitiesOfType<FieldEntity>(updateWork, updateWork.FieldRepository, token, fields, localFields, false);
                                updateEntitiesOfType<PickupListEntity>(updateWork, updateWork.PickupListRepository, token, pickupLists, localPickupLists, false);
                                updateEntitiesOfTypeIfNewer<ModuleEntity>(updateWork, updateWork.ModuleRepository, token, modules, localModules, false);
                                updateEntitiesOfType<SyncedSettings>(updateWork, updateWork.SyncedSettingsRepo, token, syncedSettings, localSyncedSettings, false);
                            }

                            if (token.IsCancellationRequested) return;

                            deleteEntitiesOfType<DriverEntity>(uow, uow.DriverRepository, token, drivers);
                            deleteEntitiesOfType<TruckEntity>(uow, uow.TruckRepository, token, trucks);
                            deleteEntitiesOfType<ModuleEntity>(uow, uow.ModuleRepository, token, modules);
                            deleteEntitiesOfType<PickupListEntity>(uow, uow.PickupListRepository, token, pickupLists);
                            deleteEntitiesOfType<FieldEntity>(uow, uow.FieldRepository, token, fields);
                            deleteEntitiesOfType<FarmEntity>(uow, uow.FarmRepository, token, farms);
                            deleteEntitiesOfType<ClientEntity>(uow, uow.ClientRepository, token, clients);

                            cleanDupedModules(uow, uow.ModuleRepository, token);
                                                       

                            if (truck != null)
                            {
                                //send aggregate events to cloud
                                Logging.Logger.Log("INFO", "Send aggregate events");
                                foreach (var moduleEvent in uow.AggregateEventRepository.GetDirtyOrderedByTime())
                                {
                                    if (token.IsCancellationRequested) return;
                                    if (truck != null) moduleEvent.TruckID = truck.Id;
                                    if (driver != null) moduleEvent.DriverID = driver.Id;
                                    moduleEvent.Source = InputSource.TRUCK;

                                    //lookup module
                                    var module = uow.ModuleRepository.FindSingle(m => m.Name == moduleEvent.SerialNumber, "Field.Farm.Client", "PickupList");

                                    //add on field/farm/client/pickup list info it may be needed to re-create list at gin
                                    if (module != null)
                                    {
                                        moduleEvent.PickupListId = module.PickupListId;
                                        moduleEvent.PickupListName = module.ListName;

                                        if (module.Field != null)
                                            moduleEvent.FarmId = module.Field.FarmId;

                                        moduleEvent.FarmName = module.FarmName;
                                        moduleEvent.FieldId = module.FieldId;
                                        moduleEvent.FieldName = module.FieldName;
                                        moduleEvent.ModuleId = module.Id;

                                        if (module.Field.Farm != null)
                                            moduleEvent.ClientId = module.Field.Farm.ClientId;

                                        moduleEvent.ClientName = module.ClientName;
                                    }

                                    await DocumentDBContext.UpsertItemAsync<AggregateEvent>(moduleEvent);
                                    uow.AggregateEventRepository.Update(moduleEvent, true);
                                    uow.SaveChanges();
                                }

                                if (token.IsCancellationRequested) return;

                                //send list of pickup lists this truck has
                                Logging.Logger.Log("INFO", "Send list of pickup lists this truck has downloaded.");
                                var lists = uow.PickupListRepository.FindMatching(t => t.AssignedTrucks.Any(x => x.Id == truck.Id), new string[] { "AssignedTrucks" });
                                TruckListsDownloaded document2 = new TruckListsDownloaded();
                                document2.Id = "TRUCKDOWNLOADS_" + truck.Id;
                                document2.Name = truck.Id;
                                document2.Source = InputSource.TRUCK;
                                document2.SyncedToCloud = true;
                                document2.Created = DateTime.UtcNow;
                                document2.PickupListsDownloaded = new List<string>();
                                document2.PickupListsDownloaded.AddRange(lists.Select(x => x.Id).ToArray());
                                await DocumentDBContext.UpsertItemAsync<TruckListsDownloaded>(document2);
                            }
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
        
        private static async Task pushEntitiesOfType<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities, List<string> IdsToIgnore, params string[] includes) where TObject : BaseEntity, new()
        {
            try
            {
                Logging.Logger.Log("INFO", "Pushing entities of type " +  typeof(TObject).ToString());
                var remoteIds = remoteEntities.Select(t => t.Id).ToArray();
                var newEntities = repo.FindMatching(t => t.Source == InputSource.TRUCK && !remoteIds.Contains(t.Id) && !IdsToIgnore.Contains(t.Id), includes);
                foreach (var entity in newEntities)
                {
                    if (cToken.IsCancellationRequested) return;
                    await DocumentDBContext.UpsertItemAsync<TObject>(entity);
                    repo.Update(entity, true);
                    uow.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static async Task pushModules(IUnitOfWork uow, IModuleRepository repo, CancellationToken cToken, IEnumerable<ModuleEntity> remoteEntities, params string[] includes) 
        {
            try
            {
                Logging.Logger.Log("INFO", "Pushing entities of type Module");
                //var remoteIds = remoteEntities.Select(t => t.Id).ToList();

                //only push modules if they are not already in cloud, they were created on the truck, and they are not on the unassigned list
                //TODO PICKUP LIST ID
                var newEntities = repo.FindMatching(t => t.Source == InputSource.TRUCK && t.PickupListId != GUIDS.UNASSIGNED_LIST_ID, includes).ToList();

                int count = 1;
                foreach (var entity in newEntities/*.Where(t => !remoteIds.Contains(t.Id))*/)
                {
                    Console.WriteLine("Pushing entity: " + entity.Name);

                    if (cToken.IsCancellationRequested) return;
                    await DocumentDBContext.UpsertItemAsync<ModuleEntity>(entity);

                    var remoteEntity = remoteEntities.SingleOrDefault(m => m.Id == entity.Id);
                    if (remoteEntity != null)
                    {
                        remoteEntity.PickupListId = entity.PickupListId;
                        remoteEntity.FieldId = entity.FieldId;                        
                    }

                    repo.Update(entity, true);
                    count++;
                    if (count == 100)
                    {
                        count = 1;
                        Console.WriteLine("Saving changes");
                        uow.SaveChanges();
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

        private static void addEntitiesOfType<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities, IEnumerable<TObject> localEntities) where TObject : BaseEntity, new()
        {
            try
            {
                //add entities in remote source but not local
                Logging.Logger.Log("INFO", "Add entities of type " + typeof(TObject).ToString());
                var localIds = localEntities.Select(t => t.Id).ToList();
                int count = 1;
                foreach (var e in remoteEntities.Where(t=> !localIds.Contains(t.Id)))
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

        private static void updateEntitiesOfType<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities, IEnumerable<TObject> localEntities, bool detachIfAttached) where TObject : BaseEntity, new()
        {
            try
            {
                Logging.Logger.Log("INFO", "Update entities of type " + typeof(TObject).ToString());

                var localEntityIds = localEntities.Select(t => t.Id).ToArray();

                //update entities that have same id
                int i = 1;
                foreach (var remoteEntity in remoteEntities.Where(t => localEntityIds.Contains(t.Id)))
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

        private static void updateEntitiesOfTypeIfNewer<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities, IEnumerable<TObject> localEntities, bool detachIfAttached) where TObject : BaseEntity, new()
        {
            try
            {
                Logging.Logger.Log("INFO", "Update entities of type " + typeof(TObject).ToString());

                var localEntityIds = localEntities.Select(t => t.Id).ToArray();

                //update entities that have same id
                int i = 1;
                foreach (var remoteEntity in remoteEntities.Where(t => localEntityIds.Contains(t.Id)))
                {
                    DateTime remoteTimestamp = remoteEntity.Created;
                    if (remoteEntity.Updated.HasValue)
                        remoteTimestamp = remoteEntity.Updated.Value;

                    var localEntity = localEntities.SingleOrDefault(x => x.Id == remoteEntity.Id);
                    DateTime localTimestamp = localEntity.Created;
                    if (localEntity.Updated.HasValue)
                    {
                        localTimestamp = localEntity.Updated.Value;
                    }

                    if (localTimestamp <= remoteTimestamp)
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
                }
                Console.WriteLine("Saving changes");
                uow.SaveChanges();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }



        /// <summary>
        /// Clean up duplicate serial numbers in the event the truck added a module with a serial number that existed at the gin
        /// in this scenario the truck will get the gin module and the module it created, only the gin module should stay
        /// </summary>
        /// <param name="uow"></param>
        /// <param name="repo"></param>
        /// <param name="cToken"></param>
        private static void cleanDupedModules(IUnitOfWork uow, IModuleRepository repo, CancellationToken cToken)
        {
            if (cToken.IsCancellationRequested) return;

            var modulesWithTruckSource = repo.FindMatching(m => m.Source == InputSource.TRUCK);
            var modulesWithGinSource = repo.FindMatching(m => m.Source == InputSource.GIN);

            foreach(var m in modulesWithTruckSource.Where(m => modulesWithGinSource.Any(g => g.Name == m.Name)))
            {
                repo.Delete(m);
            }

            uow.SaveChanges();
        }

        private static async Task processDeletedDocuments(IUnitOfWork uow, CancellationToken cToken, string thisTruckID, List<TruckPickupListRelease> truckReleaseDocuments)
        {
            if (cToken.IsCancellationRequested) return;
            try
            {
                var docsDeleted = new List<DocumentToProcess>();
                
                foreach (var d in uow.DocumentsToProcessRepository.GetAll()) {
                    if (d.EntityType == EntityType.PICKUPLIST)
                    {
                        var releaseDoc = new TruckPickupListRelease();
                        releaseDoc.Id = Guid.NewGuid().ToString();
                        releaseDoc.Source = InputSource.TRUCK;
                        releaseDoc.SyncedToCloud = true;
                        releaseDoc.Name = "LIST_RELEASE_" + thisTruckID + "_" + d.Id;
                        releaseDoc.TruckID = thisTruckID;
                        releaseDoc.PickupListID = d.Id;
                        truckReleaseDocuments.Add(releaseDoc);
                    }
                    docsDeleted.Add(d);
                }

                if (truckReleaseDocuments.Count() > 0)
                {
                    await DocumentDBContext.BulkUpsert<TruckPickupListRelease>(truckReleaseDocuments, 500);
                }

                uow.DocumentsToProcessRepository.BulkDelete(docsDeleted);
                uow.SaveChanges();

                
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static void deleteEntitiesOfType<TObject>(IUnitOfWork uow, IEntityRepository<TObject> repo, CancellationToken cToken, IEnumerable<TObject> remoteEntities) where TObject : BaseEntity, new()
        {
            Logging.Logger.Log("INFO", "Remove entities of type " + typeof(TObject).ToString());

            var localEntities = repo.GetAll();
            var entitiesToDelete = new List<TObject>();

            //delete entities that are no longer in gin list and that originated from gin
            foreach (var e in localEntities.Where(l => !remoteEntities.Any(r => r.Id == l.Id) && l.Source != InputSource.TRUCK))
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
        
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            try
            {
                if (tokenSource != null) tokenSource.Cancel();             

                Logging.Logger.CleanUp();
            }
            catch (Exception exc)
            {
                string s = exc.Message;
            }          
        }
    }
}
