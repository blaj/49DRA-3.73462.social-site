using SocialSite.Data;
using SocialSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialSite.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly AuthDbContext _dbContext;

        public PostRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Create(Post entity)
        {
            try
            {
                _dbContext.Posts.Add(entity);
                _dbContext.SaveChanges();
                return true;
            } 
            catch
            {
                return false;
            }
        }

        public Post FindOneById(int? id)
        {
            return _dbContext.Posts.Where(p => p.Id == id).FirstOrDefault();
        }

        public IEnumerable<Post> FindAll()
        {
            return _dbContext.Posts.Include(p => p.ApplicationUser).Include(p => p.Comments).OrderByDescending(p => p.CreatedOn).ToList();
        }

        public bool IsPostExistsById()
        {
            throw new NotImplementedException();
        }

        public bool Delete(Post entity)
        {
            try
            {
                _dbContext.Posts.Remove(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Post entity)
        {
            try
            {
                _dbContext.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }
    }

    public interface IPostRepository
    {
        Post FindOneById(int? id);
        bool Create(Post entity);
        bool Update(Post entity);
        bool Delete(Post entity);
        bool IsPostExistsById();
        IEnumerable<Post> FindAll();
    }
}
