//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.Data.EF;
using System.Data.SqlClient;

namespace CottonDBMS.EF.Repositories
{
    public class PickupListRepository : EntityRepository<PickupListEntity>, IPickupListRepository
    {
        public PickupListRepository(AppDBContext context) : base(context)
        {

        }

        public IEnumerable<string> GetModuleIdsOnPickupList()
        {
            return  _context.PickupLists.Include("AssignedModules").Where(list => list.AssignedModules.Any()).SelectMany(t => t.AssignedModules).Select(m => m.Id).ToList();
        }

        private IQueryable<PickupListEntity> GetFilteredQuery(PickupListFilter filter)
        {
            string farmName = filter.Farm.Trim();
            string clientName = filter.Client.Trim();
            string fieldName = filter.Field.Trim();
                                   
            var q = _context.PickupLists
                   .Include("AssignedTrucks")
                   .Include("DownloadedToTrucks")
                   .Include("Field.Farm.Client").AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(clientName)) q = q.Where(p => p.Field.Farm.Client.Name == clientName);
            if (!string.IsNullOrWhiteSpace(farmName)) q = q.Where(p => p.Field.Farm.Name == farmName);
            if (!string.IsNullOrWhiteSpace(fieldName)) q = q.Where(p => p.Field.Name == fieldName);
            if (filter.Status.HasValue) q = q.Where(p => p.PickupListStatus == filter.Status.Value);
            if (filter.StartDate.HasValue) q = q.Where(p => p.Created >= filter.StartDate.Value);
            if (filter.EndDate.HasValue) q = q.Where(p => p.Created <= filter.EndDate.Value);

            if (filter.Sort1Ascending)
            {
                if (filter.SortCol1.ToLower() == "client") q = q.OrderBy(m => m.Field.Farm.Client.Name);
                if (filter.SortCol1.ToLower() == "name") q = q.OrderBy(m => m.Name);
                else if (filter.SortCol1.ToLower() == "farm") q = q.OrderBy(m => m.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") q = q.OrderBy(m => m.Field.Name);
                else if (filter.SortCol1.ToLower() == "status") q = q.OrderBy(m => m.PickupListStatus);
                else if (filter.SortCol1.ToLower() == "timestamp") q = q.OrderBy(m => m.Created);
            }
            else
            {
                if (filter.SortCol1.ToLower() == "client") q = q.OrderByDescending(m => m.Field.Farm.Client.Name);
                if (filter.SortCol1.ToLower() == "name") q = q.OrderByDescending(m => m.Name);
                else if (filter.SortCol1.ToLower() == "farm") q = q.OrderByDescending(m => m.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") q = q.OrderByDescending(m => m.Field.Name);
                else if (filter.SortCol1.ToLower() == "status") q = q.OrderByDescending(m => m.PickupListStatus);
                else if (filter.SortCol1.ToLower() == "timestamp") q = q.OrderByDescending(m => m.Created);
            }

            return q;
        }

        public  PagedResult<PickupListEntity> GetLists(PickupListFilter filter, int pageSize, int pageNo, int modulesPerLoad)
        {
            try
            {
                var filteredQuery = GetFilteredQuery(filter);
                var countQuery = GetFilteredQuery(filter);

                var cResult = new PagedResult<PickupListEntity>();
                cResult.Total = countQuery.Count();
                cResult.TotalPages = cResult.Total / pageSize;
                if (cResult.Total % pageSize > 0) cResult.TotalPages++;
                cResult.LastPageNo = pageNo;
                cResult.ResultData = new List<PickupListEntity>();
                if (pageNo <= cResult.TotalPages)
                {
                    cResult.ResultData.AddRange(filteredQuery.Skip(pageSize * (pageNo - 1)).Take(pageSize));
                }


                var listIds = cResult.ResultData.Select(p => p.Id).ToList();

                var modulesInFieldCounts = _context.Modules.Where(m => m.PickupListId != null && m.ModuleStatus == ModuleStatus.IN_FIELD && listIds.Contains(m.PickupListId))
                   .GroupBy(p => p.PickupListId)
                   .Select(g => new { PickupListId = g.Key, Count = g.Count() });


                var modulesAtGinCounts = _context.Modules.Where(m => m.PickupListId != null && m.ModuleStatus == ModuleStatus.AT_GIN && listIds.Contains(m.PickupListId))
                  .GroupBy(p => p.PickupListId)
                  .Select(g => new { PickupListId = g.Key, Count = g.Count() });


                foreach (var result in cResult.ResultData)
                {
                    result.ModulesPerLoad = modulesPerLoad;
                    result.SearchSetTotalModules = 0;

                    if (result.Destination == PickupListDestination.GIN_YARD)
                    {
                        var grouping = modulesInFieldCounts.SingleOrDefault(g => g.PickupListId == result.Id);
                        if (grouping != null)
                        {
                            result.SearchSetModulesRemaining = grouping.Count;
                        }
                    }

                    if (result.Destination == PickupListDestination.GIN_FEEDER)
                    {
                        var grouping = modulesAtGinCounts.SingleOrDefault(g => g.PickupListId == result.Id);
                        if (grouping != null)
                        {
                            result.SearchSetModulesRemaining = grouping.Count;
                        }
                    }
                }

              


                return cResult;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error fetching modules.", exc);
            }
        }

        public IEnumerable<string> GetDownloadedPickupListIds()
        {
            return _context.PickupLists.Include("DownloadedToTrucks").Where(p => p.DownloadedToTrucks.Any()).Select(p => p.Id).ToList();
        }

        public bool CanSavePickupList(string id, string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return false;
            }                       

            return !(_context.PickupLists.Any(r => r.Name == name.Trim() && r.Id != id));
        }

