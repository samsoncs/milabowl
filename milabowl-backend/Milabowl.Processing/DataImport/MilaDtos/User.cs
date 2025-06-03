namespace Milabowl.Processing.DataImport.MilaDtos;

public class User
{
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string EntryName { get; set; }
    public int FantasyEntryId { get; set; }
    public int FantasyResultId { get; set; }
    public int Rank { get; set; }
    public int LastRank { get; set; }
    public int EventTotal { get; set; }
    public IList<UserLeague> UserLeagues { get; set; }
    public IList<UserHistory> UserHistories { get; set; }
    public IList<Lineup> Lineups { get; set; }
    public IList<UserHeadToHeadEvent> HeadToHeadEvents { get; set; }
}
