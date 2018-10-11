//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.DataModels;

namespace CottonDBMS.Interfaces
{
    public interface IAggregateEventRepository : IEntityRepository<AggregateEvent>
    {
        IEnumerable<AggregateEvent> GetAllOrderedByTime();
        IEnumerable<AggregateEvent> GetDirtyOrderedByTime();
        IEnumerable<AggregateEvent> GetEventsSinceLastLoad();
    }
}
