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
using GalaSoft.MvvmLight.Messaging;

namespace CottonDBMS.EF.Repositories
{
    /// <summary>
    /// Base class implementation of generic repository using
    /// Entity Framework and MS SQL Local Database for data 
    /// storage.
    /// </summary>
    /// <typeparam name="TObject"></typeparam>
    public class GenericRepository<TObject>: IGenericRepository<TObject> where TObject : class
    {
        protected AppDBContext _context = null;
        
        public GenericRepository(AppDBContext context)
        {        
            _context = context;
        }

        public void DisableChangeTracking()
        {
            _context.Configuration.AutoDetectChangesEnabled = false;
        }

        public void EnableChangeTracking()
        {
            _context.Configuration.AutoDetectChangesEnabled = true;
        }

        /// <summary>
        /// Gets all entities of type 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TObject> GetAll(string[] includes=null)
        {
            var query = _context.Set<TObject>().AsQueryable();

            if (includes != null)
            {
                foreach(var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.ToList();
        }

        /// <summary>
        /// Finds a single entity matching predict.  If no match returns null.
        /// If multiple matches an exception is thrown.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TObject FindSingle(Expression<Func<TObject, bool>> predicate)
        {
            return _context.Set<TObject>().SingleOrDefault(predicate);
        }

        public TObject FindSingle(Expression<Func<TObject, bool>> predicate, params string[] includes)
        {
            var q = _context.Set<TObject>().AsQueryable();

            if (includes != null)
            {
                foreach (var i in includes)
                    q = q.Include(i);
            }

            return q.SingleOrDefault(predicate);
        }



        /// <summary>
        /// Returns all entities that match predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TObject> FindMatching(Expression<Func<TObject, bool>> predicate, string[] includes=null)
        {
            var q = _context.Set<TObject>().AsQueryable();
            
            if (includes != null)
            {
                foreach (var i in includes)
                    q = q.Include(i);
            }
            q = q.Where(predicate);

            return q.ToList();
        }

        /// <summary>
        /// Adds an entity to the context to be inserted on Save
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TObject entity)
        {            
            _context.Set<TObject>().Add(entity);            
        }

        /// <summary>
        /// Marks an entity for removal on next save
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TObject entity)
        {
            _context.Set<TObject>().Remove(entity);
            //_context.Entry(entity).State = EntityState.Deleted;
        }

        public virtual void BulkDelete(IEnumerable<TObject> entities)
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
        public virtual void Update(TObject entity)
        {            
            _context.Entry(entity).State = EntityState.Modified;

            //Messenger.Default.Send<EntitySavedMessage>(new EntitySavedMessage { DataObject = entity });
        }
    }
    
}