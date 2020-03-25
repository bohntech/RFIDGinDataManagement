//Licensed under MIT License see LICENSE.TXT in project root folder
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity;
using CottonDBMS.DataModels;
using CottonDBMS.Interfaces;
using CottonDBMS.EF.Repositories;
using CottonDBMS.Data.EF;
using GalaSoft.MvvmLight.Messaging;

namespace CottonDBMS.EF.Repositories
{
    public class EntityRepository<TObject> :  GenericRepository<TObject>, IEntityRepository<TObject> where TObject: BaseEntity 
    {


        public EntityRepository(AppDBContext context) : base(context)
        {            
        }

        public TObject GetById(string id)
        {
            return _context.Set<TObject>().SingleOrDefault(t => t.Id == id);
        }

        public TObject GetById(string id, params string[] includes)
        {
            var query = _context.Set<TObject>().AsQueryable();
            if (includes != null)
            {
                foreach (var i in includes) query = query.Include(i);
            }
            return query.SingleOrDefault(t => t.Id == id);
        }

        public IEnumerable<TObject> GetDirty(params string[] includes)
        {
            var query = _context.Set<TObject>().AsQueryable();
            if (includes != null)
            {
                foreach (var i in includes) query = query.Include(i);
            }

            return query.Where(t => !t.SyncedToCloud).ToList();
        }       

        public override void Add(TObject entity)
        {
            entity.SyncedToCloud = false;
            _context.Set<TObject>().Add(entity);
            Messenger.Default.Send<EntitySavedMessage>(new EntitySavedMessage { DataObject = entity });
        }

        /// <summary>
        /// Marks an entity for removal on next save
        /// </summary>
        /// <param name="entity"></param>
        public override void Delete(TObject entity)
        {
            var item = _context.Set<TObject>().FirstOrDefault(t => t.Id == entity.Id);            
            _context.Entry(item).State = EntityState.Deleted;
            
            
            //only add if id doesn't already exist and it's not a Bale or GinLoad (Bales and GinLoads are not synced to cloud)
            if (entity.EntityType != EntityType.BALE && entity.EntityType != EntityType.GIN_LOAD && !_context.DocumentsToProcess.Any(i => i.Id == entity.Id) )
            {
                _context.Set<DocumentToProcess>().Add(new DocumentToProcess { Id = entity.Id, EntityType = entity.EntityType, Name = entity.Name, SelfLink = entity.SelfLink, SyncedToCloud = false });

                if (entity.EntityType == EntityType.MODULE) //for modules also ensure ownership record is deleted
                {
                    _context.Set<DocumentToProcess>().Add(new DocumentToProcess { Id = "OWNERSHIP-" + entity.Id, EntityType = entity.EntityType, Name = entity.Name, SelfLink = entity.SelfLink, SyncedToCloud = false });
                }
            }

        }

        public void MarkAllDirty()
        {
            var items = _context.Set<TObject>().Where(t => t.SyncedToCloud == true);

            foreach (var i in items)
            {
                i.SyncedToCloud = false;
                this.Update(i);
            }
        }

        public override void BulkDelete(IEnumerable<TObject> entities)
        {            
            foreach (var e in entities)
            {
                this.Delete(e);
            }            
        }

        /// <summary>
        /// Marks an entity for update on next save
        /// </summary>
        /// <param name="entity"></param>
        public override void Update(TObject entity)
        {
            entity.SyncedToCloud = false;
            _context.Entry(entity).State = EntityState.Modified;            
        }

        /// <summary>
        /// Marks an entity for update on next save
        /// </summary>
        /// <param name="entity"></param>
        //public virtual void ReplaceWith(TObject entity)
        //{
        //    var existingEntity = _context.Set<TObject>().Local.SingleOrDefault(t => t.Id == entity.Id);

        //    if (existingEntity != null)
        //    {
        //        _context.Entry(existingEntity).State = EntityState.Detached;
        //        _context.Set<TObject>().Local.Remove(existingEntity);
        //    }
                        
        //    _context.Set<TObject>().Attach(entity);
        //    _context.Entry(entity).State = EntityState.Modified;
            
        //    //Messenger.Default.Send<EntitySavedMessage>(new EntitySavedMessage { DataObject = entity });
        //}

        public virtual void QuickUpdate(TObject entity, bool detachIfAttached)
        {
            if (detachIfAttached)
            {
                var existingEntity = _context.Set<TObject>().Local.SingleOrDefault(t => t.Id == entity.Id);

                if (existingEntity != null)
                {
                    _context.Entry(existingEntity).State = EntityState.Detached;
                    _context.Set<TObject>().Local.Remove(existingEntity);
                }
            }

            _context.Set<TObject>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            //Messenger.Default.Send<EntitySavedMessage>(new EntitySavedMessage { DataObject = entity });
        }

        public void Update(TObject entity, bool syncedToCloud)
        {
            entity.SyncedToCloud = syncedToCloud;
            _context.Entry(entity).State = EntityState.Modified;
            //Messenger.Default.Send<EntitySavedMessage>(new EntitySavedMessage { DataObject = entity });
        }

        public virtual void Save(TObject entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                entity.Created = DateTime.UtcNow;
                this.Add(entity);
            }
            else
            {
                entity.Updated = DateTime.UtcNow;
                this.Update(entity);
            }
        }

        public virtual void SaveUseObjectDates(TObject entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Id))
            {
                entity.Id = Guid.NewGuid().ToString();                
                this.Add(entity);
            }
            else
            {                
                this.Update(entity);
            }
        }

        //will save the entity and use preferred ID to 
        public virtual void CreateWithID(TObject entity, string preferredID)
        {

            Guid newID = Guid.NewGuid();

            if (Guid.TryParse(preferredID, out newID))
            {
                newID = Guid.Parse(preferredID);
            }
            else
            {
                newID = Guid.NewGuid();
            }

            entity.Id = newID.ToString();
            entity.Created = DateTime.UtcNow;
            this.Add(entity);
        }

        public IEnumerable<string> GetAllIds()
        {
            return _context.Set<TObject>().Select(t => t.Id).ToList();
        }

        public IEnumerable<string> GetIdsForChanged(DateTime updatedAfter)
        {
            return _context.Set<TObject>().Where(t => t.Created > updatedAfter || (t.Updated.HasValue && t.Updated.Value > updatedAfter)).Select(t => t.Id).ToList();
        }
    }
}
