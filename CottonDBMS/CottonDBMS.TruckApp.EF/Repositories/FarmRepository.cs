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
    public class FarmRepository : EntityRepository<FarmEntity>, IFarmRepository
    {
        public FarmRepository(AppDBContext context) : base(context)
        {

        }

        public bool CanSaveFarm(string clientId, string farmId, string Name, bool validateAllIds)
        {

            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (validateAllIds && string.IsNullOrWhiteSpace(clientId))
            {
                return false;
            }

            return !_context.Farms
                    .Any(r => r.Name.ToLower() == Name.ToLower() && r.Id != farmId);             
        }

        public bool CanSaveFarmUniqueToClient(string clientId, string farmId, string Name, bool validateAllIds)
        {

            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (validateAllIds && string.IsNullOrWhiteSpace(clientId))
            {
                return false;
            }

            return !_context.Farms
                    .Any(r => r.Name.ToLower() == Name.ToLower() && r.Id != farmId && r.ClientId == clientId);
        }

        public IEnumerable<string> GetFarmIdsLinkedToFields()
        {
            return _context.Farms.Include("Fields").Where(c => c.Fields.Any()).Select(c => c.Id).ToList();            
        }

        public FarmEntity EnsureFarmCreatedUniqueToClient(ClientEntity existingClient, string farmName, InputSource source)
        {

            var dbClient = _context.Clients.Include("Farms").FirstOrDefault(c => c.Id == existingClient.Id);

            if (dbClient == null)
            {
                throw new Exception("Client required to create farm.");
            }

            FarmEntity existingFarm = dbClient.Farms.FirstOrDefault(f => f.Name == farmName.Trim());
            if (existingFarm == null)
            {
                //create new farm                
                existingFarm = new FarmEntity();
                existingFarm.Name = farmName;
                existingFarm.ClientId = dbClient.Id;
                existingFarm.Source = source;

                var canSaveFarm = this.CanSaveFarmUniqueToClient(dbClient.Id, existingFarm.Id, existingFarm.Name, true);

                if (!canSaveFarm)
                {
                    Logging.Logger.Log("INFO", string.Format("Cannot save farm {0} on client {1}", existingFarm.Name, dbClient.Name));
                    return null;
                }
                else
                {
                    this.Save(existingFarm);
                    return existingFarm;
                }
            }
            else
            {
                return existingFarm;
            }
        }

        public FarmEntity EnsureFarmCreated(ClientEntity existingClient, string farmName, InputSource source)
        {           

            var dbClient = _context.Clients.Include("Farms").FirstOrDefault(c => c.Id == existingClient.Id);

            if (dbClient == null)
            {
                throw new Exception("Client required to create farm.");
            }

            FarmEntity existingFarm = dbClient.Farms.FirstOrDefault(f => f.Name == farmName.Trim());
            if (existingFarm == null)
            {
                //create new farm                
                existingFarm = new FarmEntity();                                                
                existingFarm.Name = farmName;
                existingFarm.ClientId = dbClient.Id;
                existingFarm.Source = source;

                var canSaveFarm = this.CanSaveFarm(dbClient.Id, existingFarm.Id, existingFarm.Name, true);

                if (!canSaveFarm)
                {
                    Logging.Logger.Log("INFO", string.Format("Cannot save farm {0} on client {1}", existingFarm.Name, dbClient.Name));
                    return null;
                }
                else
                {
                    this.Save(existingFarm);
                    return existingFarm;
                }
            }
            else
            {
                return existingFarm;
            }
        }

        public IEnumerable<FarmEntity> GetAllMatchingFarms(string clientName, string farmName)
        {
            return _context.Farms.Include("Client").Include("Fields")
                    .Where(farm => (clientName == "" || farm.Client.Name == clientName)
                    && (farmName.Trim() == "" || farm.Name == farmName.Trim())
                    ).OrderBy(f => f.Name).ToList();
        }
    }
}
