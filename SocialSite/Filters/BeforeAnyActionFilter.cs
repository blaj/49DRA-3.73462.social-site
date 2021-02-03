using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Filters
{
    public class BeforeAnyActionFilter : IActionFilter
    {
        private readonly AuthDbContext _dbContext;

        public BeforeAnyActionFilter(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var currentUser = context.HttpContext.User;

            if (currentUser != null)
            {
                var user = _dbContext.ApplicationUsers.Where(u => u.UserName == currentUser.Identity.Name).FirstOrDefault();
                user.LastActivity = DateTime.Now;

                _dbContext.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }
}
