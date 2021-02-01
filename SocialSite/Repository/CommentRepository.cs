using SocialSite.Data;
using SocialSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AuthDbContext _dbContext;

        public CommentRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(Comment entity)
        {
            try
            {
                _dbContext.Comments.Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(Comment entity)
        {
            try
            {
                _dbContext.Comments.Remove(entity);
                _dbContext.SaveChanges();
                return true;
            } catch
            {
                return false;
            }
        }

        public Comment FindOneById(int? id)
        {
            return _dbContext.Comments.Where(p => p.Id == id).FirstOrDefault();
        }
    }

    public interface ICommentRepository
    {
        bool Create(Comment entity);
        bool Delete(Comment entity);
        Comment FindOneById(int? id);
    }
}
