using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Project Name")]
        public string? ProjectName { get; set; }
        [DisplayName("Description")]
        public string? ProjectDescription { get; set; }
        [Required]
        [DisplayName("State")]
        public string? ProjectState { get; set; }
        // Navigation property for the list of descriptions
        public ICollection<ProjectTask>? ProjectTasks { get; set; } = new List<ProjectTask>();
        [Required]
        [DisplayName("Start Date")]
        public DateTime ProjectStartDate { get; set; }
        [Required]
        [DisplayName("Due Date")]
        public DateTime ProjectDueDate { get; set; }
        [Required]
        [DisplayName("Completed Date")]
        public DateTime ProjectCompletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}