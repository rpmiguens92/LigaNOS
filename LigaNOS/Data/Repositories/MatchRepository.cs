using LigaNOS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public class MatchRepository : GenericRepository<Match>, IMatchRepository
    {
        private readonly DataContext _context;
        public MatchRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<Match> GetAllWithUsers()
        {
            return _context.Matches.Include(m => m.HomeClub).Include(m => m.AwayClub);
        }

        public async Task CreateAsync(Match match)
        {
            await _context.Matches.AddAsync(match);
            await _context.SaveChangesAsync();
        }
    }
}
