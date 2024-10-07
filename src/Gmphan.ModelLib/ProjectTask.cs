using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib
{
    public class ProjectTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [DisplayName("Task Name")]
        public string? ProjectTaskName { get; set; }
        [DisplayName("Task Description")]
        public string? ProjectTaskDescription { get; set; }
        [Required]
        [DisplayName("Task State")]
        public string? ProjectTaskState { get; set; }
        // Navigation property for the list of descriptions
        public ICollection<ProjectTaskActivity>? ProjectTaskActivities { get; set; } = new List<ProjectTaskActivity>();
        [Required]
        [DisplayName("Start Date")]
        public DateTime ProjectTaskStartDate { get; set; }
        [Required]
        [DisplayName("Due Date")]
        public DateTime ProjectTaskDueDate { get; set; }
        [Required]
        [DisplayName("Completed Date")]
        public DateTime ProjectTaskCompletedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }

    }
}