using System;
using System.Collections.Generic;
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
            await _unityOfWork.ProjectRepoUOW.AddAsync(obj);
            await _unityOfWork.SaveAsync();
            await _getTAndCacheGeneric.UpdateCacheAsync("allProject", _unityOfWork.ProjectRepoUOW.GetAllAsync);
        }
        public async Task<ProjectListView> GetProjectListViewServAsync()
        {
            IEnumerable<Project> projects =await _getTAndCacheGeneric.GetTAsync(
                "allProject", 
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
        public async Task<List<ProjectView>> GetProjectViewListServAsync()
        {
            IEnumerable<Project> projects =await _getTAndCacheGeneric.GetTAsync("allProject", _unityOfWork.ProjectRepoUOW.GetAllAsync);
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
            Project project = await _getTAndCacheGeneric.GetTAsync($"project{id}", () => _unityOfWork.ProjectRepoUOW.Get3LayerProjectRepo(id));
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
                    ProjectTaskName = task.ProjectTaskName,
                    ProjectTaskState = task.ProjectTaskState //need to sort
                }).ToList()
            };
            projectDetailView.SortProjectTasksByCustomStateOrder();
            return projectDetailView;
        }
        public async Task UpdateProjectDetailServAsync(ProjectDetailView obj)
        {
            Project detailMain = new Project
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
            await _unityOfWork.ProjectRepoUOW.UpdateAsync(detailMain);
            await _unityOfWork.SaveAsync();
            await _getTAndCacheGeneric.UpdateCacheAsync($"project{obj.Id}", () => _unityOfWork.ProjectRepoUOW.Get3LayerProjectRepo(obj.Id));
        }
        public async Task<ProjectTaskDetailView> GetProjectTaskDetailViewServAsync(int id)
        {
            ProjectTask projectTask = await _getTAndCacheGeneric.GetTAsync($"projectTask{id}", () => _unityOfWork.ProjectTaskRepoUOW.GetProjectTaskDetailRepo(id));
            ProjectTaskDetailView projectTaskDetailView = new ProjectTaskDetailView
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
            projectTaskDetailView.SortNoteByCreatedDate();
            return projectTaskDetailView;
        }
        public async Task AddNewProjectTaskServAsync(ProjectTaskDetailView obj)
        {
            ProjectTask projectTask = new ProjectTask
            {
                ProjectTaskName = obj.ProjectTaskName,
                ProjectTaskDescription = obj.ProjectTaskDescription,
                ProjectTaskState = obj.ProjectTaskState,
                ProjectTaskStartDate = DateTime.SpecifyKind(obj.ProjectTaskStartDate, DateTimeKind.Utc),
                ProjectTaskDueDate = DateTime.SpecifyKind(obj.ProjectTaskDueDate, DateTimeKind.Utc),
                ProjectTaskCompletedDate = obj.ProjectTaskCompletedDate.HasValue
                    ? DateTime.SpecifyKind(obj.ProjectTaskCompletedDate.Value, DateTimeKind.Utc)
                    : (DateTime?)null,
                ProjectId = obj.ProjectId //important foreign key
            };
            try
            {
                await _unityOfWork.ProjectTaskRepoUOW.AddAsync(projectTask);
                await _unityOfWork.SaveAsync();
                //I need to update project{id} because the Project Detail will need to be refresh to include the new task
                await _getTAndCacheGeneric.UpdateCacheAsync($"project{obj.ProjectId}", () => _unityOfWork.ProjectRepoUOW.Get3LayerProjectRepo(obj.ProjectId));
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
            
        }
        public async Task<bool> UpdateProjectTaskDetailServAsync(ProjectTaskDetailView obj)
        {
            ProjectTask projectTask = new ProjectTask
            {
                Id = obj.Id,
                ProjectTaskName = obj.ProjectTaskName,
                ProjectTaskDescription = obj.ProjectTaskDescription,
                ProjectTaskState = obj.ProjectTaskState,
                ProjectTaskStartDate = DateTime.SpecifyKind(obj.ProjectTaskStartDate, DateTimeKind.Utc),
                ProjectTaskDueDate = DateTime.SpecifyKind(obj.ProjectTaskDueDate, DateTimeKind.Utc),
                ProjectTaskCompletedDate = obj.ProjectTaskCompletedDate.HasValue
                    ? DateTime.SpecifyKind(obj.ProjectTaskCompletedDate.Value, DateTimeKind.Utc)
                    : (DateTime?)null,
                ProjectId = obj.ProjectId //important foreign key
            };
            try
            {
                await _unityOfWork.ProjectTaskRepoUOW.UpdateAsync(projectTask);
                await _unityOfWork.SaveAsync();
                //I need to update project{id} because the Project Detail will need to be refresh to include the new task
                await _getTAndCacheGeneric.UpdateCacheAsync($"project{obj.ProjectId}", () => _unityOfWork.ProjectRepoUOW.Get3LayerProjectRepo(obj.ProjectId));
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
        }

        public async Task<bool> AddTaskActivityNote(int taskId, string note)
        {
            ProjectTaskActivity projectTaskActivity = new ProjectTaskActivity
            {
                ActivityNote = note,
                ProjectTaskId = taskId,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                await _unityOfWork.ProjectTaskActivityRepoUOW.AddAsync(projectTaskActivity);
                await _unityOfWork.SaveAsync();
                await _getTAndCacheGeneric.UpdateCacheAsync($"projectTask{taskId}", () => _unityOfWork.ProjectTaskRepoUOW.GetProjectTaskDetailRepo(taskId));
                return true;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                throw;
            }
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