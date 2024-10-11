using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
    public class MatchViewModel : Match
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        public string HomeClub { get; set; }
        public string AwayClub { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public DateTime MatchDate { get; set; }
        public string MatchTime { get; set; }
        public string Stadium { get; set; }
    }
}
