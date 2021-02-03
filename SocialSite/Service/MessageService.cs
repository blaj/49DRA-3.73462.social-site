using SocialSite.Areas.Identity.Data;
using SocialSite.Dto.Message;
using SocialSite.Models;
using SocialSite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Message Create(ApplicationUser sender, ApplicationUser recipient, MessageCreateRequest request)
        {
            return _messageRepository.Create(new Message { Content = request.Content, Sender = sender, Recipient = recipient, SendAt = DateTime.Now });
        }

        public List<MessageResponse> GetMessageResponses(ApplicationUser sender, ApplicationUser recipient)
        {
            var messages = _messageRepository.FindAllBySenderOrRecipient(sender, sender).ToList();
            var messageResponses = new List<MessageResponse>();

            messages.ForEach(message => 
            {
                var isSendByYou = message.SenderId == sender.Id;
                var username = message.Sender.FirstName + " " + message.Sender.LastName;

                messageResponses.Add(new MessageResponse { Content = message.Content, IsSendByYou = isSendByYou, UserName = username });
            });

            return messageResponses;
        }
    }

    public interface IMessageService
    {
        public List<MessageResponse> GetMessageResponses(ApplicationUser sender, ApplicationUser recipient);
        public Message Create(ApplicationUser sender, ApplicationUser recipient, MessageCreateRequest request);
    }
}
