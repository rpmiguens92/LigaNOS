using LigaNOS.Data.Entities;
using System;
namespace LigaNOS.Models
{
    public class ClubStatViewModel : Club
    {
        public int ClubId { get; set; }

        public string ClubName { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public Guid ImageFileId { get; set; }
        public string ImageFullPath => ImageFileId == Guid.Empty
            ? $"https://liganos.azurewebsites.net/images/noimage.jpg"
           : $"https://liganos.blob.core.windows.net/clubs/{ImageFileId}";
    }
}
