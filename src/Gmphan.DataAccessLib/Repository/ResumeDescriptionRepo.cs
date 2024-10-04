using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class ResumeDescriptionRepo : Repository<ResumeDescription>, IResumeDescriptionRepo
    {
        private readonly AppDbContext _db;
        public ResumeDescriptionRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(ResumeDescription obj)
        {
            _db.ResumeDescriptions.Update(obj);
        }
    }
}