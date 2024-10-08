using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ProjectView
    {
        public List<Project> Projects { get; set; }
        public ProjectView()
        {
            Projects = new List<Project>();
        }
    }
}