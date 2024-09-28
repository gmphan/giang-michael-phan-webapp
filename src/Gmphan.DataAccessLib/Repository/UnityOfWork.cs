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
        public IResumeHeaderRepo ResumeHeaderRepoUOW { get; private set;}   
        public IResumeSummaryRepo ResumeSummaryRepoUOW { get; private set; }
        public IResumeExperienceRepo ResumeExperienceRepoUOW { get; private set; }
        public UnityOfWork(AppDbContext db)
        {
            _db = db;
            QuoteCollectionRepoUOW = new QuoteCollectionRepo(_db);
            ResumeHeaderRepoUOW = new ResumeHeaderRepo(_db);
            ResumeSummaryRepoUOW = new ResumeSummaryRepo(_db);
            ResumeExperienceRepoUOW = new ResumeExperienceRepo(_db);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}