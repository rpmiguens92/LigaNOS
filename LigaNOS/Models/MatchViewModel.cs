using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Models
{
    public class MatchViewModel 
    {
        public int Id { get; set; }

        [Display(Name = "Teams")]
        public IFormFile ImageFile { get; set; }
        [Display(Name = "Home Club")]
        public int HomeClubId { get; set; }
        [Display(Name = "Away Club")]
        public int AwayClubId { get; set; }  

        public string HomeClub { get; set; }
        public string AwayClub { get; set; }

        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }

        public string HomeClubImagePath { get; set; }   
        public string AwayClubImagePath { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime MatchDay { get; set; }
        [Display(Name = "Match Time")]
        public string MatchTime { get; set; }
        [Display(Name = "Stadium")]
        public string Stadium { get; set; }
    }
}
