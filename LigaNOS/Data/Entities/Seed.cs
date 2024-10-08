using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using LigaNOS.Helpers;
using System.Linq;
using System.Xml.Linq;
using static System.Reflection.Metadata.BlobBuilder;

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
                AddClub("club", user);
                AddClub("club", user);
                AddClub("club", user);
                await _context.SaveChangesAsync();

            }
            if (!_context.Players.Any())
            {
                AddPlayer("Cristiano Ronaldo", user);
                AddPlayer("Pepe", user);
                AddPlayer("Luís Figo", user);
                await _context.SaveChangesAsync();  

            }
       
        }

        private void AddClub(string name, User user)
        {
            var clubName = GenerateRandomClubName();
            _context.Clubs.Add(new Club
            {
                Name = clubName,
                Coach = GenerateRandomCoach(),
                Stadium = $"Estádio do {clubName}", //ensure the stadium has the same name as the club
                User = user,
            });
        }

        private void AddPlayer(string name, User user)
        {
            _context.Players.Add(new Player
            {
                Name = name,
                DateOfBirth = DateTime.Today,
                Position = GenerateRandomPosition(),
                ClubId = _context.Clubs.FirstOrDefault().Id,
                User = user,
            });
          
        }

        private void AddMatch(string name, User user)
        {

            var homeClubName = GenerateRandomClub();
            var awayClubName = GenerateRandomClub();

            // ensure the away club is different from the home club
            while (awayClubName == homeClubName)
            {
                awayClubName = GenerateRandomClub();
            }

            var homeClub = _context.Clubs.FirstOrDefault(c => c.Name == homeClubName);
            var awayClub = _context.Clubs.FirstOrDefault(c => c.Name == awayClubName);

            if (homeClub == null || awayClub == null)
            {
                throw new InvalidOperationException("One or both clubs not found in the database.");
            }

            _context.Matches.Add(new Match
            {
                MatchDay = DateTime.Today,
                MatchTime = GenerateRandomMatchTime(),
                HomeClub = homeClub,
                AwayClub = awayClub,
                Stadium = homeClub.Stadium, // set the stadium to the home club's stadium
                User = user,
            });
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

        private string GenerateRandomClub()
        {
            string[] Clubs = { "FC Porto", "SL Benfica", "Sporting CP", "SC Braga", "Vitória SC", "Boavista FC", "CD Tondela", "FC Famalicão", "Moreirense FC", "CD Santa Clara", "CS Marítimo", "CD Nacional", "Rio Ave FC", "Gil Vicente FC", "FC Paços de Ferreira", "Portimonense SC", "CD Aves", "Belenenses SAD", "CD Feirense", "GD Chaves" };
            int index = _random.Next(Clubs.Length);
            return Clubs[index];
        }

        private string GenerateRandomMatchTime()
        {
            string[] matchTimes = { "19:15", "19:30", "19:45", "20:00","20:15","20:30","20:45","21:00","21:15" };
            return matchTimes[_random.Next(matchTimes.Length)];
        }

    }
}
   
 
