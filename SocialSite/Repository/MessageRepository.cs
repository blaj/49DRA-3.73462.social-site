using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using SocialSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialSite.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AuthDbContext _dbContext;

        public MessageRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Message Create(Message message)
        {
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
            return message;
        }

        public ICollection<Message> FindAllBySenderOrRecipient(ApplicationUser sender, ApplicationUser recipient)
        {
            return _dbContext.Messages
                .Where(m => m.SenderId == sender.Id || m.RecipientId == recipient.Id)
                .Include(m => m.Sender)
                .Include(m => m.Recipient)
                .OrderByDescending(m => m.SendAt)
                .ToList();
        }
    }

    public interface IMessageRepository
    {
        public ICollection<Message> FindAllBySenderOrRecipient(ApplicationUser sender, ApplicationUser recipient);
        public Message Create(Message message);
    }
}
