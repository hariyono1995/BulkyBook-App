using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalBulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            //_db.ShoppingCarts.AsNoTracking();
            //_db.Products.Include(u => u.Category).Include(u => u.CoverType);
            this.dbSet = _db.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        //Include prop - "Category,CoverType"
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter != null)
            {
                query = dbSet.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> query;

            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

              query = dbSet.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        //void IRepository<T>.Add(T entity)
        //{
        //    dbSet.Add(entity);
        //}

        //void IRepository<T>.AddRange(IEnumerable<T> entities)
        //{
        //    dbSet.AddRange(entities);
        //}

        //IEnumerable<T> IRepository<T>.GetAll()
        //{
        //    IQueryable<T> query = dbSet;
        //    return query.ToList();
        //}

        //T IRepository<T>.GetFirstOrDefault(Expression<Func<T, bool>> filter)
        //{
        //    IQueryable<T> query = dbSet.Where(filter);
        //    return query.FirstOrDefault();
        //}

        //void IRepository<T>.Remove(T entity)
        //{
        //    dbSet.Remove(entity);
        //}

        //void IRepository<T>.RemoveRange(T entities)
        //{
        //    dbSet.RemoveRange(entities);
        //}
    }
}
