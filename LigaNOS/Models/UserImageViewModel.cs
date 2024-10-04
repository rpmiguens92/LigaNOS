using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
    public class UserImageViewModel: User
    {
 
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
