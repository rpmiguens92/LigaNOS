using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;

namespace LigaNOS.Data.Entities
{
    public class Employee : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid ImageFileId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters lenght.")]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public string Role { get; set; }

        public User User { get; set; }

 

        public string ImageFullPath => ImageFileId == Guid.Empty
        ? $"https://liganos.azurewebsites.net/images/noimage.jpg"
       : $"https://liganos.blob.core.windows.net/employees/{ImageFileId}";
    }
 
}
