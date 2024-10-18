using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LigaNOS.Models
{
    public class StatViewModel : Stat
    {
        public List<MatchViewModel> MatchResults { get; set; }
        public List<ClubStatViewModel> ClubStats { get; set; }

    }
}



