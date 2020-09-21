using ImportLeague.Models;
using System.Threading.Tasks;

namespace ImportLeague.Services.Interfaces
{
    public interface ILeagueService
    {
        Task<Competition> ImportLeague(string leagueCode);
        Task<int?> GetTotalPlayers(string leagueCode);
    }
}

