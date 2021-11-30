using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Milabowl.Business.DTOs.Import;
using Milabowl.Infrastructure.Models;

namespace Milabowl.Business.Mappers
{
    public interface IFantasyMapper
    {
        Event GetEventFromEventDTO(EventDTO e);
        Team GetTeamFromTeamDTO(TeamDTO t);
        Player GetPlayerFromPlayerDTO(PlayerDTO p, IList<Team> teams);
        League GetLeagueFromLeagueDTO(LeagueDTO leagueDto);
        User GetUserFromResultDTO(ResultDTO r);
        UserLeague GetUserLeagueFromUserAndLeague(User u, League league);
        PlayerEvent GetPlayerEvent(ElementDTO e, Event evt, IList<Player> players);
        Lineup GetLineup(Event evt, User user);
        PlayerEventLineup GetPlayerEventLineup(PickDTO p, Lineup lineup, IList<PlayerEvent> playerEvents, Event evt);
        IList<UserHeadToHeadEvent> GetUserHeadToHeadEvents(HeadToHeadResultDTO headToHeadResult, Event evt, IList<User> users);
    }

    public class FantasyMapper: IFantasyMapper
    {
        public Event GetEventFromEventDTO(EventDTO e)
        {
            return new Event
            {
                EventId = Guid.NewGuid(),
                Deadline = e.DeadlineTime,
                FantasyEventId = e.Id,
                Finished = e.Finished,
                DataChecked = e.DataChecked,
                Name = e.Name,
                GameWeek = int.Parse(Regex.Match(e.Name, @"\s[0-9]{1,2}\b").Value)
            };
        }

        public Team GetTeamFromTeamDTO(TeamDTO t)
        {
            return new Team
            {
                TeamId = Guid.NewGuid(),
                TeamName = t.Name,
                TeamShortName = t.ShortName,
                FantasyTeamId = t.Id,
                FantasyTeamCode = t.Code
            };
        }

        public Player GetPlayerFromPlayerDTO(PlayerDTO p, IList<Team> teams)
        {
            return new Player
            {
                PlayerId = Guid.NewGuid(),
                FirstName = p.FirstName,
                LastName = p.SecondName,
                NowCost = p.NowCost,
                FantasyPlayerId = p.Id,
                Team = teams.FirstOrDefault(t => t.FantasyTeamId == p.Team),
                Code = p.Code,
                ElementType = p.ElementType,
                EventPoints = p.EventPoints,
                Form = p.Form,
                InDreamteam = p.InDreamteam,
                News = p.News,
                NewsAdded = p.NewsAdded,
                Photo = p.Photo,
                PointsPerGame = p.PointsPerGame,
                SelectedByPercent = p.SelectedByPercent,
                Special = p.Special,
                Status = p.Status,
                TotalPoints = p.TotalPoints,
                TransfersIn = p.TransfersIn,
                TransfersInEvent = p.TransfersInEvent,
                TransfersOut = p.TransfersOut,
                TransfersOutEvent = p.TransfersOutEvent,
                ValueForm = p.ValueForm,
                ValueSeason = p.ValueSeason,
                WebName = p.WebName,
                Minutes = p.Minutes,
                GoalsScored = p.goals_scored,
                Assists = p.Assists,
                CleanSheets = p.CleanSheets,
                GoalsConceded = p.GoalsConceded,
                OwnGoals = p.OwnGoals,
                PenaltiesSaved = p.PenaltiesSaved,
                PenaltiesMissed = p.PenaltiesMissed,
                YellowCards = p.YellowCards,
                RedCards = p.RedCards,
                Saves = p.Saves,
                Bonus = p.Bonus,
                Bps = p.Bps,
                Influence = p.Influence,
                Creativity = p.Creativity,
                Threat = p.Threat
            };
        }

        public League GetLeagueFromLeagueDTO(LeagueDTO leagueDto)
        {
            return new League
            {
                LeagueId = Guid.NewGuid(),
                AdminEntry = leagueDto.admin_entry,
                Closed = leagueDto.closed,
                CodePrivacy = leagueDto.code_privacy,
                Created = leagueDto.created,
                FantasyLeagueId = leagueDto.id,
                LeagueType = leagueDto.league_type,
                Name = leagueDto.name,
                Scoring = leagueDto.scoring,
                StartEvent = leagueDto.start_event,
            };
        }

