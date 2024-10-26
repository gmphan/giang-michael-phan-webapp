using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IContactMeRepo : IRepository<ContactMe>
    {
        public Task UpdateAsync(ContactMe obj);
    }
}