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

public record MilaResults
{
    public required IList<GameWeekResults> ResultsByWeek { get; init; }
    public required IList<UserResults> ResultsByUser { get; init; }
    public required IList<OverallResult> OverallScore { get; init; }
    public required IList<Rule> Rules { get; init; }
    public required bool IsLive { get; init; }
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
    public required IList<MilaResultDto> Results { get; init; }
    public int GameWeek { get; init; }
}

public class UserResults
{
    public required IList<MilaResultDto> Results { get; init; }
    public required string TeamName { get; init; }
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
