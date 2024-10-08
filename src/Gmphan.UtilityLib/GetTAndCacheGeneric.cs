using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Gmphan.UtilityLib
{   
    public class GetTAndCacheGeneric : IGetTAndCacheGeneric
    {
        private readonly ILogger<GetTAndCacheGeneric> _logger;
        private readonly IMemoryCache _memoryCache;

        public GetTAndCacheGeneric(ILogger<GetTAndCacheGeneric> logger
                                , IMemoryCache memoryCache)
        {
                _logger = logger;
                _memoryCache = memoryCache;
        }
        public async Task<T> GetTAsync<T>(string cacheKey, Func<Task<T>> getRecordFromDb) where T : class
        {
            if(!_memoryCache.TryGetValue(cacheKey, out T result))
            {
                _logger.LogInformation("Querying data from database.");

                // Data was not found in cache, retreive data from database
                result = await getRecordFromDb();

                // Set the data to memoryCache
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                         .SetSlidingExpiration(TimeSpan.FromMinutes(15)) //Cache will expire if note accessed within 15 minutes
                                         .SetAbsoluteExpiration(TimeSpan.FromHours(6)); //Cache will expire within 6 hours no matter what.
                
                // Set data to cache
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Data was pull from Memory Cache: {cacheKey}");
            }
            return result;
        }
        public async Task UpdateCacheAsync<T>(string cacheKey, Func<Task<T>> getRecordFromDb) where T : class
        {
            _memoryCache.Remove(cacheKey);
            if(!_memoryCache.TryGetValue(cacheKey, out T result))
            {
                _logger.LogInformation($"Updating MemoryCache for: {cacheKey}");

                // Data was not found in cache, retreive data from database
                result = await getRecordFromDb();

                // Set the data to memoryCache
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                         .SetSlidingExpiration(TimeSpan.FromMinutes(15)) //Cache will expire if note accessed within 15 minutes
                                         .SetAbsoluteExpiration(TimeSpan.FromHours(6)); //Cache will expire within 6 hours no matter what.
                
                // Set data to cache
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Memory Cache Data didn't update for: {cacheKey}");
            }
        }
    }
}