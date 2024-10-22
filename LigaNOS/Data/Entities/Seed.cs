using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using LigaNOS.Helpers;
using System.Linq;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using LigaNOS.Controllers;
using Microsoft.AspNetCore.Hosting.Server;

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

            
            await EnsureRoleExistsAsync("Admin");
            await EnsureRoleExistsAsync("Emplo");
            await EnsureRoleExistsAsync("Club");

           
            var userAdmin = await CreateUserAndAssignRoleAsync("miguens.rp@gmail.com", "Rita", "Miguens", "Admin", "123456");
            var userEmplo = await CreateUserAndAssignRoleAsync("miguel@cinel.pt", "Miguel", "Miguens", "Emplo", "123456");
            var userClub = await CreateUserAndAssignRoleAsync("maria@cinel.pt", "Maria", "Miguens", "Club", "123456");

            
            if (!_context.Clubs.Any())
            {
                AddClub("Sport Lisboa e Benfica", "Jorge Jesus", "Estádio do Benfica", userAdmin);
                AddClub("Ericeirense", "Jorge Deus", "Estádio do Ericeira", userAdmin);
                AddClub("FC Porto", "Sérgio Conceição", "Estádio do Dragão", userAdmin);
                AddClub("Estrela da Amadora", "Bernardo Cruz", "Estádio da Amadora", userAdmin);

                await _context.SaveChangesAsync();
            }

            var clubs = _context.Clubs.ToList();
            if (clubs.Count < 2)
            {
                throw new InvalidOperationException("Not enough clubs to create matches.");
            }

            
            if (!_context.Players.Any())
            {
                foreach (var club in clubs)
                {
                    AddPlayer(club, userAdmin);
                }
                await _context.SaveChangesAsync();
            }

            
            if (!_context.Matches.Any())
            {
                AddMatch(userAdmin);
                await _context.SaveChangesAsync();
            }
        }

        private async Task EnsureRoleExistsAsync(string roleName)
        {
            var roleExists = await _userHelper.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _userHelper.CheckRoleAsync(roleName);
            }
        }

        private async Task<User> CreateUserAndAssignRoleAsync(string email, string firstName, string lastName, string role, string password)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email
                };

                var result = await _userHelper.AddUserAsync(user, password);
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException($"Could not create the {role} user.");
                }
            }

            if (!await _userHelper.IsUserInRoleAsync(user, role))
            {
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private void AddClub(string name, string coach, string stadium, User user)
        {
            _context.Clubs.Add(new Club
            {
                Name = name,
                Coach = coach,
                Stadium = stadium,
                User = user
            });
        }

        private void AddPlayer(Club club, User user)
        {
            _context.Players.Add(new Player
            {
                Name = GenerateRandomPlayerName(),
                DateOfBirth = GenerateRandomDateOfBirth(),
                Position = GenerateRandomPosition(),
                ClubId = club.Id,
                User = user
            });
        }

        private void AddMatch(User user)
        {
            var clubs = _context.Clubs.ToList();
            if (clubs.Count < 2)
            {
                throw new InvalidOperationException("Not enough clubs to create a match.");
            }

            var homeClub = clubs[_random.Next(clubs.Count)];
            Club awayClub;

            do
            {
                awayClub = clubs[_random.Next(clubs.Count)];
            } while (awayClub.Id == homeClub.Id);

            _context.Matches.Add(new Match
            {
                MatchDay = GenerateRandomMatchDay(),
                MatchTime = GenerateRandomMatchTime(),
                HomeClub = homeClub,
                AwayClub = awayClub,
                Stadium = homeClub.Stadium,
                User = user
            });
        }

        private DateTime GenerateRandomDateOfBirth()
        {
            int daysToSubtract = _random.Next(18 * 365, 40 * 365);
            return DateTime.Today.AddDays(-daysToSubtract);
        }

        private string GenerateRandomPlayerName()
        {
            string[] playerNames = { "DiMaria", "Pepe", "CR7", "Moreira", "Mantorras", "Figo" };
            return playerNames[_random.Next(playerNames.Length)];
        }

        private string GenerateRandomPosition()
        {
            string[] positions = { "Forward", "Midfielder", "Defender", "Goalkeeper" };
            return positions[_random.Next(positions.Length)];
        }

        private string GenerateRandomMatchTime()
        {
            string[] matchTimes = { "19:15", "19:30", "19:45", "20:00", "20:15", "20:30", "20:45", "21:00", "21:15" };
            return matchTimes[_random.Next(matchTimes.Length)];
        }

        private DateTime GenerateRandomMatchDay()
        {
            int daysToAdd = _random.Next(0, 31);
            return DateTime.Today.AddDays(daysToAdd);
        }
    }
}
   
 
