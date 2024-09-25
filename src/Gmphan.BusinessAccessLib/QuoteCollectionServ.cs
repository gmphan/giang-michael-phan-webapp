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
            return await GetTAsync("quoteCollections", _unityOfWork.QuoteCollectionRepoUOW.GetAll);
        }

        public async Task AddQuoteCollectionAsync(QuoteCollection quoteCollection)
        {
            await _unityOfWork.QuoteCollectionRepoUOW.Add(quoteCollection);
            await _unityOfWork.SaveAsync();
        }

        public async Task<QuoteCollection> GetQuoteCollectionAsync(int? id)
        {
            return await _unityOfWork.QuoteCollectionRepoUOW.Get(u => u.Id == id);
        }

        public async Task UpdateQuoteCollectionAsync(QuoteCollection quoteCollection)
        {
            await _unityOfWork.QuoteCollectionRepoUOW.Update(quoteCollection);
            await _unityOfWork.SaveAsync();
        }

        public async Task DeleteQuoteCollectionAsync(QuoteCollection quoteCollection)
        {
            await _unityOfWork.QuoteCollectionRepoUOW.Remove(quoteCollection);
            await _unityOfWork.SaveAsync();
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
                _memoryCache.Set(cacheKey, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Data was pull from Memory Cache: {cacheKey}");
            }
            return result;
        }
    }
}