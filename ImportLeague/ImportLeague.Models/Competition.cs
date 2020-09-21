using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;

namespace ImportLeague.Models
{
    public class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string AreaName { get; set; }
        public List<CompetitionTeams> CompetitionTeams { get; set; }
    }
}
