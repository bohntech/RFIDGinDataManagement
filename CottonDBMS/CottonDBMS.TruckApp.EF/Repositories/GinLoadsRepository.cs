//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.Data.EF;
using System.Data.Entity;

namespace CottonDBMS.EF.Repositories
{
    public class GinLoadRepository : EntityRepository<GinLoadEntity>, IGinLoadRepository
    {
        public GinLoadRepository(AppDBContext context) : base(context)
        {

        }

        private IOrderedQueryable<GinLoadEntity> GetFilteredQuery(GinLoadsFilter filter)
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
            
            var filteredQuery = _context.GinLoads.Include("Field.Farm.Client").AsQueryable();

            if (!string.IsNullOrEmpty(filter.BridgeId))
                filteredQuery = filteredQuery.Where(c => c.ScaleBridgeId == filter.BridgeId);

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

            if (!string.IsNullOrEmpty(filter.GinTagLoadNumber))
                filteredQuery = filteredQuery.Where(c => c.GinTagLoadNumber == filter.GinTagLoadNumber);

            if (!string.IsNullOrEmpty(filter.BridgeLoadNumber))
            {
                int temp = 0;

                if (int.TryParse(filter.BridgeLoadNumber, out temp))
                    filteredQuery = filteredQuery.Where(c => c.ScaleBridgeLoadNumber == temp);
            }                       

            IOrderedQueryable<GinLoadEntity> orderable = null;

            if (filter.Sort1Ascending)
            {
                if (filter.SortCol1.ToLower() == "client") orderable = filteredQuery.OrderBy(m => m.Field.Farm.Client.Name);
                else if (filter.SortCol1.ToLower() == "farm") orderable = filteredQuery.OrderBy(m => m.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") orderable = filteredQuery.OrderBy(m => m.Field.Name);
                else if (filter.SortCol1.ToLower() == "scale bridge id") orderable = filteredQuery.OrderBy(m => m.Name);
                else if (filter.SortCol1.ToLower() == "gin tag load #") orderable = filteredQuery.OrderBy(m => m.GinTagLoadNumber);
                else if (filter.SortCol1.ToLower() == "scale bridge load #") orderable = filteredQuery.OrderBy(m => m.ScaleBridgeLoadNumber);                
                else if (filter.SortCol1.ToLower() == "timestamp") orderable = filteredQuery.OrderBy(m => m.Created);
                else if (filter.SortCol1.ToLower() == "created") orderable = filteredQuery.OrderBy(m => m.Created);
                else if (filter.SortCol1.ToLower() == "updated") orderable = filteredQuery.OrderBy(m => m.Updated);
            }
            else
            {
                if (filter.SortCol1.ToLower() == "client") orderable = filteredQuery.OrderByDescending(m => m.Field.Farm.Client.Name);
                else if (filter.SortCol1.ToLower() == "farm") orderable = filteredQuery.OrderByDescending(m => m.Field.Farm.Name);
                else if (filter.SortCol1.ToLower() == "field") orderable = filteredQuery.OrderByDescending(m => m.Field.Name);
                else if (filter.SortCol1.ToLower() == "scale bridge id") orderable = filteredQuery.OrderByDescending(m => m.Name);
                else if (filter.SortCol1.ToLower() == "gin tag load #") orderable = filteredQuery.OrderByDescending(m => m.GinTagLoadNumber);
                else if (filter.SortCol1.ToLower() == "scale bridge load #") orderable = filteredQuery.OrderByDescending(m => m.ScaleBridgeLoadNumber);
                else if (filter.SortCol1.ToLower() == "timestamp") orderable = filteredQuery.OrderByDescending(m => m.Created);
                else if (filter.SortCol1.ToLower() == "created") orderable = filteredQuery.OrderByDescending(m => m.Created);
                else if (filter.SortCol1.ToLower() == "updated") orderable = filteredQuery.OrderByDescending(m => m.Updated);
            }

            return orderable;
        }

