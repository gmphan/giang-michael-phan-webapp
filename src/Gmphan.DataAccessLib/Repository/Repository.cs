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
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            // Query the whole set
            IQueryable<T> query = _dbSet;

            // Reassign query down to one set of T
            query = query.Where(filter);

            // Execute the query asynchronously and return the result
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = _dbSet;
            return await query.ToListAsync();
        }

       public async Task<T> GetLatestRecordAsync<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            // Get the latest record based on the key selector asynchronously
            T? latestRecord = await _dbSet.OrderByDescending(keySelector).FirstOrDefaultAsync();
            return latestRecord;
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }
    }
}