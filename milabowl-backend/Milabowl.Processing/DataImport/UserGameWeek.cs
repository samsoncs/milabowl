using Milabowl.Processing.DataImport.MilaDtos;

namespace Milabowl.Processing.DataImport;

public record Event(int GameWeek, string Name);

public record HeadToHeadEvent(
    int Points,
    int Win,
    int Draw,
    int Loss,
    int Total,
    bool IsKnockout,
    int LeagueId,
    bool IsBye
);

public record User(
    int Id,
    string UserName,
    string TeamName,
    int Rank,
    int LastRank,
    int EventTotal
);

public record UserGameWeek(
    Event @Event,
    HeadToHeadEvent HeadToHeadEvent,
    User User,
    IList<PlayerEvent> Lineup,
    string? ActiveChip
)
{
    public IList<UserGameWeek> Opponents { get; private set; } = new List<UserGameWeek>();

    public void AddOpponentsForGameWeek(IList<UserGameWeek> userGameWeeks)
    {
        Opponents = userGameWeeks.Where(u => u.User.Id != User.Id).ToList();
    }

    public decimal TotalScore => Lineup.Sum(l => l.TotalPoints * l.Multiplier);
};

public record PlayerEvent(
    string FirstName,
    string Surname,
    //Guid PlayerEventId,
    int FantasyPlayerEventId,
    // Player Player,
    //Guid FkPlayerId,
    //Event Event,
    //Guid FkEventId,
    int Minutes,
    int GoalsScored,
    int Assists,
    int GoalsConceded,
    int CleanSheets,
    int PenaltiesSaved,
    int OwnGoals,
    int PenaltiesMissed,
    int YellowCards,
    int RedCards,
    int Saves,
    int Bonus,
    int Bps,
    string Influence,
    string Creativity,
    string Threat,
    string IctIndex,
    int TotalPoints,
    bool InDreamteam,
    int Multiplier,
    bool IsCaptain,
    bool IsViceCaptain,
    int PlayerPosition
);
