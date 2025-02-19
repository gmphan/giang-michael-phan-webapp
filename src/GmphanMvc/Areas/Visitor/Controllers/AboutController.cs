using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.ModelLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmphanMvc.Areas.Visitor.Controllers
{
    [Area("Visitor")]
    public class AboutController : Controller
    {
        private readonly ILogger<AboutController> _logger;
        private readonly IAboutServ _aboutServ;

        public AboutController(ILogger<AboutController> logger
                                , IAboutServ aboutServ)
        {
            _logger = logger;
            _aboutServ = aboutServ;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}