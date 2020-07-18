using Microsoft.EntityFrameworkCore;
using Patriot.Core.Repositories.Interfaces;
using Patriot.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Patriot.Core.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T: class
    {

        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsEnumerable();
        }


        public IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
                var d = query.ToList();
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }


            if (orderBy != null)
            {
                var dd = orderBy(query).ToList();
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }


        public virtual T GetByID(object id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            TrySetProperty(entity, "CreatedBy", "test");
            TrySetProperty(entity, "CreatedDate", DateTime.Now);
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            TrySetProperty(entityToUpdate, "ModifiedBy", "test");
            TrySetProperty(entityToUpdate, "ModifiedDate", DateTime.Now);
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public IEnumerable<T> GetWithRawSql(string query, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters).ToList();
        }


        private void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }
    }
}
