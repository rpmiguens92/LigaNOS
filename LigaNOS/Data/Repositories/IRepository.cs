using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
