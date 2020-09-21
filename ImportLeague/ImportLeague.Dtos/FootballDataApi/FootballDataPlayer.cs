using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLeague.Dtos
{
    public class FootballDataPlayer
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string Nationality { get; set; }
    }
}
