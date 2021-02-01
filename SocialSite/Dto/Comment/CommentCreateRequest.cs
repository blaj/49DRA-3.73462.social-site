using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Dto.Comment
{
    public class CommentCreateRequest
    {
        public int PostId { get; set; }

        [Display(Name = "Treść")]
        [Required(ErrorMessage = "Treść komentarza nie może być pusta.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
