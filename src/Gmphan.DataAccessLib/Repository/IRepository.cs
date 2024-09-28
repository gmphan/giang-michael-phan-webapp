using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IRepository<T> where T : class
    {
        //Get set of T
        Task<IEnumerable<T>> GetAllAsync();

        //Get one set of T
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> GetLatestRecordAsync<TKey>(Expression<Func<T, TKey>> keySelector);
        Task AddAsync(T entity);
        Task RemoveAsync(T entity);

        //Remove multiple entities of T
        Task RemoveRangeAsync(IEnumerable<T> entity);
    }
}