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
        IEnumerable<T> GetAll();

        //Get one set of T
        T Get(Expression<Func<T, bool>> filter);
        T GetLatestRecord<TKey>(Expression<Func<T, TKey>> keySelector);
        void Add(T entity);
        void Remove(T entity);

        //Remove multiple entities of T
        void RemoveRange(IEnumerable<T> entity);
    }
}