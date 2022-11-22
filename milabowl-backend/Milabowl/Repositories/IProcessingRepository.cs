using Milabowl.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Milabowl.Repositories;

public interface IProcessingRepository
{
    Task<IList<Event>> GetEventsToProcess();
    Task<IList<User>> GetUserToProcess(Guid evtId);
    Task<bool> IsEventAlreadyCalculated(string eventName, string userEntryName);
    Task<(Player? mostTradedIn, Player? mostTradedOut)> GetMostTradedPlayers(Guid eventId);
    Task<UserHeadToHeadEvent?> GetOpponentHeadToHead(int userHeadToHeadEventId, Guid userId);
    Task AddMilaGwScores(IList<MilaGWScore> milaGwScores);
}