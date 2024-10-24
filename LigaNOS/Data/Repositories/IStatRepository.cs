using LigaNOS.Data.Entities;
using System.Linq;

namespace LigaNOS.Data.Repositories
{
    public interface IStatRepository : IGenericRepository<Stat>
    {
        public IQueryable<Stat> GetAll();
 

    }
}

