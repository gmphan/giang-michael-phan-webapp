using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class ResumeSummaryRepo : Repository<ResumeSummary>, IResumeSummaryRepo
    {
        private readonly AppDbContext _db;
        public ResumeSummaryRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(ResumeSummary obj)
        {
            _db.ResumeSummaries.Update(obj);
        }
    }
}