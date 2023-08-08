using Milabowl.Domain.Entities.Fantasy;
using Milabowl.Domain.Entities.Milabowl;

namespace Milabowl.Domain.Processing;

public interface IMilaPointsProcessorService
{
    Task UpdateMilaPoints();
}

public class MilaPointsProcessorService : IMilaPointsProcessorService
{
    private readonly IMilaRuleBusiness _milaRuleBusiness;
    private readonly IProcessingRepository _repository;
    private string _BombHolder = "Henrik Granum";

    public MilaPointsProcessorService(IMilaRuleBusiness milaRuleBusiness, IProcessingRepository repository)
    {
        _milaRuleBusiness = milaRuleBusiness;
        _repository = repository;
    }

    public async Task UpdateMilaPoints()
    {
        var random = new Random(42);
        var bombRounds = new List<int>();

        while (bombRounds.Count < 7)
        {
            var nextRandom = random.Next(2, 38);
            if (bombRounds.Contains(nextRandom))
            {
                continue;
            }
            
            bombRounds.Add(nextRandom);
        }
        
        var events = await _repository.GetEventsToProcess();

        foreach (var evt in events)
        {
            var users = await _repository.GetUserToProcess(evt.EventId);
            var (mostTradedInPlayer, mostTradedOutPlayer) = await _repository.GetMostTradedPlayers(evt.EventId);

            var milaGameweekScores = new List<MilaGWScore>();

            foreach (var user in users)
            {
                if (await _repository.IsEventAlreadyCalculated(evt.Name, user.EntryName))
                {
                    continue;
                }

                var pointsPerGameweekAsc = user.Lineups
                    .Where(l => l.Event.GameWeek <= evt.GameWeek)
                    .OrderBy(l => l.Event.GameWeek)
                    .Select(l => l.PlayerEventLineups.Sum(pel => pel.PlayerEvent.TotalPoints * pel.Multiplier))
                    .ToList();

                var playerEventsForUserOnEvent = user.Lineups
                    ?.FirstOrDefault(l => l.Event.EventId == evt.EventId)
                    ?.PlayerEventLineups
                    .Select(pel => new MilaRuleData
                    {
                        Multiplier = pel.Multiplier,
                        IsCaptain = pel.IsCaptain,
                        Minutes = pel.PlayerEvent.Minutes,
                        GoalsScored = pel.PlayerEvent.GoalsScored,
                        Assists = pel.PlayerEvent.Assists,
                        CleanSheets = pel.PlayerEvent.CleanSheets,
                        GoalsConceded = pel.PlayerEvent.GoalsConceded,
                        OwnGoals = pel.PlayerEvent.OwnGoals,
                        YellowCards = pel.PlayerEvent.YellowCards,
                        RedCards = pel.PlayerEvent.RedCards,
                        TotalPoints = pel.PlayerEvent.TotalPoints,
                        PlayerPosition = pel.PlayerEvent.Player.ElementType,
                        Player = pel.PlayerEvent.Player
                    }
                    )
                    .ToList();


                var playersForUserLastWeek = user.Lineups
                    ?.FirstOrDefault(l => l.Event.GameWeek == evt.GameWeek - 1)
                    ?.PlayerEventLineups
                    ?.Select(pel => pel.PlayerEvent.Player);

                var userHeadToHead = user.HeadToHeadEvents
                    ?.FirstOrDefault(l => l.Event.EventId == evt.EventId);

                var opponentHeadToHead = await _repository.GetOpponentHeadToHead(userHeadToHead.FantasyUserHeadToHeadEventID, user.UserId);

                var userCaptain = user.Lineups
                    ?.FirstOrDefault(l => l.Event.EventId == evt.EventId)
                    ?.PlayerEventLineups.FirstOrDefault(pel => pel.IsCaptain)
                    ?.PlayerEvent.Player;


                var headToHeadDto = new UserHeadToHead
                {
                    DidWin = userHeadToHead?.Win == 1,
                    UserPoints = userHeadToHead?.Points,
                    OpponentPoints = opponentHeadToHead?.Points
                };

                var subsIn = playersForUserLastWeek != null ?
                    playerEventsForUserOnEvent?
                        .Where(pe => playersForUserLastWeek.All(ip => ip.PlayerId != pe.Player.PlayerId))
                        .Select(p => p.Player).ToList() : new List<Player>();

                var subsOut = playerEventsForUserOnEvent != null && playersForUserLastWeek != null ?
                    playersForUserLastWeek
                        .Where(p => playerEventsForUserOnEvent.All(pe => pe.Player.PlayerId != p.PlayerId)).ToList()
                    : new List<Player>();

                var milaPoints = new MilaGWScore
                {
                    MilaGWScoreId = Guid.NewGuid(),
                    GW = evt.Name,
                    GameWeek = evt.GameWeek,
                    UserName = user.UserName,
                    TeamName = user.EntryName,
                    GWScore = _milaRuleBusiness.GetGWScore(playerEventsForUserOnEvent),
                    CapFail = _milaRuleBusiness.GetCapFailScore(playerEventsForUserOnEvent),
                    BenchFail = _milaRuleBusiness.GetBenchFailScore(playerEventsForUserOnEvent),
                    CapKeep = _milaRuleBusiness.GetCapKeepScore(playerEventsForUserOnEvent),
                    CapDef = _milaRuleBusiness.GetCapDefScore(playerEventsForUserOnEvent),
                    GW69 = _milaRuleBusiness.GetSixtyNine(playerEventsForUserOnEvent),
                    RedCard = _milaRuleBusiness.GetRedCardScore(playerEventsForUserOnEvent),
                    YellowCard = _milaRuleBusiness.GetYellowCardScore(playerEventsForUserOnEvent),
                    MinusIsPlus = _milaRuleBusiness.GetMinusIsPlusScore(playerEventsForUserOnEvent), //TODO: Change to MinusIsPlus
                    IncreaseStreak = _milaRuleBusiness.GetIncreaseStreakScore(pointsPerGameweekAsc, evt.GameWeek),
                    EqualStreak = pointsPerGameweekAsc.Count > 1 && pointsPerGameweekAsc[^1] == pointsPerGameweekAsc[^2] ? 6.9m : 0,
                    HeadToHeadMeta = _milaRuleBusiness.GetHeadToHeadMetaScore(headToHeadDto),
                    UniqueCap = _milaRuleBusiness.GetUniqueCaptainScore(userCaptain, evt.Lineups),
                    SixtyNineSub = _milaRuleBusiness.GetSixtyNineSub(playerEventsForUserOnEvent),
                    TrendyBitch = _milaRuleBusiness.GetTrendyBitchScore(subsIn, subsOut, mostTradedInPlayer, mostTradedOutPlayer),
                    ActiveChip = user.Lineups?.First(l => l.Event.EventId == evt.EventId)?.ActiveChip
                };

                milaGameweekScores.Add(milaPoints);
            }

            var points = 0.0;
            foreach (var grp in milaGameweekScores.OrderBy(m => m.GWScore).GroupBy(g => g.GWScore))
            {
                foreach (var userWithScore in grp.ToList())
                {
                    userWithScore.GWPositionScore = (decimal)points / 2;
                }
                points++;
            }

            var points2 = 0;
            foreach (var grp in milaGameweekScores.OrderByDescending(m => m.GWScore).GroupBy(g => g.GWScore))
            {
                foreach (var userWithScore in grp.ToList())
                {
                    userWithScore.GWPosition = points2;
                }
                points2++;
            }
            
            foreach (var mgs in milaGameweekScores)
            {
                mgs.CalculateMilaPoints();
            }
            
            // PowerUp / Chip calculation
            foreach (var lineup in evt.Lineups.Where(l => l.ActiveChip == "bboost"))
            {
                var currentGameweekScore =
                    milaGameweekScores.FirstOrDefault(mgw => mgw.UserName == lineup.User.UserName);
                var nextContender = milaGameweekScores.FirstOrDefault(m => m.GWPosition == currentGameweekScore?.GWPosition - 1);
                if (nextContender is not null)
                {
                    nextContender.GreenShell = -3;
                }
                else if(currentGameweekScore is not null)
                {
                    currentGameweekScore.GreenShell = -3;
                }
            }
            
            foreach (var lineup in evt.Lineups.Where(l => l.ActiveChip == "wildcard"))
            {
                var currentGameweekScore =
                    milaGameweekScores.FirstOrDefault(mgw => mgw.UserName == lineup.User.UserName);
                var prevContender = milaGameweekScores.FirstOrDefault(m => m.GWPosition == currentGameweekScore?.GWPosition + 1);
                if (prevContender is not null)
                {
                    prevContender.Banana = -3;
                }
                else if(currentGameweekScore is not null)
                {
                    currentGameweekScore.Banana = -3;
                }
            }

            foreach (var lineup in evt.Lineups.Where(l => l.ActiveChip == "freehit"))
            {
                var userInFront = await _repository.GetUsernameDirectlyInFront(evt.GameWeek, lineup.User.UserName);
                var userInFrontGameWeek =
                    milaGameweekScores.First(mgw => mgw.UserName == userInFront);
                userInFrontGameWeek.RedShell = -3;
            }

            foreach (var mgs in milaGameweekScores)
            {
                mgs.CalculateChipPoints();
            }

            // Bomb
            var bombHolder = milaGameweekScores.First(b => b.UserName == _BombHolder);
            bombHolder.BombState = BombState.Holding;

            var bombHolderH2H = users.First(u => u.UserName == _BombHolder)
                .HeadToHeadEvents
                .First(l => l.Event.EventId == evt.EventId);
            if (bombHolder.ActiveChip is not null)
            {
                var bombRecipient = milaGameweekScores.FirstOrDefault(m => m.GWPosition == bombHolder?.GWPosition - 1) 
                                    ?? milaGameweekScores.OrderBy(m => m.GWPosition).Skip(1).First();

                bombHolder.BombState = BombState.HandingOver_Chip;
                bombRecipient.BombState = BombState.Receiving;
                _BombHolder = bombRecipient.UserName;
            }
            else if (bombHolderH2H.Win == 1)
            {
                var recipientH2H = evt.PlayerHeadToHeadEvents.FirstOrDefault(p =>
                    p.FantasyUserHeadToHeadEventID == bombHolderH2H.FantasyUserHeadToHeadEventID
                    && p.User.UserName != bombHolderH2H.User.UserName
                );

                MilaGWScore bombRecipient;
                if(recipientH2H is not null)
                {
                    bombRecipient =
                        milaGameweekScores.First(m => m.UserName == recipientH2H.User.UserName);
                }
                else
                {
                    bombRecipient = milaGameweekScores
                        .OrderByDescending(mgw => mgw.MilaPoints)
                        .First();

                    if (bombRecipient.UserName == bombHolder.UserName)
                    {
                        bombRecipient = milaGameweekScores
                            .OrderByDescending(mgw => mgw.MilaPoints)
                            .Skip(1)
                            .First();
                    }
                }

                bombHolder.BombState = BombState.HandingOver_H2H;
                bombRecipient.BombState = BombState.Receiving;
                _BombHolder = bombRecipient.UserName;
            }

            var roundEndBombHolder = milaGameweekScores
                .First(b => b.BombState == BombState.Holding || b.BombState == BombState.Receiving);
            if (roundEndBombHolder.UserName == _BombHolder)
            {
                roundEndBombHolder.BombState = BombState.Holding;
            }

            if (bombRounds.Contains(evt.GameWeek))
            {
                roundEndBombHolder.BombState = BombState.Exploded;
                roundEndBombHolder.BombPoints = -5;
                roundEndBombHolder.MilaPoints += roundEndBombHolder.BombPoints;

                var newBombHolder = milaGameweekScores.FirstOrDefault(m => m.GWPosition == bombHolder?.GWPosition - 1) 
                                    ?? milaGameweekScores.OrderBy(m => m.GWPosition).Skip(1).First();
                _BombHolder = newBombHolder.UserName;
            }

            await _repository.AddMilaGwScores(milaGameweekScores);

        }

        //await this._db.SaveChangesAsync();
    }
}