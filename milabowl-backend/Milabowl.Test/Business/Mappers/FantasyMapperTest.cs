using System;
using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using FluentAssertions;
using Milabowl.Business.DTOs.Import;
using Milabowl.Business.Mappers;
using Milabowl.Infrastructure.Models;
using Milabowl.Test.Helpers;
using NUnit.Framework;

namespace Milabowl.Test.Business.Mappers
{
    [TestFixture]
    public class FantasyMapperTest
    {
        private readonly IFantasyMapper _fantasyMapper;

        public FantasyMapperTest()
        {
            this._fantasyMapper = new FantasyMapper();
        }

        [Test]
        public void ShouldMapEvent()
        {
            var eventDto = this.GetFakedDto<EventDTO>();
            eventDto.Name = "Gameweek 1";

            var evt = this._fantasyMapper.GetEventFromEventDTO(eventDto);

            evt.AssertAllPropertiesNotNull();
            evt.FantasyEventId.Should().Be(eventDto.Id);
            evt.Name.Should().Be(eventDto.Name);
            evt.Deadline.Should().Be(eventDto.DeadlineTime);
            evt.Finished.Should().Be(eventDto.Finished);
            evt.DataChecked.Should().Be(eventDto.DataChecked);
            evt.GameWeek.Should().Be(1);
        }

        [Test]
        public void ShouldMapTeam()
        {
            var teamDto = this.GetFakedDto<TeamDTO>();

            var team = this._fantasyMapper.GetTeamFromTeamDTO(teamDto);

            team.AssertAllPropertiesNotNull();
            team.TeamName.Should().Be(teamDto.Name);
            team.TeamShortName.Should().Be(teamDto.ShortName);
            team.FantasyTeamId.Should().Be(teamDto.Id);
            team.FantasyTeamCode.Should().Be(teamDto.Code);
        }

        [Test]
        public void ShouldMapPlayer()
        {
            var playerDto = this.GetFakedDto<PlayerDTO>();
            var teams = new List<Team>{ new Team{ TeamId = Guid.NewGuid(), FantasyTeamId = playerDto.Team } };

            var player = this._fantasyMapper.GetPlayerFromPlayerDTO(playerDto, teams);

            player.AssertAllPropertiesNotNull();
            player.Team.TeamId.Should().Be(teams[0].TeamId);
            player.FirstName.Should().Be(playerDto.FirstName);
            player.LastName.Should().Be(playerDto.SecondName);
            player.NowCost.Should().Be(playerDto.NowCost);
            player.FantasyPlayerId.Should().Be(playerDto.Id);
            player.Code.Should().Be(playerDto.Code);
            player.ElementType.Should().Be(playerDto.ElementType);
            player.EventPoints.Should().Be(playerDto.EventPoints);
            player.Form.Should().Be(playerDto.Form);
            player.InDreamteam.Should().Be(playerDto.InDreamteam);
            player.News.Should().Be(playerDto.News);
            player.NewsAdded.Should().Be(playerDto.NewsAdded);
            player.Photo.Should().Be(playerDto.Photo);
            player.PointsPerGame.Should().Be(playerDto.PointsPerGame);
            player.SelectedByPercent.Should().Be(playerDto.SelectedByPercent);
            player.Special.Should().Be(playerDto.Special);
            player.Status.Should().Be(playerDto.Status);
            player.TotalPoints.Should().Be(playerDto.TotalPoints);
            player.TransfersIn.Should().Be(playerDto.TransfersIn);
            player.TransfersInEvent.Should().Be(playerDto.TransfersInEvent);
            player.TransfersOutEvent.Should().Be(playerDto.TransfersOutEvent);
            player.TransfersOut.Should().Be(playerDto.TransfersOut);
            player.ValueForm.Should().Be(playerDto.ValueForm);
            player.ValueSeason.Should().Be(playerDto.ValueSeason);
            player.WebName.Should().Be(playerDto.WebName);
            player.Minutes.Should().Be(playerDto.Minutes);
            player.GoalsScored.Should().Be(playerDto.goals_scored);
            player.Assists.Should().Be(playerDto.Assists);
            player.CleanSheets.Should().Be(playerDto.CleanSheets);
            player.GoalsConceded.Should().Be(playerDto.GoalsConceded);
            player.OwnGoals.Should().Be(playerDto.OwnGoals);
            player.PenaltiesSaved.Should().Be(playerDto.PenaltiesSaved);
            player.PenaltiesMissed.Should().Be(playerDto.PenaltiesMissed);
            player.YellowCards.Should().Be(playerDto.YellowCards);
            player.RedCards.Should().Be(playerDto.RedCards);
            player.Saves.Should().Be(playerDto.Saves);
            player.Bonus.Should().Be(playerDto.Bonus);
            player.Bps.Should().Be(playerDto.Bps);
            player.Influence.Should().Be(playerDto.Influence);
            player.Creativity.Should().Be(playerDto.Creativity);
            player.Threat.Should().Be(playerDto.Threat);
        }

