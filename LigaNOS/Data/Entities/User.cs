using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
 

namespace LigaNOS.Data.Entities
{
    public class User : IdentityUser
    {

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(9, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        public string Document { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
