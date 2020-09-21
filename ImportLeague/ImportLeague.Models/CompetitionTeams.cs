namespace ImportLeague.Models
{
    public class CompetitionTeams
    {
        public int Id { get; set; }
        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}