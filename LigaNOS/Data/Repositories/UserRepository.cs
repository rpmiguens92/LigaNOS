using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaNOS.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task CreateAsync(User entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetByIdAsync(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(User entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
