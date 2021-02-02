using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using SocialSite.Dto.Common;
using SocialSite.Dto.User;
using SocialSite.Repository;
using SocialSite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialSite.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostRepository _postRepository;
        private readonly IUserService _userService;
        private readonly AuthDbContext _dbContext;

        public UserController(UserManager<ApplicationUser> userManager, IPostRepository postRepository, IUserService userService, AuthDbContext dbContext)
        {
            _userManager = userManager;
            _postRepository = postRepository;
            _userService = userService;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(string? id)
        {
            try
            {
                var user = _dbContext.ApplicationUsers.Where(u => u.Id == id).Include(u => u.Friends).Include(u => u.FriendOf).ThenInclude(u => u.ApplicationUser).FirstOrDefault();
                var loggedUser = await _userManager.GetUserAsync(User);
                var posts = _postRepository.FindAllByUser(user).ToList();
                var isAlreadyFriends = _userService.IsAlreadyFriends(user, loggedUser);

                return View(new DetailsViewModel { ApplicationUser = user, Posts = posts, IsAlreadyFriends = isAlreadyFriends });
            } 
            catch (SystemException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }


        public async Task<IActionResult> AddFriend(string? friendId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var friend = await _userManager.FindByIdAsync(friendId);
                _userService.AddFriend(friend, user);

                return RedirectToAction(nameof(Details), new { id = friendId });
            }
            catch (SystemException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> RemoveFriend(string? friendId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var friend = await _userManager.FindByIdAsync(friendId);
                _userService.RemoveFriend(friend, user);

                return RedirectToAction(nameof(Details), new { id = friendId });
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
