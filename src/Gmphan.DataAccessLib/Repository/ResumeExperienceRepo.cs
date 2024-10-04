using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Microsoft.EntityFrameworkCore;

namespace Gmphan.DataAccessLib.Repository
{
    public class ResumeExperienceRepo : Repository<ResumeExperience>, IResumeExperienceRepo
    {
        private readonly AppDbContext _db;
        public ResumeExperienceRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ResumeExperience>> GetAllResumeExperienceAsync()
        {
            IEnumerable<ResumeExperience> experiences = await _db.ResumeExperiences
                                            .Include(re => re.Descriptions)  // Eager loading ResumeDescriptions
                                            .ToListAsync();
            return experiences;                         
        }
        public async Task<ResumeExperience> GetSingleResumeExperienceAsync(int id)
        {
            ResumeExperience experience = await _db.ResumeExperiences
                                       .Include(re => re.Descriptions)  // Eager load ResumeDescriptions
                                       .SingleOrDefaultAsync(re => re.Id == id);
            return experience;
        }
        public async Task UpdateAsync(ResumeExperience obj)
        {
            _db.ResumeExperiences.Update(obj);
        }
    }
}