using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Areas.Identity.Data;
using SocialSite.Dto.Comment;
using SocialSite.Dto.Common;
using SocialSite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Controllers
{
    public class CommentController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommentService _commentService;

        public CommentController(UserManager<ApplicationUser> userManager, ICommentService commentService)
        {
            _userManager = userManager;
            _commentService = commentService;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(Prefix = "CommentCreateRequest")] CommentCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                _commentService.Create(request, user);
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
                _commentService.Delete(id);
                return RedirectToAction("Index", "Home");
            }
            catch (SystemException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public IActionResult Error(string message)
        {
            var errorResponse = new ErrorResponse { Title = "Błąd", Message = message };
            return View(errorResponse);
        }
    }
}
