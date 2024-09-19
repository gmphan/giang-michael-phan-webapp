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
        Task<IEnumerable<T>> GetAll();

        //Get one set of T
        Task<T> Get(Expression<Func<T, bool>> filter);
        Task<T> GetLatestRecord<TKey>(Expression<Func<T, TKey>> keySelector);
        Task Add(T entity);
        Task Remove(T entity);

        //Remove multiple entities of T
        Task RemoveRange(IEnumerable<T> entity);
    }
}