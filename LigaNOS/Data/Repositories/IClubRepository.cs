using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public interface IClubRepository : IGenericRepository<Club>
    {
        public IQueryable GetAllWithUsers();
        Task<bool> HasMatchesAsync(int clubId);
        Task<List<Club>> GetAllAsync();
    }
}