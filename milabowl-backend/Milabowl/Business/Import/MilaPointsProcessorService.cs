using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Milabowl.Infrastructure.Contexts;
using Milabowl.Infrastructure.Models;
using Milabowl.Business.DTOs;
using System.Collections.Generic;
using Milabowl.Business.DTOs.Rules;

namespace Milabowl.Business.Import
{
    public interface IMilaPointsProcessorService
    {
        Task UpdateMilaPoints();
    }

    public class MilaPointsProcessorService: IMilaPointsProcessorService
    {
        private readonly IMilaRuleBusiness _milaRuleBusiness;
        private readonly FantasyContext _db;

        public MilaPointsProcessorService(IMilaRuleBusiness milaRuleBusiness, FantasyContext db)
        {
            this._milaRuleBusiness = milaRuleBusiness;
            this._db = db;
        }

        public async Task UpdateMilaPoints()
        {
            //Get data needed to calc points from db
            var events = await this._db.Events.Where(e => e.Finished && e.DataChecked).ToListAsync(); //All events that are finished, and have points calculated

            foreach (var evt in events)
            {
                var users = await this._db.Users
                    .Include(u => u.Lineups)
                        .ThenInclude(l => l.PlayerEventLineups)
                            .ThenInclude(pel => pel.PlayerEvent)
                                .ThenInclude (pel => pel.Player)
                    .Include(u => u.Lineups)
                        .ThenInclude(l => l.PlayerEventLineups)
                    .Include(u => u.Lineups)
                    .Include(u => u.HeadToHeadEvents)
                        .ThenInclude(hu => hu.Event)
                    .Where(u => u.Lineups.Any(l => l.Event.EventId == evt.EventId))
                    .ToListAsync();

                var milaGameweekScores = new List<MilaGWScore>();    

                foreach (var user in users)
                {
                    //Do not calculate if already calculated for the week
                    if (await this._db.MilaGWScores.AnyAsync(m => 
                        m.GW == evt.Name && 
                        m.TeamName == user.EntryName))
                    {
                        continue;
                    }
                   
                    var pointsPerGameweekDsc = user.Lineups
                        .OrderByDescending(l => l.Event.GameWeek)
                        .Select(l => l.PlayerEventLineups.Sum(pel => pel.PlayerEvent.TotalPoints*pel.Multiplier))
                        .ToList();

                    var pointsPerGameweekAsc = user.Lineups
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
                            }
                        )
                        .ToList();

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
                        EqualStreak = pointsPerGameweekDsc.Count > 1 && pointsPerGameweekDsc[0] == pointsPerGameweekDsc[1] ? 6.9m : 0,
                    };

                    if (evt.Deadline > new DateTime(2021, 12, 31))
                    {
                        var userHeadToHead = user.HeadToHeadEvents
                            ?.FirstOrDefault(l => l.Event.EventId == evt.EventId);

                        var userCaptain = user.Lineups
                            ?.FirstOrDefault(l => l.Event.EventId == evt.EventId)
                            ?.PlayerEventLineups.FirstOrDefault(pel => pel.IsCaptain)
                            ?.PlayerEvent.Player;

                        var opponentHeadToHead = await this._db.UserHeadToHeadEvents.FirstOrDefaultAsync(h =>
                            h.FantasyUserHeadToHeadEventID == userHeadToHead.FantasyUserHeadToHeadEventID
                            && h.FkUserId != user.UserId);

                        var headToHeadDto = new UserHeadToHeadDTO
                        {
                            DidWin = userHeadToHead?.Win == 1,
                            UserPoints = userHeadToHead?.Points,
                            OpponentPoints = opponentHeadToHead?.Points
                        };

                        milaPoints.HeadToHeadMeta = this._milaRuleBusiness.GetHeadToHeadMetaScore(headToHeadDto);
                        //milaPoints.HeadToHeadStrongOpponentWin = this._milaRuleBusiness.GetHeadToHeadStrongerOpponentScore(headToHeadDto);
                        milaPoints.UniqueCap = this._milaRuleBusiness.GetUniqueCaptainScore(userCaptain, evt.Lineups);
                        milaPoints.SixtyNineSub = this._milaRuleBusiness.GetSixtyNineSub(playerEventsForUserOnEvent);
                    }

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
                    mgs.MilaPoints = 
                        mgs.CapFail 
                        + mgs.CapKeep 
                        + mgs.BenchFail 
                        + mgs.GWPositionScore 
                        + mgs.GW69 
                        + mgs.RedCard 
                        + mgs.YellowCard 
                        + mgs.MinusIsPlus 
                        + mgs.IncreaseStreak 
                        + mgs.EqualStreak 
                        + mgs.Hit
                        //+ mgs.HeadToHeadStrongOpponentWin
                        + mgs.HeadToHeadMeta
                        + mgs.UniqueCap
                        + mgs.SixtyNineSub;
                }

                await this._db.MilaGWScores.AddRangeAsync(milaGameweekScores);

            }
            await this._db.SaveChangesAsync();
        }
    }
}
