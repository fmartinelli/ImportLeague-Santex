using AutoMapper;
using ImportLeague.Dtos;
using ImportLeague.Models;
using ImportLeague.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImportLeague.Services.FootballDataApi
{
    public class FootballDataApiReader : IFootballDataApiReader
    {
        private readonly IMapper mapper;
        private readonly ILogger<LeagueService> logger;
        public string FootballServiceApiKey { get; }
        public string FootballServiceApiHost { get; }

        public FootballDataApiReader(IConfiguration Configuration, IMapper mapper, ILogger<LeagueService> logger)
        {
            FootballServiceApiKey = Configuration["FootballServiceApiKey"];
            FootballServiceApiHost = Configuration["FootballServiceApiHost"];
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<FootballDataCompetitionResponse> GetLeague(string leagueCode)
        {
            try
            {
                string requestUrl = $"{FootballServiceApiHost}/competitions/{leagueCode}/teams";
                var client = new RestClient(requestUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("X-Auth-Token", FootballServiceApiKey);
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    return JsonConvert.DeserializeObject<FootballDataCompetitionResponse>(response.Content);
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        return null;
                    }
                    logger.LogError($"FootballDataApiReader: {response.StatusCode} - {response.ErrorMessage}");
                    throw new Exception("FootballServiceApi connection error");
                }
            }
            catch (Exception e)
            {
                logger.LogError($"FootballDataApiReader: {e.ToString()}");
                throw new Exception("FootballServiceApi connection error");
            }
        }

        public async Task<IEnumerable<FootballDataPlayer>> GetTeamPlayers(int teamId)
        {
            try
            {
                string requestUrl = $"{FootballServiceApiHost}/teams/{teamId}";
                var client = new RestClient(requestUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("X-Auth-Token", FootballServiceApiKey);

                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    var teamResponse = JsonConvert.DeserializeObject<FootballDataTeamResponse>(response.Content);
                    return teamResponse.Squad;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        logger.LogInformation("****Waiting a minute for more API requests****");
                        Thread.Sleep(6000); //wait 60 seconds https://www.football-data.org/documentation/api#request-throttling
                        return await GetTeamPlayers(teamId);
                    }
                    else
                    {
                        logger.LogError($"FootballDataApiReader: {response.StatusCode} - {response.ErrorMessage}");
                        throw new Exception($"FootballServiceApi connection error - GetTeamPlayers:{teamId}");
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError($"FootballDataApiReader: {e.ToString()}");
                throw new Exception($"FootballServiceApi connection error - GetTeamPlayers:{teamId}");
            }
        }
    }
}
