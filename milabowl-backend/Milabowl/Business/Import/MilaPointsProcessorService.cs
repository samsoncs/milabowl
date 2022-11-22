using System;
using System.Linq;
using System.Threading.Tasks;
using Milabowl.Infrastructure.Models;
using Milabowl.Business.DTOs;
using System.Collections.Generic;
using Milabowl.Business.DTOs.Rules;
using Milabowl.Repositories;

namespace Milabowl.Business.Import;

public interface IMilaPointsProcessorService
{
    Task UpdateMilaPoints();
}

public class MilaPointsProcessorService: IMilaPointsProcessorService
{
    private readonly IMilaRuleBusiness _milaRuleBusiness;
    private readonly IProcessingRepository _repository;

    public MilaPointsProcessorService(IMilaRuleBusiness milaRuleBusiness, IProcessingRepository repository)
    {
        this._milaRuleBusiness = milaRuleBusiness;
        this._repository = repository;
    }

    public async Task UpdateMilaPoints()
    {
        var events = await this._repository.GetEventsToProcess();

        foreach (var evt in events)
        {
            var users = await _repository.GetUserToProcess(evt.EventId);
            var (mostTradedInPlayer, mostTradedOutPlayer) = await _repository.GetMostTradedPlayers(evt.EventId);

            var milaGameweekScores = new List<MilaGWScore>();    

            foreach (var user in users)
            {
                if (await this._repository.IsEventAlreadyCalculated(evt.Name, user.EntryName))
                {
                    continue;
                }

                var pointsPerGameweekAsc = user.Lineups
                    .Where(l => l.Event.GameWeek <= evt.GameWeek)
                    .OrderBy(l => l.Event.GameWeek)
                    .Select(l => l.PlayerEventLineups.Sum(pel => pel.PlayerEvent.TotalPoints*pel.Multiplier))
                    .ToList();

                var playerEventsForUserOnEvent = user.Lineups
                    ?.FirstOrDefault(l => l.Event.EventId == evt.EventId)
                    ?.PlayerEventLineups
                    .Select(pel => new MilaRuleDTO  {
                            Multiplier =        pel.Multiplier,
                            IsCaptain =         pel.IsCaptain,
                            Minutes =           pel.PlayerEvent.Minutes,
                            GoalsScored =       pel.PlayerEvent.GoalsScored,
                            Assists =           pel.PlayerEvent.Assists,
                            CleanSheets =       pel.PlayerEvent.CleanSheets,
                            GoalsConceded =     pel.PlayerEvent.GoalsConceded,
                            OwnGoals =          pel.PlayerEvent.OwnGoals,
                            YellowCards =       pel.PlayerEvent.YellowCards,
                            RedCards =          pel.PlayerEvent.RedCards,
                            TotalPoints =       pel.PlayerEvent.TotalPoints,
                            PlayerPosition =    pel.PlayerEvent.Player.ElementType,
                            Player =            pel.PlayerEvent.Player
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


                var headToHeadDto = new UserHeadToHeadDTO
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
                    GWScore = this._milaRuleBusiness.GetGWScore(playerEventsForUserOnEvent),                 
                    CapFail = this._milaRuleBusiness.GetCapFailScore(playerEventsForUserOnEvent),
                    BenchFail = this._milaRuleBusiness.GetBenchFailScore(playerEventsForUserOnEvent),
                    CapKeep = this._milaRuleBusiness.GetCapKeepScore(playerEventsForUserOnEvent),
                    CapDef = this._milaRuleBusiness.GetCapDefScore(playerEventsForUserOnEvent),
                    GW69 = this._milaRuleBusiness.GetSixtyNine(playerEventsForUserOnEvent),
                    RedCard = this._milaRuleBusiness.GetRedCardScore(playerEventsForUserOnEvent), 
                    YellowCard = this._milaRuleBusiness.GetYellowCardScore(playerEventsForUserOnEvent),
                    MinusIsPlus = this._milaRuleBusiness.GetMinusIsPlusScore(playerEventsForUserOnEvent), //TODO: Change to MinusIsPlus
                    IncreaseStreak = this._milaRuleBusiness.GetIncreaseStreakScore(pointsPerGameweekAsc, evt.GameWeek),
                    EqualStreak = pointsPerGameweekAsc.Count > 1 && pointsPerGameweekAsc[^1] == pointsPerGameweekAsc[^2] ? 6.9m : 0,
                    HeadToHeadMeta = this._milaRuleBusiness.GetHeadToHeadMetaScore(headToHeadDto),
                    UniqueCap = this._milaRuleBusiness.GetUniqueCaptainScore(userCaptain, evt.Lineups),
                    SixtyNineSub = this._milaRuleBusiness.GetSixtyNineSub(playerEventsForUserOnEvent),
                    TrendyBitch = this._milaRuleBusiness.GetTrendyBitchScore(subsIn, subsOut, mostTradedInPlayer, mostTradedOutPlayer)
                };

                milaGameweekScores.Add(milaPoints);
            }           

            var points = 0.0;
            foreach(var grp in milaGameweekScores.OrderBy(m => m.GWScore).GroupBy(g => g.GWScore)){
                foreach(var userWithScore in grp.ToList()){
                    userWithScore.GWPositionScore = (decimal) points / 2;
                }
                points++;                   
            }

            var points2 = 0;
            foreach(var grp in milaGameweekScores.OrderByDescending(m => m.GWScore).GroupBy(g => g.GWScore)){
                foreach(var userWithScore in grp.ToList()){
                    userWithScore.GWPosition = points2;
                }
                points2++;                   
            }

            foreach (var mgs in milaGameweekScores)
            {
                mgs.CalculateMilaPoints();
            }

            await _repository.AddMilaGwScores(milaGameweekScores);

        }
            
        //await this._db.SaveChangesAsync();
    }
}