        public PagedResult<GinLoadEntity> GetLoads(GinLoadsFilter filter, int pageSize, int pageNo)
        {
            try
            {
                var filteredQuery = GetFilteredQuery(filter);
                var countQuery = GetFilteredQuery(filter);

                var cResult = new PagedResult<GinLoadEntity>();

                cResult.Total = countQuery.Count();
                cResult.TotalPages = cResult.Total / pageSize;
                if (cResult.Total % pageSize > 0) cResult.TotalPages++;
                cResult.LastPageNo = pageNo;
                cResult.ResultData = new List<GinLoadEntity>();
                if (pageNo <= cResult.TotalPages)
                {
                    cResult.ResultData.AddRange(filteredQuery.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList());
                }
                return cResult;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error fetching gin loads.", exc);
            }
        }

        public bool CanSaveLoadNumber(string id, string loadNumber)
        {
            int temp = 0;
            if (String.IsNullOrWhiteSpace(loadNumber))
            {
                return false;
            }
            else if (int.TryParse(loadNumber, out temp))
            {
                return !_context.GinLoads.Any(m => m.ScaleBridgeLoadNumber == temp && m.Id != id);
            }
            else
            {
                return false;
            }
        }

        public bool CanGinTagLoadNumber(string id, string loadNumber)
        {            
            if (String.IsNullOrWhiteSpace(loadNumber))
                return false;
            else 
                return !_context.GinLoads.Any(m => m.GinTagLoadNumber == loadNumber && m.Id != id);
        }

        public int LastLoadNumber()
        {
            var lastLoad = _context.GinLoads.OrderByDescending(x => x.ScaleBridgeLoadNumber).FirstOrDefault();
            return (lastLoad != null) ? lastLoad.ScaleBridgeLoadNumber : 1;
        }

        public IEnumerable<string> GetUndeletableIds()
        {
            return new List<string>();
        }

        public string GetLoadIdForGinTicketNumber(string ginTicketLoadNumber)
        {
            var item = _context.GinLoads.SingleOrDefault(l => l.GinTagLoadNumber == ginTicketLoadNumber);

            return (item == null) ? null : item.Id;
        }

        public override void Save(GinLoadEntity entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                entity.SyncedToCloud = false;
                entity.Created = DateTime.UtcNow;
                this.Add(entity);
            }
            else
            {
                entity.Updated = DateTime.UtcNow;
                entity.SyncedToCloud = false;
                this.Update(entity);
            }

            //check for modules with this id and update gin ticket load #
            var modules = _context.Modules.Where(m => m.GinLoadId == entity.Id).ToList();
            foreach (var module in modules)
            {
                module.GinTagLoadNumber = entity.GinTagLoadNumber;                
            }

            //check for modules with gin tag number but no id
            var unLinkedModules = _context.Modules.Where(m => m.GinTagLoadNumber == entity.GinTagLoadNumber && string.IsNullOrEmpty(m.GinLoadId));
            foreach(var m in unLinkedModules)
            {
                m.GinLoadId = entity.Id;                
            }

        }

        /// <summary>
        /// Marks an entity for removal on next save
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(GinLoadEntity entity)
        {
            var item = _context.Set<GinLoadEntity>().FirstOrDefault(t => t.Id == entity.Id);

            var affectedModules = _context.Set<ModuleEntity>().Where(module => module.GinLoadId == entity.Id).ToList();
            var affectedBales = _context.Set<BaleEntity>().Where(bale => bale.GinLoadId == entity.Id).ToList();
            foreach(var m in affectedModules)
            {
                m.GinTagLoadNumber = null;
                m.GinLoadId = null;
                _context.Entry(m).State = EntityState.Modified;
            }

            foreach(var b in affectedBales)
            {
                b.GinLoadId = null;
                b.GinTicketLoadNumber = null;
                _context.Entry(b).State = EntityState.Modified;
            }

            _context.Entry(item).State = EntityState.Deleted;            

        }
    }
}
