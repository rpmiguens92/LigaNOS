using LigaNOS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DataContext _context;
        public PlayerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task CreatePlayerAsync(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlayerAsync(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            return await _context.Players.Include(p => p.Clubs).ToListAsync();//verificar again
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _context.Players.Include(p => p.ClubId)
                                         .FirstOrDefaultAsync(p => p.PlayerId == id);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            _context.Players.Update(player);
            await _context.SaveChangesAsync();
        }
    }
}

