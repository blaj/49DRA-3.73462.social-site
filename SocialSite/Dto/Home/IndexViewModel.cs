﻿using SocialSite.Areas.Identity.Data;
using SocialSite.Dto.Comment;
using SocialSite.Dto.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Dto.Home
{
    public class IndexViewModel
    {
        public PostCreateRequest PostCreateRequest { get; set; }
        public List<Models.Post> Posts { get; set; }
        public ApplicationUser ApplicationUser { get; set;  }
        public CommentCreateRequest CommentCreateRequest { get; set; }
    }
}