        public IEnumerable<ModuleEntity> GetAvailableModulesForPickupList(string listId, string fieldId, PickupListDestination destination)
        {
            //get list of modules for this field that are on any pickup list with same destination
            //var modulesForField = _context.Modules.Where(m => m.FieldId == fieldId).ToList();
            List<string> moduleIdsOnPickupList = null;

            if (destination == PickupListDestination.GIN_YARD)
            {
                moduleIdsOnPickupList = _context.PickupLists.Include("AssignedModules").Where(p => p.FieldId == fieldId && p.Id != listId && p.Destination == PickupListDestination.GIN_YARD).SelectMany(p => p.AssignedModules).Where(m => m.ModuleStatus == ModuleStatus.IN_FIELD).Select(m => m.Id).ToList();
                return _context.Modules.Where(m => !moduleIdsOnPickupList.Contains(m.Id) && m.FieldId == fieldId && m.ModuleStatus == ModuleStatus.IN_FIELD).OrderBy(t => t.Name).ToList();
            }
            else
            {
                moduleIdsOnPickupList = _context.PickupLists.Include("AssignedModules").Where(p => p.FieldId == fieldId && p.Id != listId && p.Destination == PickupListDestination.GIN_FEEDER).SelectMany(p => p.AssignedModules).Where(m => m.ModuleStatus == ModuleStatus.AT_GIN).Select(m => m.Id).ToList();
                return _context.Modules.Where(m => !moduleIdsOnPickupList.Contains(m.Id) && m.FieldId == fieldId && m.ModuleStatus == ModuleStatus.AT_GIN).OrderBy(t => t.Name).ToList();
            }          
            
        }

        public FieldEntity FindFieldWithSerialNumber(string serial)
        {
            var module = _context.Modules.Include("Field.Farm.Client").FirstOrDefault(m => m.Name == serial);
            return module.Field;
        }

