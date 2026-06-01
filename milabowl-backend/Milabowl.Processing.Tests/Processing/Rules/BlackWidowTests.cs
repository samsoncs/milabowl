using Milabowl.Processing.DataImport.Models;
using Milabowl.Processing.Processing.Rules;
using Milabowl.Processing.Tests.Utils;
using Shouldly;

namespace Milabowl.Processing.Tests.Processing.Rules;

public class BlackWidowTests : MilaRuleTest<BlackWidow>
{
    private static bool EvenWins(int gameWeek) => new Random(gameWeek).Next(2) == 0;
    private static int GwWhereEvenWins() => Enumerable.Range(1, 38).First(EvenWins);
    private static int GwWhereOddWins() => Enumerable.Range(1, 38).First(g => !EvenWins(g));

    [Fact]
    public void Should_get_zero_points_when_no_hits()
    {
        var userGameWeek = new ManagerGameWeekStateBuilder().WithTransferCost(0).Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(0);
    }

    [Fact]
    public void Should_get_positive_points_when_hit_and_even_opponents_win_and_even_opponents_with_hit()
    {
        var gw = GwWhereEvenWins();
        var opponent1 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var opponent2 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithEvent(new Event(gw, ""))
            .WithTransferCost(4)
            .WithOpponents(opponent1, opponent2)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(0.42m);
    }

    [Fact]
    public void Should_get_negative_points_when_hit_and_even_opponents_win_and_odd_opponents_with_hit()
    {
        var gw = GwWhereEvenWins();
        var opponent1 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithEvent(new Event(gw, ""))
            .WithTransferCost(4)
            .WithOpponents(opponent1)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(-0.42m);
    }

    [Fact]
    public void Should_get_positive_points_when_hit_and_odd_opponents_win_and_odd_opponents_with_hit()
    {
        var gw = GwWhereOddWins();
        var opponent1 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithEvent(new Event(gw, ""))
            .WithTransferCost(4)
            .WithOpponents(opponent1)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(0.42m);
    }

    [Fact]
    public void Should_get_negative_points_when_hit_and_odd_opponents_win_and_even_opponents_with_hit()
    {
        var gw = GwWhereOddWins();
        var opponent1 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var opponent2 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithEvent(new Event(gw, ""))
            .WithTransferCost(4)
            .WithOpponents(opponent1, opponent2)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(-0.42m);
    }

    [Fact]
    public void Should_scale_points_by_number_of_hits()
    {
        var gw = GwWhereEvenWins();
        var opponent1 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var opponent2 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithEvent(new Event(gw, ""))
            .WithTransferCost(8)
            .WithOpponents(opponent1, opponent2)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(0.84m);
    }

    [Fact]
    public void Should_cap_points_at_4_point_2()
    {
        var gw = GwWhereEvenWins();
        var opponent1 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var opponent2 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithEvent(new Event(gw, ""))
            .WithTransferCost(100)
            .WithOpponents(opponent1, opponent2)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(4.2m);
    }

    [Fact]
    public void Should_cap_points_at_negative_4_point_2()
    {
        var gw = GwWhereEvenWins();
        var opponent1 = new ManagerGameWeekStateBuilder().WithTransferCost(4).Build();
        var userGameWeek = new ManagerGameWeekStateBuilder()
            .WithEvent(new Event(gw, ""))
            .WithTransferCost(100)
            .WithOpponents(opponent1)
            .Build();

        var points = Rule.Calculate(userGameWeek);

        points.Points.ShouldBe(-4.2m);
    }
}
