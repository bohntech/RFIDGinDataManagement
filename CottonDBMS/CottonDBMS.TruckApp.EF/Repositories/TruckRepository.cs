//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.Data.EF;
using System.Device.Location;

namespace CottonDBMS.EF.Repositories
{
    public class TruckRepository : EntityRepository<TruckEntity>, ITruckRepository
    {
        

        public TruckRepository(AppDBContext context) : base(context)
        {

        }

        public IEnumerable<string> GetUndeletableIds(IEnumerable<TruckEntity> itemsToDelete)
        {
            var truckIds = itemsToDelete.Select(t => t.Id).ToList();
            var pickupLists = _context.PickupLists.Include("AssignedTrucks").Include("DownloadedToTrucks").ToList();
            List<string> undeletableIds = new List<string>();

            foreach (var tId in truckIds) {
                foreach (var p in pickupLists)
                {
                    if (p.AssignedTrucks.Any(t => t.Id == tId) || p.DownloadedToTrucks.Any(t => t.Id == tId))
                    {
                        undeletableIds.Add(tId);
                    }
                }
            }

            return undeletableIds;
        }

        public void ClearTruckData()
        {   
            _context.Database.ExecuteSqlCommand("DELETE FROM DocumentToProcesses");
            _context.Database.ExecuteSqlCommand("DELETE FROM ModuleHistoryEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM ModuleEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListAssignedTrucks");            
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListDownloadedToTrucks");
            _context.Database.ExecuteSqlCommand("DELETE FROM PickupListEntities");
            _context.Database.ExecuteSqlCommand("DELETE FROM AggregateEvents");            
        }

        public void ClearLoadNumber(string serialNumber)
        {
            var eventsWithSerialNumber = _context.AggregateEvents.Where(t => t.SerialNumber == serialNumber).ToList();

            foreach(var evt in eventsWithSerialNumber)
            {
                //evt.SyncedToCloud = false; //cause this event to get resent
                evt.LoadNumber = "";
            }            
        }

        public void ClearClientFarmFieldData()
        {
            //delete           
            var fieldRepo = new FieldRepository(_context);
            var farmRepo = new FarmRepository(_context);
            var clientRepo = new ClientRepository(_context);
            
            var allFields = fieldRepo.GetAll();
            fieldRepo.BulkDelete(allFields);

            var allFarms = farmRepo.GetAll();
            farmRepo.BulkDelete(allFarms);

            var allClients = clientRepo.GetAll();
            clientRepo.BulkDelete(allClients);

            _context.SaveChanges();
        }

        public string GetLastLoadNumber()
        {
            var truckIDSetting = _context.Settings.SingleOrDefault(t => t.Key == TruckClientSettingKeys.TRUCK_ID);
            var syncedSettings = _context.SyncedSettings.FirstOrDefault();

            var loadPrefix = (syncedSettings != null) ? syncedSettings.LoadPrefix : DateTime.Now.Year.ToString();
            var modulesPerLoad = (syncedSettings != null) ? syncedSettings.ModulesPerLoad : 4;

            if (truckIDSetting != null)
            {
                var truck = _context.Truck.SingleOrDefault(t => t.Id == truckIDSetting.Value);

                if (truck != null)
                {
                    var lastEvent = _context.AggregateEvents.Where(e => e.TruckID == truck.Id && !string.IsNullOrEmpty(e.LoadNumber)).OrderByDescending(t => t.Timestamp).Take(1).FirstOrDefault();
                    if (lastEvent != null)
                    {
                        return lastEvent.LoadNumber;
                    }
                }
            }

            return "";
        }
        

        public string GetNextLoadNumber()
        {
            var truckIDSetting = _context.Settings.SingleOrDefault(t => t.Key == TruckClientSettingKeys.TRUCK_ID);
            var syncedSettings = _context.SyncedSettings.FirstOrDefault();

            var loadPrefix = (syncedSettings != null) ? syncedSettings.LoadPrefix : DateTime.Now.Year.ToString();
            var modulesPerLoad = (syncedSettings != null) ? syncedSettings.ModulesPerLoad : 4;

            if (truckIDSetting != null)
            {
                var allTrucks = _context.Truck.ToList();
                var truck = _context.Truck.SingleOrDefault(t => t.Id == truckIDSetting.Value);
                

                if (truck != null)
                {
                    var lastEvent = _context.AggregateEvents.Where(e => e.TruckID == truck.Id && !string.IsNullOrEmpty(e.LoadNumber)).OrderByDescending(t => t.Timestamp).Take(1).FirstOrDefault();
                    if (lastEvent != null)
                    {
                        string lastNumber = lastEvent.LoadNumber;
                        string numberPart = lastNumber.Replace(loadPrefix+truck.LoadPrefix, "");

                        int number = 0;
                        if (int.TryParse(numberPart, out number))
                        {
                            number++;
                        }

                        return loadPrefix + truck.LoadPrefix + number.ToString();
                    }
                    else
                    {
                        return loadPrefix + truck.LoadPrefix + syncedSettings.StartingLoadNumber.ToString();
                    }
                }                
            }

            throw new Exception("Unable to generate next load number.  Check that truck ID is selected.");
        }

        public bool CanSaveTruckId(string id, string truckid)
        {
            return !_context.Truck.Any(t => t.Id != id && t.Name == truckid);
        }

        public bool CanSaveTruckPrefix(string id, string prefix)
        {
            return !_context.Truck.Any(t => t.Id != id && t.LoadPrefix == prefix);
        }

       
    }
}
