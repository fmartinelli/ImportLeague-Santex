using ImportLeague.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Repositories.Interfaces
{
    public interface ITeamsRepository
    {
        IEnumerable<Team> GetAll();
        Team GetById(int teamId);
        bool ContainsId(int id);
        void Add(Team team);
        void Remove(string teamId);
        void Update(Team team);
        Team GetByExternalId(int id);
    }
}
