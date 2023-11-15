using Milabowl.Domain.Entities.Fantasy;
using Milabowl.Domain.Entities.Milabowl;

namespace Milabowl.Domain.Processing;

public interface IProcessingRepository
{
    Task<IList<Event>> GetEventsToProcess();
    Task<int> GetNumGameWeeks();

    Task<IList<User>> GetUserToProcess(Guid evtId);
    Task<bool> IsEventAlreadyCalculated(string eventName, string userEntryName);
    Task<(Player? mostTradedIn, Player? mostTradedOut)> GetMostTradedPlayers(Guid eventId);
    Task<UserHeadToHeadEvent?> GetOpponentHeadToHead(int userHeadToHeadEventId, Guid userId);
    Task AddMilaGwScores(IList<MilaGWScore> milaGwScores);
    Task<string> GetUsernameDirectlyInFront(Random random, int gameWeek, string userName);
    Task<IList<Player>> GetPlayersForGw(IList<Player> players);
}