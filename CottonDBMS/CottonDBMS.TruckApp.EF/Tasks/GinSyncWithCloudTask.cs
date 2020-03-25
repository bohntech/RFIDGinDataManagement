//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using CottonDBMS.Cloud;
using CottonDBMS.DataModels;
using CottonDBMS.EF.Repositories;
using CottonDBMS.Interfaces;
using System.Linq.Expressions;
using System.Device.Location;

namespace CottonDBMS.EF.Tasks
{
    public static class GinSyncWithCloudTask
    {
        static CancellationTokenSource tokenSource = null;
        static CancellationToken token;
        static bool initialized = false;
        static bool syncRunning = false;
        static bool forceRun = false;
        static string statusMessage = "";
        static DateTime? lastRunTime = null;

        static bool isProcessing = false;

        #region Public Methods
        public static void Reset()
        {           
            initialized = false;
            Init();
        }

        public static bool IsProcessing
        {
            get
            {
                return isProcessing;
            }
        }

        public static void ForceSyncRun()
        {
            forceRun = true;
        }

        public static string StatusMessage
        {
            get
            {
                return statusMessage;
            }
        }

        public static DateTime? LastRun
        {
            get
            {
                return lastRunTime;
            }
        }

        public static void WaitForSyncToFinish()
        {
            while(syncRunning)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        public static void Init()
        {
            if (!initialized)
            {
                try
                {
                    tokenSource = new CancellationTokenSource();
                    token = tokenSource.Token;

                    Task.Run(async () =>
                    {                      
                        System.Threading.Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                        syncRunning = true;
                        while (true && !token.IsCancellationRequested)
                        {                            
                            try
                            {
                                Logging.Logger.Log("INFO", "Starting sync to cloud");

                                string setupWizardSetting = null;
                                using (var uow = new UnitOfWork())
                                {
                                    setupWizardSetting = uow.SettingsRepository.GetSettingWithDefault(GinAppSettingKeys.SETUP_WIZARD_COMPLETE, "");
                                }

                                if (DocumentDBContext.Initialized && !string.IsNullOrEmpty(setupWizardSetting))
                                {
                                    statusMessage = "Running...";
                                    isProcessing = true;
                                    lastRunTime = DateTime.Now;

                                    Logging.Logger.Log("INFO", "Process released lists.");
                                    await processTruckListReleaseDocuments(token);

                                    Logging.Logger.Log("INFO", "Delete entities.");
                                    await deleteEntities(token);                                                                      

                                    using (var uow = new UnitOfWork())
                                    {
                                        Logging.Logger.Log("INFO", "Push settings to cloud.");
                                        await pushSettingsToCloud(uow);
                                        
                                        //add records from cloud with input source = TRUCK     
                                        await addTruckCreatedEntities<TruckEntity>(uow, uow.TruckRepository, token);
                                        await addTruckCreatedEntities<DriverEntity>(uow, uow.DriverRepository, token);

                                        statusMessage = "Downloading clients added by trucks.";
                                        await addTruckCreatedEntities<ClientEntity>(uow, uow.ClientRepository, token);

                                        statusMessage = "Downloading farms added by trucks.";
                                        await addTruckCreatedEntities<FarmEntity>(uow, uow.FarmRepository, token);

                                        statusMessage = "Downloading fields added by trucks.";
                                        await addTruckCreatedEntities<FieldEntity>(uow, uow.FieldRepository, token);

                                        statusMessage = "Downloading pickup lists added by trucks.";
                                        await addTruckCreatedEntities<PickupListEntity>(uow, uow.PickupListRepository, token);
                                    }

                                    using (var uow = new UnitOfWork())
                                    {
                                        //add or update modules from truck (module documents in cloud with INPUT SOURCE = TRUCK)
                                        statusMessage = "Downloading modules added by trucks.";
                                        await updateTruckModifiedModules(uow, uow.ModuleRepository, token);
                                    }


                                    //upsert entities from local gin db that have been added or modified.
                                    using (var uow = new UnitOfWork())
                                    {
                                        statusMessage = "Updating truck records in cloud.";
                                        await syncEntities<TruckEntity>(uow, uow.TruckRepository, token, null);
                                        statusMessage = "Updating driver records in cloud.";
                                        await syncEntities<DriverEntity>(uow, uow.DriverRepository, token, null);
                                        statusMessage = "Updating client records in cloud.";
                                        await bulkSyncEntities<ClientEntity>(uow, uow.ClientRepository, token, null);
                                        statusMessage = "Updating farm records in cloud.";
                                        await bulkSyncEntities<FarmEntity>(uow, uow.FarmRepository, token, null);
                                        statusMessage = "Updating field records in cloud.";
                                        await bulkSyncEntities<FieldEntity>(uow, uow.FieldRepository, token, null);
                                        statusMessage = "Updating pickup list records in cloud.";
                                        await syncEntities<PickupListEntity>(uow, uow.PickupListRepository, token, null, "AssignedTrucks", "DownloadedToTrucks");
                                                                                
                                    }

                                    using (var uow = new UnitOfWork())
                                    {
                                        statusMessage = "Updating module records in cloud.";
                                        uow.DisableChangeTracking();
                                        await bulkSyncEntities<ModuleEntity>(uow, uow.ModuleRepository, token, (p => p.PickupListId != null && p.SyncedToCloud==false)); //need to only sync dirty and use bulk insert
                                    }

                                    //pull in aggregate events
                                    statusMessage = "Download truck load/unload events.";
                                    await downloadTruckEvents(token);

                                    statusMessage = "Processing truck events.";
                                    processTruckEvents(token);

                                    statusMessage = "Updating pickup list statuses.";
                                    await updatePickupListStatuses(token);

                                    //process scale load scans
                                    statusMessage = "Downloading scanned gin loads.";
                                    await downloadLoadScans(token);

                                    statusMessage = "Processing scanned gin loads.";
                                    processLoadScans(token);

                                    //process feeder load scans
                                    statusMessage = "Downloading feeder scans.";
                                    await downloadFeederScans(token);

                                    statusMessage = "Processing feeder scans.";
                                    processFeederScans(token);

                                    //process PBI scans
                                    statusMessage = "Downloading PBIs and weights.";
                                    await downloadPBIScans(token);

                                    statusMessage = "Processing PBIs and weights.";
                                    processPBIScans(token);

                                    //write updates to module ownership
                                    statusMessage = "Syncing module ownership table.";
                                    await updateOwnershipTable(token);
                                }                            
                                
                                Logging.Logger.Log("INFO", "Finished sync to cloud");
                            }
                            catch (Exception exc)
                            {
                                Logging.Logger.Log(exc);
                            }

                            isProcessing = false;

                            if (!token.IsCancellationRequested)
                            {
                                statusMessage = "Waiting for next sync time.";
                                int i = 0;
                                while (i < 60 && !token.IsCancellationRequested && !forceRun) { System.Threading.Thread.Sleep(1000); i++; }
                                forceRun = false;
                            }
                            else
                                Logging.Logger.Log("INFO", "SYNC TO CLOUD THREAD CANCELLATION REQUESTED.");
                        }
                        syncRunning = false;

                    }, token);

                    initialized = true;
                }
                catch (Exception exc)
                {
                    Logging.Logger.Log(exc);
                }
            }
        }

        public static void Cancel()
        {
            if (tokenSource != null)
                tokenSource.Cancel();
        }
        #endregion

        #region Private Methods
        private static async Task updateOwnershipTable(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var setting = uow.SettingsRepository.FindSingle(s => s.Key == GinAppSettingKeys.LAST_SYNC_TIME);
                    DateTime lastSyncTime = DateTime.UtcNow;
                    DateTime syncStarted = DateTime.UtcNow;
                    if (setting != null)
                    {
                        lastSyncTime = DateTime.Parse(setting.Value);
                    }
                    else
                    {
                        lastSyncTime = DateTime.Now.AddYears(-1); //first time sync has been run
                    }

                    var clientIds = uow.ClientRepository.GetIdsForChanged(lastSyncTime);
                    var farmIds = uow.FarmRepository.GetIdsForChanged(lastSyncTime);
                    var fieldIds = uow.FieldRepository.GetIdsForChanged(lastSyncTime);
                    var moduleIds = uow.ModuleRepository.GetIdsForChanged(lastSyncTime);
                    var ginLoadIds = uow.GinLoadRepository.GetIdsForChanged(lastSyncTime);

                    var affectedModules = uow.ModuleRepository.FindMatching(m => fieldIds.Contains(m.FieldId)
                    || clientIds.Contains(m.Field.Farm.ClientId) || farmIds.Contains(m.Field.FarmId) || ginLoadIds.Contains(m.GinLoadId) || moduleIds.Contains(m.Id),
                    new string[] { "Field.Farm.Client", "GinLoad" }
                    );

                    List<ModuleOwnershipEntity> ownershipRecords = new List<ModuleOwnershipEntity>();

                    foreach (var m in affectedModules)
                    {

                        if (!cToken.IsCancellationRequested)
                        {
                            ModuleOwnershipEntity ownership = new ModuleOwnershipEntity();
                            ownership.Id = "OWNERSHIP-" + m.Id;
                            ownership.Name = m.Name;
                            ownership.Created = DateTime.UtcNow;
                            ownership.Updated = DateTime.UtcNow;
                            ownership.BridgeLoadNumber = (m.GinLoad != null) ? m.GinLoad.ScaleBridgeLoadNumber : 0;
                            ownership.Client = m.ClientName;
                            ownership.Deleted = false;
                            ownership.Farm = m.FarmName;
                            ownership.Field = m.FieldName;
                            ownership.GinTagLoadNumber = (m.GinLoad != null) ? m.GinLoad.GinTagLoadNumber : m.GinTagLoadNumber;
                            ownership.ImportedLoadNumber = m.ImportedLoadNumber;
                            ownership.TruckLoadNumber = m.LoadNumber;

                            ownership.Location = (m.GinLoad != null) ? m.GinLoad.YardLocation : "" ;
                            ownership.PickedBy = (m.GinLoad != null) ? m.GinLoad.PickedBy : "";
                            ownership.Variety = (m.GinLoad != null) ? m.GinLoad.Variety : "";
                            ownership.TruckID = (m.GinLoad != null) ? m.GinLoad.TruckID : "";
                            ownership.TrailerNumber = (m.GinLoad != null) ? m.GinLoad.TrailerNumber : "";
                            ownership.LoadGrossWeight = (m.GinLoad != null) ? m.GinLoad.GrossWeight : 0.00M;
                            ownership.LoadNetWeight = (m.GinLoad != null) ? m.GinLoad.NetWeight : 0.00M;
                            ownership.LoadSplitWeight1 = (m.GinLoad != null) ? m.GinLoad.SplitWeight1 : 0.00M;
                            ownership.LoadSplitWeight2 = (m.GinLoad != null) ? m.GinLoad.SplitWeight2 : 0.00M;

                            ownership.Status = m.StatusName;
                            ownershipRecords.Add(ownership);
                        }
                        else
                        {
                            break;
                        }
                    }

                    await DocumentDBContext.BulkUpsert<ModuleOwnershipEntity>(ownershipRecords, 1000);

                    uow.SettingsRepository.UpsertSetting(GinAppSettingKeys.LAST_SYNC_TIME, syncStarted.ToString());
                    uow.SaveChanges();
                }
            }
            catch (Exception excOuter)
            {
                Logging.Logger.Log(excOuter);
            }
        }

