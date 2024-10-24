using LigaNOS.Data.Entities;
using LigaNOS.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace LigaNOS.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Club ToClub(ClubViewModel model, Guid path, bool isNew)
        {
            return new Club
            {
                
                ImageFileId = path,
                Name = model.Name,
                Coach = model.Coach,
                Stadium = model.Stadium,

            };
        }

        public Player ToPlayer(PlayerViewModel model, Guid path, bool isNew)
        {
            return new Player
            { 
                ImageFileId = path,
                Name = model.Name,
                DateOfBirth = model.DateOfBirth,
                Position = model.Position,
                ClubId = model.ClubId,
            };
        }

        public Match ToMatch(MatchViewModel model, Guid path, bool isNew)
        {
            return new Match
            { 
                HomeClub = new Club { Name = model.HomeClub },
                AwayClub = new Club { Name = model.AwayClub },
                HomeGoals = model.HomeGoals,
                AwayGoals = model.AwayGoals,
                MatchDay = model.MatchDay,
                Stadium = model.Stadium,
                MatchTime = model.MatchTime,

            };
        }


        public ClubViewModel ToClubViewModel(Club club)
        {
            return new ClubViewModel
            {
                Id = club.Id,
                ImageFile = ConvertToIFormFile(club.ImageFileId),
                Name = club.Name,
                Coach = club.Coach,
                Stadium = club.Stadium,
            };
        }

        private IFormFile ConvertToIFormFile(Guid imageFile)
        {
                var stream = new MemoryStream(); 
                return new FormFile(stream, 0, stream.Length, null, imageFile.ToString());

        }

        public PlayerViewModel ToPlayerViewModel(Player player)
        {
             return new PlayerViewModel
             {
                  
                 ImageFile = ConvertToIFormFile(player.ImageFileId),
                 Name = player.Name,
                 DateOfBirth = player.DateOfBirth,
                 Position = player.Position,
                 ClubId = player.ClubId,
             };
        }

        public MatchViewModel ToMatchViewModel(Match match)
        {
            if (match == null) throw new ArgumentNullException(nameof(match));
            return new MatchViewModel
            {

                Id = match.Id,
                HomeClub = match.HomeClub?.Name,
                AwayClub = match.AwayClub?.Name,
                HomeGoals = match.HomeGoals,
                AwayGoals = match.AwayGoals,
                MatchDay = match.MatchDay,
                Stadium = match.Stadium,
                MatchTime = match.MatchTime,
                    
            };
        }
        public Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew)
        {
            return new Employee
            {
                Id = isNew ? 0 : model.Id,
                ImageFileId = imageId,
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Email = model.Email,
                Role = model.Role,
                User = model.User,

            };
        }

        public EmployeeViewModel ToEmployeeViewModel(Employee employee)
        {
            return new EmployeeViewModel
            {
                Id = employee.Id,
                ImageFileId = employee.ImageFileId,
                Name = employee.Name,
                Address = employee.Address,
                Phone = employee.Phone,
                Email = employee.Email,
                Role = employee.Role,
                User = employee.User,

            };

        }
    }
}
