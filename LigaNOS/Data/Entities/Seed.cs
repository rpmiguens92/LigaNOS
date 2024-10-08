using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using LigaNOS.Helpers;
using System.Linq;
using System.Xml.Linq;

namespace LigaNOS.Data.Entities
{
    public class Seed

    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;
        public Seed(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            var user = await _userHelper.GetUserByEmailAsync("miguens.rp@gmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Rita",
                    LastName = "Miguens",
                    Email = "miguens.rp@gmail.com",
                    UserName = "miguens.rp@gmail.com",
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }
            if (!_context.Players.Any())
            {
                AddPlayer("Cristiano Ronaldo", user);
                AddPlayer("Pepe", user);
                await _context.SaveChangesAsync(); // Ensure changes are saved to the database

            }


        }

        private void AddPlayer(string name, User user)
        {
            _context.Players.Add(new Player
            {
                Name = name,
                DateOfBirth = DateTime.Today,
                Position = "Forward",
                ClubId = _context.Clubs.FirstOrDefault().Id,
                User = user,
            });
          
        }
    }
}
   
 
