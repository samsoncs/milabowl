using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class UniquePlayersTests : MilaRuleTest<UniquePlayers>
{
    [Fact]
    public void Should_award_3_points_if_user_has_highest_weighted_score()
    {
        var opponent = GetOpponent(9);
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(x => x.FantasyPlayerEventId, 1)
                    .RuleFor(x => x.TotalPoints, 10)
            )
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(3);
    }

    [Fact]
    public void Should_award_2_points_if_user_has_second_highest_weighted_score()
    {
        var opponent = GetOpponent(11);

        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(x => x.FantasyPlayerEventId, 1)
                    .RuleFor(x => x.TotalPoints, 10)
            )
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(2);
    }

    [Fact]
    public void Should_award_1_points_if_user_has_third_highest_weighted_score()
    {
        var opponent1 = GetOpponent(12);
        var opponent2 = GetOpponent(11);

        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(x => x.FantasyPlayerEventId, 1)
                    .RuleFor(x => x.TotalPoints, 10)
            )
            .WithOpponents(opponent1, opponent2)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(1);
    }

    [Fact]
    public void Should_award_0_points_if_user_has_fourth_highest_weighted_score()
    {
        var opponent1 = GetOpponent(13);
        var opponent2 = GetOpponent(12);
        var opponent3 = GetOpponent(11);

        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(x => x.FantasyPlayerEventId, 1)
                    .RuleFor(x => x.TotalPoints, 10)
            )
            .WithOpponents(opponent1, opponent2, opponent3)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    private ManagerGameWeekState GetOpponent(int points)
    {
        return new ManagerGameWeekStateBuilder()
            .WithLineup(
                TestStateFactory
                    .GetPlayer()
                    .RuleFor(x => x.FantasyPlayerEventId, 1)
                    .RuleFor(x => x.TotalPoints, points)
            )
            .Build();
    }
}
