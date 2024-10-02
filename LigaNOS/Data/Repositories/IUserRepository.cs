using LigaNOS.Data.Entities;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
