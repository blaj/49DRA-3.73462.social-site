using SocialSite.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Models
{
    public class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime SendAt { get; set; }

        public string SenderId { get; set; }
        public string RecipientId { get; set; }

        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Recipient { get; set; }
    }
}
