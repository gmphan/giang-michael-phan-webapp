using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
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
        public async Task UpdateResumeSummaryAsync(ResumeSummary obj)
        {
            await _unityOfWork.ResumeSummaryRepoUOW.UpdateAsync(obj);
            await _unityOfWork.SaveAsync();
            await UpdateCache("resumeSummary");
        }

        public async Task CreateResumeExperienceAsync(ExperienceAdminView model)
        {
            var resumeExperience = model.ResumeExperience;

            // Set the creation and modification dates
            resumeExperience.CreatedDate = DateTime.UtcNow;
            resumeExperience.UpdatedDate = DateTime.UtcNow;

            // Add ResumeExperience along with associated descriptions (if any)
            _unityOfWork.ResumeExperienceRepoUOW.AddAsync(resumeExperience);
            await _unityOfWork.SaveAsync();
            // If there are any descriptions, they will be added through the navigation property
            if (model.ResumeDescriptions != null && model.ResumeDescriptions.Any())
            {
                foreach (var description in model.ResumeDescriptions)
                {
                    description.ResumeExperienceId = resumeExperience.Id; // Link each description to the experience
                    _unityOfWork.ResumeDescriptionRepoUOW.AddAsync(description); // This may be handled automatically, but ensure this if necessary
                }
            }

            // await UpdateCache("resumeExperience");
            // Save all changes to the database
            await _unityOfWork.SaveAsync();
        }
        public Task<ResumeExperience> GetSingleResumeExperienceServAsync(int id)
        {
            return _unityOfWork.ResumeExperienceRepoUOW.GetSingleResumeExperienceAsync(id);
        }

        public async Task UpdateResumeExperienceServAsync(ResumeExperience model)
        {
            // Retrieve the existing ResumeExperience from the database
            var existingExperience = await _unityOfWork.ResumeExperienceRepoUOW.GetSingleResumeExperienceAsync(model.Id);

            if (existingExperience == null)
            {
                throw new Exception("ResumeExperience not found");
            }

            // Update the properties of ResumeExperience
            existingExperience.Title = model.Title;
            existingExperience.Company = model.Company;
            existingExperience.Country = model.Country;
            existingExperience.City = model.City;
            existingExperience.State = model.State;
            existingExperience.ZipCode = model.ZipCode;
            existingExperience.CurrentlyWorkHere = model.CurrentlyWorkHere;
            existingExperience.FromMonth = model.FromMonth;
            existingExperience.FromYear = model.FromYear;
            existingExperience.ToMonth = model.ToMonth;
            existingExperience.ToYear = model.ToYear;
            existingExperience.UpdatedDate = DateTime.UtcNow;

            // Mark the ResumeExperience as modified
            await _unityOfWork.ResumeExperienceRepoUOW.UpdateAsync(existingExperience);

            // Handle ResumeDescriptions
            var existingDescriptions = existingExperience.Descriptions.ToList(); // Get existing descriptions

            // Update existing and add new descriptions
            foreach (var description in model.Descriptions)
            {
                var existingDescription = existingDescriptions.FirstOrDefault(d => d.Id == description.Id);

                if (existingDescription != null)
                {
                    // Update the existing description
                    existingDescription.DescriptionText = description.DescriptionText;

                    // Mark the description as modified
                    _unityOfWork.ResumeDescriptionRepoUOW.UpdateAsync(existingDescription);
                }
                else
                {
                    // Add new description
                    description.ResumeExperienceId = model.Id; // Link to ResumeExperience
                    await _unityOfWork.ResumeDescriptionRepoUOW.AddAsync(description);
                }
            }

            // Remove deleted descriptions
            foreach (var existingDescription in existingDescriptions)
            {
                if (!model.Descriptions.Any(d => d.Id == existingDescription.Id))
                {
                    _unityOfWork.ResumeDescriptionRepoUOW.RemoveAsync(existingDescription);
                }
            }
            // Save the changes
            await _unityOfWork.SaveAsync();
        }

        public async Task DeleteResumeExperienceServAsync(ResumeExperience model)
        {
            await _unityOfWork.ResumeExperienceRepoUOW.RemoveAsync(model);
            await _unityOfWork.SaveAsync();
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