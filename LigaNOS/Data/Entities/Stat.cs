using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace LigaNOS.Data.Entities
{
    public class Stat  : IEntity
    {
        public int Id { get; set; }
        public Match Match { get; set; }
        public int MatchId { get; set; }
        public Club HomeClub { get; set; }
        public int HomeClubId { get; set; }
        public Club AwayClub { get; set; }
        public int AwayClubId { get; set; }
        public int HomeClubGoals { get; set; }
        public int AwayClubGoals { get; set; }
        public int HomeClubGoalsConceded => AwayClubGoals;
        public int AwayClubGoalsConceded => HomeClubGoals;
        public int HomeClubPoints
        {
            get
            {
                if (HomeClubGoals > AwayClubGoals) return 3;
                if (HomeClubGoals == AwayClubGoals) return 1;
                return 0;
            }
        }
        public int AwayClubPoints
        {
            get
            {
                if (AwayClubGoals > HomeClubGoals) return 3;
                if (AwayClubGoals == HomeClubGoals) return 1;
                return 0;
            }
        }
        public ICollection<Club> Clubs { get; set; }
        public ICollection<Match> Matches { get; set; }
        public User User { get; set; }
    }
}
