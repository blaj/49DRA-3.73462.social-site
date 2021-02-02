using SocialSite.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Models
{
    public class ApplicationUserFriend
    {

        public string FriendId { get; set; }
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser Friend { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
