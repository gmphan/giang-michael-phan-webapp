using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ProjectTaskDetailView
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string? ProjectTaskName { get; set; }
        public string? ProjectTaskDescription { get; set; }
        public string? ProjectTaskState { get; set; }
        public DateTime ProjectTaskStartDate { get; set; }
        public DateTime ProjectTaskDueDate { get; set; }
        public DateTime? ProjectTaskCompletedDate { get; set; }
        public List<ProjectTaskActivityView> ProjectTaskActivities { get; set; } = new List<ProjectTaskActivityView>();
    }
}