using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IEnumerable<SelectListItem> GetComboClubs()
        {
            var list = _context.Clubs.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Name
            }).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a club...)",
                Value = "0"
            });
            return list;
        }
            public async Task<bool> HasMatchesAsync(int clubId)
        {

            return await _context.Matches.AnyAsync(m => m.HomeClubId == clubId || m.AwayClubId == clubId);
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.FindAsync(id);
        }

        public async Task<List<Club>> GetAllAsync()
        {
            return await _context.Clubs.ToListAsync();
        }
    }
}
