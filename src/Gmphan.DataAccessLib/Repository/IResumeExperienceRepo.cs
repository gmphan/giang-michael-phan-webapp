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
        Task UpdateAsync(ResumeExperience obj);
    }
}