using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Gmphan.UtilityLib;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Gmphan.BusinessAccessLib
{
    public class ProjectServ : IProjectServ
    {
        private readonly ILogger<ProjectServ> _logger;
        private readonly IUnityOfWork _unityOfWork;
        private readonly IGetTAndCacheGeneric _getTAndCacheGeneric;
        public ProjectServ(ILogger<ProjectServ> logger
                        , IUnityOfWork unityOfWork
                        , IGetTAndCacheGeneric getTAndCacheGeneric)
        {
            _logger = logger;
            _unityOfWork = unityOfWork;
            _getTAndCacheGeneric = getTAndCacheGeneric;
        }
        public async Task AddNewProjectServAsync(Project obj)
        {
            await _unityOfWork.ProjectRepoUOW.AddAsync(obj);
            await _unityOfWork.SaveAsync();
            await _getTAndCacheGeneric.UpdateCacheAsync("project", _unityOfWork.ProjectRepoUOW.GetAllAsync);
        }
        public async Task<List<ProjectView>> GetProjectViewListServAsync()
        {
            IEnumerable<Project> projects =await _getTAndCacheGeneric.GetTAsync("project", _unityOfWork.ProjectRepoUOW.GetAllAsync);
            return projects.Select(p => new ProjectView
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                ProjectDescription = p.ProjectDescription,
                ProjectState = p.ProjectState,
                ProjectStartDate = p.ProjectStartDate,
                ProjectDueDate = p.ProjectDueDate,
                ProjectCompletedDate = p.ProjectCompletedDate,
                ProjectSummary = p.ProjectSummary,
                ProjectTasks = p.ProjectTasks?.Select(pt => new ProjectTaskView
                {
                    // Map properties from ProjectTask to ProjectTaskView here
                    // For example: Id = pt.Id, TaskName = pt.TaskName, etc.
                }).ToList() ?? new List<ProjectTaskView>()
            }).ToList();
        }

        public async Task<ProjectDetailView> GetProjectDetailViewServAsync(int id)
        {
            Project project = await _getTAndCacheGeneric.GetTAsync("project", () => _unityOfWork.ProjectRepoUOW.Get3LayerProjectRepo(id));
            if (project == null)
            {
                return null;
            }
            ProjectDetailView projectDetailView = new ProjectDetailView
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                ProjectDescription = project.ProjectDescription,
                ProjectState = project.ProjectState,
                ProjectStartDate = project.ProjectStartDate,
                ProjectDueDate = project.ProjectDueDate,
                ProjectCompletedDate = project.ProjectCompletedDate,
                ProjectSummary = project.ProjectSummary,
                ProjectTasks = project.ProjectTasks.Select(task => new ProjectTaskView
                {
                    Id = task.Id,
                    ProjectTaskName = task.ProjectTaskName
                }).ToList()
            };
            return projectDetailView;
        }

        public async Task<ProjectTaskDetailView> GetProjectTaskDetailViewServAsync(int id)
        {
            ProjectTask projectTask = await _getTAndCacheGeneric.GetTAsync($"projectTask{id}", () => _unityOfWork.ProjectTaskRepoUOW.GetProjectTaskDetailRepo(id));
            ProjectTaskDetailView projectTaskDetailView = new ProjectTaskDetailView
            {
                Id = projectTask.Id,
                ProjectTaskName = projectTask.ProjectTaskName,
                ProjectTaskDescription = projectTask.ProjectTaskDescription,
                ProjectTaskState = projectTask.ProjectTaskState,
                ProjectTaskStartDate = projectTask.ProjectTaskStartDate,
                ProjectTaskDueDate = projectTask.ProjectTaskDueDate,
                ProjectTaskCompletedDate = projectTask.ProjectTaskCompletedDate,
                ProjectTaskActivities = projectTask.ProjectTaskActivities.Select(
                    activity => new ProjectTaskActivityView
                    {
                        Id = activity.Id,
                        ActivityNote = activity.ActivityNote,
                        CreatedDate = activity.CreatedDate
                    }
                ).ToList()
            };
            return projectTaskDetailView;
        }
         public async Task<ProjectView> GetProjectView3LayerServAsync(int id)
        {
            Project eagerLoadProject = await _getTAndCacheGeneric.GetTAsync("project", () => _unityOfWork.ProjectRepoUOW.Get3LayerProjectRepo(id));
            if (eagerLoadProject == null)
            {
                return null;
            }
            // Map the Project entity to projectView
            var projectView = new ProjectView
            {
                Id = eagerLoadProject.Id,
                ProjectName = eagerLoadProject.ProjectName,
                ProjectDescription = eagerLoadProject.ProjectDescription,
                ProjectState = eagerLoadProject.ProjectState,
                ProjectStartDate = eagerLoadProject.ProjectStartDate,
                ProjectDueDate = eagerLoadProject.ProjectDueDate,
                ProjectCompletedDate = eagerLoadProject.ProjectCompletedDate,
                ProjectSummary = eagerLoadProject.ProjectSummary,
                ProjectTasks = eagerLoadProject.ProjectTasks.Select(task => new ProjectTaskView
                {
                    Id = task.Id,
                    ProjectTaskName = task.ProjectTaskName,
                    ProjectTaskDescription = task.ProjectTaskDescription,
                    ProjectTaskState = task.ProjectTaskState,
                    ProjectTaskStartDate = task.ProjectTaskStartDate,
                    ProjectTaskDueDate = task.ProjectTaskDueDate,
                    ProjectTaskCompletedDate = task.ProjectTaskCompletedDate,
                    ProjectTaskActivities = task.ProjectTaskActivities.Select(activity => new ProjectTaskActivityView
                    {
                        Id = activity.Id,
                        ActivityNote = activity.ActivityNote
                        // CreatedDate = activity.CreatedDate
                    }).ToList()
                }).ToList()
            };

            return projectView;
        }

        public async Task UpdateActivityMainServAsync(ProjectView obj)
        {
            Project activityMain = new Project
            {
                Id = obj.Id,
                ProjectName = obj.ProjectName,
                ProjectSummary = obj.ProjectSummary,
                ProjectDescription = obj.ProjectDescription,
                ProjectState = obj.ProjectState,
                ProjectStartDate = DateTime.SpecifyKind(obj.ProjectStartDate, DateTimeKind.Utc),
                ProjectCompletedDate = obj.ProjectCompletedDate.HasValue
                    ? DateTime.SpecifyKind(obj.ProjectCompletedDate.Value, DateTimeKind.Utc)
                    : (DateTime?)null,
                UpdatedDate = DateTime.UtcNow
            };
            await _unityOfWork.ProjectRepoUOW.UpdateAsync(activityMain);
            await _unityOfWork.SaveAsync();
            await _getTAndCacheGeneric.UpdateCacheAsync("project", _unityOfWork.ProjectRepoUOW.GetAllAsync);
        }

    }
}