using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class HomeContentRepo : Repository<HomeContent>, IHomeContentRepo
    {
        private readonly AppDbContext _db;
        public HomeContentRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }   
        public Task UpdateAsync(HomeContent obj)
        {
            throw new NotImplementedException();
        }
    }
}