using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.BusinessAccessLib
{
    public interface IResumeServ
    {
        public Task<ResumeHeader> GetLatestResumeHeaderAsync();
        public Task<ResumeSummary> GetLatestResumeSummaryAsync();
        public Task<IEnumerable<ResumeExperience>> GetAllResumeExperienceAsync();
    }
}