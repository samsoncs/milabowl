namespace Milabowl.Processing.DataImport.MilaDtos;

public class League
{
    public Guid LeagueId { get; set; }
    public int FantasyLeagueId { get; set; }
    public required string Name { get; set; }
    public DateTime Created { get; set; }
    public bool Closed { get; set; }
    public required string LeagueType { get; set; }
    public required string Scoring { get; set; }
    public int AdminEntry { get; set; }
    public int StartEvent { get; set; }
    public required string CodePrivacy { get; set; }
    public required IList<UserLeague> UserLeagues { get; set; }
}
