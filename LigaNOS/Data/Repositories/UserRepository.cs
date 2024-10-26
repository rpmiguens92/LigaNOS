using LigaNOS.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
     
        public class UserRepository  : IUserRepository 
        { 
            private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
    
     

            public async Task<IdentityResult> AddUserAsync(User user, string password)
            {
            return await _userManager.CreateAsync(user, password);
        }

            public async Task<IdentityResult> DeleteUserAsync(User user)
            {
            return await _userManager.DeleteAsync(user);
        }

            public IQueryable GetAllWithRoles()
            {
                return _context.Users;
            }

            public Task<User> GetUserByEmailAsync(string email)
            {
                throw new System.NotImplementedException();
            }

            public async Task<User> GetUserByIdAsync(string userId)
            {
            return await _userManager.FindByIdAsync(userId);
            }

            public async Task<IdentityResult> UpdateUserAsync(User user)
            {
            return await _userManager.UpdateAsync(user);
            }
        }
    }