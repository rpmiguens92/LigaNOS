using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace LigaNOS.Data.Entities
{
    public class Club : IEntity
    {
        public int Id { get; set; }

        [Display(Name = "Symbol")]
        [Required]
        public Guid ImageFile { get; set; }

        [Display(Name = "Club")]
        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Coach { get; set; }

        public string Stadium { get; set; }

        public ICollection<Player> Players { get; set; }
        public User User { get; set; }

        
    }
}
