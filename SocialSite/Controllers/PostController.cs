using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using SocialSite.Dto.Common;
using SocialSite.Dto.Post;
using SocialSite.Models;
using SocialSite.Service;

namespace SocialSite.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(IPostService postService, AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "PostCreateRequest")] PostCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                _postService.Create(request, user);
                return RedirectToAction("Index", "Home");
            } 
            catch (SystemException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                var post = _postService.FetchPost(id);
                var postUpdateRequest = new PostUpdateRequest { Title = post.Title, Content = post.Content };
                return View(postUpdateRequest);
            } 
            catch (SystemException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Title, Content")] PostUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            try
            {
                _postService.Update(id, request);
                return RedirectToAction("Index", "Home");
            }
            catch (SystemException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Delete(int? id)
        {
            try
            {
                _postService.Delete(id);
                return RedirectToAction("Index", "Home");
            }
            catch (SystemException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var errorResponse = new ErrorResponse{ Title = "Błąd", Message = message };
            return View(errorResponse);
        }
    }
}
