namespace Milabowl.Processing.DataImport.MilaDtos;

public class UserLeague
{
    public Guid UserLeagueId { get; set; }
    public required User User { get; set; }
    public Guid FkUserId { get; set; }
    public required League League { get; set; }
    public Guid FkLeagueId { get; set; }
}
