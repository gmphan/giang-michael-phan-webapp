using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.BusinessAccessLib
{
    public interface ISeedAdminIdentityServ
    {
        public Task SeedAdminUser(IServiceProvider serviceProvider);
    }
}