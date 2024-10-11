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
            List<ProjectView> projectViews = new List<ProjectView>();
            projectViews = await _projectServ.GetProjectViewListServAsync();
            return View(projectViews);
        }

        public async Task<IActionResult> ProjectDetail(int id)
        {
            // Project3LayerView project3LayerView = await _projectServ.Get3LayerProjectServAsync(id);
            ProjectView projectView3Layer = await _projectServ.GetProjectView3LayerServAsync(id);
            if (projectView3Layer == null)
            {
                return NotFound();
            }
            return View(projectView3Layer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}