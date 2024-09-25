using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.DataAccessLib.Repository;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmphanMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class QuoteAdminController : Controller
    {
        private readonly ILogger<QuoteController> _logger;
        private readonly IQuoteCollectionServ _quoteCollectionServ;

        public QuoteAdminController(ILogger<QuoteController> logger
                                    , IQuoteCollectionServ quoteCollectionServ)
        {
            _logger = logger;
            _quoteCollectionServ = quoteCollectionServ;
        }

        public async Task<IActionResult> Index()
        {
            QuoteView quoteView = new QuoteView();
            quoteView.QuoteCollections = (List<QuoteCollection>)await _quoteCollectionServ.GetAllQuoteCollectionAsync();
            return View(quoteView);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(QuoteCollection quoteCollectionObj)
        {
            if(ModelState.IsValid)
            {
                quoteCollectionObj.CreatedDate = DateTime.UtcNow;
                quoteCollectionObj.UpdatedDate = quoteCollectionObj.CreatedDate;
                await _quoteCollectionServ.AddQuoteCollectionAsync(quoteCollectionObj);
                return RedirectToAction("Index", "QuoteAdmin");
            }
            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            QuoteCollection quoteCollection = await _quoteCollectionServ.GetQuoteCollectionAsync(id);
            return View(quoteCollection);
        }
        [HttpPost]
        public async Task<IActionResult> Update(QuoteCollection obj)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // Ensure CreatedDate is treated as Utc if the form submission altered it
            obj.CreatedDate = DateTime.SpecifyKind(obj.CreatedDate, DateTimeKind.Utc);
            obj.UpdatedDate = DateTime.UtcNow;
            await _quoteCollectionServ.UpdateQuoteCollectionAsync(obj);
            return RedirectToAction("Index", "QuoteAdmin");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            QuoteCollection quoteCollection = await _quoteCollectionServ.GetQuoteCollectionAsync(id);
            if (quoteCollection == null)
            {
                return NotFound();
            }
            return View(quoteCollection);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {
            QuoteCollection obj = await _quoteCollectionServ.GetQuoteCollectionAsync(id);
            if(obj == null)
            {
                return NotFound();
            }
            await _quoteCollectionServ.DeleteQuoteCollectionAsync(obj);
            return RedirectToAction("Index", "QuoteAdmin");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}