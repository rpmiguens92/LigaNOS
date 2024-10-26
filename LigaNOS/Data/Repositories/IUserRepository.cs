using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public interface IUserRepository 
    {

        Task<User> GetUserByIdAsync(string userId);

        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<IdentityResult> UpdateUserAsync(User user);

        Task<IdentityResult> DeleteUserAsync(User user);
    }
}
