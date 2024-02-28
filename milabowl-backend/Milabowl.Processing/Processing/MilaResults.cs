namespace Milabowl.Processing.Processing;

public class MilaResults
{
    public IList<GameWeekResults> ResultsByWeek { get; set; }
    public IList<UserResults> ResultsByUser { get; set; }
    public IList<MilaResult> OverallScore { get; set; }
}

public class GameWeekResults
{
    public IList<MilaResult> Results { get; set; }
    public int GameWeek { get; set; }
}

public class UserResults
{
    public IList<MilaResult> Results { get; set; }
    public string TeamName { get; set; }
}

public record MilaResult(
    string Gw,
    decimal GwScore,
    string TeamName,
    string UserName,
    int UserId,
    decimal GwPosition,
    int GameWeek,
    decimal? CumulativeMilaPoints,
    decimal CumulativeAverageMilaPoints,
    decimal TotalCumulativeAverageMilaPoints,
    int? MilaRank,
    int? MilaRankLastWeek,
    Dictionary<string, RuleResult> Rules
);

public record RuleResult(decimal Points, string ShortName);
