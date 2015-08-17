using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Dynamic.Database
{
    public class EntitySet<TEntity> :
        IQueryable<TEntity>, IQueryable,
        IEnumerable<TEntity>, IEnumerable
    {
        private dynamic _DynamicSet;
        private DbSet _DbSet;

        public EntitySet(
            object dynamicSet,
            DbSet dbSet)
        {
            _DynamicSet = dynamicSet;
            _DbSet = dbSet;
        }

        #region IQueryable / IEnumerable Methods

        public Type ElementType
        {
            get { return _DbSet.ElementType; }
        }

        public Expression Expression
        {
            get { return (_DynamicSet as IQueryable).Expression; }
        }

        public IQueryProvider Provider
        {
            get { return (_DynamicSet as IQueryable).Provider; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _DynamicSet as IEnumerator;
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return (_DynamicSet as IEnumerable<TEntity>).GetEnumerator();
        }

        #endregion

        #region IDbSet Methods

        public TEntity Add(
            TEntity entity)
        {
            return (TEntity)_DbSet.Add(entity);
        }

        public TEntity Attach(
            TEntity entity)
        {
            return (TEntity)_DbSet.Attach(entity);
        }

        public TEntity Create()
        {
            return (TEntity)_DbSet.Create();
        }

        public TEntity Find(
            params object[] keyValues)
        {
            return (TEntity)_DbSet.Find(keyValues);
        }

        public IEnumerable<TEntity> Local
        {
            get { return _DynamicSet.Local; }
        }

        public TEntity Remove(
            TEntity entity)
        {
            return (TEntity)_DbSet.Remove(entity);
        }

        #endregion
    }
}
