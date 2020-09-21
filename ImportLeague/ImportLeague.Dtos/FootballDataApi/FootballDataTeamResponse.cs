using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Dtos
{
    public class FootballDataTeamResponse
    {
        public IEnumerable<FootballDataPlayer> Squad { get; set; }
    }
}
