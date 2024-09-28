using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class ResumeExperienceRepo : Repository<ResumeExperience>, IResumeExperienceRepo
    {
        private readonly AppDbContext _db;
        public ResumeExperienceRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(ResumeExperience obj)
        {
            _db.ResumeExperiences.Update(obj);
        }
    }
}