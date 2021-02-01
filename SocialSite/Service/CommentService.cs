using SocialSite.Areas.Identity.Data;
using SocialSite.Dto.Comment;
using SocialSite.Exception;
using SocialSite.Models;
using SocialSite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostService _postService;

        public CommentService(ICommentRepository commentRepository, IPostService postService)
        {
            _commentRepository = commentRepository;
            _postService = postService;
        }

        public bool Create(CommentCreateRequest request, ApplicationUser user)
        {
            var post = _postService.FetchPost(request.PostId);
            return _commentRepository.Create(new Models.Comment { Post = post, Content = request.Content, CreatedOn = DateTime.Now, ApplicationUser = user });
        }

        public bool Delete(int? id)
        {
            var comment = FetchComment(id);
            return _commentRepository.Delete(comment);
        }

        public Comment FetchComment(int? id)
        {
            if (String.IsNullOrWhiteSpace(id.ToString()))
            {
                throw new ArgumentNullException("Id postu nie może być puste!");
            }

            var comment = _commentRepository.FindOneById(id);

            if (comment.Equals(null))
            {
                throw new EntityNotFoundException();
            }

            return comment;
        }
    }

    public interface ICommentService
    {
        bool Create(CommentCreateRequest request, ApplicationUser user);
        bool Delete(int? id);
        Comment FetchComment(int? id);
    }
}
