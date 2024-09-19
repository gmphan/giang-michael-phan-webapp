using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.DataAccessLib.Repository
{
    public class UnityOfWork : IUnityOfWork
    {
        private readonly AppDbContext _db;
        public IQuoteCollectionRepo QuoteCollectionRepoUOW { get; private set;}

        public UnityOfWork(AppDbContext db)
        {
            _db = db;
            QuoteCollectionRepoUOW = new QuoteCollectionRepo(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}