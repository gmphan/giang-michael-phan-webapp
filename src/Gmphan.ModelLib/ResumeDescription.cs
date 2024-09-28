using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gmphan.ModelLib
{
    public class ResumeDescription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string? DescriptionText { get; set; }

        [Required]
        public int ResumeExperienceId { get; set; }

        // Navigation property to ResumeExperience
        [ForeignKey("ResumeExperienceId")]
        public ResumeExperience? ResumeExperience { get; set; }
    }
}