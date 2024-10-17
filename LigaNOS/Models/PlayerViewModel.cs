using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
    public class PlayerViewModel : Player
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
        
    }
}
