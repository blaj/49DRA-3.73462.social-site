using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SocialSite.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Controllers
{
    public abstract class AbstractController : Controller
    {
        private readonly AuthDbContext _dbContext;

        public AbstractController(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
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
    }
}
