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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuoteCollectionServ _quoteCollectionServ;

        public HomeController(ILogger<HomeController> logger
                            , IQuoteCollectionServ quoteCollectionServ)
        {
            _logger = logger;
            _quoteCollectionServ = quoteCollectionServ;
        }

        public async Task<IActionResult> Index()
        {
            HomeView homeView = new HomeView();
            homeView.QuoteCollections = (List<QuoteCollection>)await _quoteCollectionServ.GetAllQuoteCollectionAsync();
            return View(homeView);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}