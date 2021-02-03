using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialSite.Areas.Identity.Data;
using SocialSite.Data;
using SocialSite.Dto.Message;
using SocialSite.Models;
using SocialSite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SocialSite.Controllers
{
    public class MessageController : AbstractController
    {
        private readonly AuthDbContext _dbContext;
        private readonly IMessageService _messageService;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageController(AuthDbContext dbContext, IMessageService messageService, UserManager<ApplicationUser> userManager) : base(dbContext)
        {
            _dbContext = dbContext;
            _messageService = messageService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<Message>> SendMessage(string recipientUserId, MessageCreateRequest request)
        {
            try
            {
                var sender = await _userManager.GetUserAsync(User);
                var recipient = await _userManager.FindByIdAsync(recipientUserId);

                return _messageService.Create(sender, recipient, request);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<List<MessageResponse>> GetMessages(string id)
        {
            try
            {
                var sender = await _userManager.GetUserAsync(User);
                var recipient = await _userManager.FindByIdAsync(id);

                return _messageService.GetMessageResponses(sender, recipient);
            }
            catch
            {
                return new List<MessageResponse>();
            }
        }
    }
}
