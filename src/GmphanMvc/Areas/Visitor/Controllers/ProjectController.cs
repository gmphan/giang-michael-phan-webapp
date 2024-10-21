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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}