namespace Milabowl.Processing.Processing;

public record MilaResult(
    string Gw,
    decimal GwScore,
    string TeamName,
    string UserName,
    int UserId,
    int GameWeek,
    IList<MilaRuleResult> Rules
);

public class MilaResults
{
    public required IList<GameWeekResults> ResultsByWeek { get; set; }
    public required IList<UserResults> ResultsByUser { get; set; }
    public required IList<OverallResult> OverallScore { get; set; }
    public required IList<Rule> Rules { get; set; }
}

public record Rule(string Name, string ShortName, string Description);

public record OverallResult(
    decimal GwScore,
    string TeamName,
    string Gw,
    string UserName,
    int UserId,
    decimal GwPosition,
    int GameWeek,
    decimal? CumulativeMilaPoints,
    decimal CumulativeAverageMilaPoints,
    decimal TotalCumulativeAverageMilaPoints,
    int? MilaRank,
    int? MilaRankLastWeek,
    IList<MilaRuleResult> Rules
);

public class GameWeekResults
{
    public required IList<MilaResultDto> Results { get; set; }
    public int GameWeek { get; set; }
}

public class UserResults
{
    public required IList<MilaResultDto> Results { get; set; }
    public required string TeamName { get; set; }
}

public record RuleResult(decimal Points, string ShortName);

public record MilaResultDto(
    string Gw,
    decimal GwScore,
    string TeamName,
    string UserName,
    int UserId,
    decimal GwPosition,
    int GameWeek,
    int MilaRank,
    int? MilaRankLastWeek,
    decimal? CumulativeMilaPoints,
    decimal CumulativeAverageMilaPoints,
    decimal TotalCumulativeAverageMilaPoints,
    IList<MilaRuleResult> Rules
);
