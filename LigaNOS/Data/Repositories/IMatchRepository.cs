using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match> GetMatchByIdAsync(int id);
        Task CreateMatchAsync(Match match);
        Task UpdateMatchAsync(Match match); Task DeleteMatchAsync(int id);

    }
}