        [Test]
        public void ShouldMapLeague()
        {
            var leagueDto = this.GetFakedDto<LeagueDTO>();

            var league = this._fantasyMapper.GetLeagueFromLeagueDTO(leagueDto);

            league.AssertAllPropertiesNotNull();
            league.LeagueId.Should().NotBeEmpty();
            league.AdminEntry.Should().Be(leagueDto.admin_entry);
            league.Closed.Should().Be(leagueDto.closed);
            league.CodePrivacy.Should().Be(leagueDto.code_privacy);
            league.Created.Should().Be(leagueDto.created);
            league.FantasyLeagueId.Should().Be(leagueDto.id);
            league.LeagueType.Should().Be(leagueDto.league_type);
            league.Name.Should().Be(leagueDto.name);
            league.Scoring.Should().Be(leagueDto.scoring);
            league.StartEvent.Should().Be(leagueDto.start_event);
        }

        [Test]
        public void ShouldMapUser()
        {
            var resultDto = this.GetFakedDto<ResultDTO>();

            var user = this._fantasyMapper.GetUserFromResultDTO(resultDto);

            user.AssertAllPropertiesNotNull();
            user.UserId.Should().NotBeEmpty();
            user.EntryName.Should().Be(resultDto.entry_name);
            user.UserName.Should().Be(resultDto.player_name);
            user.FantasyEntryId.Should().Be(resultDto.entry);
            user.FantasyResultId.Should().Be(resultDto.id);
            user.Rank.Should().Be(resultDto.rank);
            user.LastRank.Should().Be(resultDto.last_rank);
            user.EventTotal.Should().Be(resultDto.event_total);
        }

        [Test]
        public void ShouldMapUserLeague()
        {
            var user = new User{ UserId = Guid.NewGuid() };
            var league = new League{ LeagueId = Guid.NewGuid() };

            var userLeague = this._fantasyMapper.GetUserLeagueFromUserAndLeague(user, league);

            userLeague.User.UserId.Should().Be(user.UserId);
            userLeague.League.LeagueId.Should().Be(league.LeagueId);
        }

        [Test]
        public void ShouldMapPlayerEvent()
        {
            var elementDto = this.GetFakedDto<ElementDTO>();
            var statsDto = this.GetFakedDto<StatsDTO>();
            elementDto.stats = statsDto;
            var evt = new Event{ EventId = Guid.NewGuid() };
            var players = new List<Player>{ new Player{ PlayerId = Guid.NewGuid(), FantasyPlayerId = elementDto.id } };

            var playerEvent = this._fantasyMapper.GetPlayerEvent(elementDto, evt, players, new List<ElementHistoryRootDTO>(), new List<FixtureDTO>());

            playerEvent.AssertAllPropertiesNotNull();
            playerEvent.Event.EventId.Should().Be(evt.EventId);
            playerEvent.Player.PlayerId.Should().Be(players[0].PlayerId);
            playerEvent.GoalsScored.Should().Be(elementDto.stats.goals_scored);
            playerEvent.Assists.Should().Be(elementDto.stats.assists);
            playerEvent.TotalPoints.Should().Be(elementDto.stats.total_points);
            playerEvent.Bonus.Should().Be(elementDto.stats.bonus);
            playerEvent.Bps.Should().Be(elementDto.stats.bps);
            playerEvent.CleanSheets.Should().Be(elementDto.stats.clean_sheets);
            playerEvent.Creativity.Should().Be(elementDto.stats.creativity);
            playerEvent.Minutes.Should().Be(elementDto.stats.minutes);
            playerEvent.GoalsConceded.Should().Be(elementDto.stats.goals_conceded);
            playerEvent.OwnGoals.Should().Be(elementDto.stats.own_goals);
            playerEvent.PenaltiesMissed.Should().Be(elementDto.stats.penalties_missed);
            playerEvent.PenaltiesSaved.Should().Be(elementDto.stats.penalties_saved);
            playerEvent.YellowCards.Should().Be(elementDto.stats.yellow_cards);
            playerEvent.RedCards.Should().Be(elementDto.stats.red_cards);
            playerEvent.Saves.Should().Be(elementDto.stats.saves);
            playerEvent.InDreamteam.Should().Be(elementDto.stats.in_dreamteam);
            playerEvent.IctIndex.Should().Be(elementDto.stats.ict_index);
            playerEvent.Threat.Should().Be(elementDto.stats.threat);
            playerEvent.Influence.Should().Be(elementDto.stats.influence);
            playerEvent.FantasyPlayerEventId.Should().Be(elementDto.id);
        }

