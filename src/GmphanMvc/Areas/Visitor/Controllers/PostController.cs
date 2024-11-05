using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Gmphan.BusinessAccessLib;
using Gmphan.ModelLib;
using Gmphan.ModelLib.ViewModels;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GmphanMvc.Areas.Visitor.Controllers
{
    [Area("Visitor")]
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostServ _postServ;

        public PostController(ILogger<PostController> logger
                            , IPostServ postServ)
        {
            _logger = logger;
            _postServ = postServ;
        }

        public async Task<IActionResult> Index()
        {
            PostMetaView postMetaView = await _postServ.GetPostMetaViewServAsync();
            return View(postMetaView);
        }

        public async Task<IActionResult> GetPostDetail(int id)
        {
            PostDetailView postDetailView = await _postServ.GetPostDetailViewServAsync(id);
            return PartialView("_Detail", postDetailView);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _postServ.AddNewPostServAsync(obj);
            return RedirectToAction("index");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}