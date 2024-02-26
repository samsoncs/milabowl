namespace Milabowl.Processing.DataImport;

public record UserGameWeek(IList<PlayerEvent> Lineup);

public record PlayerEvent(
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
    int PlayerPosition,
    int IncreaseStreak,
    int EqualStreak
);