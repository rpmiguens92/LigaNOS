using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;


namespace LigaNOS.Data.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        

        [Display(Name = "Photo")]
        [Required]
        public Guid ImageFile { get; set; }


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

        [Required]
        public string Function { get; set; }

     
    }
}
