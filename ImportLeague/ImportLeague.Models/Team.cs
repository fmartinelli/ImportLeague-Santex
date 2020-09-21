using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Models
{
    public class Team
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Name { get; set; }
        public string Tla { get; set; }
        public string ShortName { get; set; }
        public string AreaName { get; set; }
        public string Email { get; set; }
        public List<Player> Players { get; set; }
        public List<CompetitionTeams> CompetitionTeams { get; set; }
    }
}
