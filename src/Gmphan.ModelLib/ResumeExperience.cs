using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gmphan.ModelLib
{
    public class ResumeExperience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }
        
        [Required]
        public string? Company { get; set; }
        
        [Required]
        public string? Country { get; set; }
        
        [Required]
        public string? City { get; set; }
        
        [Required]
        public string? State { get; set; }
        
        [Required]
        [DisplayName("Zip Code")]
        public string? ZipCode { get; set; }
        
        [DisplayName("I am currently working in this role")]
        public bool CurrentlyWorkHere { get; set; } = false; // Set default value to false

        [Required]
        [DisplayName("From Month")]
        public string? FromMonth { get; set; }

        [Required]
        [DisplayName("From Year")]
        public int? FromYear { get; set; }

        [DisplayName("To Month")]
        public string? ToMonth { get; set; }

        [DisplayName("To Year")]
        public int? ToYear { get; set; }

        // Navigation property for the list of descriptions
        public ICollection<ResumeDescription>? Descriptions { get; set; } = new List<ResumeDescription>();

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }
    }
}