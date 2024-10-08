using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Gmphan.ModelLib;
using Gmphan.DataAccessLib.Repository;
using Microsoft.Extensions.Caching.Memory;
using System.Security;

namespace Gmphan.BusinessAccessLib
{
    public class QuoteCollectionServ : IQuoteCollectionServ
    {
        private readonly ILogger<QuoteCollectionServ> _logger;
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMemoryCache _memoryCache;
        public QuoteCollectionServ(ILogger<QuoteCollectionServ> logger
                                    , IUnityOfWork unityOfWork
                                    , IMemoryCache memoryCache)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<QuoteCollection>> GetAllQuoteCollectionAsync()
        {
            return await GetTAsync("quoteCollections", _unityOfWork.QuoteCollectionRepoUOW.GetAllAsync);
        }

        public async Task AddQuoteCollectionAsync(QuoteCollection quoteCollection)
        {
            await _unityOfWork.QuoteCollectionRepoUOW.AddAsync(quoteCollection);
            await _unityOfWork.SaveAsync();
            await UpdateCache("quoteCollections");
        }

        public async Task<QuoteCollection> GetQuoteCollectionAsync(int? id)
        {
            return await _unityOfWork.QuoteCollectionRepoUOW.GetAsync(u => u.Id == id);
        }

        public async Task UpdateQuoteCollectionAsync(QuoteCollection quoteCollection)
        {
            await _unityOfWork.QuoteCollectionRepoUOW.UpdateAsync(quoteCollection);
            await _unityOfWork.SaveAsync();
            await UpdateCache("quoteCollections");
        }

        public async Task DeleteQuoteCollectionAsync(QuoteCollection quoteCollection)
        {
            await _unityOfWork.QuoteCollectionRepoUOW.RemoveAsync(quoteCollection);
            await _unityOfWork.SaveAsync();
            await UpdateCache("quoteCollections");
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
        public async Task UpdateCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
            await GetTAsync(cacheKey, _unityOfWork.QuoteCollectionRepoUOW.GetAllAsync);
        }
    }
}