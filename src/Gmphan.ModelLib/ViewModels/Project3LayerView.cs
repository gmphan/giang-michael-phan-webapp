using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class Project3LayerView 
    {
        public Project Project { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
        public List<ProjectTaskActivity> ProjectTaskActivities { get; set; }

        // Add this to capture the selected task ID
        public int SelectedTaskId { get; set; }
        public Project3LayerView()
        {
            Project = new Project();
            ProjectTasks = new List<ProjectTask>();
            ProjectTaskActivities = new List<ProjectTaskActivity>();
        }
    }
}