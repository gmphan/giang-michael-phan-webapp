using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ProjectListView
    {
        public List<ProjectView> ProjectViews { get; set; }
        public ProjectListView()
        {
            ProjectViews = new List<ProjectView>();
        }
        public void SortProjectsByStateOrder()
        {
            // Define the custom order for the ProjectState
            var stateOrder = new List<string> 
            { 
                "WorkingInProcess", 
                "Waiting", 
                "OnHold", 
                "Completed", 
                "Canceled" 
            };

            // Sort the Projects based on the custom order
            ProjectViews = ProjectViews
                .OrderBy(pt => stateOrder.IndexOf(pt.ProjectState))
                .ToList();
        }
    }
}