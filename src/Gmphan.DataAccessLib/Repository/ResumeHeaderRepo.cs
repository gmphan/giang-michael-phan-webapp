using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class ResumeHeaderRepo : Repository<ResumeHeader>, IResumeHeaderRepo
    {
        private readonly AppDbContext _db;
        //the base(db) is to pass the db to the base classes - Repository, or
        //else I will get error message from "public class ResumeHeaderRepo"
        public ResumeHeaderRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task UpdateAsync(ResumeHeader obj)
        {
            _db.ResumeHeaders.Update(obj);
        }
    }
}