using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Dtos
{
    public class FootballDataCompetition
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public FootballDataCompetitionArea Area { get; set; }
    }
}
