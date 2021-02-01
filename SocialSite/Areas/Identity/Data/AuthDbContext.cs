using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialSite.Areas.Identity.Data;
using SocialSite.Models;

namespace SocialSite.Data
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>(EntityFrameworkQueryableExtensions =>
            {

            });

            builder.Entity<Comment>(EntityFrameworkQueryableExtensions =>
            {

            });

            base.OnModelCreating(builder);
        }
    }
}
