using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public interface IMatchRepository : IGenericRepository<Match>
    {
        public IQueryable GetAllWithUsers();
    }
}
