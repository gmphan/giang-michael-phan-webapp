using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IHomeContentRepo : IRepository<HomeContent>
    {
        public Task UpdateAsync(HomeContent obj);
    }
}