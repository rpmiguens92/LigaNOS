using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaNOS.Data.Entities
{
    public class Match : IEntity
    {
        public int Id { get; set; }

        public Club HomeClub { get; set; }

        public Club AwayClub { get; set; }

        public int HomeGoals { get; set; }

        public int AwayGoals { get; set; }

        public DateTime MatchDay { get; set; }

        public string Stadium { get; set; }

        public string MatchTime { get; set; }

        [ForeignKey("HomeClubId")]
        public int? HomeClubId { get; set; }
        [ForeignKey("AwayClubId")]
        public int? AwayClubId { get; set; }  

        public User User { get; set; }

 

    }
}
