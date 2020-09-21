using ImportLeague.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Repositories.Interfaces
{
    public interface IPlayersRepository : IDisposable
    {
        IEnumerable<Player> GetAll();
        Player GetByID(int playerId);
        bool ContainsId(int id);
        void Add(Player player);
        void Remove(string playerId);
        void Update(Player player);
        void AddRange(IEnumerable<Player> players);
    }
}
