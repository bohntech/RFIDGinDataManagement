//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.EF.Repositories;
using CottonDBMS.Data.EF;
using GalaSoft.MvvmLight.Messaging;

namespace CottonDBMS.EF.Repositories
{
    public class ModuleRepository : EntityRepository<ModuleEntity>, IModuleRepository
    {
        private IOrderedQueryable<ModuleEntity> GetFilteredQuery(ModuleFilter filter)
        {
            string farmName = filter.Farm.Trim();
            string clientName = filter.Client.Trim();
            string fieldName = filter.Field.Trim();

            DateTime? start = null;
            if (filter.StartDate.HasValue)
            {
                start = filter.StartDate.Value;
            }

            DateTime? end = null;
            if (filter.EndDate.HasValue)
            {
                end = filter.EndDate.Value;
            }

            bool hasStatus = filter.Status.HasValue;
            ModuleStatus status = ModuleStatus.IN_FIELD;
            if (hasStatus) status = filter.Status.Value;

            var filteredQuery = _context.Modules.Include("Field.Farm.Client").AsQueryable();

            if (!string.IsNullOrEmpty(filter.SerialNumber))
                filteredQuery = filteredQuery.Where(c => c.Name == filter.SerialNumber);

            if (filter.StartDate.HasValue)
                filteredQuery = filteredQuery.Where(c => c.Created >= start.Value);

            if (filter.EndDate.HasValue)
                filteredQuery = filteredQuery.Where(c => c.Created <= end.Value);

            if (!string.IsNullOrEmpty(fieldName))
                filteredQuery = filteredQuery.Where(c => c.Field.Name == fieldName);

            if (!string.IsNullOrEmpty(farmName))
                filteredQuery = filteredQuery.Where(c => c.Field.Farm.Name == farmName);

            if (!string.IsNullOrEmpty(clientName))
                filteredQuery = filteredQuery.Where(c => c.Field.Farm.Client.Name == clientName);

            if (!string.IsNullOrEmpty(filter.LoadNumber))
                filteredQuery = filteredQuery.Where(c => c.LoadNumber == filter.LoadNumber);

            if (!string.IsNullOrEmpty(filter.TruckID))
                filteredQuery = filteredQuery.Where(c => c.TruckID == filter.TruckID);

            if (!string.IsNullOrEmpty(filter.Driver))
                filteredQuery = filteredQuery.Where(c => c.Driver == filter.Driver);

            if (filter.Status.HasValue)
                filteredQuery = filteredQuery.Where(c => c.ModuleStatus == status);

            IOrderedQueryable<ModuleEntity> orderable = null;

            if (filter.Sort1Ascending)
            {
                if (filter.SortCol1.ToLower() == "client") orderable = filteredQuery.OrderBy(m => m.Field.Farm.Client.Name);
                else if (filter.SortCol1.ToLower() == "farm") orderable = filteredQuery.OrderBy(m => m.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") orderable = filteredQuery.OrderBy(m => m.Field.Name);
                else if (filter.SortCol1.ToLower() == "serial no") orderable = filteredQuery.OrderBy(m => m.Name);
                else if (filter.SortCol1.ToLower() == "load #") orderable = filteredQuery.OrderBy(m => m.LoadNumber);
                else if (filter.SortCol1.ToLower() == "imported load #") orderable = filteredQuery.OrderBy(m => m.ImportedLoadNumber);
                else if (filter.SortCol1.ToLower() == "truck id") orderable = filteredQuery.OrderBy(m => m.TruckID);
                else if (filter.SortCol1.ToLower() == "driver") orderable = filteredQuery.OrderBy(m => m.Driver);
                else if (filter.SortCol1.ToLower() == "status") orderable = filteredQuery.OrderBy(m => m.ModuleStatus);
                else if (filter.SortCol1.ToLower() == "timestamp") orderable = filteredQuery.OrderBy(m => m.Created);
            }
            else
            {
                if (filter.SortCol1.ToLower() == "client") orderable = filteredQuery.OrderByDescending(m => m.Field.Farm.Client.Name);
                else if (filter.SortCol1.ToLower() == "farm") orderable = filteredQuery.OrderByDescending(m => m.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") orderable = filteredQuery.OrderByDescending(m => m.Field.Name);
                else if (filter.SortCol1.ToLower() == "serial no") orderable = filteredQuery.OrderByDescending(m => m.Name);
                else if (filter.SortCol1.ToLower() == "load #") orderable = filteredQuery.OrderByDescending(m => m.LoadNumber);
                else if (filter.SortCol1.ToLower() == "imported load #") orderable = filteredQuery.OrderByDescending(m => m.ImportedLoadNumber);
                else if (filter.SortCol1.ToLower() == "truck id") orderable = filteredQuery.OrderByDescending(m => m.TruckID);
                else if (filter.SortCol1.ToLower() == "driver") orderable = filteredQuery.OrderByDescending(m => m.Driver);
                else if (filter.SortCol1.ToLower() == "status") orderable = filteredQuery.OrderByDescending(m => m.ModuleStatus);
                else if (filter.SortCol1.ToLower() == "timestamp") orderable = filteredQuery.OrderByDescending(m => m.Created);
            }

            return orderable;
        }

        private IQueryable<ModuleHistoryEntity> GetFilteredEventQuery(ModuleFilter filter)
        {
            string farmName = filter.Farm.Trim();
            string clientName = filter.Client.Trim();
            string fieldName = filter.Field.Trim();

            DateTime? start = null;
            if (filter.StartDate.HasValue)
            {
                start = filter.StartDate.Value;
            }

            DateTime? end = null;
            if (filter.EndDate.HasValue)
            {
                end = filter.EndDate.Value;
            }

            bool hasStatus = filter.Status.HasValue;
            ModuleStatus status = ModuleStatus.IN_FIELD;
            if (hasStatus) status = filter.Status.Value;
            
            var filteredQuery = _context.ModuleHistory.Include("Module.Field.Farm.Client").AsQueryable();

            if (!string.IsNullOrEmpty(filter.SerialNumber))
                filteredQuery = filteredQuery.Where(c => c.Module.Name == filter.SerialNumber);

            if (filter.StartDate.HasValue)
                filteredQuery = filteredQuery.Where(c => c.Created >= start.Value);

            if (filter.EndDate.HasValue)
                filteredQuery = filteredQuery.Where(c => c.Created <= end.Value);

            if (!string.IsNullOrEmpty(fieldName))
                filteredQuery = filteredQuery.Where(c => c.Module.Field.Name == fieldName);

            if (!string.IsNullOrEmpty(farmName))
                filteredQuery = filteredQuery.Where(c => c.Module.Field.Farm.Name == farmName);

            if (!string.IsNullOrEmpty(clientName))
                filteredQuery = filteredQuery.Where(c => c.Module.Field.Farm.Client.Name == clientName);

            if (!string.IsNullOrEmpty(filter.LoadNumber))
                filteredQuery = filteredQuery.Where(c => c.Module.LoadNumber == filter.LoadNumber);

            if (!string.IsNullOrEmpty(filter.TruckID))
                filteredQuery = filteredQuery.Where(c => c.TruckID == filter.TruckID);

            if (!string.IsNullOrEmpty(filter.Driver))
                filteredQuery = filteredQuery.Where(c => c.Driver == filter.Driver);

            if (filter.Status.HasValue)
                filteredQuery = filteredQuery.Where(c => c.ModuleStatus == status);

            IOrderedQueryable<ModuleHistoryEntity> orderable = null;

            if (filter.Sort1Ascending)
            {
                if (filter.SortCol1.ToLower() == "client") orderable = filteredQuery.OrderBy(m => m.Module.Field.Farm.Client.Name);
                else if (filter.SortCol1.ToLower() == "farm") orderable = filteredQuery.OrderBy(m => m.Module.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") orderable = filteredQuery.OrderBy(m => m.Module.Field.Name);
                else if (filter.SortCol1.ToLower() == "serial no") orderable = filteredQuery.OrderBy(m => m.Module.Name);
                else if (filter.SortCol1.ToLower() == "load #") orderable = filteredQuery.OrderBy(m => m.Module.LoadNumber);
                else if (filter.SortCol1.ToLower() == "truck id") orderable = filteredQuery.OrderBy(m => m.TruckID);
                else if (filter.SortCol1.ToLower() == "driver") orderable = filteredQuery.OrderBy(m => m.Driver);
                else if (filter.SortCol1.ToLower() == "status") orderable = filteredQuery.OrderBy(m => m.ModuleStatus);
                else if (filter.SortCol1.ToLower() == "timestamp") orderable = filteredQuery.OrderBy(m => m.Created);
            }
            else
            {
                if (filter.SortCol1.ToLower() == "client") orderable = filteredQuery.OrderByDescending(m => m.Module.Field.Farm.Client.Name);
                else if (filter.SortCol1.ToLower() == "farm") orderable = filteredQuery.OrderByDescending(m => m.Module.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") orderable = filteredQuery.OrderByDescending(m => m.Module.Field.Name);
                else if (filter.SortCol1.ToLower() == "serial no") orderable = filteredQuery.OrderByDescending(m => m.Module.Name);
                else if (filter.SortCol1.ToLower() == "load #") orderable = filteredQuery.OrderByDescending(m => m.Module.LoadNumber);
                else if (filter.SortCol1.ToLower() == "truck id") orderable = filteredQuery.OrderByDescending(m => m.TruckID);
                else if (filter.SortCol1.ToLower() == "driver") orderable = filteredQuery.OrderByDescending(m => m.Driver);
                else if (filter.SortCol1.ToLower() == "status") orderable = filteredQuery.OrderByDescending(m => m.ModuleStatus);
                else if (filter.SortCol1.ToLower() == "timestamp") orderable = filteredQuery.OrderByDescending(m => m.Created);
            }

            if (filter.Sort2Ascending)
            {
                if (filter.SortCol2.ToLower() == "client") orderable = orderable.ThenBy(m => m.Module.Field.Farm.Client.Name);
                else if (filter.SortCol2.ToLower() == "farm") orderable = orderable.ThenBy(m => m.Module.Field.Farm.Name);
                else if (filter.SortCol2.ToLower() == "field") orderable = orderable.ThenBy(m => m.Module.Field.Name);
                else if (filter.SortCol2.ToLower() == "serial no") orderable = orderable.ThenBy(m => m.Module.Name);
                else if (filter.SortCol2.ToLower() == "load #") orderable = orderable.ThenBy(m => m.Module.LoadNumber);
                else if (filter.SortCol2.ToLower() == "truck id") orderable = orderable.ThenBy(m => m.TruckID);
                else if (filter.SortCol2.ToLower() == "driver") orderable = orderable.ThenBy(m => m.Driver);
                else if (filter.SortCol2.ToLower() == "status") orderable = orderable.ThenBy(m => m.ModuleStatus);
                else if (filter.SortCol2.ToLower() == "timestamp") orderable = orderable.ThenBy(m => m.Created);
            }
            else
            {
                if (filter.SortCol2.ToLower() == "client") orderable = orderable.ThenByDescending(m => m.Module.Field.Farm.Client.Name);
                else if (filter.SortCol2.ToLower() == "farm") orderable = orderable.ThenByDescending(m => m.Module.Field.Farm.Name);
                else if (filter.SortCol2.ToLower() == "field") orderable = orderable.ThenByDescending(m => m.Module.Field.Name);
                else if (filter.SortCol2.ToLower() == "serial no") orderable = orderable.ThenByDescending(m => m.Module.Name);
                else if (filter.SortCol2.ToLower() == "load #") orderable = orderable.ThenByDescending(m => m.Module.LoadNumber);
                else if (filter.SortCol2.ToLower() == "truck id") orderable = orderable.ThenByDescending(m => m.TruckID);
                else if (filter.SortCol2.ToLower() == "driver") orderable = orderable.ThenByDescending(m => m.Driver);
                else if (filter.SortCol2.ToLower() == "status") orderable = orderable.ThenByDescending(m => m.ModuleStatus);
                else if (filter.SortCol2.ToLower() == "timestamp") orderable = orderable.ThenByDescending(m => m.Created);
            }

            if (filter.Sort3Ascending)
            {
                if (filter.SortCol3.ToLower() == "client") orderable = orderable.ThenBy(m => m.Module.Field.Farm.Client.Name);
                else if (filter.SortCol3.ToLower() == "farm") orderable = orderable.ThenBy(m => m.Module.Field.Farm.Name);
                else if (filter.SortCol3.ToLower() == "field") orderable = orderable.ThenBy(m => m.Module.Field.Name);
                else if (filter.SortCol3.ToLower() == "serial no") orderable = orderable.ThenBy(m => m.Module.Name);
                else if (filter.SortCol3.ToLower() == "load #") orderable = orderable.ThenBy(m => m.Module.LoadNumber);
                else if (filter.SortCol3.ToLower() == "truck id") orderable = orderable.ThenBy(m => m.TruckID);
                else if (filter.SortCol3.ToLower() == "driver") orderable = orderable.ThenBy(m => m.Driver);
                else if (filter.SortCol3.ToLower() == "status") orderable = orderable.ThenBy(m => m.ModuleStatus);
                else if (filter.SortCol3.ToLower() == "timestamp") orderable = orderable.ThenBy(m => m.Created);
            }
            else
            {
                if (filter.SortCol3.ToLower() == "client") orderable = orderable.ThenByDescending(m => m.Module.Field.Farm.Client.Name);
                else if (filter.SortCol3.ToLower() == "farm") orderable = orderable.ThenByDescending(m => m.Module.Field.Farm.Name);
                else if (filter.SortCol3.ToLower() == "field") orderable = orderable.ThenByDescending(m => m.Module.Field.Name);
                else if (filter.SortCol3.ToLower() == "serial no") orderable = orderable.ThenByDescending(m => m.Module.Name);
                else if (filter.SortCol3.ToLower() == "load #") orderable = orderable.ThenByDescending(m => m.Module.LoadNumber);
                else if (filter.SortCol3.ToLower() == "truck id") orderable = orderable.ThenByDescending(m => m.TruckID);
                else if (filter.SortCol3.ToLower() == "driver") orderable = orderable.ThenByDescending(m => m.Driver);
                else if (filter.SortCol3.ToLower() == "status") orderable = orderable.ThenByDescending(m => m.ModuleStatus);
                else if (filter.SortCol3.ToLower() == "timestamp") orderable = orderable.ThenByDescending(m => m.Created);
            }

            return orderable;
        }

        public ModuleRepository(AppDBContext context) : base(context)
        {

        }

        public void ClearGinModuleData()
        {
            //delete            
            _context.Database.ExecuteSqlCommand("DELETE FROM ModuleHistoryEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM ModuleEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListAssignedTrucks");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListDownloadedToTrucks");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM AggregateEvents");
        }

        public PagedResult<ModuleEntity> GetModules(ModuleFilter filter, int pageSize, int pageNo)
        {
            try
            {
                var filteredQuery = GetFilteredQuery(filter);
                var countQuery = GetFilteredQuery(filter);

                var cResult = new PagedResult<ModuleEntity>();

                cResult.Total = countQuery.Count();
                cResult.TotalPages = cResult.Total / pageSize;
                if (cResult.Total % pageSize > 0) cResult.TotalPages++;
                cResult.LastPageNo = pageNo;
                cResult.ResultData = new List<ModuleEntity>();
                if (pageNo <= cResult.TotalPages)
                {
                    cResult.ResultData.AddRange(filteredQuery.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList());
                }
                return cResult;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error fetching modules.", exc);
            }
        }

        public IEnumerable<ModuleEntity> GetAllMatchingModules(ModuleFilter filter)
        {
            try
            {
                var orderable = GetFilteredQuery(filter);
                if (filter.Sort2Ascending)
                {
                    if (filter.SortCol2.ToLower() == "client") orderable = orderable.ThenBy(m => m.Field.Farm.Client.Name);
                    else if (filter.SortCol2.ToLower() == "farm") orderable = orderable.ThenBy(m => m.Field.Farm.Name);
                    else if (filter.SortCol2.ToLower() == "field") orderable = orderable.ThenBy(m => m.Field.Name);
                    else if (filter.SortCol2.ToLower() == "serial no") orderable = orderable.ThenBy(m => m.Name);
                    else if (filter.SortCol2.ToLower() == "load #") orderable = orderable.ThenBy(m => m.LoadNumber);
                    else if (filter.SortCol2.ToLower() == "truck id") orderable = orderable.ThenBy(m => m.TruckID);
                    else if (filter.SortCol2.ToLower() == "driver") orderable = orderable.ThenBy(m => m.Driver);
                    else if (filter.SortCol2.ToLower() == "status") orderable = orderable.ThenBy(m => m.ModuleStatus);
                    else if (filter.SortCol2.ToLower() == "timestamp") orderable = orderable.ThenBy(m => m.Created);
                }
                else
                {
                    if (filter.SortCol2.ToLower() == "client") orderable = orderable.ThenByDescending(m => m.Field.Farm.Client.Name);
                    else if (filter.SortCol2.ToLower() == "farm") orderable = orderable.ThenByDescending(m => m.Field.Farm.Name);
                    else if (filter.SortCol2.ToLower() == "field") orderable = orderable.ThenByDescending(m => m.Field.Name);
                    else if (filter.SortCol2.ToLower() == "serial no") orderable = orderable.ThenByDescending(m => m.Name);
                    else if (filter.SortCol2.ToLower() == "load #") orderable = orderable.ThenByDescending(m => m.LoadNumber);
                    else if (filter.SortCol2.ToLower() == "truck id") orderable = orderable.ThenByDescending(m => m.TruckID);
                    else if (filter.SortCol2.ToLower() == "driver") orderable = orderable.ThenByDescending(m => m.Driver);
                    else if (filter.SortCol2.ToLower() == "status") orderable = orderable.ThenByDescending(m => m.ModuleStatus);
                    else if (filter.SortCol2.ToLower() == "timestamp") orderable = orderable.ThenByDescending(m => m.Created);
                }

                if (filter.Sort3Ascending)
                {
                    if (filter.SortCol3.ToLower() == "client") orderable = orderable.ThenBy(m => m.Field.Farm.Client.Name);
                    else if (filter.SortCol3.ToLower() == "farm") orderable = orderable.ThenBy(m => m.Field.Farm.Name);
                    else if (filter.SortCol3.ToLower() == "field") orderable = orderable.ThenBy(m => m.Field.Name);
                    else if (filter.SortCol3.ToLower() == "serial no") orderable = orderable.ThenBy(m => m.Name);
                    else if (filter.SortCol3.ToLower() == "load #") orderable = orderable.ThenBy(m => m.LoadNumber);
                    else if (filter.SortCol3.ToLower() == "truck id") orderable = orderable.ThenBy(m => m.TruckID);
                    else if (filter.SortCol3.ToLower() == "driver") orderable = orderable.ThenBy(m => m.Driver);
                    else if (filter.SortCol3.ToLower() == "status") orderable = orderable.ThenBy(m => m.ModuleStatus);
                    else if (filter.SortCol3.ToLower() == "timestamp") orderable = orderable.ThenBy(m => m.Created);
                }
                else
                {
                    if (filter.SortCol3.ToLower() == "client") orderable = orderable.ThenByDescending(m => m.Field.Farm.Client.Name);
                    else if (filter.SortCol3.ToLower() == "farm") orderable = orderable.ThenByDescending(m => m.Field.Farm.Name);
                    else if (filter.SortCol3.ToLower() == "field") orderable = orderable.ThenByDescending(m => m.Field.Name);
                    else if (filter.SortCol3.ToLower() == "serial no") orderable = orderable.ThenByDescending(m => m.Name);
                    else if (filter.SortCol3.ToLower() == "load #") orderable = orderable.ThenByDescending(m => m.LoadNumber);
                    else if (filter.SortCol3.ToLower() == "truck id") orderable = orderable.ThenByDescending(m => m.TruckID);
                    else if (filter.SortCol3.ToLower() == "driver") orderable = orderable.ThenByDescending(m => m.Driver);
                    else if (filter.SortCol3.ToLower() == "status") orderable = orderable.ThenByDescending(m => m.ModuleStatus);
                    else if (filter.SortCol3.ToLower() == "timestamp") orderable = orderable.ThenByDescending(m => m.Created);
                }

                return orderable.ToList();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error fetching modules.", exc);
            }
        }

        public IEnumerable<ModuleHistoryEntity> GetAllMatchingLocations(ModuleFilter filter)
        {
            return this.GetFilteredEventQuery(filter).ToList();
        }

        public IEnumerable<ModulePoint> GetModulePoints(ModuleFilter filter)
        {
            try
            {
                var filteredQuery = GetFilteredQuery(filter);              
                return filteredQuery.Select(m => new ModulePoint { Id = m.Id, SerialNumber = m.Name, Latitude = m.Latitude, Longitude = m.Longitude, ModuleEvent = ModuleEventType.IMPORTED_FROM_FILE, ModuleStatus = m.ModuleStatus }).ToList();              
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error fetching modules.", exc);
            }
        }

        public IEnumerable<ModulePoint> GetModulePointHistory(ModuleFilter filter)
        {
            try
            {
                var filteredQuery =  this.GetFilteredEventQuery(filter);
                return filteredQuery.Select(m => new ModulePoint { Id = m.Module.Id, SerialNumber = m.Module.Name, Latitude = m.Latitude, Longitude = m.Longitude, ModuleEvent = ModuleEventType.IMPORTED_FROM_FILE, ModuleStatus = m.ModuleStatus }).ToList();
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error fetching modules.", exc);
            }
        }


        public bool CanSaveModule(string id, string serialNumber)
        {
            if (String.IsNullOrWhiteSpace(serialNumber))
            {
                return false;
            }

            return !_context.Modules.Any(m => m.Name == serialNumber && m.Id != id);
        }

        public bool ModuleIDExists(string id, string moduleID)
        {
            if (String.IsNullOrWhiteSpace(moduleID))
            {
                return false;
            }

            return _context.Modules.Any(m => m.ModuleId == moduleID && m.Id != id);
        }

        public string GetNextManualLoadNumber(string prefix, int pad)
        {
            var lastSN = _context.Modules.Where(m => m.LoadNumber.StartsWith(prefix)).OrderBy(m => m.LoadNumber).Select(m => m.LoadNumber).Take(1).ToList().LastOrDefault();

            int number = 1;

            if (!string.IsNullOrEmpty(lastSN))
            {
                number = int.Parse(lastSN.Replace(prefix, ""));
                number++;
            }

            return string.Format("{0}{1}", prefix, number.ToString().PadLeft(pad, '0'));
        }

        public int GetLoadCountByStatus(ModuleStatus status, int modulesPerLoad)
        {
            //get count of unique assigned load numbers
            int numberedLoads = _context.Modules.Where(m => m.ModuleStatus == status && m.LoadNumber != "" && m.LoadNumber != null).Select(m => m.LoadNumber).Distinct().Count();

            int unassignedModules = _context.Modules.Where(m => m.ModuleStatus == status && (m.LoadNumber == "" || m.LoadNumber == null)).Count();

            int unassignedLoads = unassignedModules / modulesPerLoad;

            if (unassignedModules % modulesPerLoad > 0) unassignedLoads++;

            return numberedLoads + unassignedLoads;
        }

        public int GetModuleCountByStatus(ModuleStatus status)
        {
            //get count of unique assigned load numbers
            return _context.Modules.Count(m => m.ModuleStatus == status);
        }

        public int GetTotalModuleCount() {             
            return _context.Modules.Count();
        }

        public override void Delete(ModuleEntity entity)
        {
            var item = _context.Set<ModuleEntity>().FirstOrDefault(t => t.Id == entity.Id);  
            _context.Entry(item).State = EntityState.Deleted;
        }
       

        public List<string> GetAllSerialNumbers()
        {
            return _context.Modules.Select(m => m.Name).ToList();
        }
    }
}
