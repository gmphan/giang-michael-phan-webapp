using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class PostDetailView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; } // for long content
        public List<string> Tags { get; set; } // optional, for categorization
    }
}