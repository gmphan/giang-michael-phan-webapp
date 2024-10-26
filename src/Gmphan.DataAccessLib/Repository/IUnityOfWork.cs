using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IUnityOfWork
    {
        IQuoteCollectionRepo QuoteCollectionRepoUOW { get; }
        IResumeHeaderRepo ResumeHeaderRepoUOW { get; }
        IResumeSummaryRepo ResumeSummaryRepoUOW { get; }
        IResumeExperienceRepo ResumeExperienceRepoUOW { get; }
        IResumeDescriptionRepo ResumeDescriptionRepoUOW { get; }
        IProjectRepo ProjectRepoUOW { get; }
        IProjectTaskRepo ProjectTaskRepoUOW { get; }    
        IProjectTaskActivityRepo ProjectTaskActivityRepoUOW { get; }
        IContactMeRepo ContactMeRepoUOW { get; }
        public Task SaveAsync();
    }
}