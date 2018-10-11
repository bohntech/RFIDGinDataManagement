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
    public class ClientRepository : EntityRepository<ClientEntity>, IClientRepository
    {
        public ClientRepository(AppDBContext context) : base(context)
        {
        }
        
        public ClientEntity EnsureClientCreated(string clientName, InputSource source)
        {
            clientName = clientName.Trim();

            var existingClients = this.FindMatching(c => c.Name == clientName);
            var existingClient = existingClients.FirstOrDefault();
            bool addClient = false;

            if (existingClient == null)
            {
                //add client/farm/field and module
                addClient = true;
                existingClient = new ClientEntity();                          
                existingClient.Name = clientName;
                existingClient.Source = source;

                var canSaveClient = this.CanSaveClient(existingClient.Id, existingClient.Name);

                if (!canSaveClient)
                {
                    Logging.Logger.Log("INFO", "Cannot save new client with name " + clientName);
                    return null;
                }
                else
                {
                    if (addClient)
                    {
                        this.Save(existingClient);
                        _context.SaveChanges();
                    }                    
                    return existingClient;
                }
            }
            else
            {
                return existingClient;
            }
        }

        public bool CanSaveClient(string clientId, string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            return !_context.Clients.Any(p => p.Name.ToLower() == Name.ToLower() && !(p.Id == clientId));
        }

        public IEnumerable<string> GetClientIdsLinkedToFarms()
        {
            return _context.Clients.Where(c => c.Farms.Count() > 0).Select(c => c.Id).ToList();           
        }

    }
}