        public string GetListIDWithSerialNumber(string serial, double lat, double lng, PickupListDestination dest)
        {
            var result = _context.PickupLists.Include("AssignedModules").FirstOrDefault(p => p.AssignedModules.Any(m => m.Name == serial) && p.Destination == dest);
            var module = _context.Modules.Include("Field.Farm.Client").SingleOrDefault(m => m.Name == serial);

            if (result != null)
            {
                return result.Id;
            }

            //if unassigned list already exists just return it
            result = _context.PickupLists.Include("AssignedModules").FirstOrDefault(p => p.Id == GUIDS.UNASSIGNED_LIST_ID);

            if (result != null) return result.Id;


            var settingsRepo = new SettingsRepository(_context);
            var syncedSettingsRepo = new SyncedSettingRepository(_context);
            var syncedSettings = syncedSettingsRepo.GetAll().FirstOrDefault();
            var fieldRepo = new FieldRepository(_context);
            var farmRepo = new FarmRepository(_context);
            var clientRepo = new ClientRepository(_context);

            var truck = settingsRepo.GetCurrentTruck();


            ClientEntity client = null;
            FarmEntity farm = null;
            FieldEntity field = null;

            if (module == null) //if no module already exists then create an un-assigned list
            {
                client = clientRepo.GetById(GUIDS.UNASSIGNED_CLIENT_ID);

                if (client == null)
                {
                    client = new ClientEntity();
                    client.Id = GUIDS.UNASSIGNED_CLIENT_ID;
                    client.Name = "Unassigned";
                    client.Created = DateTime.UtcNow;
                    client.Source = InputSource.TRUCK;
                    _context.Clients.Add(client);
                }

                farm = farmRepo.GetById(GUIDS.UNASSIGNED_FARM_ID);
                if (farm == null)
                {
                    farm = new FarmEntity();
                    farm.Id = GUIDS.UNASSIGNED_FARM_ID;
                    farm.ClientId = client.Id;
                    farm.SyncedToCloud = true;
                    farm.Source = InputSource.TRUCK;
                    farm.Name = "Unassigned";
                    farm.Created = DateTime.UtcNow;
                    _context.Farms.Add(farm);
                }

                field = fieldRepo.GetById(GUIDS.UNASSIGNED_FIELD_ID);
                if (field == null)
                {
                    field = new FieldEntity();
                    field.Id = GUIDS.UNASSIGNED_FIELD_ID;
                    field.FarmId = farm.Id;
                    field.SyncedToCloud = true;
                    field.Source = InputSource.TRUCK;
                    field.Name = "Unassigned";
                    field.Created = DateTime.UtcNow;
                    _context.Fields.Add(field);
                }
            }
            else
            {
                client = module.Field.Farm.Client;
                farm = module.Field.Farm;
                field = module.Field;
            }

            PickupListEntity list = new PickupListEntity();
            list.Id = GUIDS.UNASSIGNED_LIST_ID;
            list.Latitude = lat;
            list.Longitude = lng;
            list.Source = InputSource.TRUCK;
            list.ModulesPerLoad = (syncedSettings != null) ? syncedSettings.ModulesPerLoad : 4;
            list.FieldId = field.Id;
            list.Name = "Unassigned";
            list.PickupListStatus = PickupListStatus.OPEN;
            list.SyncedToCloud = true;
            list.Created = DateTime.UtcNow;
            list.AssignedTrucks = new List<TruckEntity>();
            list.DownloadedToTrucks = new List<TruckEntity>();
            list.Destination = dest;
            list.AssignedTrucks.Add(truck);
            list.DownloadedToTrucks.Add(truck);
            _context.PickupLists.Add(list);
            _context.SaveChanges();
            //List<string> serialsToMove = new List<string>();
            //serialsToMove.Add(serial);
            //this.MoveModulesToList(list, serialsToMove);
            return list.Id;
        }

      

