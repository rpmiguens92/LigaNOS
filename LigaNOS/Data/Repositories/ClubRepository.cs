using LigaNOS.Data.Entities;

namespace LigaNOS.Data.Repositories
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        private readonly DataContext _context;
        public ClubRepository(DataContext context) : base(context)
        {
            _context = context;
        }
    }
}
