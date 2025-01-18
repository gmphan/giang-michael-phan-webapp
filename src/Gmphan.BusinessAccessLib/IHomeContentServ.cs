using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.BusinessAccessLib
{
    // the reason that QuoteCollection is not included because I decided to dynmically the homepage content later.
    public interface IHomeContentServ
    {
        public Task<IEnumerable<HomeContent>> GetHomeContentAsync();
    }
}