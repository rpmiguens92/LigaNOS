using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaNOS.Data.Entities
{
    public class Player : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Photo")]
        [Required]
        public Guid ImageFileId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        

        [Required]
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [MaxLength(50)]
        public string Position { get; set; }

      
        [Display(Name = "Club")]
     
        public int ClubId { get; set; }


        [ForeignKey("ClubId")]
    
        public Club Clubs { get; set; }
        public User User { get; set; }
        public string ImageFullPath => ImageFileId == Guid.Empty
           ? $"https://liganos.azurewebsites.net/images/noimage.jpg"
          : $"https://liganos.blob.core.windows.net/players/{ImageFileId}";
    }
}
