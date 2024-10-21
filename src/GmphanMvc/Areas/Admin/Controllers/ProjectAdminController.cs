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
                await _projectServ.AddNewProjectServAsync(obj);
                return RedirectToAction("Index");
            }
            // add tempdata for unsuccessfull adding
            return View(obj);
        }

        public async Task<IActionResult> Detail(int id)
        {
            ProjectDetailView projectDetailView = await _projectServ.GetProjectDetailViewServAsync(id);
            return View(projectDetailView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail (ProjectDetailView obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            bool result = await _projectServ.UpdateProjectServAsync(obj);
            if(result) 
            {
                //need to include successful tempdata  
                return View(obj); 
            }  

            // if result is false
            return View(obj); //later need to add error tempdata here          
        }

        public async Task<IActionResult> Task(int id)
        {
            ProjectTaskView projectTaskView = await _projectServ.GetProjectTaskViewServAsync(id);
            return View(projectTaskView);
        }

        // public async Task<IActionResult> TaskCreate()
        // {
        //     return View();
        // }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _TaskCreate(ProjectTask obj)
        {
            if (ModelState.IsValid)
            {
                bool result = await _projectServ.AddNewTaskSerAsync(obj);
                if (result) 
                {
                    // add tempdata later
                    return RedirectToAction("Detail", new { Id = obj.ProjectId });
                }
                return BadRequest();
            }
            // need to add tempdate
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}