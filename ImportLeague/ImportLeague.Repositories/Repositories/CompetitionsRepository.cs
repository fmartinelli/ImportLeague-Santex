using ImportLeague.Models;
using ImportLeague.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportLeague.Repositories.Repositories
{
    public class CompetitionsRepository : ICompetitionsRepository, IDisposable
    {
        private ImportLeagueContext context;

        public CompetitionsRepository(ImportLeagueContext context)
        {
            this.context = context;
        }

        public IEnumerable<Competition> GetAll()
        {
            return context.Competitions
                .Include(comp => comp.CompetitionTeams);
        }

        public bool ContainsCompetitionCode(string code)
        {
            return context.Competitions.Any(c => c.Code.Equals(code));
        }

        public Competition GetByCode(string code)
        {
            return context.Competitions
                .Include(comp => comp.CompetitionTeams)
                .FirstOrDefault(c => c.Code.Equals(code));
        }

        public int GetCompetitionPlayersNumberByCode(string code)
        {
            var competition = context.Competitions
                .Include(c => c.CompetitionTeams)
                .ThenInclude(competitionTeams => competitionTeams.Team)
                .ThenInclude(team => team.Players)
                    .Single(b => b.Code == code);

            var teams=competition.CompetitionTeams.Select(ct => ct.Team);

            var playersCount = teams.Sum(t=> t.Players.Count);
            return playersCount;
        }

        public void Add(Competition competition)
        {
            context.Competitions.Add(competition);
        }

        public void Remove(string competitionId)
        {
            Competition competition = context.Competitions.Find(competitionId);
            context.Competitions.Remove(competition);
        }

        public void Update(Competition competition)
        {
            context.Competitions.Update(competition);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
