using LigaNOS.Models;
using System.Threading.Tasks;

namespace LigaNOS.Data.Entities
{
    public interface IMatchGenerator
    {
        Task<MatchViewModel> GenerateMatch();
    }
}