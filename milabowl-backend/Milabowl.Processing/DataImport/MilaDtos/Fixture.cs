namespace Milabowl.Processing.DataImport.MilaDtos;

public class Fixture
{
    public Guid FixtureId { get; set; }
    public bool Finished { get; set; }
    public bool FinishedProvisional { get; set; }
    public int FantasyFixtureId { get; set; }
    public DateTime? KickoffTime { get; set; }
    public int Minutes { get; set; }
    public bool ProvisionalStartTime { get; set; }
    public bool? Started { get; set; }
    public int? TeamAwayScore { get; set; }
    public int? TeamHomeScore { get; set; }
    public int TeamHomeDifficulty { get; set; }
    public int TeamAwayDifficulty { get; set; }

    public required Team TeamAway { get; set; }
    public Guid FkTeamAwayId { get; set; }
    public required Team TeamHome { get; set; }
    public Guid FkTeamHomeId { get; set; }
    public required Event Event { get; set; }
    public Guid FkEventId { get; set; }
}
