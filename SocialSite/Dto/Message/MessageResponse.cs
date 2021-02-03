using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Dto.Message
{
    public class MessageResponse
    {
        public string Content { get; set; }

        public bool IsSendByYou { get; set; }
        public string UserName { get; set; }
    }
}
