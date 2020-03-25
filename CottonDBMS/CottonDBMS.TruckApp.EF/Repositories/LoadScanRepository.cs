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
    public class LoadScanRepository : EntityRepository<LoadScanEntity>, ILoadScanRepository
    {
        public LoadScanRepository(AppDBContext context) : base(context)
        {

        }

        public int LastLoadNumber()
        {
            int lastLoadNumber = 1;
            var lastLoad = _context.LoadScans.OrderByDescending(x => x.BridgeLoadNumber).FirstOrDefault();
            return lastLoadNumber = (lastLoad != null) ? lastLoad.BridgeLoadNumber : 0;            
        }

        public LoadScanEntity LastLoad()
        {            
            var lastLoad = _context.LoadScans.OrderByDescending(x => x.Created).FirstOrDefault();
            return lastLoad;
        }

        public void ClearBridgeScanData()
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
            _context.Database.ExecuteSqlCommand("DELETE FROM BaleEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM GinLoadEntities");
        }
        
    }
}
