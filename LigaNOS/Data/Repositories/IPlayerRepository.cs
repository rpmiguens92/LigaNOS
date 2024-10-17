using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using LigaNOS.Models;
using System.Linq;

namespace LigaNOS.Data.Repositories
{
    public interface IPlayerRepository : IGenericRepository<Player>
    {
        public IQueryable GetAllWithUsers();
        
    }
}
