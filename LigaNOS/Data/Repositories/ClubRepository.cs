using LigaNOS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LigaNOS.Data.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        private readonly DataContext _context;
        public ClubRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable GetAllWithUsers()
        {
            return _context.Clubs.Include(c => c.User);
        }

    }
}
