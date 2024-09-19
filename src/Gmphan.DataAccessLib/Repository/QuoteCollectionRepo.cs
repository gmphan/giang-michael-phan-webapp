using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public class QuoteCollectionRepo : Repository<QuoteCollection>, IQuoteCollectionRepo
    {
        private readonly AppDbContext _db;

        //the base(db) is to pass the db to the base classes - Repository, or
        //else I will get error message from "public class QuoteCollectionRepo"
        public QuoteCollectionRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public async Task Update(QuoteCollection obj)
        {
            _db.QuoteCollections.Update(obj);
        }
    }
}