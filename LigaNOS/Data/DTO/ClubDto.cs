using System;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Data.DTO
{
    public class ClubDto
    {

        public int Id { get; set; }

        [Display(Name = "Symbol")]
        public string ImageUrl { get; set; } // Usado para fornecer o caminho completo da imagem.

        [Display(Name = "Club")]
        public string Name { get; set; }

        public string Coach { get; set; }

        public string Stadium { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int Draws { get; set; }

    }
}
