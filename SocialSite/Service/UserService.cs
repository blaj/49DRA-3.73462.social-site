using Microsoft.AspNetCore.Identity;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialSite.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _dbContext;

        public UserService(UserManager<ApplicationUser> userManager, AuthDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public bool AddFriend(ApplicationUser friend, ApplicationUser user)
        {
            try
            {
                var userFromDb = _dbContext.ApplicationUsers.Where(u => u.Id == user.Id).Include(u => u.Friends).FirstOrDefault();
                var newUserFriend = new Models.ApplicationUserFriend { Friend = friend, FriendId = friend.Id, ApplicationUser = user, ApplicationUserId = user.Id };

                if (IsAlreadyFriends(friend, user))
                {
                    return false;
                }

                userFromDb.Friends.Add(newUserFriend);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsAlreadyFriends(ApplicationUser friend, ApplicationUser user)
        {
            var userFromDb = _dbContext.ApplicationUsers.Where(u => u.Id == user.Id).Include(u => u.Friends).FirstOrDefault();
            return userFromDb.Friends.Where(f => f.ApplicationUserId == user.Id && f.FriendId == friend.Id).Any();
        }

        public bool RemoveFriend(ApplicationUser friend, ApplicationUser user)
        {
            try
            {
                var userFromDb = _dbContext.ApplicationUsers.Where(u => u.Id == user.Id).Include(u => u.Friends).FirstOrDefault();

                if (!IsAlreadyFriends(friend, user))
                {
                    return false;
                }

                var existingUserFriend = userFromDb.Friends.Where(f => f.ApplicationUserId == user.Id && f.FriendId == friend.Id).FirstOrDefault();

                userFromDb.Friends.Remove(existingUserFriend);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public interface IUserService
    {
        bool AddFriend(ApplicationUser friend, ApplicationUser user);
        bool RemoveFriend(ApplicationUser friend, ApplicationUser user);
        bool IsAlreadyFriends(ApplicationUser friend, ApplicationUser user);
    }
}
