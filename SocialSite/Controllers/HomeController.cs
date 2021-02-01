using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using SocialSite.Dto.Home;
using SocialSite.Models;
using SocialSite.Repository;

namespace SocialSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postRepository;

        public HomeController(AuthDbContext context, UserManager<ApplicationUser> userManager, IPostRepository postRepository)
        {
            _context = context;
            _userManager = userManager;
            _postRepository = postRepository;
        }

        public async Task<IActionResult> Index()
        {
            var posts = _postRepository.FindAll().ToList();
            var user = await _userManager.GetUserAsync(User);

            var indexViewModel = new IndexViewModel { Posts = posts, PostCreateRequest = new Dto.Post.PostCreateRequest(), ApplicationUser = user };

            return View(indexViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}