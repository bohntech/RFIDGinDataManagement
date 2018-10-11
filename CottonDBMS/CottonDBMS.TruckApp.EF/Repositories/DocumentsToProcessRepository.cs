//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using CottonDBMS.Interfaces;
using CottonDBMS.DataModels;
using CottonDBMS.Data.EF;

namespace CottonDBMS.EF.Repositories
{
    public class DocumentsToProcessRepository : EntityRepository<DocumentToProcess>, IDocumentsToProcessRepository
    {
        public DocumentsToProcessRepository(AppDBContext context) : base(context)
        {

        }

        public override void BulkDelete(IEnumerable<DocumentToProcess> entities)
        {
            var idsToDelete = entities.Select(t => t.Id).ToList();
            var res = _context.DocumentsToProcess.Where(e => idsToDelete.Contains(e.Id));
            _context.DocumentsToProcess.RemoveRange(res);
        }

        public override void Delete(DocumentToProcess entity)
        {            
            _context.Set<DocumentToProcess>().Remove(entity);
        }
    }
}
