using FluentAssertions;
using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class HardestThrustTests: MilaRuleTest<HardestThrust>
{
    [Fact]
    public void Should_get_1_point_6_points_if_players_defenders_scored_most_goals()
    {
        var oneGoalDefender = TestStateFactory
            .GetPlayer()
            .RuleFor(r => r.PlayerPosition, PlayerPosition.DEF)
            .RuleFor(r => r.GoalsScored, 1);
        var twoGoalDefender = TestStateFactory
            .GetPlayer()
            .RuleFor(r => r.PlayerPosition, PlayerPosition.DEF)
            .RuleFor(r => r.GoalsScored, 2);
        var opponent = new ManagerGameWeekStateBuilder()
            .WithLineup(oneGoalDefender)
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(twoGoalDefender)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(1.6m);
    }

    [Fact]
    public void Should_get_0_points_if_players_defenders_scored_least_goals()
    {
        var oneGoalDefender = TestStateFactory
            .GetPlayer()
            .RuleFor(r => r.PlayerPosition, PlayerPosition.DEF)
            .RuleFor(r => r.GoalsScored, 1);
        var twoGoalDefender = TestStateFactory
            .GetPlayer()
            .RuleFor(r => r.PlayerPosition, PlayerPosition.DEF)
            .RuleFor(r => r.GoalsScored, 2);
        var opponent = new ManagerGameWeekStateBuilder()
            .WithLineup(twoGoalDefender)
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(oneGoalDefender)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }

    [Fact]
    public void Should_get_0_points_if_players_defenders_scored_shared_most()
    {
        var oneGoalDefender = TestStateFactory
            .GetPlayer()
            .RuleFor(r => r.PlayerPosition, PlayerPosition.DEF)
            .RuleFor(r => r.GoalsScored, 1);
        var opponent = new ManagerGameWeekStateBuilder()
            .WithLineup(oneGoalDefender)
            .Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(oneGoalDefender)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.Should().Be(0);
    }
}
