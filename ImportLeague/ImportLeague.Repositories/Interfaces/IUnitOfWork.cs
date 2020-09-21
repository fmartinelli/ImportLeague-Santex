using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImportLeague.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ICompetitionsRepository Competitions { get; }
        IPlayersRepository Players { get; }
        ITeamsRepository Teams { get; }

        void Complete();

        Task CompleteAsync();

        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync();

        void CommitTransaction();

    }
}
