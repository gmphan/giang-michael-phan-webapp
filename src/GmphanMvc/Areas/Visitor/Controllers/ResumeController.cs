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
    public class ResumeController : Controller
    {
        private readonly ILogger<ResumeController> _logger;
        private readonly IResumeServ _resumeServ;

        public ResumeController(ILogger<ResumeController> logger
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