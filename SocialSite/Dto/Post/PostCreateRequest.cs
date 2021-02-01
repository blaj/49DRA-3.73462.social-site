using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Dto.Post
{
    public class PostCreateRequest
    {

        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł posta nie może byc pusty.")]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [Display(Name = "Treść")]
        [Required(ErrorMessage = "Treść posta nie może być pusta.")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}
