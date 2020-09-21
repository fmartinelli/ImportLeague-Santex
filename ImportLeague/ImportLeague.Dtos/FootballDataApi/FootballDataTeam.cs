using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Dtos
{
    public class FootballDataTeam
    {
        public int Id { get; set; }
        public string Tla { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Email { get; set; }
        public FootballDataCompetitionArea Area { get; set; }

    }
}
