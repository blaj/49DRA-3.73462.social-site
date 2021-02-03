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

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasOne<ApplicationUser>(p => p.ApplicationUser)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.ApplicationUserId);

            builder.Entity<Comment>(b =>
            {
                b.HasOne<ApplicationUser>(c => c.ApplicationUser)
                    .WithMany(c => c.Comments)
                    .HasForeignKey(c => c.ApplicationUserId);

                b.HasOne<Post>(c => c.Post)
                    .WithMany(c => c.Comments)
                    .HasForeignKey(c => c.PostId);
            });

            builder.Entity<ApplicationUserFriend>(b => 
            {
                b.HasKey(u => new { u.ApplicationUserId, u.FriendId });

                b.HasOne(u => u.ApplicationUser)
                    .WithMany(u => u.Friends)
                    .HasForeignKey(u => u.ApplicationUserId);

                b.HasOne(u => u.Friend)
                    .WithMany(u => u.FriendOf)
                    .HasForeignKey(u => u.FriendId);
            });

            builder.Entity<Message>(b => 
            {
                b.HasOne(m => m.Sender)
                    .WithMany(m => m.SendMessages)
                    .HasForeignKey(m => m.SenderId);

                b.HasOne(m => m.Recipient)
                    .WithMany(m => m.ReceiveMessages)
                    .HasForeignKey(m => m.RecipientId);
            });

            base.OnModelCreating(builder);
        }
    }
}
