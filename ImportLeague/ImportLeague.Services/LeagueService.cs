using AutoMapper;
using ImportLeague.Dtos;
using ImportLeague.Models;
using ImportLeague.Repositories.Interfaces;
using ImportLeague.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Competition = ImportLeague.Models.Competition;

namespace ImportLeague.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<LeagueService> logger;

        private IFootballDataApiReader Reader { get; }
        private IConfiguration Configuration { get; }

        public LeagueService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<LeagueService> logger, IConfiguration configuration, IFootballDataApiReader footballDataApiReader)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            Reader = footballDataApiReader;
        }

        public async Task<Competition> ImportLeague(string leagueCode)
        {

            if (unitOfWork.Competitions.GetByCode(leagueCode) != null)
                throw new InvalidOperationException("League already imported");

            FootballDataCompetitionResponse response = await Reader.GetLeague(leagueCode);

            if (response == null)
                return null;

            Competition competition = mapper.Map<Competition>(response.Competition);

            logger.LogDebug(competition.ToString());

            var competitionTeams = mapper.Map<IEnumerable<Team>>(response.Teams);

            logger.LogDebug($"Competition teams:{competitionTeams.Count()}");

            var alreadyImported = new List<Team>();
            var toImport = new List<Team>();

            foreach (var team in competitionTeams)
            {
                Team alreadyImportedteam = unitOfWork.Teams.GetByExternalId(team.ExternalId);
                if (alreadyImportedteam != null)
                {
                    alreadyImported.Add(alreadyImportedteam);
                    continue;
                }

                var dtoPlayers = await Reader.GetTeamPlayers(team.ExternalId);
                var players = mapper.Map<IEnumerable<Player>>(dtoPlayers);

                logger.LogDebug($"players on team({team.Name}):{players.Count()}");
                unitOfWork.Players.AddRange(players);

                team.Players = players.ToList();

                logger.LogDebug(team.ToString());
                unitOfWork.Teams.Add(team);
                toImport.Add(team);
            }

            unitOfWork.Competitions.Add(competition);

            unitOfWork.Complete();

            competition.CompetitionTeams = toImport
                .Select( ct => new CompetitionTeams() 
                { 
                    TeamId = ct.Id,
                    Team= ct, 
                    CompetitionId = competition.Id,
                    Competition = competition
                }).ToList();

            competition.CompetitionTeams.AddRange(
                alreadyImported
                .Select(ct => new CompetitionTeams()
                {
                    TeamId = ct.Id,
                    Team = ct,
                    CompetitionId = competition.Id,
                    Competition = competition
                }));

            unitOfWork.Competitions.Update(competition);

            unitOfWork.Complete();

            return competition;
        }

        public async Task<int?> GetTotalPlayers(string leagueCode)
        {
            int? result = null;
            if (unitOfWork.Competitions.ContainsCompetitionCode(leagueCode))
            {
                result = unitOfWork.Competitions.GetCompetitionPlayersNumberByCode(leagueCode);
            }
            return result;
        }
    }
}
