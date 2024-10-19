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
        public Task AddNewProjectServAsync(Project obj);
        public Task<ProjectListView> GetProjectListViewServAsync();
        public Task<ProjectDetailView> GetProjectDetailViewServAsync(int id);
        public Task<ProjectTaskDetailView> GetProjectTaskDetailViewServAsync(int id); // will remove this after fix admin
        public Task<ProjectTaskView> GetProjectTaskViewServAsync(int id);
        public Task UpdateProjectDetailServAsync(ProjectDetailView obj);
        public Task AddNewProjectTaskServAsync(ProjectTaskDetailView obj);
        public Task<bool> UpdateProjectTaskDetailServAsync(ProjectTaskDetailView obj);
        public Task<bool> AddTaskActivityNote(int taskId, string note);

        //below will be clean out
        public Task<List<ProjectView>> GetProjectViewListServAsync();
        public Task<ProjectView> GetProjectView3LayerServAsync(int id);
        public Task UpdateActivityMainServAsync(ProjectView obj);
    }
}