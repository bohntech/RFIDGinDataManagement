using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Data.Models.Json;

namespace CottonDBMS.Data.Interfaces
{
    public interface ITruckDocumentRepository
    {
        Task<IEnumerable<TruckDocument>> GetAllTrucksAsync();
        Task<bool> UpdateTruckAsync(TruckDocument doc);
        Task<bool> AddTruckAsync(TruckDocument doc);
        Task DeleteTrucksAsync(string[] idsToDelete);
    }
}
