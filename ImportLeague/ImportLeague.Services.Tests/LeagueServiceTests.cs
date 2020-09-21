using AutoMapper;
using ImportLeague.Dtos;
using ImportLeague.Models;
using ImportLeague.Repositories.Interfaces;
using ImportLeague.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ImportLeague.Services.Tests
{
    public class LeagueServiceTests
    {
        [Fact]
        public async Task ImportLeague_WhenLeagueCodeDoesNotExistsInDB_ShouldImportAndReturnCompetition()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task ImportLeague_WhenLeagueCodeDoesNotExistsOnFootballApi_ShouldReturnNullWithoutImport()
        {
            var leagueCode = "CL";

            Competition mockResult = null;

            var competitionsRepository = new Mock<ICompetitionsRepository>();
            competitionsRepository.Setup(x => x.GetByCode(leagueCode)).Returns(mockResult);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.Competitions).Returns(competitionsRepository.Object);

            FootballDataCompetitionResponse readerResponse = null;
            var reader = new Mock<IFootballDataApiReader>();
            reader.Setup(x => x.GetLeague(leagueCode)).ReturnsAsync(readerResponse);

            var service = LeagueServiceFactory.Create(unitOfWork.Object, reader.Object);

            var result = await service.ImportLeague(leagueCode);

            competitionsRepository.Verify(x => x.GetByCode(leagueCode));

            Assert.Null(result);
        }

        [Fact]
        public async Task ImportLeague_WhenLeagueCodeExistsInDB_ShouldThrowsInvalidOperationException()
        {
            var leagueCode = "CL";

            Competition mockResult = new Competition() { Code=leagueCode};

            var competitionsRepository = new Mock<ICompetitionsRepository>();
            competitionsRepository.Setup(x => x.GetByCode(leagueCode)).Returns(mockResult);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.Competitions).Returns(competitionsRepository.Object);

            var service = LeagueServiceFactory.Create(unitOfWork.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.ImportLeague(leagueCode));

            competitionsRepository.Verify(x => x.GetByCode(leagueCode));
        }

        [Fact]
        public async Task ImportLeague_WhenLeagueCodeDoesNotExistsInDBAndShareTeamsWithAalreadyImportedCompetition_ShouldImportAndReturnCompetitionWithoutImportSharedTeams()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public async Task GetTotalPlayers_WhenLeagueCodeDoesNotExistsInDB_ShouldReturnNull()
        {
            var leagueCode = "CL";

            bool mockResult = false;

            var competitionsRepository = new Mock<ICompetitionsRepository>();
            competitionsRepository.Setup(x => x.ContainsCompetitionCode(leagueCode)).Returns(mockResult);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.Competitions).Returns(competitionsRepository.Object);

            var service = LeagueServiceFactory.Create(unitOfWork.Object);

            var result = await service.GetTotalPlayers(leagueCode);

            competitionsRepository.Verify(x => x.ContainsCompetitionCode(leagueCode));
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTotalPlayers_WhenLeagueCodeExistsInDB_ShouldTotalNumberOfPlayers()
        {
            var leagueCode = "CL";

            bool mockResult = true;

            var competitionsRepository = new Mock<ICompetitionsRepository>();
            competitionsRepository.Setup(x => x.GetCompetitionPlayersNumberByCode(leagueCode)).Returns(5);
            competitionsRepository.Setup(x => x.ContainsCompetitionCode(leagueCode)).Returns(mockResult);

            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(x => x.Competitions).Returns(competitionsRepository.Object);

            var service = LeagueServiceFactory.Create(unitOfWork.Object);

            var result = await service.GetTotalPlayers(leagueCode);

            competitionsRepository.Verify(x => x.ContainsCompetitionCode(leagueCode));
            competitionsRepository.Verify(x => x.GetCompetitionPlayersNumberByCode(leagueCode));
            Assert.IsType<int>(result);
            Assert.NotNull(result);
            Assert.Equal(5, result);
        }
    }

    public static class LeagueServiceFactory
    {
        public static LeagueService Create(IUnitOfWork unitOfWork,
            IFootballDataApiReader footballDataApiReader = null,
            IMapper mapper = null,
            ILogger<LeagueService> logger = null,
            IConfiguration configuration = null
            )
        {
            return new LeagueService(
               unitOfWork,
               mapper ?? new Mock<IMapper>().Object,
               logger ?? new Mock<ILogger<LeagueService>>().Object,
               configuration ?? new Mock<IConfiguration>().Object,
               footballDataApiReader ?? new Mock<IFootballDataApiReader>().Object
               );
        }
    }
}