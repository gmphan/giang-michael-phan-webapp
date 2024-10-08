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
            ProjectView projectView = new ProjectView();
            projectView.Projects  = (List<Project>)await _projectServ.GetAllProjectServAsync();
            return View(projectView);
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}