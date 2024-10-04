using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IResumeExperienceRepo : IRepository<ResumeExperience>
    {
        Task<IEnumerable<ResumeExperience>> GetAllResumeExperienceAsync();
        Task<ResumeExperience> GetSingleResumeExperienceAsync(int id);
        Task UpdateAsync(ResumeExperience obj);
    }
}