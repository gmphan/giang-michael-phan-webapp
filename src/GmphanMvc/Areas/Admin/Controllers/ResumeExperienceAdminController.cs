using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
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
    public class ResumeExperienceAdminController : Controller
    {
        private readonly ILogger<ResumeExperienceAdminController> _logger;
        private readonly IResumeServ _resumeServ;

        public ResumeExperienceAdminController(ILogger<ResumeExperienceAdminController> logger
                                            , IResumeServ resumeServ)
        {
            _logger = logger;
            _resumeServ = resumeServ;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var model = new ExperienceAdminView
            {
                ResumeExperience = new ResumeExperience
                {
                    CurrentlyWorkHere = false,
                },
                ResumeDescriptions = new List<ResumeDescription>
                {
                    new ResumeDescription(), new ResumeDescription() // Two initial description fields
                }
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExperienceAdminView obj)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            // Check if the value is being toggled correctly
            // Console.WriteLine($"Currently working here: {obj.ResumeExperience.CurrentlyWorkHere}");

            await _resumeServ.CreateResumeExperienceAsync(obj);

            return RedirectToAction("index", "ResumeAdmin");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}