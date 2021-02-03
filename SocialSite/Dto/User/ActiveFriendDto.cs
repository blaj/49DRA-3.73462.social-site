using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Dto.User
{
    public class ActiveFriendDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ActivityType ActivityType { get; set; }
    }

    public enum ActivityType
    {
        OFFLINE,
        AWAY,
        ONLINE
    }
}
