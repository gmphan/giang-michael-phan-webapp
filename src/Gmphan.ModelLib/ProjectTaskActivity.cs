using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib
{
    public class ProjectTaskActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Activity Note")]
        public string? ActivityNote { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public int ProjectTaskId { get; set; }
        [ForeignKey("ProjectTaskId")]
        public ProjectTask ProjectTask { get; set; }
    }
}