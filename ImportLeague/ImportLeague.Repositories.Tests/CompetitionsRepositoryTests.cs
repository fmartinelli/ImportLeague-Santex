using ImportLeague.Models;
using ImportLeague.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Xunit;

namespace ImportLeague.Repositories.Tests
{
    public class CompetitionsRepositoryTests
    {
        [Fact]
        public void GetByCode_IfCompetitionExists_ReturnsCompetition()
        {
            var leagueCode = "CL";

            var competition = new Competition { Id = 1, Code = leagueCode };

            var competitions = new List<Competition>() {
                competition
            };

            var context = GetDbContext(competitions);

            var repository = GetCompetitionRepository(context);

            var result = repository.GetByCode(leagueCode);

            Assert.IsType<Competition>(result);
            Assert.Equal(competition, result);
        }

        [Fact]
        public void GetByCode_IfCompetitionDoesNotExists_ReturnsNull()
        {
            var leagueCode = "CL";

            var competition = new Competition { Id = 1, Code = "PL" };

            var competitions = new List<Competition>() {
                competition
            };

            var context = GetDbContext(competitions);

            var repository = GetCompetitionRepository(context);

            var result = repository.GetByCode(leagueCode);

            Assert.Null(result);
        }

        private CompetitionsRepository GetCompetitionRepository(ImportLeagueContext dbContext)
        {
            return new CompetitionsRepository(dbContext);
        }

        private ImportLeagueContext GetDbContext(List<Competition> competitions)
        {
            var options = new DbContextOptionsBuilder<ImportLeagueContext>()
            .UseInMemoryDatabase(databaseName: "CompetitionsDatabase")
            .Options;

            // Insert seed data into the database using one instance of the context
            var context = new ImportLeagueContext(options);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Competitions.AddRange(competitions);

            context.SaveChanges();

            return context;
        }
    }
}
