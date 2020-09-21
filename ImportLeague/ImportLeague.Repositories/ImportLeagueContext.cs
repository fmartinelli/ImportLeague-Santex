using ImportLeague.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace ImportLeague.Repositories
{
    public partial class ImportLeagueContext: DbContext
    {
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }

        public ImportLeagueContext(DbContextOptions<ImportLeagueContext> options) : base(options)
        {

        }

    }

    public class ImportLeagueContextFactory : IDesignTimeDbContextFactory<ImportLeagueContext>
    {
        public ImportLeagueContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ImportLeagueContext>()
            .UseSqlServer("Server=.;Initial Catalog=FMartinelli.ImportLeagueDb;Integrated Security=true;");

            return new ImportLeagueContext(optionsBuilder.Options);
        }
    }
}
