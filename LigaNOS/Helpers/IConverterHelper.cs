using LigaNOS.Data.Entities;
using LigaNOS.Models;
using System;

namespace LigaNOS.Helpers
{
    public interface IConverterHelper
    {
        Club ToClub(ClubViewModel model, Guid imageId, bool isNew);
        Player ToPlayer(PlayerViewModel model, Guid imageId, bool isNew);
        Match ToMatch(MatchViewModel model, Guid imageId, bool isNew);
        Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew);

        ClubViewModel ToClubViewModel(Club club);
        PlayerViewModel ToPlayerViewModel(Player player);
        MatchViewModel ToMatchViewModel(Match match);
        EmployeeViewModel ToEmployeeViewModel(Employee employee);

    }
}
