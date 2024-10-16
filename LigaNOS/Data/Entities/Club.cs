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
        public Guid ImageFileId { get; set; }

        [Display(Name = "Club")]
        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The field {0} can contain {1} characters length.")]
        public string Coach { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public string Stadium { get; set; }
        public ICollection<Match> HomeMatches { get; set; }
        public ICollection<Match> AwayMatches { get; set; }
        public ICollection<Stat> HomeStats { get; set; }
        public ICollection<Stat> AwayStats { get; set; }

        public ICollection<Player> Players { get; set; }
        public User User { get; set; }

        public string ImageFullPath => ImageFileId == Guid.Empty
             ? $"https://liganos.azurewebsites.net/images/noimage.jpg"
            : $"https://liganos.blob.core.windows.net/clubs/{ImageFileId}";


    }
}
