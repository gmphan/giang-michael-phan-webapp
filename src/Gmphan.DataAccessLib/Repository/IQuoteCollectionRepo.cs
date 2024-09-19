using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;

namespace Gmphan.DataAccessLib.Repository
{
    public interface IQuoteCollectionRepo : IRepository<QuoteCollection>
    {
        void Update(QuoteCollection obj);
    }
}