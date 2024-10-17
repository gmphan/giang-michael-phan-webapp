using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmphanMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProjectAdminController : Controller
    {
        private readonly ILogger<ProjectAdminController> _logger;
        private readonly IProjectServ _projectServ;

        public ProjectAdminController(ILogger<ProjectAdminController> logger
                                    , IProjectServ projectServ)
        {
            _logger = logger;
            _projectServ = projectServ;
        }

        public async Task<IActionResult> Index()
        {
            // List<ProjectView> projectViews = new List<ProjectView>();
            // projectViews = await _projectServ.GetProjectViewListServAsync();
            ProjectListView projectListView = await _projectServ.GetProjectListViewServAsync();
            return View(projectListView);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project obj)
        {
            if (ModelState.IsValid)
            {
                obj.ProjectStartDate = DateTime.SpecifyKind(obj.ProjectStartDate, DateTimeKind.Utc);
                obj.ProjectDueDate = DateTime.SpecifyKind(obj.ProjectDueDate, DateTimeKind.Utc);    
                obj.CreatedDate = DateTime.UtcNow;
                obj.UpdatedDate = obj.CreatedDate;
                await _projectServ.AddNewProjectServAsync(obj);
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            ProjectDetailView projectDetailView = await _projectServ.GetProjectDetailViewServAsync(id);
            projectDetailView.SortProjectTasksByCustomStateOrder();
            if (projectDetailView == null)
            {
                return NotFound();
            }
            return View(projectDetailView);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> _DetailMain(ProjectDetailView obj)
        {
            if (!ModelState.IsValid)
            {
                // need to return a error message here
                return View(obj);
            }
            await _projectServ.UpdateProjectDetailServAsync(obj);
            return PartialView(obj);
        }
        public async Task<IActionResult> _TaskDetail(int id)
        {
            ProjectTaskDetailView projectTaskDetailView = await _projectServ.GetProjectTaskDetailViewServAsync(id);
            return PartialView("_TaskDetail", projectTaskDetailView);
        }

        [HttpPost]
        public async Task<IActionResult> _TaskDetail(ProjectTaskDetailView obj)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            bool success = await _projectServ.UpdateProjectTaskDetailServAsync(obj);
            if (success)
            {
                return Ok(); // HTTP 200 status
            }
            else
            {
                return StatusCode(500, "Failed to add note"); // HTTP 500 status with message
            }
        }

        public async Task<IActionResult> _CreateTask(int projectId)
        {
            // Create a new ProjectTaskDetailView and set the ProjectId
            var model = new ProjectTaskDetailView
            {
                ProjectId = projectId // Pass the ProjectId to the model
            };
            // Return the partial view with the model
            return PartialView("_CreateTask", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _CreateTask(ProjectTaskDetailView model)
        {
            if (!ModelState.IsValid)
            {
                // need to have tempdata to display error message 
                return RedirectToAction("Detail", new { id = model.ProjectId });
            }
            // add task to Tasks table 
            await _projectServ.AddNewProjectTaskServAsync(model);

            // model.ProjectId as second parameter will not work because it being treated as query string value
            // not as a route parameter name id. the route value explicitly using an anonymous object
            return RedirectToAction("Detail", new { id = model.ProjectId });
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddTaskNote(int taskId, string note)
        {
            bool success = await _projectServ.AddTaskActivityNote(taskId, note);

            if (success)
            {
                ProjectTaskDetailView projectTaskDetailView = await _projectServ.GetProjectTaskDetailViewServAsync(taskId);
                // return Ok(); // HTTP 200 status
                return PartialView("_AddTaskNote", projectTaskDetailView);
            }
            else
            {
                return StatusCode(500, "Failed to add note"); // HTTP 500 status with message
            }
        }
        // public async Task<IActionResult> Activity(int id)
        // {
        //     // Project3LayerView project3LayerView = await _projectServ.Get3LayerProjectServAsync(id);
        //     ProjectView projectView3Layer = await _projectServ.GetProjectView3LayerServAsync(id);
        //     if (projectView3Layer == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(projectView3Layer);
        // }

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> _ActivityMain(ProjectView obj)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         await _projectServ.UpdateActivityMainServAsync(obj);
        //         return PartialView("_ActivityMain", obj);
        //     }
        //     return PartialView(obj);
        // }

        // [HttpPost]
        // // [ValidateAntiForgeryToken] can't have this, the ajax will break due to the parameters not within a model
        // public async Task<IActionResult> _ActivityTask(
        //                                             int SelectedTaskId
        //                                             ,string TaskDescription
        //                                             , string TaskState, DateTime TaskStartDate
        //                                             , DateTime TaskDueDate, DateTime? TaskCompletedDate
        //                                             , List<string> TaskActivities
        //                                             )
        // {
        //     return PartialView("_ActivityTask");
        // }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}