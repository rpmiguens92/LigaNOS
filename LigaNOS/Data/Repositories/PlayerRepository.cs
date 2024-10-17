using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace LigaNOS.Data.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private readonly DataContext _context;
        public PlayerRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable GetAllWithUsers()
        {
            return _context.Players.Include(p => p.Clubs);
        }
        public IEnumerable<SelectListItem> GetComboPlayers()
        {
            var list = _context.Players.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = $"{p.Id}"
            }).ToList();
           
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a player...)",
                Value = "0"
            });

            return list;
        }
    }
}

