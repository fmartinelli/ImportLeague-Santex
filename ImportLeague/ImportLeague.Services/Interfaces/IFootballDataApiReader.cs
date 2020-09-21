using ImportLeague.Dtos;
using ImportLeague.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImportLeague.Services.Interfaces
{
    public interface IFootballDataApiReader
    {
        Task<FootballDataCompetitionResponse> GetLeague(string leagueCode);
        Task<IEnumerable<FootballDataPlayer>> GetTeamPlayers(int teamCode);
    }
}
