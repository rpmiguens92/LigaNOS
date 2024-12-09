using System;

namespace LigaNOS.Data.DTO
{
    public class MatchDto
    {
        public int Id { get; set; }
        public string HomeClub { get; set; }
        public string AwayClub { get; set; }
        public int HomeGoals { get; set; }
        public int AwayGoals { get; set; }
        public DateTime MatchDay { get; set; }
        public string MatchTime { get; set; }
        public string Stadium { get; set; }
    }
}
