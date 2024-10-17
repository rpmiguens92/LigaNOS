using LigaNOS.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaNOS.Helpers
{
    public interface ITeamService
    {
        Task<List<Club>> GetTeamsAsync();
    }
}