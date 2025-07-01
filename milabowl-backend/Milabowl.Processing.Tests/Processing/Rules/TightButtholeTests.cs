using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class TightButtholeTests : MilaRuleTest<TightButthole>
{
    [Fact]
    public void Should_get_2_point_1_points_if_player_conceded_fewest_goals()
    {
        var player = TestStateFactory.GetPlayer().RuleFor(r => r.GoalsConceded, 1);
        var opponent = new ManagerGameWeekStateBuilder().WithLineup(player, player).Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(player)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(2.1m);
    }

    [Fact]
    public void Should_get_0_points_if_player_did_not_conceded_fewest_goals()
    {
        var player = TestStateFactory.GetPlayer().RuleFor(r => r.GoalsConceded, 1);
        var opponent = new ManagerGameWeekStateBuilder().WithLineup(player).Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(player)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(0);
    }

    [Fact]
    public void Benched_players_should_not_count_for_conceding_goals()
    {
        var player = TestStateFactory.GetPlayer().RuleFor(r => r.GoalsConceded, 1);
        var benchedPlayer = TestStateFactory.GetBenchPlayer().RuleFor(r => r.GoalsConceded, 1);
        var opponent = new ManagerGameWeekStateBuilder().WithLineup(player).Build();
        var state = new ManagerGameWeekStateBuilder()
            .WithLineup(benchedPlayer)
            .WithOpponents(opponent)
            .Build();

        var result = Rule.Calculate(state);

        result.Points.ShouldBe(2.1m);
    }
}