        [Test]
        public void ShouldMapUserHeadToHeadEvents()
        {
            var headToHeadResultDto = this.GetFakedDto<HeadToHeadResultDTO>();
            var evt = new Event { EventId = Guid.NewGuid() };
            var users = new List<User>
            {
                new (){ UserId = Guid.NewGuid(), FantasyEntryId = headToHeadResultDto.entry_1_entry.Value },
                new (){ UserId = Guid.NewGuid(), FantasyEntryId = headToHeadResultDto.entry_2_entry.Value },
            };

            var userHeadToHeadEvents = this._fantasyMapper.GetUserHeadToHeadEvents(headToHeadResultDto, evt, users);

            userHeadToHeadEvents.Count.Should().Be(2);
            AssertUserHeadToHeadEvent(userHeadToHeadEvents[0], users[0], evt, headToHeadResultDto);
            AssertUserHeadToHeadEvent(userHeadToHeadEvents[1], users[1], evt, headToHeadResultDto, false);
        }

        [Test]
        public void ShouldMapLineup()
        {
            var evt = new Event { EventId = Guid.NewGuid() };
            var user = new User { UserId = Guid.NewGuid() };

            var lineup = this._fantasyMapper.GetLineup(evt, user);

            lineup.LineupId.Should().NotBeEmpty();
            lineup.Event.EventId.Should().Be(evt.EventId);
            lineup.User.UserId.Should().Be(user.UserId);
        }

        [Test]
        public void ShouldMapPlayerEventLineup()
        {
            var pickDto = this.GetFakedDto<PickDTO>();
            var lineup = new Lineup { LineupId = Guid.NewGuid() };
            var evt = new Event { EventId = Guid.NewGuid() };
            var playerEvents = new List<PlayerEvent>{ 
                new PlayerEvent
                {
                    FantasyPlayerEventId = 1,
                    Player = new Player{ FantasyPlayerId = pickDto.element},
                    Event = new Event{ GameWeek = evt.GameWeek }
                }
            };

            var playerEventLineup = this._fantasyMapper.GetPlayerEventLineup(pickDto, lineup, playerEvents, evt);

            playerEventLineup.AssertAllPropertiesNotNull();
            playerEventLineup.PlayerEventLineupId.Should().NotBeEmpty();
            playerEventLineup.Lineup.LineupId.Should().Be(lineup.LineupId);
            playerEventLineup.PlayerEvent.FantasyPlayerEventId.Should().Be(playerEvents[0].FantasyPlayerEventId);
            playerEventLineup.Multiplier.Should().Be(pickDto.multiplier);
            playerEventLineup.IsCaptain.Should().Be(pickDto.is_captain);
            playerEventLineup.IsViceCaptain.Should().Be(pickDto.is_vice_captain);
        }

        private void AssertUserHeadToHeadEvent(UserHeadToHeadEvent userHeadToHeadEvent, User user, Event evt, HeadToHeadResultDTO headToHeadResultDto, bool checkEntry1 = true)
        {
            userHeadToHeadEvent.User.Should().Be(user);
            userHeadToHeadEvent.Event.Should().Be(evt);
            userHeadToHeadEvent.Draw.Should().Be(checkEntry1 ? headToHeadResultDto.entry_1_draw : headToHeadResultDto.entry_2_draw);
            userHeadToHeadEvent.Win.Should().Be(checkEntry1 ? headToHeadResultDto.entry_1_win : headToHeadResultDto.entry_2_win);
            userHeadToHeadEvent.Loss.Should().Be(checkEntry1 ? headToHeadResultDto.entry_1_loss : headToHeadResultDto.entry_2_loss);
            userHeadToHeadEvent.Total.Should().Be(checkEntry1 ? headToHeadResultDto.entry_1_total : headToHeadResultDto.entry_2_total);
            userHeadToHeadEvent.Points.Should().Be(checkEntry1 ? headToHeadResultDto.entry_1_points : headToHeadResultDto.entry_2_points);
            userHeadToHeadEvent.FantasyUserHeadToHeadEventID.Should().Be(headToHeadResultDto.id);
            userHeadToHeadEvent.UserHeadToHeadEventID.Should().NotBeEmpty();
            userHeadToHeadEvent.IsKnockout.Should().Be(headToHeadResultDto.is_knockout);
            userHeadToHeadEvent.IsBye.Should().Be(headToHeadResultDto.is_bye);
            userHeadToHeadEvent.LeagueID.Should().Be(headToHeadResultDto.league);
        }

        private T GetFakedDto<T>() where T: class
        {
            var faker = new Faker<T>();

            faker.RuleForType(typeof(int), r => r.Random.Int());
            faker.RuleForType(typeof(int?), r => (int?)r.Random.Int());
            faker.RuleForType(typeof(string), r => r.Company.CompanyName());
            faker.RuleForType(typeof(bool), r => r.Random.Bool());
            faker.RuleForType(typeof(DateTime?), r => r.Date.Soon().OrNull(r, 0));
            faker.RuleForType(typeof(DateTime), r => r.Date.Soon());

            return faker.Generate();
        }
    }
}
