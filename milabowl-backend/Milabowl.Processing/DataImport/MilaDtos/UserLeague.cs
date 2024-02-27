namespace Milabowl.Processing.DataImport.MilaDtos;

public class UserLeague
{
    public Guid UserLeagueId { get; set; }
    public User User { get; set; }
    public Guid FkUserId { get; set; }
    public League League { get; set; }
    public Guid FkLeagueId { get; set; }
}