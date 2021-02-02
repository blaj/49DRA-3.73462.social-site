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
using SocialSite.Service;

namespace SocialSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserService _userService;

        public HomeController(AuthDbContext context, UserManager<ApplicationUser> userManager, IPostRepository postRepository, ICommentRepository commentRepository, IUserService userService)
        {
            _context = context;
            _userManager = userManager;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var posts = _postRepository.FindAll().ToList();
            var userFromPrincipal = await _userManager.GetUserAsync(User);
            var user = _context.ApplicationUsers.Where(u => u.Id == userFromPrincipal.Id).Include(u => u.Friends).Include(u => u.FriendOf).FirstOrDefault();
            var comments = _commentRepository.FindAllByUser(user).ToList();

            var indexViewModel = new IndexViewModel { Posts = posts, Comments = comments, PostCreateRequest = new Dto.Post.PostCreateRequest(), ApplicationUser = user };

            return View(indexViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}