        private static async Task downloadTruckEvents(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var truckEvents = await DocumentDBContext.GetAllItemsAsync<AggregateEvent>(t => t.EntityType == EntityType.AGGREGATE_EVENT);
                    foreach (var e in truckEvents.OrderBy(t => t.Timestamp))
                    {
                        if (cToken.IsCancellationRequested) return;
                        var newEvent = new AggregateEvent();
                        e.CopyTo(newEvent);
                        newEvent.Processed = false;
                        uow.AggregateEventRepository.Add(newEvent);
                        uow.SaveChanges();
                        await DocumentDBContext.DeleteItemAsync<AggregateEvent>(e.Id);
                    }
                }           
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }
        
        private static void processTruckEvents(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    if (cToken.IsCancellationRequested) return;
                    var truckEvents = uow.AggregateEventRepository.FindMatching(e => e.Processed == false);
                    var affectedModuleSerials = truckEvents.Select(t => t.SerialNumber).Distinct().ToList();
                    var affectedModules = uow.ModuleRepository.FindMatching(m => affectedModuleSerials.Contains(m.Name), new string[] { "ModuleHistory" });

                    var allPickupLists = uow.PickupListRepository.GetAll().ToList();
                    //var allFields = uow.FieldRepository.GetAll().ToList();
                    
                    if (cToken.IsCancellationRequested) return;
                    var allTrucks = uow.TruckRepository.GetAll();

                    if (cToken.IsCancellationRequested) return;
                    var allDrivers = uow.DriverRepository.GetAll();

                    foreach (var e in truckEvents.OrderBy(t => t.Timestamp))
                    {
                        if (cToken.IsCancellationRequested) return;
                        try
                        {
                            var truck = allTrucks.SingleOrDefault(x => x.Id == e.TruckID);
                            var driver = allDrivers.SingleOrDefault(x => x.Id == e.DriverID);

                            var affectedModule = affectedModules.FirstOrDefault(m => m.Name == e.SerialNumber);

                            if (affectedModule == null && string.IsNullOrEmpty(e.FieldId))
                            {
                                e.Processed = true;
                                uow.AggregateEventRepository.Update(e);
                                uow.SaveChanges();
                                continue;
                            }

                            if (affectedModule == null) //if module not in system - re-add it
                            {
                                Logging.Logger.Log("INFO", "Creating new module for serial " + e.SerialNumber);
                                affectedModule = new ModuleEntity();
                                var pickupList = uow.PickupListRepository.GetById(e.PickupListId, new string[] {"Field.Farm.Client"});
                                if (pickupList == null)
                                {
                                    //TODO PICKUPLIST ID
                                    affectedModule.PickupListId = null;
                                }
                                else
                                {
                                    //TODO PICKUPLIST ID
                                    affectedModule.PickupListId = e.PickupListId;
                                    affectedModule.FieldId = pickupList.FieldId;
                                }                                
                                
                                affectedModule.Created = e.Created;
                                if (!string.IsNullOrEmpty(e.ModuleId))
                                    affectedModule.Id = e.ModuleId;
                                else
                                    affectedModule.Id = Guid.NewGuid().ToString();

                                affectedModule.SyncedToCloud = false;
                                uow.ModuleRepository.Add(affectedModule); //added
                                uow.SaveChanges();                        //added

                                affectedModule = uow.ModuleRepository.GetById(affectedModule.Id, "ModuleHistory");
                            }

                            if (affectedModule != null)
                            {
                                affectedModule.Latitude = e.AverageLat;
                                affectedModule.Longitude = e.AverageLong;
                                affectedModule.ModuleId = e.Epc;

                                var pickupList = allPickupLists.SingleOrDefault(p => p.Id == e.Id);

                                if (pickupList != null)
                                {
                                    affectedModule.PickupListId = pickupList.Id;
                                    affectedModule.FieldId = pickupList.FieldId;
                                }

                                //only events for field pickup will be assigned a load number
                                if (!string.IsNullOrWhiteSpace(e.LoadNumber))
                                    affectedModule.LoadNumber = e.LoadNumber;

                                ModuleHistoryEntity historyItem = new ModuleHistoryEntity();
                                historyItem.Id = Guid.NewGuid().ToString();
                                historyItem.Created = e.Timestamp;
                                historyItem.Driver = (driver != null) ? driver.Name : "";
                                historyItem.TruckID = (truck != null) ? truck.Name : "";
                                historyItem.ModuleId = affectedModule.Id;
                                historyItem.Latitude = e.AverageLat;
                                historyItem.Longitude = e.AverageLong;
                                affectedModule.ModuleHistory.Add(historyItem);
                                affectedModule.Driver = historyItem.Driver;
                                affectedModule.TruckID = historyItem.TruckID;

                                if (e.EventType == EventType.LOADED)
                                {
                                    historyItem.ModuleEventType = ModuleEventType.LOADED;
                                    if (affectedModule.ModuleStatus == ModuleStatus.IN_FIELD)
                                    {
                                        affectedModule.ModuleStatus = ModuleStatus.PICKED_UP;
                                        uow.ModuleRepository.Save(affectedModule);
                                    }
                                }
                                else if (e.EventType == EventType.UNLOADED)
                                {
                                    historyItem.ModuleEventType = ModuleEventType.UNLOADED;

                                    //detect gin feeder drop
                                    if (uow.SettingsRepository.EventAtFeeder(e))
                                    {
                                        affectedModule.ModuleStatus = ModuleStatus.ON_FEEDER;
                                    }
                                    else if (uow.SettingsRepository.EventOnGinYard(e))
                                    {
                                        affectedModule.ModuleStatus = ModuleStatus.AT_GIN;
                                    }                                    
                                    else
                                    {
                                        affectedModule.ModuleStatus = ModuleStatus.IN_FIELD; //change status back to field
                                        affectedModule.LoadNumber = null;
                                    }
                                }
                                historyItem.ModuleStatus = affectedModule.ModuleStatus;
                                affectedModule.SyncedToCloud = false;
                                uow.ModuleRepository.Save(affectedModule);
                                uow.SaveChanges();

                                e.Processed = true;
                                uow.AggregateEventRepository.Update(e);
                                uow.SaveChanges();
                            }
                           
                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log("WARNING", "AN ERROR OCCURRED PROCESSING MODULES EVENTS");
                Logging.Logger.Log(exc);
            }
        }

        private static async Task downloadTruckRegistrations(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var truckRegistrations = await DocumentDBContext.GetAllItemsAsync<TruckRegistrationEntity>(t => t.EntityType == EntityType.TRUCK_REGISTRATION);
                    foreach (var e in truckRegistrations.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        var newItem = new TruckRegistrationEntity();
                        e.CopyTo(newItem);
                        newItem.Processed = false;
                        uow.TruckRegistrationsRepository.Add(newItem);
                        uow.SaveChanges();
                        await DocumentDBContext.DeleteItemAsync<TruckRegistrationEntity>(e.Id);
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static void processTruckRegistrations(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    if (cToken.IsCancellationRequested) return;
                    var truckRegistrations = uow.TruckRegistrationsRepository.FindMatching(e => e.Processed == false);
                    var affectedTruckIDs = truckRegistrations.Select(t => t.Name).Distinct().ToList();
                    var affectedTrucks = uow.TruckRepository.FindMatching(t => affectedTruckIDs.Contains(t.Name));

                    foreach (var registration in truckRegistrations.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        try
                        {
                            var affectedTruck = affectedTrucks.FirstOrDefault(t => t.Name == registration.Name);

                            if (affectedTruck == null) //if module not in system - re-add it
                            {
                                Logging.Logger.Log("INFO", "Creating new truck from bridge registration " + registration.Name);
                                affectedTruck = new TruckEntity();
                                affectedTruck.Created = registration.Created;
                                affectedTruck.SyncedToCloud = false;
                                uow.TruckRepository.Add(affectedTruck);
                            }
                            else
                            {
                                affectedTruck.Updated = DateTime.UtcNow;
                            }

                            affectedTruck.Name = registration.Name;
                            affectedTruck.LicensePlate = registration.LicensePlate;
                            affectedTruck.LoadPrefix = "C-" + DateTime.Now.ToString("MMddyyyyHHmmss");
                            affectedTruck.OwnerName = registration.Owner;
                            affectedTruck.OwnerPhone = registration.OwnerPhone;
                            affectedTruck.Source = InputSource.GIN;
                            affectedTruck.SyncedToCloud = false;

                            if (registration.Weight.HasValue)
                                affectedTruck.TareWeight = registration.Weight.Value;

                            affectedTruck.CustomHauler = true;
                            uow.TruckRepository.Save(affectedTruck);

                            registration.Processed = true;
                            uow.TruckRegistrationsRepository.Update(registration);
                            uow.SaveChanges();

                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log("WARNING", "AN ERROR OCCURRED PROCESSING BRIDGE TRUCK REGISTRATIONS");
                Logging.Logger.Log(exc);
            }
        }

        private static async Task downloadLoadScans(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var loadScans = await DocumentDBContext.GetAllItemsAsync<LoadScanEntity>(t => t.EntityType == EntityType.LOAD_SCAN);
                    foreach (var e in loadScans.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        var newItem = new LoadScanEntity();                        
                        e.CopyTo(newItem);
                        newItem.Id = Guid.NewGuid().ToString(); //set new id to have a record of every incoming scan
                        newItem.Processed = false;
                        uow.LoadScanRepository.Add(newItem);
                        uow.SaveChanges();
                        await DocumentDBContext.DeleteItemAsync<LoadScanEntity>(e.Id);
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static void processLoadScans(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    if (cToken.IsCancellationRequested) return;
                    var loadScans = uow.LoadScanRepository.FindMatching(e => e.Processed == false);
                    var affectedGinLoadNumbers = loadScans.Select(t => t.GinTagLoadNumber).Distinct().ToList();
                    var affectedBridgeLoadNumbers = loadScans.Select(t => t.BridgeLoadNumber).Distinct().ToList();
                    var affectedGinLoads = uow.GinLoadRepository.FindMatching(t => affectedGinLoadNumbers.Contains(t.GinTagLoadNumber) || affectedBridgeLoadNumbers.Contains(t.ScaleBridgeLoadNumber), new string[] {"Modules" });
                    
                    foreach (var scan in loadScans.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        try
                        {
                            if (string.IsNullOrEmpty(scan.Client))
                            {
                                scan.Client = "UNKNOWN";                               
                            }

                            if (string.IsNullOrEmpty(scan.Farm))
                            {                                
                                scan.Farm = "UNKNOWN";                                
                            }

                            if (string.IsNullOrEmpty(scan.Field))
                            {
                                scan.Field = "UNKNOWN";
                            }

                            var client = uow.ClientRepository.EnsureClientCreated(scan.Client, InputSource.GIN);
                            uow.SaveChanges();
                            var farm = uow.FarmRepository.EnsureFarmCreatedUniqueToClient(client, scan.Farm, InputSource.GIN);
                            uow.SaveChanges();
                            var field = uow.FieldRepository.EnsureFieldCreated(farm, scan.Field, InputSource.GIN);
                            uow.SaveChanges();

                            var affectedGinLoad = affectedGinLoads.FirstOrDefault(t => t.GinTagLoadNumber == scan.GinTagLoadNumber); 
                            
                            if (affectedGinLoad == null)  //if tag number doesn't have a match then update based on BridgeLoadNumber
                            {
                                var timeStampChangeDate = DateTime.Parse("10/23/2019 12:00AM").ToUniversalTime();
                                affectedGinLoad = affectedGinLoads.FirstOrDefault(t => t.ScaleBridgeLoadNumber == scan.BridgeLoadNumber && (scan.Created < timeStampChangeDate || t.Created == scan.Created));
                            }

                            if (affectedGinLoad == null) //if load not in system - re-add it
                            {
                                Logging.Logger.Log("INFO", "Creating new gin load from bridge scan for tag number " + scan.GinTagLoadNumber);
                                affectedGinLoad = new GinLoadEntity();
                                affectedGinLoad.Created = scan.Created;
                                affectedGinLoad.SyncedToCloud = false;
                                //uow.GinLoadRepository.Add(affectedGinLoad);                                                           
                                //affectedGinLoad = uow.GinLoadRepository.GetById(affectedGinLoad.Id);
                            }
                            else
                            {                               
                                 affectedGinLoad.Updated = DateTime.UtcNow;                               
                            }

                            affectedGinLoad.Name = scan.Name;
                            affectedGinLoad.GinTagLoadNumber = scan.GinTagLoadNumber;

                            affectedGinLoad.GrossWeight = scan.GrossWeight;
                            affectedGinLoad.NetWeight = scan.NetWeight;
                            affectedGinLoad.SplitWeight1 = scan.SplitWeight1;
                            affectedGinLoad.SplitWeight2 = scan.SplitWeight2;                            
                            affectedGinLoad.Name = scan.Name;
                            affectedGinLoad.PickedBy = scan.PickedBy;
                            affectedGinLoad.ScaleBridgeId = scan.BridgeID;
                            affectedGinLoad.ScaleBridgeLoadNumber = scan.BridgeLoadNumber;
                            affectedGinLoad.Source = InputSource.GIN;
                            affectedGinLoad.SyncedToCloud = false;
                            affectedGinLoad.TrailerNumber = scan.TrailerNumber;
                            affectedGinLoad.TruckID = scan.TruckID;
                            affectedGinLoad.Variety = scan.Variety;
                            affectedGinLoad.YardLocation = scan.YardRow;
                            affectedGinLoad.SubmittedBy = scan.SubmittedBy;

                            //add client/farm/field information
                            affectedGinLoad.FieldId = field.Id;
                            uow.GinLoadRepository.SaveUseObjectDates(affectedGinLoad);
                            uow.SaveChanges();

                            //check for modules no longer in load and remove
                            var serialsInScan = scan.ScanData.Scans.Select(t => t.SerialNumber).ToArray();
                            
                            var modulesToRemoveFromLoad = uow.ModuleRepository.FindMatching(m => m.GinTagLoadNumber == scan.GinTagLoadNumber && !serialsInScan.Contains(m.Name), new string[] { "ModuleHistory" }).ToList();
                            foreach (var m in modulesToRemoveFromLoad) {
                                //var relatedScan = scan.ScanData.Scans.FirstOrDefault(s => s.SerialNumber == m.Name);
                                m.GinTagLoadNumber = "";
                                //m.BridgeLoadNumber = null;
                                m.LastBridgeId = null;
                                m.GinLoadId = null;
                                ModuleHistoryEntity historyItem = new ModuleHistoryEntity();                                
                                historyItem.Id = Guid.NewGuid().ToString();
                                historyItem.Created = scan.LastCreatedOrUpdated;
                                historyItem.Driver = "";
                                historyItem.TruckID = scan.TruckID;
                                historyItem.BridgeId = scan.BridgeID;
                                historyItem.BridgeLoadNumber = null;
                                historyItem.GinTagLoadNumber = "";
                                historyItem.Name = m.Name; //scan.GinTagLoadNumber;
                                historyItem.ModuleStatus = scan.TargetStatus;
                                historyItem.Latitude = scan.Latitude;
                                historyItem.Longitude = scan.Longitude;
                                historyItem.ModuleEventType = ModuleEventType.BRIDGE_SCAN;
                                historyItem.SyncedToCloud = false;
                                historyItem.ModuleId = m.Id;
                                historyItem.Latitude = scan.Latitude;
                                historyItem.Longitude = scan.Longitude;                                
                                m.ModuleHistory.Add(historyItem);
                                uow.ModuleRepository.Save(m);
                                uow.SaveChanges();
                            }           

                            foreach (var moduleScan in scan.ScanData.Scans) {                               
                             
                                var module = uow.ModuleRepository.FindSingle(m => m.Name == moduleScan.SerialNumber, new string[] { "ModuleHistory" });
                                //var relatedScan = scan.ScanData.Scans.FirstOrDefault(s => module != null && s.SerialNumber == module.Name);
                                if (module == null) //no existing module so create new one
                                {
                                    module = new ModuleEntity();
                                    module.Created = (moduleScan.ScanTime.HasValue) ? moduleScan.ScanTime.Value : DateTime.UtcNow;
                                    module.Id = Guid.NewGuid().ToString();
                                    module.ClassingModuleId = uow.ModuleRepository.NextModuleClassingId();
                                    module.Driver = "";
                                    module.FieldId = field.Id;
                                   // module.Created = DateTime.UtcNow;
                                    module.ImportedLoadNumber = "";
                                    module.IsConventional = false;
                                    module.LoadNumber = "";
                                    module.ModuleId = moduleScan.EPC;
                                    module.Name = moduleScan.SerialNumber;                                    
                                    module.Source = InputSource.GIN;
                                    module.SyncedToCloud = false;
                                    module.TruckID = scan.TruckID;
                                    uow.ModuleRepository.Add(module);
                                    uow.SaveChanges();
                                    module = uow.ModuleRepository.GetById(module.Id, "ModuleHistory");
                                }
                                else
                                {                                   
                                    module.Updated = DateTime.UtcNow;                                   
                                }

                                //update module with scan information and link to load
                                module.TruckID = scan.TruckID;
                                module.LastBridgeId = scan.BridgeID;
                                module.GinLoadId = affectedGinLoad.Id;

                                if (string.IsNullOrEmpty(module.FirstBridgeId))
                                    module.FirstBridgeId = scan.BridgeID;

                                module.GinTagLoadNumber = scan.GinTagLoadNumber;
                                module.ClassingModuleId = "";
                                module.ModuleStatus = scan.TargetStatus;
                                module.Latitude = scan.Latitude;
                                module.Longitude = scan.Longitude;                                

                                ModuleHistoryEntity historyItem = new ModuleHistoryEntity();
                                historyItem.Id = Guid.NewGuid().ToString();    
                                
                                if (moduleScan.ScanTime.HasValue)
                                {
                                    //only set timestamp to scan time for first if scan time hasn't already been used for a history item
                                    //this keeps manual load updates from using the same timestamp
                                    if (!module.ModuleHistory.Any(m => m.ModuleEventType == ModuleEventType.BRIDGE_SCAN && m.GinTagLoadNumber == scan.GinTagLoadNumber
                                    && m.Created.ToString("MM/dd/yyyy hh:mm:ss tt") == moduleScan.ScanTime.Value.ToString("MM/dd/yyyy hh:mm:ss tt")))
                                    {
                                        historyItem.Created = moduleScan.ScanTime.Value;
                                    }
                                    else
                                    {
                                        historyItem.Created = scan.LastCreatedOrUpdated;
                                    }
                                }
                                else
                                {
                                    historyItem.Created = scan.LastCreatedOrUpdated;
                                }
                                
                                //historyItem.Created = moduleScan.ScanTime.HasValue ? moduleScan.ScanTime.Value : DateTime.UtcNow;
                                historyItem.Updated = scan.LastCreatedOrUpdated;
                                historyItem.Driver = "";
                                historyItem.TruckID = scan.TruckID;
                                historyItem.BridgeId = scan.BridgeID;
                                historyItem.BridgeLoadNumber = scan.BridgeLoadNumber;
                                historyItem.GinTagLoadNumber = scan.GinTagLoadNumber;
                                historyItem.Name = module.Name;
                                historyItem.ModuleStatus = scan.TargetStatus;
                                historyItem.Latitude = scan.Latitude;
                                historyItem.Longitude = scan.Longitude;
                                historyItem.ModuleEventType = ModuleEventType.BRIDGE_SCAN;
                                historyItem.SyncedToCloud = false;
                                historyItem.ModuleId = module.Id;
                                historyItem.Latitude = scan.Latitude;
                                historyItem.Longitude = scan.Longitude;
                                module.ModuleHistory.Add(historyItem);
                                uow.ModuleRepository.Save(module);
                                uow.SaveChanges();
                            }

                            scan.Processed = true;
                            uow.LoadScanRepository.Save(scan);
                            uow.SaveChanges();
                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log("WARNING", "AN ERROR OCCURRED PROCESSING BRIDGE TRUCK REGISTRATIONS");
                Logging.Logger.Log(exc);
            }
        }

        private static async Task downloadFeederScans(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var loadScans = await DocumentDBContext.GetAllItemsAsync<FeederScanEntity>(t => t.EntityType == EntityType.FEEDER_SCAN);
                    foreach (var e in loadScans.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        var newItem = new FeederScanEntity();
                        e.CopyTo(newItem);
                        newItem.Id = Guid.NewGuid().ToString(); //set new id to have a record of every incoming scan
                        newItem.Processed = false;
                        uow.FeederScanRepository.Add(newItem);
                        uow.SaveChanges();
                        await DocumentDBContext.DeleteItemAsync<FeederScanEntity>(e.Id);
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static void processFeederScans(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    if (cToken.IsCancellationRequested) return;
                    var scans = uow.FeederScanRepository.FindMatching(e => e.Processed == false);
                    var affectedModuleSerials = scans.Select(t => t.Name).Distinct().ToList();
                    var affectedModules = uow.ModuleRepository.FindMatching(m => affectedModuleSerials.Contains(m.Name), new string[] { "ModuleHistory", "GinLoad" });
                  
                    if (cToken.IsCancellationRequested) return;

                    foreach (var e in scans.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        try
                        {
                            var affectedModule = affectedModules.FirstOrDefault(m => m.Name == e.Name);

                            if (affectedModule == null) //if module not in system - re-add it
                            {
                                Logging.Logger.Log("INFO", "Creating new module for feeder scan of " + e.Name);
                                var client = uow.ClientRepository.EnsureClientCreated("UNKNOWN", InputSource.GIN);
                                uow.SaveChanges();
                                var farm = uow.FarmRepository.EnsureFarmCreated(client, "UNKNOWN", InputSource.GIN);
                                uow.SaveChanges();
                                var field = uow.FieldRepository.EnsureFieldCreated(farm, "UNKNOWN", InputSource.GIN);
                                uow.SaveChanges();

                                affectedModule = new ModuleEntity();
                                affectedModule.Id = Guid.NewGuid().ToString();
                                affectedModule.Name = e.Name;
                                affectedModule.ModuleId = e.EPC;
                                affectedModule.Created = e.Created;
                                affectedModule.FieldId = field.Id;
                                affectedModule.SyncedToCloud = false;

                                uow.ModuleRepository.Add(affectedModule);
                                uow.SaveChanges();                       
                                affectedModule = uow.ModuleRepository.GetById(affectedModule.Id, "ModuleHistory", "GinLoad");
                            }

                            affectedModule.Latitude = e.Latitude;
                            affectedModule.Longitude = e.Longitude;
                            affectedModule.ModuleId = e.EPC;
                            affectedModule.ModuleStatus = e.TargetStatus;
                            affectedModule.LastBridgeId = e.BridgeID;

                            ModuleHistoryEntity historyItem = new ModuleHistoryEntity();
                            historyItem.Id = Guid.NewGuid().ToString();
                            historyItem.Created = e.Created;
                            historyItem.Driver = "";
                            historyItem.TruckID = "";
                            historyItem.BridgeId = e.BridgeID;
                            historyItem.ModuleId = affectedModule.Id;
                            historyItem.Latitude = e.Latitude;
                            historyItem.Longitude = e.Longitude;
                            historyItem.GinTagLoadNumber = affectedModule.GinTagLoadNumber;
                            
                            if (!string.IsNullOrEmpty(affectedModule.BridgeLoadNumber))
                                historyItem.BridgeLoadNumber = int.Parse(affectedModule.BridgeLoadNumber);
                            else
                                historyItem.BridgeLoadNumber = null;

                            affectedModule.ModuleHistory.Add(historyItem);                            
                            historyItem.ModuleStatus = e.TargetStatus;
                            historyItem.ModuleStatus = affectedModule.ModuleStatus;
                            historyItem.ModuleEventType =  ModuleEventType.BRIDGE_SCAN;
                            affectedModule.SyncedToCloud = false;
                            uow.ModuleRepository.Save(affectedModule);
                            uow.SaveChanges();

                            e.Processed = true;
                            uow.FeederScanRepository.Update(e);
                            uow.SaveChanges();
                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log("WARNING", "AN ERROR OCCURRED PROCESSING FEEDER SCANS");
                Logging.Logger.Log(exc);
            }
        }

        private static async Task downloadPBIScans(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var scans = await DocumentDBContext.GetAllItemsAsync<BaleScanEntity>(t => t.EntityType == EntityType.BALE_SCAN);
                    foreach (var e in scans.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        var newItem = new BaleScanEntity();
                        e.CopyTo(newItem);
                        newItem.Id = Guid.NewGuid().ToString(); //set new id to have a record of every incoming scan
                        newItem.Processed = false;
                        uow.BaleScanRepository.Add(newItem);
                        uow.SaveChanges();
                        await DocumentDBContext.DeleteItemAsync<BaleScanEntity>(e.Id);
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static void processPBIScans(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    if (cToken.IsCancellationRequested) return;
                    var scans = uow.BaleScanRepository.FindMatching(e => e.Processed == false);
                    var affectedPBIs = scans.Select(t => t.PbiNumber).Distinct().ToList();
                    var affectedBales = uow.BalesRepository.FindMatching(m => affectedPBIs.Contains(m.Name), new string[] { /*"Module", "GinLoad"*/ }).ToList();

                    if (cToken.IsCancellationRequested) return;

                    foreach (var e in scans.OrderBy(t => t.Created))
                    {
                        if (cToken.IsCancellationRequested) return;
                        try
                        {
                            var affectedBale = affectedBales.FirstOrDefault(m => m.Name == e.Name);

                            if (affectedBale == null) //if bale not in system add it
                            {
                                Logging.Logger.Log("INFO", "Creating new bale for PBI scan " + e.Name);
                                affectedBale = new BaleEntity();
                                affectedBale.Id = Guid.NewGuid().ToString();
                                affectedBale.Name = e.Name;
                                affectedBale.PbiNumber = e.PbiNumber;                                                                                           
                                affectedBale.Created = e.Created;
                                affectedBale.SyncedToCloud = false;
                                uow.BalesRepository.Add(affectedBale);
                                affectedBales.Add(affectedBale);
                            }
                            else
                            {
                                affectedBale.Updated = e.LastCreatedOrUpdated;
                                uow.BalesRepository.Update(affectedBale);
                            }
                            affectedBale.WeightFromScale = e.ScaleWeight;
                            affectedBale.TareWeight = e.TareWeight;
                            affectedBale.ScanNumber = e.ScanNumber;
                            affectedBale.SyncedToCloud = false;
                            affectedBale.NetWeight = e.ScaleWeight - e.TareWeight;
                            //uow.BalesRepository.Save(affectedBale);
                            uow.SaveChanges();

                            e.Processed = true;
                            uow.BaleScanRepository.Update(e);
                            uow.SaveChanges();
                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log("WARNING", "AN ERROR OCCURRED PROCESSING PBI SCANS");
                Logging.Logger.Log(exc);
            }
        }

        private static async Task updatePickupListStatuses(CancellationToken cToken)
        {

            if (cToken.IsCancellationRequested) return;

            using (var uow = new UnitOfWork())
            {
                //get truck assigned pickup lists
                var truckLists = await DocumentDBContext.GetAllItemsAsync<TruckListsDownloaded>(t => t.EntityType == EntityType.TRUCK_LISTS_DOWNLOADED);
                var allTrucks = uow.TruckRepository.GetAll();

                if (cToken.IsCancellationRequested) return;

                foreach (var pickupList in uow.PickupListRepository.GetAll(new string[] { "AssignedModules", "AssignedTrucks", "DownloadedToTrucks" }))
                {
                    try
                    {
                        if (cToken.IsCancellationRequested) return;
                        var originalStatus = pickupList.PickupListStatus;

                        //find truck to remove from list
                        var trucksToDelete = new List<TruckEntity>();
                        var trucksToAdd = new List<TruckEntity>();

                        //find trucks that show to have downloaded the list in gin db but are not
                        //showing to have downloaded it in the cloud list
                        foreach (var truck in pickupList.DownloadedToTrucks)
                        {
                            if (cToken.IsCancellationRequested) return;
                            var truckPickupListsDownloaded = truckLists.SingleOrDefault(t => t.Name == truck.Id && t.PickupListsDownloaded.Contains(pickupList.Id));
                            if (truckPickupListsDownloaded == null)
                            {
                                trucksToDelete.Add(truck);
                            }
                        }

                        foreach (var t in trucksToDelete)
                        {
                            if (cToken.IsCancellationRequested) return;
                            pickupList.DownloadedToTrucks.Remove(t);
                        }

                        //find trucks that have this pickup list, but are not listed on saved list
                        foreach (var truckList in truckLists)
                        {
                            if (cToken.IsCancellationRequested) return;
                            if (truckList.PickupListsDownloaded.Contains(pickupList.Id) && !pickupList.DownloadedToTrucks.Any(t => t.Id == truckList.Name))
                            {
                                var truck = allTrucks.SingleOrDefault(t => t.Id == truckList.Name);
                                if (truck != null) pickupList.DownloadedToTrucks.Add(truck);
                            }
                        }

                        var dest = pickupList.Destination;

                        if (pickupList.AssignedModules.Count() > pickupList.OriginalModuleCount)
                        {
                            //modules got added on the truck so we need to set the original count for proper status tracking
                            pickupList.OriginalModuleCount = pickupList.AssignedModules.Count();
                            pickupList.OriginalSerialNumbers = "";
                            foreach (var m in pickupList.AssignedModules)
                            {
                                pickupList.OriginalSerialNumbers += m + ",";
                            }
                            pickupList.OriginalSerialNumbers = pickupList.OriginalSerialNumbers.TrimEnd(',');
                        }


                        if (pickupList.AssignedModules.Count() == 0  && pickupList.OriginalModuleCount == 0)
                        {
                            pickupList.PickupListStatus = PickupListStatus.OPEN;
                        }
                        else if (pickupList.AssignedModules.Count() == 0 && pickupList.OriginalModuleCount > 0)
                        {
                            pickupList.PickupListStatus = PickupListStatus.COMPLETE;
                        }
                        else if (pickupList.AssignedModules.Count(m => (m.ModuleStatus == ModuleStatus.PICKED_UP || m.ModuleStatus == ModuleStatus.AT_GIN || m.ModuleStatus >= ModuleStatus.ON_FEEDER) && dest == PickupListDestination.GIN_YARD) >= pickupList.AssignedModules.Count())
                        {
                            pickupList.PickupListStatus = PickupListStatus.COMPLETE;
                        }
                        else if (pickupList.AssignedModules.Count(m => m.ModuleStatus >= ModuleStatus.ON_FEEDER && dest == PickupListDestination.GIN_FEEDER) >= pickupList.AssignedModules.Count())
                        {
                            pickupList.PickupListStatus = PickupListStatus.COMPLETE;
                        }
                        else
                        {
                            pickupList.PickupListStatus = PickupListStatus.OPEN;
                        }
                        uow.PickupListRepository.Save(pickupList);                        
                        uow.SaveChanges();
                    }
                    catch (Exception exc)
                    {
                        Logging.Logger.Log(exc);
                    }
                }
            }
        }

        private static async Task pushSettingsToCloud(UnitOfWork uow)
        {
            try
            {
                var mplSetting = uow.SettingsRepository.FindSingle(t => t.Key == GinAppSettingKeys.MODULES_PER_LOAD);
                var prefixSetting = uow.SettingsRepository.FindSingle(t => t.Key == GinAppSettingKeys.LOAD_PREFIX);
                var startLoadNumberSetting = uow.SettingsRepository.FindSingle(t => t.Key == GinAppSettingKeys.STARTING_LOAD_NUMBER);
                var GoogleMapsKeySetting = uow.SettingsRepository.FindSingle(t => t.Key == GinAppSettingKeys.GOOGLE_MAPS_API_KEY);
                                
                var feederNorthLatitude = uow.SettingsRepository.GetSettingDoubleValue(GinAppSettingKeys.GIN_FEEDER_NORTH);
                var feederWestLongitude = uow.SettingsRepository.GetSettingDoubleValue(GinAppSettingKeys.GIN_FEEDER_WEST);
                var feederDetectionRadiusYards = uow.SettingsRepository.GetSettingDoubleValue(GinAppSettingKeys.GIN_FEEDER_DETECTION_RADIUS);

                var yardNorthwestCornerLatitude  = uow.SettingsRepository.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_NW_CORNER_NORTH);
                var yardNorthWestCornerLongitude = uow.SettingsRepository.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_NW_CORNER_WEST);
                var yardSouthEastCornerLatitude  = uow.SettingsRepository.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_SE_CORNER_NORTH);
                var yardSouthEastCornerLongitude = uow.SettingsRepository.GetSettingDoubleValue(GinAppSettingKeys.GIN_YARD_SE_CORNER_WEST);

                SyncedSettings document = new SyncedSettings();
                document.Id = "SETTINGS_SUMMARY";
                document.Name = "SETTINGS_SUMMARY";
                document.EntityType = EntityType.SETTING_SUMMARY;
                document.LoadPrefix = prefixSetting.Value;
                document.ModulesPerLoad = int.Parse(mplSetting.Value);
                document.StartingLoadNumber = int.Parse(startLoadNumberSetting.Value);
                document.GoogleMapsKey = GoogleMapsKeySetting.Value;

                document.FeederDetectionRadius = feederDetectionRadiusYards;
                document.FeederLatitude = feederNorthLatitude;
                document.FeederLongitude = feederWestLongitude;
                document.GinYardNWLat = yardNorthwestCornerLatitude;
                document.GinYardNWLong = yardNorthWestCornerLongitude;
                document.GinYardSELat = yardSouthEastCornerLatitude;
                document.GinYardSELong = yardSouthEastCornerLongitude;

                await DocumentDBContext.UpsertItemAsync<SyncedSettings>(document);
               
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }
                
        private static async Task processTruckListReleaseDocuments(CancellationToken cToken)
        {
            try
            {
                using (var uow = new UnitOfWork())
                {
                    var releaseDocs = await DocumentDBContext.GetAllItemsAsync<TruckPickupListRelease>(x => x.EntityType == EntityType.TRUCK_PICKUP_LIST_RELEASE);
                    uow.PickupListRepository.ReleaseListsFromTruck(releaseDocs); //remove trucks from downloaded and assigned tables
                    await DocumentDBContext.BulkDelete<TruckPickupListRelease>(releaseDocs.ToList(), 500); //remove release documents from cloud                   
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static async Task deleteEntities(CancellationToken cToken)
        {
            try
            {
                if (!cToken.IsCancellationRequested)
                {
                    using (var uow = new UnitOfWork())
                    {
                        uow.DisableChangeTracking();
                        var entitiesToDelete = uow.DocumentsToProcessRepository.GetAll();

                        long count = await DocumentDBContext.BulkDelete<DocumentToProcess>(entitiesToDelete.ToList(), 1000);

                        if (count == entitiesToDelete.Count())
                        {
                            uow.DocumentsToProcessRepository.BulkDelete(entitiesToDelete);
                            uow.SaveChanges();
                        }
                       else //clean up entities in table that may not exist in cloud anymore
                        {
                            try
                            {
                                var idsToDelete = entitiesToDelete.Select(t => t.Id).ToList();
                                var missingIds = await DocumentDBContext.GetMissingIds<BaseEntity>(idsToDelete);
                                var toDelete = entitiesToDelete.Where(t => missingIds.Contains(t.Id)).ToList();
                                uow.DocumentsToProcessRepository.BulkDelete(toDelete);
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
            catch (Exception excOuter)
            {
                Logging.Logger.Log(excOuter);
            }
        }

        private static async Task addTruckCreatedEntities<T>(UnitOfWork uow, IEntityRepository<T> repo, CancellationToken cToken) where T : BaseEntity, new()
        {
            var temp = new T();

            var entitiesAddedByTruck = await DocumentDBContext.GetAllItemsAsync<T>(t => t.EntityType == temp.EntityType && t.Source == InputSource.TRUCK);

            if (!cToken.IsCancellationRequested)
            {
                foreach (var t in entitiesAddedByTruck)
                {
                    try
                    {
                        if (!cToken.IsCancellationRequested)
                        {
                            var existing = repo.GetById(t.Id);

                            if (existing == null)
                            {
                                t.SyncedToCloud = false;
                                t.Source = InputSource.GIN;
                                repo.Add(t);
                                uow.SaveChanges();
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception exc)
                    {
                        Logging.Logger.Log(exc);
                    }
                }
            }
        }

        private static async Task updateTruckModifiedModules(UnitOfWork uow, IModuleRepository repo, CancellationToken cToken) 
        {
            var entitiesModifiedByTruck = await DocumentDBContext.GetAllItemsAsync<ModuleEntity>(t => t.EntityType == EntityType.MODULE && t.Source == InputSource.TRUCK);

            if (!cToken.IsCancellationRequested)
            {
                foreach (var t in entitiesModifiedByTruck)
                {
                    try
                    {
                        if (!cToken.IsCancellationRequested)
                        {
                            var existing = repo.GetById(t.Id);
                            bool newId = false;
                            if (existing == null) //if didn't match on id try serial number
                            {
                                existing = repo.FindSingle(m => m.Name == t.Name);
                                newId = true;
                            }

                            if (existing != null)
                            {
                                existing.SyncedToCloud = false;
                                existing.Source = InputSource.GIN;
                                existing.FieldId = t.FieldId;
                                
                                //TODO PICKUP LIST ID
                                existing.PickupListId = t.PickupListId;
                                existing.Latitude = t.Latitude;
                                existing.Longitude = t.Longitude;
                                repo.Update(existing);
                                uow.SaveChanges();

                                //if the truck posted a module with a new id but the serial number matches something else
                                //remove the truck posted doc from the cloud
                                if (newId) await DocumentDBContext.DeleteItemAsync<ModuleEntity>(t.Id);
                            }
                            else
                            {
                                t.SyncedToCloud = false;
                                t.Source = InputSource.GIN;
                                repo.Add(t);
                                uow.SaveChanges();
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (Exception exc)
                    {
                        Logging.Logger.Log(exc);
                    }
                }
            }
        }

        private static async Task syncEntities<T>(UnitOfWork uow, IEntityRepository<T> repo, CancellationToken cToken, System.Linq.Expressions.Expression<Func<T, bool>> predicate, params string[] includes) where T : BaseEntity
        {
            try
            {
                IEnumerable<T> entities = null;

                if (predicate != null) entities = repo.FindMatching(predicate, includes);
                else entities = repo.GetDirty(includes);

                if (!cToken.IsCancellationRequested)
                {
                    int i = 1;
                    foreach (var t in entities)
                    {
                        try
                        {
                            if (!cToken.IsCancellationRequested)
                            {
                                await DocumentDBContext.UpsertItemAsync<T>(t);
                                t.SyncedToCloud = true;
                                repo.Update(t, true);
                                i++;
                                if (i == 100)
                                {
                                    i = 1;
                                    uow.SaveChanges();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception exc)
                        {
                            Logging.Logger.Log(exc);
                        }
                    }

                    uow.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }

        private static async Task bulkSyncEntities<T>(UnitOfWork uow, IEntityRepository<T> repo, CancellationToken cToken, System.Linq.Expressions.Expression<Func<T, bool>> predicate, params string[] includes) where T : BaseEntity
        {
            try
            {
                IEnumerable<T> entities = null;

                if (predicate != null) entities = repo.FindMatching(predicate, includes);
                else entities = repo.GetDirty(includes);

                if (!cToken.IsCancellationRequested)
                {
                    int i = 1;

                    long count = await DocumentDBContext.BulkUpsert<T>(entities.ToList(), 1000);

                    if (count == entities.Count())
                    {
                        foreach (var t in entities)
                        {
                            try
                            {
                                if (!cToken.IsCancellationRequested)
                                {                                    
                                    t.SyncedToCloud = true;
                                    repo.QuickUpdate(t, false);
                                    i++;
                                    if (i == 100)
                                    {
                                        i = 1;
                                        uow.SaveChanges();
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (Exception exc)
                            {
                                Logging.Logger.Log(exc);
                            }
                        }
                        uow.SaveChanges();
                    }                   
                }
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
            }
        }
        #endregion
    }
}
