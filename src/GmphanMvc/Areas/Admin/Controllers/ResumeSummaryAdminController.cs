using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.ModelLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmphanMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ResumeSummaryAdminController : Controller
    {
        private readonly ILogger<ResumeSummaryAdminController> _logger;
        private readonly IResumeServ _resumeServ;

        public ResumeSummaryAdminController(ILogger<ResumeSummaryAdminController> logger
                                        , IResumeServ resumeServ)
        {
            _logger = logger;
            _resumeServ = resumeServ;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Edit()
        {
            ResumeSummary resumeSummary = await _resumeServ.GetLatestResumeSummaryAsync();
            if (resumeSummary == null)
            {
                return NotFound();
            }
            return View(resumeSummary);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ResumeSummary obj)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            // Ensure CreatedDate is treated as Utc if the form submission altered it
            obj.CreatedDate = DateTime.SpecifyKind(obj.CreatedDate, DateTimeKind.Utc);
            obj.UpdatedDate = DateTime.UtcNow;
            await _resumeServ.UpdateResumeSummaryAsync(obj);
            return RedirectToAction("index", "ResumeAdmin");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}