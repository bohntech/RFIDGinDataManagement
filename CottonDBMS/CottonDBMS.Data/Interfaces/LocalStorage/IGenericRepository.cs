using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace CottonDBMS.Data.Interfaces
{
    public interface IGenericRepository<TObject> where TObject : class
    {
        IEnumerable<TObject> GetAll();
        TObject FindSingle(Expression<Func<TObject, bool>> predicate);
        IEnumerable<TObject> FindMatching(Expression<Func<TObject, bool>> predicate);
        void Add(TObject entity);
        void Delete(TObject entity);
        void Update(TObject entity);
    }
}
