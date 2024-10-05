using System;
using System.ComponentModel.DataAnnotations;

namespace LigaNOS.Data.Entities
{
    public class Match : IEntity
    {
        public int MatchId { get; set; }

        public int HomeClubId { get; set; }

        //[Display(Name = "HomeClub")]
        //[Required]
        //public Guid ImageFile { get; set; }

        public Club HomeClub { get; set; }

        public int AwayClubId { get; set; }

        public Club AwayClub { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public DateTime MatchDay { get; set; }

        public string Stadium { get; set; }

        public string MatchTime { get; set; }

    }
}
