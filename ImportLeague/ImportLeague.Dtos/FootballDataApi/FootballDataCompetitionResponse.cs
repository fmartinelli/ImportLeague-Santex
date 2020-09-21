using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Dtos
{
    public class FootballDataCompetitionResponse
    {
        public FootballDataCompetition Competition { get; set; }
        public IEnumerable<FootballDataTeam> Teams { get; set; }
    }
}
