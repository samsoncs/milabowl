using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Milabowl.Business.DTOs.Api;
using Milabowl.Infrastructure.Models;
using Milabowl.Utils;
using NUnit.Framework;

namespace Milabowl.Test.Integration
{
    [TestFixture, Rollback]
    public class MilaResultsControllerTest: ControllerTestBase
    {
        [Test]
        public async Task ShouldGetMilaResults()
        {
            await this.HttpClient.GetAsync("api/MilaResults/clear-cache");
            var team1 = "Super Team";
            var team2 = "Team Lannister";
            var gameWeek1 = 91;
            var gameWeek2 = 92;
            var milaGwScoreTeam1Week1 = new MilaGWScore
                {MilaGWScoreId = Guid.NewGuid(), GameWeek = gameWeek1, TeamName = team1, MilaPoints = 6.9m};
            var milaGwScoreTeam1Week2 = new MilaGWScore
                {MilaGWScoreId = Guid.NewGuid(), GameWeek = gameWeek2, TeamName = team1, MilaPoints = 7m};
            var milaGwScoreTeam2Week1 = new MilaGWScore
                {MilaGWScoreId = Guid.NewGuid(), GameWeek = gameWeek1, TeamName = team2, MilaPoints = 8m};
            var milaGwScores = new List<MilaGWScore>
            {
                milaGwScoreTeam1Week1,
                milaGwScoreTeam1Week2,
                milaGwScoreTeam2Week1
            };

            await this.FantasyContext.MilaGWScores.AddRangeAsync(
                milaGwScores
            );
            await this.FantasyContext.SaveChangesAsync();

            var milaResults = await this.HttpClient.GetDeserializedAsync<MilaResultsDTO>($"api/MilaResults");

            var resultsByUserToCheck = milaResults.ResultsByUser
                .Where(r => r.TeamName == team1 || r.TeamName == team2).ToList();
            var resultsByWeekToCheck = milaResults.ResultsByWeek
                .Where(r => r.GameWeek == gameWeek1 || r.GameWeek == gameWeek2).ToList();
            var overallScoresToCheck = milaResults.OverallScore
                .Where(r => r.TeamName == team1 || r.TeamName == team2).ToList();
            resultsByUserToCheck.Count.Should().Be(2);
            resultsByWeekToCheck.Count.Should().Be(2);
            overallScoresToCheck.Count.Should().Be(2);
        }

        [Test]
        public async Task ShouldUseCache()
        {
            await this.HttpClient.GetAsync("api/MilaResults/clear-cache");
            var team1 = "Team One";
            var team2 = "Team Two";
            var milaGwScore = new MilaGWScore { MilaGWScoreId = Guid.NewGuid(), TeamName = team1, MilaPoints = 6.9m };
            await this.FantasyContext.MilaGWScores.AddAsync(milaGwScore);
            await this.FantasyContext.SaveChangesAsync();
            await this.HttpClient.GetDeserializedAsync<MilaResultsDTO>($"api/MilaResults");
            var milaGwScore2 = new MilaGWScore { MilaGWScoreId = Guid.NewGuid(), TeamName = team2, MilaPoints = 69m };
            await this.FantasyContext.MilaGWScores.AddAsync(milaGwScore2);
            await this.FantasyContext.SaveChangesAsync();

            var milaResults = await this.HttpClient.GetDeserializedAsync<MilaResultsDTO>($"api/MilaResults");

            var resultsToCheck = milaResults.ResultsByUser.Where(r => r.TeamName == team1 || r.TeamName == team2).ToList();
            resultsToCheck.Count.Should().Be(1);
        }

        [Test]
        public async Task ShouldClearCache()
        {
            var team1 = "Team One";
            var team2 = "Team Two";
            await this.HttpClient.GetAsync("api/MilaResults/clear-cache");
            var milaGwScore = new MilaGWScore { MilaGWScoreId = Guid.NewGuid(), TeamName = team1, MilaPoints = 6.9m };
            await this.FantasyContext.MilaGWScores.AddAsync(milaGwScore);
            await this.FantasyContext.SaveChangesAsync();
            await this.HttpClient.GetDeserializedAsync<MilaResultsDTO>($"api/MilaResults");
            var milaGwScore2 = new MilaGWScore { MilaGWScoreId = Guid.NewGuid(), TeamName = team2, MilaPoints = 69m };
            await this.FantasyContext.MilaGWScores.AddAsync(milaGwScore2);
            await this.FantasyContext.SaveChangesAsync();
            await this.HttpClient.GetAsync("api/MilaResults/clear-cache");

            var milaResults = await this.HttpClient.GetDeserializedAsync<MilaResultsDTO>($"api/MilaResults");

            var resultsToCheck = milaResults.ResultsByUser.Where(r => r.TeamName == team1 || r.TeamName == team2).ToList();
            resultsToCheck.Count.Should().Be(2);
        }
    }
}
