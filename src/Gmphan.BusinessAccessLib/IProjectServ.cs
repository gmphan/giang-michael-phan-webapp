using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;

namespace Gmphan.BusinessAccessLib
{
    public interface IProjectServ
    {
        // Share between Admin and Visitor
        public Task AddNewProjectServAsync(Project obj);
        public Task<ProjectListView> GetProjectListViewServAsync();
        public Task<ProjectDetailView> GetProjectDetailViewServAsync(int id);
        public Task<ProjectTaskView> GetProjectTaskViewServAsync(int id);     

        // Below are all for Admin Area
        public Task<bool> UpdateProjectServAsync(ProjectDetailView obj);
        public Task<bool> AddNewTaskSerAsync(ProjectTask obj);
       
    }
}