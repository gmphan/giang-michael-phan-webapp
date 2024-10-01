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
    public class ResumeHeaderAdminController : Controller
    {
        private readonly ILogger<ResumeHeaderAdminController> _logger;
        private readonly IResumeServ _resumeServ;

        public ResumeHeaderAdminController(ILogger<ResumeHeaderAdminController> logger
                                        , IResumeServ resumeServ)
        {
            _logger = logger;
            _resumeServ = resumeServ;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Edit()
        {
            ResumeHeader resumeHeader = await _resumeServ.GetLatestResumeHeaderAsync();
            if (resumeHeader == null)
            {
                return NotFound();
            }
            return View(resumeHeader);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ResumeHeader obj)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            // Ensure CreatedDate is treated as Utc if the form submission altered it
            obj.CreatedDate = DateTime.SpecifyKind(obj.CreatedDate, DateTimeKind.Utc);
            obj.UpdatedDate = DateTime.UtcNow;
            await _resumeServ.UpdateResumeHeaderAsync(obj);
            return RedirectToAction("Index", "ResumeAdmin");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}