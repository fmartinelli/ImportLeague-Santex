using ImportLeague.Models;
using ImportLeague.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportLeague.Repositories.Repositories
{
    public class PlayersRepository : IPlayersRepository, IDisposable
    {
        private ImportLeagueContext context;

        public PlayersRepository(ImportLeagueContext context)
        {
            this.context = context;
        }

        public IEnumerable<Player> GetAll()
        {
            return context.Players;
        }

        public Player GetByID(int id)
        {
            return context.Players.FirstOrDefault(p => p.Id == id);
        }

        public bool ContainsId(int id)
        {
            return context.Players.Any(p => p.Id == id);
        }

        public void Add(Player player)
        {
            context.Players.Add(player);
        }

        public void Remove(string playerId)
        {
            Player player = context.Players.Find(playerId);
            context.Players.Remove(player);
        }

        public void Update(Player player)
        {
            context.Players.Update(player);
        }

        public void AddRange(IEnumerable<Player> players)
        {
            context.Players.AddRange(players);
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
