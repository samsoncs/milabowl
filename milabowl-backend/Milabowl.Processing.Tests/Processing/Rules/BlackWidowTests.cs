using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class BlackWidowTests: MilaRuleTest<BlackWidow>
{
    [Fact]
    public void Should_get_zero_points_when_no_hits()
    {
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithTransferCost(0)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_get_4_point_2_points_when_hit_with_even_number_of_opponents_with_hit()
    {
        var opponent1 = new ManagerGameWeekStateBuilder()
            .WithTransferCost(4)
            .Build();
        var opponent2 = new ManagerGameWeekStateBuilder()
            .WithTransferCost(8)
            .Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithTransferCost(4)
            .WithOpponents(opponent1, opponent2)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(4.2m);
    }

    [Fact]
    public void Should_get_8_point_4_points_when_two_hits_with_even_number_of_opponents_with_hit()
    {
        var opponent1 = new ManagerGameWeekStateBuilder()
            .WithTransferCost(4)
            .Build();
        var opponent2 = new ManagerGameWeekStateBuilder()
            .WithTransferCost(8)
            .Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithTransferCost(8)
            .WithOpponents(opponent1, opponent2)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(8.4m);
    }

    [Fact]
        public void Should_get_8_point_4_points_when_three_or_more_hits_with_even_number_of_opponents_with_hit()
        {
            var opponent1 = new ManagerGameWeekStateBuilder()
                .WithTransferCost(4)
                .Build();
            var opponent2 = new ManagerGameWeekStateBuilder()
                .WithTransferCost(8)
                .Build();
            var userGameWeek = new ManagerGameWeekStateBuilder()
                .WithTransferCost(12)
                .WithOpponents(opponent1, opponent2)
                .Build();

            var points = Rule.Calculate(userGameWeek);

            points.Points.ShouldBe(8.4m);
        }

    [Fact]
    public void Should_get_negative_4_point_2_points_when_hit_with_odd_number_of_opponents_with_hit()
    {
        var opponent1 = new ManagerGameWeekStateBuilder()
            .WithTransferCost(4)
            .Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithTransferCost(4)
            .WithOpponents(opponent1)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(-4.2m);
    }
}
