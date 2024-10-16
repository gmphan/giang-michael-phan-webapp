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
        public void SortProjectTasksByCustomStateOrder()
        {
            // Define the custom order for the ProjectState
            var customStateOrder = new List<string> 
            { 
                "WorkingInProcess", 
                "Waiting", 
                "OnHold", 
                "Completed", 
                "Canceled" 
            };

            // Sort the ProjectTasks based on the custom order
            ProjectTasks = ProjectTasks
                .OrderBy(pt => customStateOrder.IndexOf(pt.ProjectTaskState))
                .ToList();
        }
    }
}