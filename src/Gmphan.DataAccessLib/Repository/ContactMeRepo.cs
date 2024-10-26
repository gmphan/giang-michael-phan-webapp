using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class ContactMeRepo : Repository<ContactMe>, IContactMeRepo
    {
        private readonly AppDbContext _db;

        public ContactMeRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public Task UpdateAsync(ContactMe obj)
        {
            throw new NotImplementedException();
        }
    }
}