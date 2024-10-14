using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ProjectDetailView
    {
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? ProjectState { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectDueDate { get; set; }
        public DateTime? ProjectCompletedDate { get; set; }
        public string? ProjectSummary { get; set; }
        public List<ProjectTaskView> ProjectTasks { get; set; } = new List<ProjectTaskView>();
    }
}