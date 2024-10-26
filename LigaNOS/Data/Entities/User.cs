using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;


namespace LigaNOS.Data.Entities
{
    public class User : IdentityUser 
    {
        [Display(Name = "Image")]
        public Guid ImageFileId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public User user { get; set; }

        public string ImageFullPath => ImageFileId == Guid.Empty
          ? $"https://liganos.azurewebsites.net/images/noimage.jpg"
         : $"https://liganos.blob.core.windows.net/users/{ImageFileId}";


    }
}
