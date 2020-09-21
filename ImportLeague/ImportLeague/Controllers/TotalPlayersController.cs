using System.Threading.Tasks;
using AutoMapper;
using ImportLeague.Dtos;
using ImportLeague.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ImportLeague.Controllers
{

    [Produces("application/json")]
    [Route("total-players")]
    [ApiVersion("1")]
    [ApiController]
    public class TotalPlayersController : ControllerBase
    {
        #region properties

        private IMapper mapper;
        private ILeagueService leagueService;
        private ILogger<TotalPlayersController> logger;

        #endregion

        #region constructor

        public TotalPlayersController(ILeagueService leagueService, IMapper mapper, ILogger<TotalPlayersController> logger)
        {
            this.mapper = mapper;
            this.leagueService = leagueService;
            this.logger = logger;
        }

        #endregion

        [HttpGet]
        [Route("{leagueCode}")]
        [SwaggerOperation(
            Summary = "Total amount of players",
            Description = @"## Returns the total number of players for a given {leagueCode}, where N is the total amount of players belonging to all teams that participate in the given league (leagueCode).",
            OperationId = "TotalPlayers",
            Tags = new[] { "Total Players"}
           )]
        [SwaggerResponse(200, "If the given leagueCode is present into the DB", typeof(TotalPlayersResponse))]
        [SwaggerResponse(404, "If the given leagueCode is not present into the DB")]
        public async Task<IActionResult> Get([SwaggerParameter(Description = "League Code", Required = true)] string leagueCode)
        {
            var result = await leagueService.GetTotalPlayers(leagueCode);

            if (!result.HasValue)
                return NotFound();

            return Ok( new TotalPlayersResponse() { Total = result.Value });
        }
    }

}
