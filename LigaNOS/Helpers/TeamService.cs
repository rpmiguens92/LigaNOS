using LigaNOS.Data;
using LigaNOS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaNOS.Helpers
{
    public class TeamService : ITeamService
    {
        private readonly DataContext _context;

        public TeamService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Club>>GetTeamsAsync()
        {
            return await _context.Clubs.ToListAsync();
        }
         
    }
}
