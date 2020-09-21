using ImportLeague.Repositories.Interfaces;
using ImportLeague.Repositories.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImportLeague.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ImportLeagueContext importLeagueContext;

        public ICompetitionsRepository Competitions { get; private set; }

        public IPlayersRepository Players { get; private set; }

        public ITeamsRepository Teams { get;  private set; }

        public UnitOfWork(ImportLeagueContext importLeagueContext)
        {
            this.importLeagueContext = importLeagueContext;

            Competitions = new CompetitionsRepository(importLeagueContext);
            Players = new PlayersRepository(importLeagueContext);
            Teams = new TeamsRepository(importLeagueContext);
        }

        public void Complete()
        {
            importLeagueContext.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await importLeagueContext.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return importLeagueContext.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return importLeagueContext.Database.BeginTransactionAsync();
        }

        public void CommitTransaction()
        {
            importLeagueContext.Database.CommitTransaction();
        }
    }
}
