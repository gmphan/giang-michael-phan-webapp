using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Gmphan.UtilityLib;
using Microsoft.EntityFrameworkCore;
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
            obj.ProjectStartDate = DateTime.SpecifyKind(obj.ProjectStartDate, DateTimeKind.Utc);
            obj.ProjectDueDate = DateTime.SpecifyKind(obj.ProjectDueDate, DateTimeKind.Utc);    
            obj.CreatedDate = DateTime.UtcNow;
            obj.UpdatedDate = obj.CreatedDate;
            await _unityOfWork.ProjectRepoUOW.AddAsync(obj);
            await _unityOfWork.SaveAsync();
            await _getTAndCacheGeneric.UpdateCacheAsync("projectList", _unityOfWork.ProjectRepoUOW.GetAllAsync);
        }
        public async Task<ProjectListView> GetProjectListViewServAsync()
        {
            IEnumerable<Project> projects =await _getTAndCacheGeneric.GetTAsync(
                "projectList", 
                _unityOfWork.ProjectRepoUOW.GetAllAsync
            );

            // Initialize the ProjectListView
            var projectListView = new ProjectListView();
            // Iterate through each project and map it to ProjectView
            foreach (var project in projects)
            {
                var projectView = new ProjectView
                {
                    Id = project.Id,
                    ProjectName = project.ProjectName,
                    ProjectDescription = project.ProjectDescription,
                    ProjectState = project.ProjectState,
                    ProjectStartDate = project.ProjectStartDate,
                    ProjectDueDate = project.ProjectDueDate,
                    ProjectCompletedDate = project.ProjectCompletedDate,
                    ProjectSummary = project.ProjectSummary
                    // Map other properties as needed
                };

                // Add the ProjectView to the ProjectViews list
                projectListView.ProjectViews.Add(projectView);
            }
            // Optionally, sort the projects by state order
            projectListView.SortProjectsByStateOrder();
            return projectListView;
        }
        public async Task<ProjectDetailView> GetProjectDetailViewServAsync(int id)
        {
            Project project = await _getTAndCacheGeneric.GetTAsync($"projectDetail{id}", () => _unityOfWork.ProjectRepoUOW.GetProjectDetailRepo(id));
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
                ProjectTaskViews = project.ProjectTasks.Select(task => new ProjectTaskView
                {
                    Id = task.Id,
                    ProjectTaskName = task.ProjectTaskName,
                    ProjectTaskDescription = task.ProjectTaskDescription,
                    ProjectTaskState = task.ProjectTaskState, //need to sort
                    ProjectTaskStartDate = task.ProjectTaskStartDate,
                    ProjectTaskDueDate = task.ProjectTaskDueDate,
                    ProjectTaskCompletedDate = task.ProjectTaskCompletedDate
                }).ToList()
            };
            projectDetailView.SortProjectTasksByCustomStateOrder();
            return projectDetailView;
        }

        public async Task<bool> UpdateProjectServAsync(ProjectDetailView obj)
        {
            Project project = new Project
            {
                Id = obj.Id,
                ProjectName = obj.ProjectName,
                ProjectSummary = obj.ProjectSummary,
                ProjectDescription = obj.ProjectDescription,
                ProjectState = obj.ProjectState,
                ProjectStartDate = DateTime.SpecifyKind(obj.ProjectStartDate, DateTimeKind.Utc),
                ProjectDueDate = DateTime.SpecifyKind(obj.ProjectDueDate, DateTimeKind.Utc),
                ProjectCompletedDate = obj.ProjectCompletedDate.HasValue
                    ? DateTime.SpecifyKind(obj.ProjectCompletedDate.Value, DateTimeKind.Utc)
                    : (DateTime?)null,
                UpdatedDate = DateTime.UtcNow
            };
            try
            {
                await _unityOfWork.ProjectRepoUOW.UpdateAsync(project);
                await _unityOfWork.SaveAsync();

                // Remember none controller query for project by itself but ProjectDetail which included task list, so cache keeping ProjectDetail not just project
                await _getTAndCacheGeneric.UpdateCacheAsync($"projectDetail{obj.Id}", () => _unityOfWork.ProjectRepoUOW.GetProjectDetailRepo(obj.Id));
                // Need to refresh the cache for the ProjectList since we have to re-render the list again.
                await _getTAndCacheGeneric.UpdateCacheAsync($"projectList", _unityOfWork.ProjectRepoUOW.GetAllAsync);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateProjectServAsync error");
                return false;
            }
        }

        public async Task<ProjectTaskView> GetProjectTaskViewServAsync(int id)
        {
            ProjectTask projectTask = await _getTAndCacheGeneric.GetTAsync($"projectTask{id}", () => _unityOfWork.ProjectTaskRepoUOW.GetProjectTaskDetailRepo(id));
            ProjectTaskView projectTaskView = new ProjectTaskView
            {
                Id = projectTask.Id,
                ProjectId = projectTask.ProjectId,
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
            projectTaskView.SortNoteByCreatedDate();
            return projectTaskView;
        }
        
        
        public async Task<bool> AddNewTaskSerAsync(ProjectTask obj)
        {
            obj.ProjectTaskStartDate = DateTime.SpecifyKind(obj.ProjectTaskStartDate, DateTimeKind.Utc);
            obj.ProjectTaskDueDate = DateTime.SpecifyKind(obj.ProjectTaskDueDate, DateTimeKind.Utc);
            obj.CreatedDate = DateTime.UtcNow;
            obj.UpdatedDate = DateTime.UtcNow;
            obj.ProjectTaskCompletedDate = obj.ProjectTaskCompletedDate.HasValue
                            ? DateTime.SpecifyKind(obj.ProjectTaskCompletedDate.Value, DateTimeKind.Utc)
                            : (DateTime?)null;
            try
            {
                await _unityOfWork.ProjectTaskRepoUOW.AddAsync(obj);
                await _unityOfWork.SaveAsync();
                // need to update cache for projectDetail because after add it redirect to Project Admin Detail page
                await _getTAndCacheGeneric.UpdateCacheAsync($"projectDetail{obj.ProjectId}", () => _unityOfWork.ProjectRepoUOW.GetProjectDetailRepo(obj.ProjectId));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddNewTaskServAsync error");
                return false;
            }
        }
        
        public async Task<bool> UpdateTaskSerAsync(ProjectTaskView obj)
        {
            ProjectTask projectTask = new ProjectTask
            {
                Id = obj.Id,
                ProjectId = obj.ProjectId,
                ProjectTaskName = obj.ProjectTaskName,
                ProjectTaskDescription = obj.ProjectTaskDescription,
                ProjectTaskState = obj.ProjectTaskState,
                ProjectTaskStartDate = DateTime.SpecifyKind(obj.ProjectTaskStartDate, DateTimeKind.Utc),
                ProjectTaskDueDate = DateTime.SpecifyKind(obj.ProjectTaskDueDate, DateTimeKind.Utc),
                ProjectTaskCompletedDate = obj.ProjectTaskCompletedDate.HasValue
                            ? DateTime.SpecifyKind(obj.ProjectTaskCompletedDate.Value, DateTimeKind.Utc)
                            : (DateTime?)null,
                UpdatedDate = DateTime.UtcNow
            };
            try
            {
                await _unityOfWork.ProjectTaskRepoUOW.UpdateAsync(projectTask);
                await _unityOfWork.SaveAsync();
                // update cache for the task
                await _getTAndCacheGeneric.UpdateCacheAsync($"projectTask{obj.Id}", () => _unityOfWork.ProjectTaskRepoUOW.GetProjectTaskDetailRepo(obj.Id));
                // also need to update cache for the project that have that task since the task is showing under the task detail
                await _getTAndCacheGeneric.UpdateCacheAsync($"projectDetail{obj.ProjectId}", () => _unityOfWork.ProjectRepoUOW.GetProjectDetailRepo(obj.ProjectId));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateTaskServAsync error");
                return false;
            }
        }

        public async Task<bool> AddTaskNoteServAsync(ProjectTaskActivity obj)
        {
            obj.CreatedDate = DateTime.UtcNow;
            try
            {
                await _unityOfWork.ProjectTaskActivityRepoUOW.AddAsync(obj);
                await _unityOfWork.SaveAsync();
                // after add note, the controller will redirect to Task action to render task detail with note again.
                await _getTAndCacheGeneric.UpdateCacheAsync($"projectTask{obj.ProjectTaskId}", () => _unityOfWork.ProjectTaskRepoUOW.GetProjectTaskDetailRepo(obj.ProjectTaskId));
                return true;
            }
            catch (Exception ex) 
            { 
                _logger.LogError(ex, "AddTaskNoteServAsync Ex Error");
                return false;
            }
        }
    }
}