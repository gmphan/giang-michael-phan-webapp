using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ProjectView
    {
        // public List<Project> Projects { get; set; }
        // public ProjectView()
        // {
        //     Projects = new List<Project>();
        // }
        public int Id { get; set; }
        public string? ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? ProjectState { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectDueDate { get; set; }
        public DateTime? ProjectCompletedDate { get; set; }
        public List<ProjectTaskView> ProjectTasks { get; set; } = new List<ProjectTaskView>();
    }
}