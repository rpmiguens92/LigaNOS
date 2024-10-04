using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using LigaNOS.Models;

namespace LigaNOS.Data.Repositories
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetAllPlayersAsync();
        Task<Player> GetPlayerByIdAsync(int id);
        Task CreatePlayerAsync(Player player);
        Task UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
    }
}
