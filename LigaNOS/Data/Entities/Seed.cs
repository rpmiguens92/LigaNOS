using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using LigaNOS.Helpers;
using System.Linq;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using LigaNOS.Controllers;

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
            if (!_context.Clubs.Any())
            {
                AddClub("Sport Lisboa e Benfica", "Jorge Jesus", "Estádio do Benfica", user);
                AddClub("Ericeirense", "Jorge Deus", "Estádio do Ericeira", user);
                AddClub("FC Porto", "Sérgio Conceição", "Estádio do Dragão", user);
                AddClub("Estrela da Amadora", "Bernardo Cruz", "Estádio da Amadora", user);

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
                    AddPlayer(club, user);
                }
                await _context.SaveChangesAsync();
            }

            if (!_context.Matches.Any())
            {
                AddMatch(user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddClub(string name, string coach, string stadium,User user)
        {
                _context.Clubs.Add(new Club
                {
                    Name = name,
                    Coach = coach,  
                    Stadium = stadium,
                    User = user,
                });
        }

        private void AddPlayer( Club clubs, User user)
        {
            _context.Players.Add(new Player
            {
                Name = GenerateRandomPlayerName(),
                DateOfBirth = GenerateRandomDateOfBirth(),
                Position = GenerateRandomPosition(),
                ClubId = clubs.Id,
                User = user,
            });
          
        }

        private void AddMatch( User user)
        { // Obter todos os clubes do banco de dados
            var clubs = _context.Clubs.ToList();

            if (clubs.Count < 2)
            {
                throw new InvalidOperationException("Not enough clubs to create a match.");
            }

            // Seleciona dois clubes aleatórios, garantindo que sejam diferentes
            var homeClub = clubs[_random.Next(clubs.Count)];
            Club awayClub;

            do
            {
                awayClub = clubs[_random.Next(clubs.Count)];
            } while (awayClub.Id == homeClub.Id);  // Garante que os clubes não sejam os mesmos

            _context.Matches.Add(new Match
            {
                MatchDay = GenerateRandomMatchDay(),
                MatchTime = GenerateRandomMatchTime(),
                HomeClub = homeClub,
                AwayClub = awayClub,
                Stadium = homeClub.Stadium, // Definir o estádio para o estádio do clube da casa
                User = user,
            });
        }
        private DateTime GenerateRandomDateOfBirth()//returns a random date between 18 and 40 years ago
        {
            int daysToSubtract = _random.Next(18 * 365, 40 * 365);
            return DateTime.Today.AddDays(-daysToSubtract);
        }
        private string GenerateRandomPlayerName()
        {
            string[] playerNames = { "DiMaria", "Pepe", "CR7", "Moreira", "Mantorras", "Figo"};
            string playerName = playerNames[_random.Next(playerNames.Length)];
            return playerName;
        }
        private string GenerateRandomCoach()
        {
            string[] coachNames= { "Pedro","Felipe", "José", "André", "Justino", "Jorge"};
            string[] coachSurnames = { "Pereira", "Rodrigues", "Santos", "Silva", "Conceição", "Teles" };
          
            string coachName = coachNames[_random.Next(coachNames.Length)];
            string coachSurname = coachSurnames[_random.Next(coachSurnames.Length)];

            return $" {coachName} {coachSurname}";
        }

        private string GenerateRandomClubName() 
        { 
            string[] clubNames = { "FC Porto", "SL Benfica", "Sporting CP", "SC Braga", "Vitória SC", "Boavista FC", "CD Tondela", "FC Famalicão", "Moreirense FC", "CD Santa Clara", "CS Marítimo", "CD Nacional", "Rio Ave FC", "Gil Vicente FC", "FC Paços de Ferreira", "Portimonense SC", "CD Aves", "Belenenses SAD", "CD Feirense", "GD Chaves" };
           string clubName = clubNames[_random.Next(clubNames.Length)];
            return clubName;
        }

        private string GenerateRandomPosition()
        {
            string[] positions = { "Forward", "Midfielder", "Defender", "Goalkeeper" };
            return positions[_random.Next(positions.Length)];
        }

     

        private string GenerateRandomMatchTime()
        {
            string[] matchTimes = { "19:15", "19:30", "19:45", "20:00","20:15","20:30","20:45","21:00","21:15" };
            return matchTimes[_random.Next(matchTimes.Length)];
        }
        
        private DateTime GenerateRandomMatchDay()//returns a random date between today and 30 days from now
        {
            int daysToAdd = _random.Next(0, 31);
            return DateTime.Today.AddDays(daysToAdd);

        }
    }
}
   
 
