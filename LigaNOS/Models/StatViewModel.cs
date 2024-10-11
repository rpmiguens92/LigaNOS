using System.Collections.Generic;

namespace LigaNOS.Models
{
    public class StatViewModel
    {
        public List<MatchViewModel> MatchResults { get; set; }
        public List<ClubStatViewModel> ClubStats{ get; set; }
    }
}
