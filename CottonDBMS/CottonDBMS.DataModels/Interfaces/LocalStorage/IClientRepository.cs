//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IClientRepository : IEntityRepository<ClientEntity>
    {
        bool CanSaveClient(string clientId, string Name);
        IEnumerable<string> GetClientIdsLinkedToFarms();
        ClientEntity EnsureClientCreated(string clientName, InputSource source);
    }
}
