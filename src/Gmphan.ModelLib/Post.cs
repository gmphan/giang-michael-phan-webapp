using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [DisplayName("Published Date")]
        public DateTime PublishedDate { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; } // for long content
        public List<string> Tags { get; set; } // optional, for categorization
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}