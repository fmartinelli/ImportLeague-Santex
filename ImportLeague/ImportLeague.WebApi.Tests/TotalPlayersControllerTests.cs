using AutoMapper;
using ImportLeague.Controllers;
using ImportLeague.Dtos;
using ImportLeague.Services.Interfaces;
using ImportLeague.WebApi.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ImportLeague.WebApi.Tests
{
    public class TotalPlayersControllerTests
    {

        [Fact]
        public async Task TotalPlayers_WhenLeagueCodeDoesNotExistsInDB_ShouldReturn404()
        {
            var leagueCode = "CL";

            int? result = null;

            var mapperStub = new MapperConfiguration(mc => mc.AddProfile(new MappingsProfile())).CreateMapper();

            var leagueService = new Mock<ILeagueService>();
            leagueService.Setup(x => x.GetTotalPlayers(It.IsAny<string>())).ReturnsAsync(result);

            var logger = new Mock<ILogger<TotalPlayersController>>();

            var controller = new TotalPlayersController(leagueService.Object, mapperStub, logger.Object);
            var actionResult = await controller.Get(leagueCode);

            leagueService.Verify(x => x.GetTotalPlayers(It.IsAny<string>()));

            Assert.IsType<NotFoundResult>(actionResult);
            
        }

        [Fact]
        public async Task TotalPlayers_WhenLeagueCodeExistsInDB_ShouldReturn200()
        {
            var leagueCode = "CL";

            int? result = 5;

            var mapperStub = new MapperConfiguration(mc => mc.AddProfile(new MappingsProfile())).CreateMapper();

            var leagueService = new Mock<ILeagueService>();
            leagueService.Setup(x => x.GetTotalPlayers(It.IsAny<string>())).ReturnsAsync(result);

            var logger = new Mock<ILogger<TotalPlayersController>>();

            var controller = new TotalPlayersController(leagueService.Object, mapperStub, logger.Object);
            var actionResult = await controller.Get(leagueCode);

            leagueService.Verify(x => x.GetTotalPlayers(It.IsAny<string>()));

            Assert.IsType<OkObjectResult>(actionResult);
            var totalPlayersResult = ((OkObjectResult)actionResult).Value as TotalPlayersResponse;

            Assert.IsType<TotalPlayersResponse>(totalPlayersResult);
            Assert.Equal(5, totalPlayersResult.Total);
        }
    }
}
