using Microsoft.AspNetCore.Identity;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialSite.Dto.User;

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

        public ICollection<ActiveFriendDto> GetActiveFriends(ApplicationUser user)
        {
            var userFromDb = _dbContext.ApplicationUsers.Where(u => u.Id == user.Id).Include(u => u.Friends).FirstOrDefault();
            var friends = userFromDb.Friends.Where(f => f.ApplicationUserId == user.Id).ToList();
            var friends2 = userFromDb.FriendOf.Where(f => f.FriendId == user.Id).ToList();

            var activeFriendsDto = new List<ActiveFriendDto>();

            friends.ForEach(friend =>
            {
                var diffTime = DateTime.Now - friend.Friend.LastActivity;
                var activityType = ActivityType.OFFLINE;

                if (diffTime.TotalMinutes < 1)
                {
                    activityType = ActivityType.ONLINE;
                } 
                else if (diffTime.TotalMinutes >= 1 && diffTime.TotalMinutes < 5)
                {
                    activityType = ActivityType.AWAY;
                }

                var activeFriend = new ActiveFriendDto { Id = friend.FriendId, FirstName = friend.Friend.FirstName, LastName = friend.Friend.LastName, ActivityType = activityType };

                activeFriendsDto.Add(activeFriend);
            });

            friends2.ForEach(friend =>
            {
                var diffTime = DateTime.Now - friend.ApplicationUser.LastActivity;
                var activityType = ActivityType.OFFLINE;

                if (diffTime.TotalMinutes < 1)
                {
                    activityType = ActivityType.ONLINE;
                }
                else if (diffTime.TotalMinutes >= 1 && diffTime.TotalMinutes < 5)
                {
                    activityType = ActivityType.AWAY;
                }

                var activeFriend = new ActiveFriendDto { Id = friend.ApplicationUserId, FirstName = friend.ApplicationUser.FirstName, LastName = friend.ApplicationUser.LastName, ActivityType = activityType };

                activeFriendsDto.Add(activeFriend);
            });

            return activeFriendsDto;
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

        ICollection<ActiveFriendDto> GetActiveFriends(ApplicationUser user);
    }
}
