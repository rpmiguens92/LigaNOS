using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Linq;


namespace LigaNOS.Models
{
    public class PlayersAndClubsViewModel
    {
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Club> Clubs { get; set; }
    }
}
