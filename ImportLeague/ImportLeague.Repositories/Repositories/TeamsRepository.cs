using ImportLeague.Models;
using ImportLeague.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportLeague.Repositories.Repositories
{
    public class TeamsRepository : ITeamsRepository, IDisposable
    {
        private ImportLeagueContext context;

        public TeamsRepository(ImportLeagueContext context)
        {
            this.context = context;
        }

        public IEnumerable<Team> GetAll()
        {
            return context.Teams
                .Include(team => team.Players);
        }

        public Team GetById(int id)
        {
            return context.Teams
                .Include(team => team.Players)
                .FirstOrDefault(t => t.Id == id);
        }

        public Team GetByExternalId(int id)
        {
            return context.Teams
                .Include(team => team.Players)
                .FirstOrDefault(t => t.ExternalId == id);
        }

        public bool ContainsId(int id)
        {
            return context.Teams.Any(t => t.Id == id);
        }

        public void Add(Team team)
        {
            context.Teams.Add(team);
        }

        public void Remove(string teamId)
        {
            Team team = context.Teams.Find(teamId);
            context.Teams.Remove(team);
        }

        public void Update(Team team)
        {
            context.Teams.Update(team);
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
