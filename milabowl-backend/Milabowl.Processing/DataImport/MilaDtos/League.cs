namespace Milabowl.Processing.DataImport.MilaDtos;

public class League
{
    public Guid LeagueId { get; set; }
    public int FantasyLeagueId { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public bool Closed { get; set; }

    //public object MaxEntries { get; set; }
    public string LeagueType { get; set; }
    public string Scoring { get; set; }
    public int AdminEntry { get; set; }
    public int StartEvent { get; set; }
    public string CodePrivacy { get; set; }
    public IList<UserLeague> UserLeagues { get; set; }
    //public object Rank { get; set; }
}
