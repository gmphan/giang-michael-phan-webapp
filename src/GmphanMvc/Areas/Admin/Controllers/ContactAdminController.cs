using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.ModelLib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmphanMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactAdminController : Controller
    {
        private readonly ILogger<ContactAdminController> _logger;
        private readonly IContactServ _contactServ;

        public ContactAdminController(ILogger<ContactAdminController> logger
                                    , IContactServ contactServ)
        {
            _logger = logger;
            _contactServ = contactServ;
        }

        public async Task<IActionResult> Index()
        {
            ContactMeListView contactMeListView = await _contactServ.GetContactMeListViewSerAsync();
            return View(contactMeListView);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}