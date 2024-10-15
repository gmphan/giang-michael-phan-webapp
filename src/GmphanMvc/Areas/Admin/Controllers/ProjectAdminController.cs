using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmphanMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            List<ProjectView> projectViews = new List<ProjectView>();
            projectViews = await _projectServ.GetProjectViewListServAsync();
            return View(projectViews);
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
            if (projectDetailView == null)
            {
                return NotFound();
            }
            return View(projectDetailView);
        }

        public async Task<IActionResult> _TaskDetail(int id)
        {
            ProjectTaskDetailView projectTaskDetailView = await _projectServ.GetProjectTaskDetailViewServAsync(id);
            return PartialView("_TaskDetail", projectTaskDetailView);
        }

        public async Task<IActionResult> _CreateTask()
        {
            // Assuming you have the necessary logic to create the model
            ProjectTaskDetailView model = new ProjectTaskDetailView();

            // Return the _CreateTask partial view with the model
            return PartialView("_CreateTask", model);
        }

        public async Task<IActionResult> Activity(int id)
        {
            // Project3LayerView project3LayerView = await _projectServ.Get3LayerProjectServAsync(id);
            ProjectView projectView3Layer = await _projectServ.GetProjectView3LayerServAsync(id);
            if (projectView3Layer == null)
            {
                return NotFound();
            }
            return View(projectView3Layer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _ActivityMain(ProjectView obj)
        {
            if (ModelState.IsValid)
            {
                await _projectServ.UpdateActivityMainServAsync(obj);
                return PartialView("_ActivityMain");
            }
            return PartialView(obj);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken] can't have this, the ajax will break due to the parameters not within a model
        public async Task<IActionResult> _ActivityTask(
                                                    int SelectedTaskId
                                                    ,string TaskDescription
                                                    , string TaskState, DateTime TaskStartDate
                                                    , DateTime TaskDueDate, DateTime? TaskCompletedDate
                                                    , List<string> TaskActivities
                                                    )
        {
            return PartialView("_ActivityTask");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}