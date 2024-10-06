using LigaNOS.Data.Entities;
using System.Collections.Generic;
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
    }
}
