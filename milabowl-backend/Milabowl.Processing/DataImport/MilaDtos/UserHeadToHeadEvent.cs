namespace Milabowl.Processing.DataImport.MilaDtos;

public class UserHeadToHeadEvent
{
    public Guid UserHeadToHeadEventId { get; set; }
    public int FantasyUserHeadToHeadEventId { get; set; }
    public required User User { get; set; }
    public Guid FkUserId { get; set; }
    public int Points { get; set; }
    public int Win { get; set; }
    public int Draw { get; set; }
    public int Loss { get; set; }
    public int Total { get; set; }
    public required Event Event { get; set; }
    public Guid FkEventId { get; set; }
    public bool IsKnockout { get; set; }
    public int LeagueId { get; set; }
    public bool IsBye { get; set; }
}
