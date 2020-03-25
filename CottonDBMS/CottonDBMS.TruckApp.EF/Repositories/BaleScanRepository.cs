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
    public class BaleScanRepository : EntityRepository<BaleScanEntity>, IBaleScanRepository
    {
        public BaleScanRepository(AppDBContext context) : base(context)
        {

        }

        public List<BaleScanEntity> Last200Scans()
        {
            return _context.BaleScans.OrderByDescending(t => t.Created).Take(200).ToList();
        }

        public int LastScanNumber()
        {
            int lastNumber = 1;
            var lastScan = _context.BaleScans.OrderByDescending(x => x.ScanNumber).FirstOrDefault();
            return lastNumber = (lastScan != null) ? lastScan.ScanNumber : 0;
        }

        public void ClearScanData()
        {
            _context.Database.ExecuteSqlCommand("DELETE FROM DocumentToProcesses");
            _context.Database.ExecuteSqlCommand("DELETE FROM ModuleHistoryEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM ModuleEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListAssignedTrucks");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListDownloadedToTrucks");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM AggregateEvents");
            _context.Database.ExecuteSqlCommand("DELETE FROM LoadScanEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM ModuleOwnerShipEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM TruckRegistrationEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM TruckEntities");            
            _context.Database.ExecuteSqlCommand("DELETE FROM GinLoadEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM BaleEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM BaleScanEntities");
        }

    }
}
