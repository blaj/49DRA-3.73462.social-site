using SocialSite.Areas.Identity.Data;
using SocialSite.Dto.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Dto.User
{
    public class DetailsViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }
        public CommentCreateRequest CommentCreateRequest { get; set; }
        public List<Models.Post> Posts { get; set; }
        public bool IsAlreadyFriends { get; set; }
    }
}
