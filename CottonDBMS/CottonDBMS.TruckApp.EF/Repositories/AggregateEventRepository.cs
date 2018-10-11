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
    public class AggregateEventRepository : EntityRepository<AggregateEvent>, IAggregateEventRepository
    {
        public AggregateEventRepository(AppDBContext context) : base(context)
        {
             
        }

        public IEnumerable<AggregateEvent> GetAllOrderedByTime()
        {
            return _context.AggregateEvents.OrderBy(t => t.Timestamp).ToList();
        }

        public IEnumerable<AggregateEvent> GetDirtyOrderedByTime()
        {
            return _context.AggregateEvents.Where(t => t.SyncedToCloud == false).OrderBy(t => t.Timestamp).ToList();
        }

        public IEnumerable<AggregateEvent> GetEventsSinceLastLoad()
        {
            var lastLoadEvent = _context.AggregateEvents.Where(a => !string.IsNullOrEmpty(a.LoadNumber)).OrderByDescending(a => a.Created).FirstOrDefault();

            if (lastLoadEvent != null)
            {
                var firstEventWithLoadNumber = _context.AggregateEvents.Where(a => a.LoadNumber == lastLoadEvent.LoadNumber).OrderBy(a => a.Created).FirstOrDefault();                
                return _context.AggregateEvents.Where(a => a.Created >= firstEventWithLoadNumber.Created).OrderBy(a => a.Created);
            }
            else
                return new List<AggregateEvent>();
        }
    }
}
