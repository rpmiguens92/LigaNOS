namespace LigaNOS.Data.DTO
{
    public class StatDto
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public string HomeClub { get; set; }
        public string AwayClub { get; set; }
        public int HomeClubGoals { get; set; }
        public int AwayClubGoals { get; set; }
        public int HomeClubGoalsConceded { get; set; }
        public int AwayClubGoalsConceded { get; set; }
        public int HomeClubPoints { get; set; }
        public int AwayClubPoints { get; set; }
        public string MatchDay { get; set; }
    }
}
