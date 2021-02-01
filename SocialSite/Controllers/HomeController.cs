using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialSite.Data;
using SocialSite.Dto.Home;
using SocialSite.Models;

namespace SocialSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AuthDbContext _context;

        public HomeController(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts.Include(p => p.ApplicationUser).OrderByDescending(p => p.CreatedOn).ToListAsync();
            var indexViewModel = new IndexViewModel { Posts = posts, PostCreateRequest = new Dto.Post.PostCreateRequest() };

            return View(indexViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}