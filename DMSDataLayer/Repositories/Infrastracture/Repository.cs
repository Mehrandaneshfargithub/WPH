using DMSDataLayer.EntityModels;
using DMSDataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.Repositories.Infrastracture
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DMSContext DMSContext;
        DbSet<TEntity> _entities;
        //public Repository()
        //   : this(new DMSContext())
        //{
        //}
        public Repository(DMSContext dmscontext)
        {
            DMSContext = dmscontext;
            _entities = DMSContext.Set<TEntity>();
        }

        public TEntity Get(Guid id)
        {

            return _entities.Find(id);
        }



        public IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
        }

        public long Count()
        {
            return _entities.Count();
        }

        public long Count(Expression<Func<TEntity, bool>> filter)
        {
            return _entities.Where(filter).Count();
        }

        public void UpdateState(TEntity entity)
        {
            DMSContext.Entry(entity).State = EntityState.Modified;
        }
        public void Detach(TEntity entity)
        {
            DMSContext.Entry(entity).State = EntityState.Detached;
        }
    }
}

