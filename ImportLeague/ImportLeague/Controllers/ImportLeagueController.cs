using System;
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
    [Route("import-league")]
    [ApiVersion("1")]
    [ApiController]
    public class ImportLeagueController : ControllerBase
    {
        #region properties

        private IMapper mapper;
        private ILeagueService leagueService;
        private ILogger<ImportLeagueController> logger;

        #endregion

        #region constructor

        public ImportLeagueController(ILeagueService leagueService, IMapper mapper, ILogger<ImportLeagueController> logger)
        {
            this.mapper = mapper;
            this.leagueService = leagueService;
            this.logger = logger;
        }

        #endregion

        [HttpGet]
        [Route("{leagueCode}")]
        [SwaggerOperation(
            Summary = "Import a Competition",
            Description = @"## Import data using the given {leagueCode}, by making requests to the http://www.football-data.org/v2.",
            OperationId = "ImportLeague",
            Tags = new[] { "Import League" }
           )]
        [SwaggerResponse(201, "When the leagueCode was successfully imported.", typeof(ImportLeagueResponse))]
        [SwaggerResponse(409, "If the given leagueCode was already imported into the DB (and in this case, it doesn’t need to be imported again).", typeof(ImportLeagueResponse))]
        [SwaggerResponse(404, "If the leagueCode was not found.", typeof(ImportLeagueResponse))]
        [SwaggerResponse(500, "If there is any connectivity issue either with the football API or the DB server.", typeof(ImportLeagueResponse))]
        public async Task<IActionResult> Get([SwaggerParameter(Description = "League Code", Required = true)] string leagueCode)
        {
            var response = new ImportLeagueResponse();
            try
            {
                var competition = await leagueService.ImportLeague(leagueCode);
                if (competition is null)
                {
                    logger.LogDebug($"Not found:{leagueCode}");
                    response.Message = "Not found";
                    return NotFound(response);
                }
                logger.LogDebug($"Successfully imported:{leagueCode}");
                response.Message = "Successfully imported";
                return StatusCode(201, response);
            }
            catch (InvalidOperationException e)
            {
                logger.LogDebug($"League already imported:{leagueCode}");
                response.Message = "League already imported";
                return StatusCode(409, response);
            }
            catch (Exception e)
            {
                logger.LogError($"Server Error:{leagueCode}");
                logger.LogError(e.ToString());
                response.Message = "Server Error";
                return StatusCode(500,response);
            }    
            
        }
    }

}
