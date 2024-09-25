using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.BusinessAccessLib
{
    public interface IQuoteCollectionServ
    {
        public Task<IEnumerable<QuoteCollection>> GetAllQuoteCollectionAsync();
        public Task AddQuoteCollectionAsync(QuoteCollection quoteCollection);
        public Task<QuoteCollection> GetQuoteCollectionAsync(int? id);
        public Task UpdateQuoteCollectionAsync(QuoteCollection quoteCollection);
    }
}