using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace Gmphan.BusinessAccessLib
{
    public class ResumeServ : IResumeServ
    {
        private readonly ILogger<ResumeServ> _logger;
        private readonly IUnityOfWork _unityOfWork;
        private readonly IMemoryCache _memoryCache;
        public ResumeServ(ILogger<ResumeServ> logger
                                , IUnityOfWork unityOfWork
                                , IMemoryCache memoryCache)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
            _memoryCache = memoryCache;
        }
        public async Task<IEnumerable<ResumeExperience>> GetAllResumeExperienceAsync()
        {
            // return await GetTAsync("resumeExperience", () => _unityOfWork.ResumeExperienceRepoUOW.GetAllAsync());
            return await _unityOfWork.ResumeExperienceRepoUOW.GetAllResumeExperienceAsync();
        }

        public async Task<ResumeHeader> GetLatestResumeHeaderAsync()
        {
            return await GetTAsync("resumeHeader", () => _unityOfWork.ResumeHeaderRepoUOW.GetLatestRecordAsync(x => x.Id));
        }

        public async Task<ResumeSummary> GetLatestResumeSummaryAsync()
        {
            return await GetTAsync("resumeSummary", () => _unityOfWork.ResumeSummaryRepoUOW.GetLatestRecordAsync(x => x.Id));
        }

        public async Task UpdateResumeHeaderAsync(ResumeHeader obj)
        {
            await _unityOfWork.ResumeHeaderRepoUOW.UpdateAsync(obj);
            await _unityOfWork.SaveAsync();
            //need to update the cache
            await UpdateCache("resumeHeader");
        }

        // import to make Func<Task<T>> and not Func<T> because all Repository func are async
        public async Task<T> GetTAsync<T> (string cacheKey, Func<Task<T>> queryFromDb) where T : class
        {
            if (!_memoryCache.TryGetValue(cacheKey, out T DataResult))
            {
                _logger.LogInformation("Querying data from Db for: ", cacheKey);

                // Data not found in cache, retrieve data from database
                DataResult = await queryFromDb();

                // Set the data to memory cache
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                                        .SetSlidingExpiration(TimeSpan.FromMinutes(15)) // Cache will expire if not accessed within 15 minutes
                                        .SetAbsoluteExpiration(TimeSpan.FromHours(6)); // Cache will expire after 6 hours

                // Set data to cache
                _memoryCache.Set(cacheKey, DataResult, cacheEntryOptions);
            }
            else
            {
                _logger.LogInformation($"Data was pulled from Memory Cache of: {cacheKey}");
            }
            return DataResult;
        }

        public async Task UpdateCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
            await GetTAsync(cacheKey, _unityOfWork.QuoteCollectionRepoUOW.GetAllAsync);
        }
    }
}