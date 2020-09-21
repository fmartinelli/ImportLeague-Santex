using AutoMapper;
using ImportLeague.Controllers;
using ImportLeague.Dtos;
using ImportLeague.Services.Interfaces;
using ImportLeague.WebApi.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ImportLeague.WebApi.Tests
{
    public class ImportLeagueControllerTests
    {
        [Fact]
        public async Task ImportLeague_WhenLeagueCodeDoesNotExistsInDB_ShouldReturn201()
        {
            var leagueCode = "CL";

            Models.Competition result = new Models.Competition() { Code = leagueCode};

            var mapperStub = new MapperConfiguration(mc => mc.AddProfile(new MappingsProfile())).CreateMapper();

            var leagueService = new Mock<ILeagueService>();
            leagueService.Setup(x => x.ImportLeague(It.IsAny<string>())).ReturnsAsync(result);

            var logger = new Mock<ILogger<ImportLeagueController>>();

            var controller = new ImportLeagueController(leagueService.Object, mapperStub, logger.Object);
            var actionResult = await controller.Get(leagueCode);

            leagueService.Verify(x => x.ImportLeague(It.IsAny<string>()));

            Assert.IsType<ObjectResult>(actionResult);
            var importLeagueResult = ((ObjectResult)actionResult).Value as ImportLeagueResponse;

            Assert.IsType<ImportLeagueResponse>(importLeagueResult);
            Assert.Equal("Successfully imported", importLeagueResult.Message);
        }

        [Fact]
        public async Task ImportLeague_WhenLeagueCodeExistsInDB_ShouldReturn409()
        {
            var leagueCode = "CL";

            InvalidOperationException result = new InvalidOperationException();

            var mapperStub = new MapperConfiguration(mc => mc.AddProfile(new MappingsProfile())).CreateMapper();

            var leagueService = new Mock<ILeagueService>();
            leagueService.Setup(x => x.ImportLeague(It.IsAny<string>())).ThrowsAsync(result);

            var logger = new Mock<ILogger<ImportLeagueController>>();

            var controller = new ImportLeagueController(leagueService.Object, mapperStub, logger.Object);
            var actionResult = await controller.Get(leagueCode);

            leagueService.Verify(x => x.ImportLeague(It.IsAny<string>()));

            Assert.IsType<ObjectResult>(actionResult);
            var importLeagueResult = ((ObjectResult)actionResult).Value as ImportLeagueResponse;

            Assert.IsType<ImportLeagueResponse>(importLeagueResult);
            Assert.Equal("League already imported", importLeagueResult.Message);
        }

        [Fact]
        public async Task ImportLeague_WhenLeagueCodeDoesNotExistsOnFootballApi_ShouldReturn404()
        {
            var leagueCode = "CL";

            Models.Competition result = null;

            var mapperStub = new MapperConfiguration(mc => mc.AddProfile(new MappingsProfile())).CreateMapper();

            var leagueService = new Mock<ILeagueService>();
            leagueService.Setup(x => x.ImportLeague(It.IsAny<string>())).ReturnsAsync(result);

            var logger = new Mock<ILogger<ImportLeagueController>>();

            var controller = new ImportLeagueController(leagueService.Object, mapperStub, logger.Object);
            var actionResult = await controller.Get(leagueCode);

            leagueService.Verify(x => x.ImportLeague(It.IsAny<string>()));

            Assert.IsType<NotFoundObjectResult>(actionResult);
            var importLeagueResult = ((NotFoundObjectResult)actionResult).Value as ImportLeagueResponse;

            Assert.IsType<ImportLeagueResponse>(importLeagueResult);
            Assert.Equal("Not found", importLeagueResult.Message);
        }

        [Fact]
        public async Task ImportLeague_WhenServiceThrowsException_ShouldReturn500()
        {
            var leagueCode = "CL";

            Exception result = new Exception();

            var mapperStub = new MapperConfiguration(mc => mc.AddProfile(new MappingsProfile())).CreateMapper();

            var leagueService = new Mock<ILeagueService>();
            leagueService.Setup(x => x.ImportLeague(It.IsAny<string>())).ThrowsAsync(result);

            var logger = new Mock<ILogger<ImportLeagueController>>();

            var controller = new ImportLeagueController(leagueService.Object, mapperStub, logger.Object);
            var actionResult = await controller.Get(leagueCode);

            leagueService.Verify(x => x.ImportLeague(It.IsAny<string>()));

            Assert.IsType<ObjectResult>(actionResult);
            var importLeagueResult = ((ObjectResult)actionResult).Value as ImportLeagueResponse;

            Assert.IsType<ImportLeagueResponse>(importLeagueResult);
            Assert.Equal("Server Error", importLeagueResult.Message);
        }
    }
}
