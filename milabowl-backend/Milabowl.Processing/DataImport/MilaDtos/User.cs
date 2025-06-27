namespace Milabowl.Processing.DataImport.MilaDtos;

public class User
{
    public Guid UserId { get; set; }
    public required string UserName { get; set; }
    public required string EntryName { get; set; }
    public int FantasyEntryId { get; set; }
    public int FantasyResultId { get; set; }
    public int Rank { get; set; }
    public int LastRank { get; set; }
    public int EventTotal { get; set; }
    public required IList<UserLeague> UserLeagues { get; set; }
    public required IList<UserHistory> UserHistories { get; set; }
    public required IList<Lineup> Lineups { get; set; }
    public required IList<UserHeadToHeadEvent> HeadToHeadEvents { get; set; }
}
