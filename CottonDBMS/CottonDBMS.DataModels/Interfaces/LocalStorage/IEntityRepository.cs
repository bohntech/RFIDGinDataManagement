//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CottonDBMS.Interfaces;

namespace CottonDBMS.Interfaces
{
    public interface IEntityRepository<TObject> : IGenericRepository<TObject> where TObject: class
    {
        TObject GetById(string id);
        void Save(TObject entity);
        TObject GetById(string id, params string[] includes);
        IEnumerable<TObject> GetDirty(params string[] includes);
        void Update(TObject entity, bool syncedToCloud);        
        void MarkAllDirty();
        void QuickUpdate(TObject entity, bool detachIfAttached);
        void CreateWithID(TObject entity, string preferredID);
    }
}
