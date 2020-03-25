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
    public class BaleRepository : EntityRepository<BaleEntity>, IBaleRepository
    {
        public BaleRepository(AppDBContext context) : base(context)
        {

        }

        private IOrderedQueryable<BaleEntity> GetFilteredQuery(BalesFilter filter)
        {          
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

            var filteredQuery = _context.Bales.AsQueryable();

            if (!string.IsNullOrEmpty(filter.PBINumber))
                filteredQuery = filteredQuery.Where(c => c.PbiNumber == filter.PBINumber);

            if (filter.StartDate.HasValue)
                filteredQuery = filteredQuery.Where(c => c.Created >= start.Value);

            if (filter.EndDate.HasValue)
                filteredQuery = filteredQuery.Where(c => c.Created <= end.Value);            

            IOrderedQueryable<BaleEntity> orderable = null;

            if (filter.Sort1Ascending)
            {
                if (filter.SortCol1.ToLower() == "pbi number") orderable = filteredQuery.OrderBy(m => m.PbiNumber);
                else if (filter.SortCol1.ToLower() == "weight from scale") orderable = filteredQuery.OrderBy(m => m.WeightFromScale);
                else if (filter.SortCol1.ToLower() == "tare weight") orderable = filteredQuery.OrderBy(m => m.TareWeight);
                else if (filter.SortCol1.ToLower() == "gin ticket load number") orderable = filteredQuery.OrderBy(m => m.GinTicketLoadNumber);
                else if (filter.SortCol1.ToLower() == "estimated seed weight") orderable = filteredQuery.OrderBy(m => m.Classing_EstimatedSeedWeight);                
                else if (filter.SortCol1.ToLower() == "created") orderable = filteredQuery.OrderBy(m => m.Created);
                else if (filter.SortCol1.ToLower() == "updated") orderable = filteredQuery.OrderBy(m => m.Updated);
            }
            else
            {
                if (filter.SortCol1.ToLower() == "pbi number") orderable = filteredQuery.OrderByDescending(m => m.PbiNumber);
                else if (filter.SortCol1.ToLower() == "weight from scale") orderable = filteredQuery.OrderByDescending(m => m.WeightFromScale);
                else if (filter.SortCol1.ToLower() == "tare weight") orderable = filteredQuery.OrderByDescending(m => m.TareWeight);
                else if (filter.SortCol1.ToLower() == "gin ticket load number") orderable = filteredQuery.OrderByDescending(m => m.GinTicketLoadNumber);
                else if (filter.SortCol1.ToLower() == "estimated seed weight") orderable = filteredQuery.OrderByDescending(m => m.Classing_EstimatedSeedWeight);
                else if (filter.SortCol1.ToLower() == "created") orderable = filteredQuery.OrderByDescending(m => m.Created);
                else if (filter.SortCol1.ToLower() == "updated") orderable = filteredQuery.OrderByDescending(m => m.Updated);
            }

            return orderable;
        }

        public PagedResult<BaleEntity> GetBales(BalesFilter filter, int pageSize, int pageNo)
        {
            try
            {
                var filteredQuery = GetFilteredQuery(filter);
                var countQuery = GetFilteredQuery(filter);

                var cResult = new PagedResult<BaleEntity>();

                cResult.Total = countQuery.Count();
                cResult.TotalPages = cResult.Total / pageSize;
                if (cResult.Total % pageSize > 0) cResult.TotalPages++;
                cResult.LastPageNo = pageNo;
                cResult.ResultData = new List<BaleEntity>();
                if (pageNo <= cResult.TotalPages)
                {
                    cResult.ResultData.AddRange(filteredQuery.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToList());
                }
                return cResult;
            }
            catch (Exception exc)
            {
                Logging.Logger.Log(exc);
                throw new Exception("Error fetching bales.", exc);
            }
        }


        public int LastScanNumber()
        {
            var result = _context.Bales.OrderByDescending(m => m.ScanNumber).FirstOrDefault();

            if (result == null)
                return 0;
            else
                return result.ScanNumber;
        }
    }
}
