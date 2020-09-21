using ImportLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportLeague.Repositories.Interfaces
{
    public interface ICompetitionsRepository
    {
        IEnumerable<Competition> GetAll();
        Competition GetByCode(string competitionCode);
        bool ContainsCompetitionCode(string competitionCode);
        void Add(Competition competition);
        void Remove(string competitionId);
        void Update(Competition competition);
        int GetCompetitionPlayersNumberByCode(string code);
    }
}
