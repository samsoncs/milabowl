using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Milabowl.Infrastructure.Contexts;
using Milabowl.Infrastructure.Models;

namespace Milabowl.Test.Helpers
{
    public class ProcessorTest
    {
        public async Task PrepareDataInDb(FantasyContext fantasyContext, string gameWeek)
        {
            var evt = new Event
            {
                EventId = Guid.NewGuid(),
                Finished = true,
                DataChecked = true,
                GameWeek = 1,
                Name = gameWeek
            };

            var player = new Player
            {
                PlayerId = Guid.NewGuid(),

            };

            var team = new Team
            {
                TeamId = Guid.NewGuid(),
                Players = new List<Player> { player }
            };

            var playerEvent = new PlayerEvent
            {
                Player = player,
                Event = evt,
                TotalPoints = 69
            };

            var user = new User
            {
                UserId = Guid.NewGuid()
            };

            var league = new League
            {
                LeagueId = Guid.NewGuid()
            };

            var userLeague = new UserLeague
            {
                User = user,
                League = league
            };

            var playerEventLineup = new PlayerEventLineup
            {
                PlayerEvent = playerEvent,
                Multiplier = 1
            };

            var lineup = new Lineup
            {
                Event = evt,
                User = user,
                PlayerEventLineups = new List<PlayerEventLineup> { playerEventLineup }
            };

            await fantasyContext.AddAsync(evt);
            await fantasyContext.AddAsync(player);
            await fantasyContext.AddAsync(playerEvent);
            await fantasyContext.AddAsync(user);
            await fantasyContext.AddAsync(league);
            await fantasyContext.AddAsync(userLeague);
            await fantasyContext.AddAsync(playerEventLineup);
            await fantasyContext.AddAsync(lineup);
            await fantasyContext.AddAsync(team);
            await fantasyContext.SaveChangesAsync();
        }

    }
}
