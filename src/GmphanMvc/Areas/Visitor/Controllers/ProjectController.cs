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
            ProjectView projectView = new ProjectView();
            projectView.Projects  = (List<Project>)await _projectServ.GetAllProjectServAsync();
            return View(projectView);
        }

        public async Task<IActionResult> ProjectDetail(int id)
        {
            Project3LayerView project3LayerView = await _projectServ.Get3LayerProjectServAsync(id);
            if (project3LayerView == null)
            {
                return NotFound();
            }
            return View(project3LayerView);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}