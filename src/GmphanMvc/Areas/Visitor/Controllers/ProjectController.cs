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

namespace GmphanMvc.Areas.Visitor.Controllers
{
    [Area("Visitor")]
    public class ProjectController : Controller
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IProjectServ _projectServ;

        public ProjectController(ILogger<ProjectController> logger
                                , IProjectServ projectServ)
        {
            _logger = logger;
            _projectServ = projectServ;
        }

        public async Task<IActionResult> Index()
        {
            // List<ProjectView> projectViews = new List<ProjectView>();
            // projectViews = await _projectServ.GetProjectViewListServAsync();
            // return View(projectViews);
            ProjectListView projectListView = await _projectServ.GetProjectListViewServAsync();
            return View(projectListView);
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

        public async Task<IActionResult> Task(int id)
        {
            ProjectTaskView projectTaskView = await _projectServ.GetProjectTaskViewServAsync(id);
            return View(projectTaskView);
        }

        // [HttpPost]
        // [IgnoreAntiforgeryToken]
        // public async Task<IActionResult> _DetailMain(ProjectDetailView obj)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         // need to return a error message here
        //         return View(obj);
        //     }
        //     await _projectServ.UpdateProjectDetailServAsync(obj);
        //     return PartialView(obj);
        // }

        // public async Task<IActionResult> _TaskDetail(int id)
        // {
        //     ProjectTaskDetailView projectTaskDetailView = await _projectServ.GetProjectTaskDetailViewServAsync(id);
        //     return PartialView("_TaskDetail", projectTaskDetailView);
        // }
        // public async Task<IActionResult> ProjectDetail(int id)
        // {
        //     // Project3LayerView project3LayerView = await _projectServ.Get3LayerProjectServAsync(id);
        //     ProjectView projectView3Layer = await _projectServ.GetProjectView3LayerServAsync(id);
        //     if (projectView3Layer == null)
        //     {
        //         return NotFound();
        //     }
        //     return View(projectView3Layer);
        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}