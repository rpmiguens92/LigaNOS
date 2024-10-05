using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        public Task CreateMatchAsync(Match match)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteMatchAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Match> GetMatchByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateMatchAsync(Match match)
        {
            throw new System.NotImplementedException();
        }
    }
}
