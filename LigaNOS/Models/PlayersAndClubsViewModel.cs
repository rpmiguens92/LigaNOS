using LigaNOS.Data.Entities;
using System.Collections.Generic;

namespace LigaNOS.Models
{
    public class PlayersAndClubsViewModel
    {
        public IEnumerable<Player> Players { get; set; }
        public IEnumerable<Club> Clubs { get; set; }
    }
}
