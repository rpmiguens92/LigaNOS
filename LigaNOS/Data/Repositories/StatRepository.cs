using LigaNOS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace LigaNOS.Data.Repositories
{
    public class StatRepository : GenericRepository<Stat>, IStatRepository
    {
        private readonly DataContext _context;
        public StatRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<Stat> GetAllWithUsers()
        {
            return _context.Stats.Include(c => c.User); 
        }


        public IQueryable<Stat> GetAll()
        {
            return _context.Stats;
        }
    }
} 
