//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CottonDBMS.Interfaces
{
    

    public interface IGenericRepository<TObject> where TObject : class
    {
        IEnumerable<TObject> GetAll(string[] includes = null);
        TObject FindSingle(Expression<Func<TObject, bool>> predicate);
        IEnumerable<TObject> FindMatching(Expression<Func<TObject, bool>> predicate, string[] includes = null);
        void Add(TObject entity);
        void Delete(TObject entity);
        void Update(TObject entity);
        void BulkDelete(IEnumerable<TObject> entities);
        TObject FindSingle(Expression<Func<TObject, bool>> predicate, params string[] includes);
        void DisableChangeTracking();
        void EnableChangeTracking();
    }
}
