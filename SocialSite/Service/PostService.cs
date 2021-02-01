using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using SocialSite.Dto.Post;
using SocialSite.Exception;
using SocialSite.Models;
using SocialSite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Service
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostService(IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _postRepository = postRepository;
            _userManager = userManager;
        }

        public bool Create(PostCreateRequest request, ApplicationUser user)
        {

            var post = new Post { Title = request.Title, Content = request.Content, CreatedOn = DateTime.Now, ApplicationUser = user };
            return _postRepository.Create(post);
        }

        public bool Delete(int? id)
        {
            var post = FetchPost(id);
            return _postRepository.Delete(post);
        }

        public bool Update(int? id, PostUpdateRequest request)
        {
            var post = FetchPost(id);

            post.Title = request.Title;
            post.Content = request.Content;

            return _postRepository.Update(post);
        }

        public Post FetchPost(int? id)
        {
            if (String.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException("Id postu nie może być puste!");
            }

            var post = _postRepository.FindOneById(id);

            if (post.Equals(null))
            {
                throw new EntityNotFoundException();
            }

            return post;
        }
    }

    public interface IPostService
    {
        bool Create(PostCreateRequest request, ApplicationUser user);
        bool Update(int? id, PostUpdateRequest request);
        bool Delete(int? id);
        public Post FetchPost(int? id);
    }
}