        public void MoveModulesToList(PickupListEntity list, List<string> serialNumbersToMove, double lat, double lng)
        {

            var settingsRepo = new SettingsRepository(_context);
            var currentTruck = settingsRepo.GetCurrentTruck();

            string lastSN = serialNumbersToMove.LastOrDefault();

            foreach (var serial in serialNumbersToMove)
            {
                var module = _context.Modules.SingleOrDefault(m => m.Name == serial);
                if (module != null)
                {
                    if (module.Name == lastSN) //only update location of last module loaded truck
                    {
                        module.Latitude = lat;
                        module.Longitude = lng;
                    }
                    module.Source = InputSource.TRUCK;
                    //TODO PICKUPLISTID
                    module.PickupListId = list.Id;
                    module.Updated = DateTime.UtcNow;
                }
                else //module does not exist so we need to add new module
                {
                    module = new ModuleEntity();
                    module.Id = Guid.NewGuid().ToString();
                    module.IsConventional = false;
                    module.Latitude = lat;
                    module.Longitude = lng;
                    module.ModuleHistory = new List<ModuleHistoryEntity>();
                    module.ModuleStatus = ModuleStatus.IN_FIELD;
                    module.Name = serial;
                    //TODO PICKUPLISTID
                    module.PickupListId = list.Id;
                    module.FieldId = list.FieldId;
                    module.Source = InputSource.TRUCK;
                    module.SyncedToCloud = false;
                    if (currentTruck != null)
                        module.TruckID = currentTruck.Id;
                    module.Created = DateTime.UtcNow;
                    _context.Modules.Add(module);
                }
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Adds an entity to the context to be inserted on Save
        /// </summary>
        /// <param name="entity"></param>
        /*public override void Add(PickupListEntity entity)
        {
            entity.SyncedToCloud = false;
            foreach (var m in entity.AssignedModules)
            {
                _context.Modules.Attach(m);
                _context.Entry<ModuleEntity>(m).State = EntityState.Modified;
            }

            foreach (var t in entity.AssignedTrucks)
            {
                _context.Truck.Attach(t);
                _context.Entry<TruckEntity>(t).State = EntityState.Modified;
            }

            foreach (var t in entity.DownloadedToTrucks)
            {
                _context.Truck.Attach(t);
                _context.Entry<TruckEntity>(t).State = EntityState.Modified;
            }

            _context.PickupLists.Add(entity);            
        }*/

        public override void Add(PickupListEntity entity)
        {
            entity.SyncedToCloud = false;
            PickupListEntity newEntity = new PickupListEntity();
            newEntity.Name = entity.Name;
            newEntity.Id = entity.Id;
            newEntity.FieldId = entity.FieldId;
            newEntity.Updated = entity.Updated;
            newEntity.Latitude = entity.Latitude;
            newEntity.Longitude = entity.Longitude;
            newEntity.PickupListStatus = entity.PickupListStatus;
            newEntity.SelfLink = entity.SelfLink;
            newEntity.Source = entity.Source;
            newEntity.Destination = entity.Destination;
            newEntity.EntityType = entity.EntityType;
            newEntity.ModulesPerLoad = entity.ModulesPerLoad;
            newEntity.OriginalModuleCount = entity.OriginalModuleCount;
            newEntity.OriginalSerialNumbers = entity.OriginalSerialNumbers;
            newEntity.AssignedModules = new List<ModuleEntity>();
            newEntity.DownloadedToTrucks = new List<TruckEntity>();
            newEntity.AssignedTrucks = new List<TruckEntity>();
            newEntity.SyncedToCloud = false;

            var assignedModuleIDs = entity.AssignedModules.Select(m => m.Id).ToArray();
            var assignedModules = _context.Modules.Where(x => assignedModuleIDs.Contains(x.Id)).ToList();
            assignedModules.ForEach(m => m.SyncedToCloud = false);
            newEntity.AssignedModules.AddRange(assignedModules);

            var assignedTruckIDs = entity.AssignedTrucks.Select(m => m.Id).ToArray();
            var assignedTrucks = _context.Truck.Where(x => assignedTruckIDs.Contains(x.Id)).ToList();
            newEntity.AssignedTrucks.AddRange(assignedTrucks);

            var downloadedTruckIDs = entity.DownloadedToTrucks.Select(m => m.Id).ToArray();
            var downloadedToTrucks = _context.Truck.Where(x => downloadedTruckIDs.Contains(x.Id)).ToList();
            newEntity.DownloadedToTrucks.AddRange(downloadedToTrucks);
                       
            _context.PickupLists.Add(newEntity);
        }

        private TruckEntity GetTruckFromContext(TruckEntity t)
        {
            var existingEntity = _context.Truck.Local.SingleOrDefault(x => x.Id == t.Id);

            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                _context.Truck.Attach(t);
                _context.Entry<TruckEntity>(t).State = EntityState.Modified;
                return t;
            }
        }

        private ModuleEntity GetModuleFromContext(ModuleEntity t)
        {
            var existingEntity = _context.Modules.Local.SingleOrDefault(x => x.Id == t.Id);

            if (existingEntity != null)
            {
                return existingEntity;
            }
            else
            {
                _context.Modules.Attach(t);
                _context.Entry<ModuleEntity>(t).State = EntityState.Modified;
                return t;
            }
        }
        
        public void ReleaseListsFromTruck(IEnumerable<TruckPickupListRelease> releaseDocuments)
        {
            StringBuilder sqlQuery = new StringBuilder();
            List<string> pickupListIds = releaseDocuments.Select(t => t.PickupListID).Distinct().ToList();
            //var modulesOnList = _context.Modules.Where(m => pickupListIds.Contains(m.PickupListId)).ToList();
            //var docToProcessIds = _context.DocumentsToProcess.Where(p => p.EntityType == EntityType.MODULE).Select(t => t.Id).ToList();

            foreach (var release in releaseDocuments)
            {
                Guid pickupListID = Guid.NewGuid();
                Guid truckID = Guid.NewGuid();

                //make sure we have guid formats to prevent SQL injection
                if (!Guid.TryParse(release.PickupListID.Replace("'", "''"), out pickupListID)) continue;
                if (!Guid.TryParse(release.TruckID.Replace("'", "''"), out truckID)) continue;

                sqlQuery.Append("DELETE PickupListAssignedTrucks WHERE PickupListId='"+pickupListID.ToString()+"' AND TruckId='"+truckID.ToString()+"';");
                sqlQuery.Append("DELETE PickupListDownloadedToTrucks WHERE PickupListId='" + pickupListID.ToString() + "' AND TruckId='" + truckID.ToString() + "';");
                sqlQuery.Append("UPDATE PickupListEntities SET SyncedToCloud=0 WHERE Id='" + pickupListID.ToString() + "';");

                _context.Database.ExecuteSqlCommand(sqlQuery.ToString());
            }
        }

        public virtual void Update(PickupListEntity entity, List<string> AssignedModuleIds, List<string> AssignedTruckIds)
        {
            var idsToDelete = new List<string>();

            //need to ensure modules removed from pickup list are removed from cloud
            var modulesToRemove = new List<ModuleEntity>();
            foreach (var deletedModule in entity.AssignedModules.Where(x => !AssignedModuleIds.Contains(x.Id)))
            {
                if (!_context.DocumentsToProcess.Any(i => i.Id == entity.Id))
                {
                    DocumentToProcess p = new DocumentToProcess();
                    p.EntityType = EntityType.MODULE;
                    p.Source = InputSource.GIN;
                    p.SelfLink = "";
                    p.Name = deletedModule.Name;
                    p.Id = deletedModule.Id;
                    p.Action = ProcessingAction.DELETE;
                    _context.DocumentsToProcess.Add(p);
                    modulesToRemove.Add(deletedModule);
                }
            }

            entity.Updated = DateTime.UtcNow;
            entity.SyncedToCloud = false;
            entity.AssignedTrucks.Clear();          

            //remove modules no longer on list
            foreach (var module in modulesToRemove)
            {
                entity.AssignedModules.Remove(module);
            }

            var modulesAlreadyOnList = new List<ModuleEntity>();
            var moduleIdsToAdd = new List<string>();
            foreach(var updatedModule in entity.AssignedModules.Where(x => AssignedModuleIds.Contains(x.Id)))
            {
                modulesAlreadyOnList.Add(updatedModule);
            }

            //add modules that have been added to list
            foreach(var moduleId in AssignedModuleIds.Where(id => !entity.AssignedModules.Any(x => x.Id == id)))
            {                
                moduleIdsToAdd.Add(moduleId);
            }

            var modulesToAdd = _context.Modules.Where(m => moduleIdsToAdd.Contains(m.Id)).ToList();
            modulesToAdd.ForEach(x => x.SyncedToCloud = false);
            entity.AssignedModules.AddRange(modulesToAdd);            
            entity.AssignedTrucks.AddRange(_context.Truck.Where(t => AssignedTruckIds.Contains(t.Id)));
        }

        public override void BulkDelete(IEnumerable<PickupListEntity> entities)
        {
            StringBuilder sqlQuery = new StringBuilder();
            List<string> pickupListIds = entities.Select(t => t.Id).Distinct().ToList();
            //var modulesOnList = _context.Modules.Where(m => pickupListIds.Contains(m.PickupListId)).ToList();
            //var docToProcessIds = _context.DocumentsToProcess.Where(p => p.EntityType == EntityType.MODULE).Select(t => t.Id).ToList();

            foreach (var list in entities)
            {
                var parm = new SqlParameter("listId", list.Id);
                parm.DbType = System.Data.DbType.String;
                sqlQuery.Append("INSERT INTO DocumentToProcesses (EntityType, Source, SelfLink, Name, Id, Action, Created, SyncedToCloud) SELECT EntityType, 1 as [Source], '' as [SelfLink], Name, Id, 1 as [Action], getdate() as [Created], 0 as [SyncedToCloud] FROM ModuleEntities WHERE PickupListId=@listId;");
                sqlQuery.Append("INSERT INTO DocumentToProcesses (EntityType, Source, SelfLink, Name, Id, Action, Created, SyncedToCloud) SELECT EntityType, 1 as [Source], '' as [SelfLink], Name, Id, 1 as [Action], getdate() as [Created], 0 as [SyncedToCloud] FROM PickupListEntities WHERE Id=@listId;");


                sqlQuery.Append("UPDATE ModuleEntities SET PickupListId=null WHERE PickupListId=@listId;");


                sqlQuery.Append("DELETE PickupListEntities WHERE Id=@listId;");
                _context.Database.ExecuteSqlCommand(sqlQuery.ToString(), parm);
            }  
        }

        public void BulkDeleteListAndModules(IEnumerable<PickupListEntity> entities)
        {
            StringBuilder sqlQuery = new StringBuilder();
            List<string> pickupListIds = entities.Select(t => t.Id).Distinct().ToList();
            //var modulesOnList = _context.Modules.Where(m => pickupListIds.Contains(m.PickupListId)).ToList();
            //var docToProcessIds = _context.DocumentsToProcess.Where(p => p.EntityType == EntityType.MODULE).Select(t => t.Id).ToList();

            foreach (var list in entities)
            {
                var parm = new SqlParameter("listId", list.Id);
                parm.DbType = System.Data.DbType.String;
                sqlQuery.Append("INSERT INTO DocumentToProcesses (EntityType, Source, SelfLink, Name, Id, Action, Created, SyncedToCloud) SELECT EntityType, 1 as [Source], '' as [SelfLink], Name, Id, 1 as [Action], getdate() as [Created], 0 as [SyncedToCloud] FROM ModuleEntities WHERE PickupListId=@listId;");
                sqlQuery.Append("INSERT INTO DocumentToProcesses (EntityType, Source, SelfLink, Name, Id, Action, Created, SyncedToCloud) SELECT EntityType, 1 as [Source], '' as [SelfLink], Name, Id, 1 as [Action], getdate() as [Created], 0 as [SyncedToCloud] FROM PickupListEntities WHERE Id=@listId;");

                sqlQuery.Append("DELETE ModuleEntities WHERE PickupListId=@listId;");

                sqlQuery.Append("DELETE PickupListEntities WHERE Id=@listId;");
                _context.Database.ExecuteSqlCommand(sqlQuery.ToString(), parm);
            }
        }
    }
}
