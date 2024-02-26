using Milabowl.Processing.DataImport;

namespace Milabowl.Processing.Processing;

public class Processor
{
    private readonly IRulesProcessor _rulesProcessor;
    
    public Processor(IRulesProcessor rulesProcessor)
    {
        _rulesProcessor = rulesProcessor;
    }

    public void ProcessMilaPoints()
    {
        Console.WriteLine("Starting to process mila points");

        var gameWeeks = new List<int>{1};
        var userGameWeeks = new List<UserGameWeek>
        {
            new UserGameWeek(new List<PlayerEvent>
            {
                new PlayerEvent(
                    1,
                    2,
                    4,
                    6,
                    3,
                    6,
                    3,
                    5,
                    1,
                    1,
                    0,
                    0,
                    1,
                    1,
                    "",
                    "",
                    "",
                    "",
                    1,
                    true,
                    2,
                    true,
                    1,
                    2,
                    1
                    )
            })
        };

        foreach (var gameWeek in gameWeeks)
        {
            foreach (var userGameWeek in userGameWeeks)
            {
                _rulesProcessor.CalculateForUserGameWeek(userGameWeek);
            }
        }
        
        Console.WriteLine("Mila points processing complete");

    }
}