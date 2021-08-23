using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Milabowl.Test.Helpers;
using NUnit.Framework;

namespace Milabowl.Test.Integration
{
    [TestFixture, Rollback]
    public class DataImportControllerTest: ControllerTestBase
    {
        private const int EVENT_ID = 1023;
        private const int TEAM_ID = 21442;
        private const int PLAYER_ID = 921505192;
        private const int LEAGUE_ID = 1531532;
        private const string USER_NAME = "User Name";

        public override void AddMockServices(IServiceCollection services)
        {
            var dataImportProviderMock = new ImportTest().GetDataProviderMock(EVENT_ID, TEAM_ID, PLAYER_ID, LEAGUE_ID, USER_NAME);
            services.AddScoped(servicesProvider => dataImportProviderMock.Object);
        }

        [Test]
        public async Task ShouldImportData()
        {
            var response = await this.HttpClient.GetAsync("api/DataImport");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            this.FantasyContext.Events.Count(e => e.FantasyEventId == EVENT_ID).Should().Be(1);
            this.FantasyContext.Leagues.Count(l => l.FantasyLeagueId == LEAGUE_ID).Should().Be(1);
            this.FantasyContext.Lineups.Count(l => l.Event.FantasyEventId == EVENT_ID).Should().Be(1);
            this.FantasyContext.PlayerEventLineups.Count(pel => pel.Lineup.Event.FantasyEventId == EVENT_ID).Should().Be(1);
            this.FantasyContext.PlayerEvents.Count(pe => pe.Event.FantasyEventId == EVENT_ID).Should().Be(1);
            this.FantasyContext.Players.Count(p => p.FantasyPlayerId == PLAYER_ID).Should().Be(1);
            this.FantasyContext.Teams.Count(t => t.FantasyTeamId == TEAM_ID).Should().Be(1);
            this.FantasyContext.Users.Count(u => u.UserName == USER_NAME).Should().Be(1);
            this.FantasyContext.UserLeagues.Count(ul => ul.League.FantasyLeagueId == LEAGUE_ID).Should().Be(1);
        }

        [Test]
        public async Task ShouldProcessData()
        {
            var gameWeek = "The Best GameWeek";
            await new ProcessorTest().PrepareDataInDb(this.FantasyContext, gameWeek);
            
            var response = await this.HttpClient.GetAsync("api/DataImport/Process");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            this.FantasyContext.MilaGWScores.Count(m => m.GW == gameWeek).Should().Be(1);
            this.FantasyContext.MilaGWScores.First(m => m.GW == gameWeek).GW69.Should().Be(6.9m);
        }

    }
}
