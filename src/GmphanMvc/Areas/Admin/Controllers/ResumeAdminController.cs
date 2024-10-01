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
    public class ResumeAdminController : Controller
    {
        private readonly ILogger<ResumeAdminController> _logger;
        private readonly IResumeServ _resumeServ;
        public ResumeAdminController(ILogger<ResumeAdminController> logger
                                    , IResumeServ resumeServ)
        {
            _logger = logger;
            _resumeServ = resumeServ;
        }

        public async Task<IActionResult> Index()
        {
            ResumeHeader resumeHeader = await _resumeServ.GetLatestResumeHeaderAsync();
            ResumeSummary resumeSummary = await _resumeServ.GetLatestResumeSummaryAsync();
            IEnumerable<ResumeExperience> resumeExperienceList = await _resumeServ.GetAllResumeExperienceAsync();

            ResumeView resumeView = new ResumeView
            {
                ResumeHeader = resumeHeader,
                ResumeSummary = resumeSummary,
                ResumeExperiences = (List<ResumeExperience>)resumeExperienceList
            };
            return View(resumeView);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}