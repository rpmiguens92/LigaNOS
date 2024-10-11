namespace LigaNOS.Models
{
    public class ClubStatViewModel
    {
        public string ClubName { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded
        {
            get; set;
        }
    }
}
