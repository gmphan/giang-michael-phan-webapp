using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gmphan.DataAccessLib.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ILogger<Repository<T>> _logger;
        private readonly AppDbContext _db;
        internal DbSet<T> _dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            this._dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            //query whole set
            IQueryable<T> query = _dbSet;

            //Reassign query down to one set of T
            query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = _dbSet;
            return query.ToList();
        }

        public T GetLatestRecord<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            T? latestRecord = _dbSet.OrderByDescending(keySelector).FirstOrDefault();
            return latestRecord;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}