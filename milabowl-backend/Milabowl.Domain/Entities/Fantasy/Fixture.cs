namespace Milabowl.Domain.Entities.Fantasy;

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

    public virtual Team TeamAway { get; set; }
    public Guid FkTeamAwayId { get; set; }
    public virtual Team TeamHome { get; set; }
    public Guid FkTeamHomeId { get; set; }
    public virtual Event Event { get; set; }
    public Guid FkEventId { get; set; }
}