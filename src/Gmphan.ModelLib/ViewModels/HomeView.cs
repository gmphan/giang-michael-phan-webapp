using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class HomeView
    {
        public List<QuoteCollection> QuoteCollections { get; set; }
        public HomeView()
        {
            QuoteCollections = new List<QuoteCollection>();
        }

        public void Add<T>(List<T> ListT,T item) where T : class
        {
            ListT.Add(item);
        }
    }
}