        public User GetUserFromResultDTO(ResultDTO r)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                EntryName = r.entry_name,
                UserName = r.player_name,
                FantasyEntryId = r.entry,
                FantasyResultId = r.id,
                Rank = r.rank,
                LastRank = r.last_rank,
                EventTotal = r.event_total
            };
        }

        public UserLeague GetUserLeagueFromUserAndLeague(User u, League league)
        {
            return new UserLeague {League = league, User = u};
        }

        public PlayerEvent GetPlayerEvent(ElementDTO e, Event evt, IList<Player> players)
        {
            return new PlayerEvent
            {
                PlayerEventId = Guid.NewGuid(),
                Event = evt,
                Player = players.FirstOrDefault(p => p.FantasyPlayerId == e.id),
                GoalsScored = e.stats.goals_scored,
                Assists = e.stats.assists,
                TotalPoints = e.stats.total_points,
                Bonus = e.stats.bonus,
                Bps = e.stats.bps,
                CleanSheets = e.stats.clean_sheets,
                Creativity = e.stats.creativity,
                Minutes = e.stats.minutes,
                GoalsConceded = e.stats.goals_conceded,
                OwnGoals = e.stats.own_goals,
                PenaltiesMissed = e.stats.penalties_missed,
                PenaltiesSaved = e.stats.penalties_saved,
                YellowCards = e.stats.yellow_cards,
                RedCards = e.stats.red_cards,
                Saves = e.stats.saves,
                InDreamteam = e.stats.in_dreamteam,
                IctIndex = e.stats.ict_index,
                Threat = e.stats.threat,
                Influence = e.stats.influence,
                FantasyPlayerEventId = e.id
            };
        }

        public IList<UserHeadToHeadEvent> GetUserHeadToHeadEvents(HeadToHeadResultDTO headToHeadResult, Event evt, IList<User> users)
        {
            var headToHeadEvents = new List<UserHeadToHeadEvent>();

            if (headToHeadResult.entry_1_entry.HasValue)
            {
                headToHeadEvents.Add(
                new UserHeadToHeadEvent
                {
                    UserHeadToHeadEventID = Guid.NewGuid(),
                    FantasyUserHeadToHeadEventID = headToHeadResult.id,
                    Event = evt,

                    User = users.FirstOrDefault(u => u.FantasyEntryId == headToHeadResult.entry_1_entry),
                    Draw = headToHeadResult.entry_1_draw,
                    Loss = headToHeadResult.entry_1_loss,
                    Win = headToHeadResult.entry_1_win,
                    Points = headToHeadResult.entry_1_points,
                    Total = headToHeadResult.entry_1_total,

                    IsKnockout = headToHeadResult.is_knockout,
                    LeagueID = headToHeadResult.league,
                    IsBye = headToHeadResult.is_bye,
                });
            }

            if (headToHeadResult.entry_2_entry.HasValue)
            {
                headToHeadEvents.Add(
                new UserHeadToHeadEvent
                {
                    UserHeadToHeadEventID = Guid.NewGuid(),
                    FantasyUserHeadToHeadEventID = headToHeadResult.id,
                    Event = evt,

                    User = users.FirstOrDefault(u => u.FantasyEntryId == headToHeadResult.entry_2_entry),
                    Draw = headToHeadResult.entry_2_draw,
                    Loss = headToHeadResult.entry_2_loss,
                    Win = headToHeadResult.entry_2_win,
                    Points = headToHeadResult.entry_2_points,
                    Total = headToHeadResult.entry_2_total,

                    IsKnockout = headToHeadResult.is_knockout,
                    LeagueID = headToHeadResult.league,
                    IsBye = headToHeadResult.is_bye,
                });
            }

            return headToHeadEvents;
        }

        public Lineup GetLineup(Event evt, User user)
        {
            return new Lineup
            {
                LineupId = Guid.NewGuid(),
                Event = evt,
                User = user,
            };
        }

        public PlayerEventLineup GetPlayerEventLineup(PickDTO p, Lineup lineup, IList<PlayerEvent> playerEvents, Event evt)
        {
            return new PlayerEventLineup
            {
                PlayerEventLineupId = Guid.NewGuid(),
                Lineup = lineup,
                PlayerEvent = playerEvents.FirstOrDefault(pe =>
                    pe.Player.FantasyPlayerId == p.element && pe.Event.GameWeek == evt.GameWeek),
                Multiplier = p.multiplier,
                IsCaptain = p.is_captain,
                IsViceCaptain = p.is_vice_captain
            };
        }
    }
}
