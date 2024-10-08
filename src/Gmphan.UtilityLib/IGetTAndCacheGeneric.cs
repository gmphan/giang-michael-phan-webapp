using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.UtilityLib
{
    public interface IGetTAndCacheGeneric
    {
        public Task<T> GetTAsync<T>(string cacheKey, Func<Task<T>> getRecordFromDb) where T : class;
        public Task UpdateCacheAsync<T>(string cacheKey, Func<Task<T>> getRecordFromDb) where T : class;
    }
}