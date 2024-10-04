using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;

namespace Gmphan.BusinessAccessLib
{
    public interface IResumeServ
    {
        public Task<ResumeHeader> GetLatestResumeHeaderAsync();
        public Task<ResumeSummary> GetLatestResumeSummaryAsync();
        public Task<IEnumerable<ResumeExperience>> GetAllResumeExperienceAsync();
        public Task UpdateResumeHeaderAsync(ResumeHeader obj);
        public Task UpdateResumeSummaryAsync(ResumeSummary obj);
        public Task CreateResumeExperienceAsync(ExperienceAdminView obj);
        public Task<ResumeExperience> GetSingleResumeExperienceServAsync(int id);
        public Task UpdateResumeExperienceServAsync(ResumeExperience model);
        public Task DeleteResumeExperienceServAsync(ResumeExperience model);
        
        
    }
}