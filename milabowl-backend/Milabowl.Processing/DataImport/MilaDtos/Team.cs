using System.ComponentModel.DataAnnotations;

namespace Milabowl.Processing.DataImport.MilaDtos;

public class Team
{
    [Key]
    public Guid TeamId { get; set; }
    public int FantasyTeamId { get; set; }
    public int FantasyTeamCode { get; set; }
    public required string TeamName { get; set; }
    public required string TeamShortName { get; set; }
    public required IList<Player> Players { get; set; }
    public required IList<Fixture> HomeFixtures { get; set; }
    public required IList<Fixture> AwayFixtures { get; set; }
}
