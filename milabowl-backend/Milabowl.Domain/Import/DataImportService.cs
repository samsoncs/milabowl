namespace Milabowl.Domain.Import;

public interface IDataImportService
{
    public Task ImportData();
}

public class DataImportService: IDataImportService
{
    private readonly IDataImportBusiness _dataImportBusiness;
    private readonly IDataImportProvider _dataImportProvider;
    private readonly IImportRepository _repository;

    public DataImportService(IDataImportBusiness dataImportBusiness, IDataImportProvider dataImportProvider, IImportRepository repository)
    {
        this._dataImportBusiness = dataImportBusiness;
        this._dataImportProvider = dataImportProvider;
        _repository = repository;
    }

    public async Task ImportData()
    {
        var eventsFromDb = await _repository.GetEvents();
        var teamsFromDb = await _repository.GetTeams();
        var fixturesFromDb = await _repository.GetFixtures();
        var playersFromDb = await _repository.GetPlayers();
        var leaguesFromDb = await _repository.GetLeagues();
        var usersFromDb = await _repository.GetUsers();
        var lineupsFromDb = await _repository.GetLineups();
        var userLeaguesFromDb = await _repository.GetUserLeagues();
        var playerEventsFromDb = await _repository.GetPlayerEvents();
        var userHeadToHeadEventsFromDb = await _repository.GetUserHeadToHeadEvents();
        var playerEventLineupsFromDb = await _repository.GetPlayerEventLineups();

        var bootstrapRoot = await this._dataImportProvider.GetBootstrapRoot();
        var events = await this._dataImportBusiness.ImportEvents(bootstrapRoot, eventsFromDb);
        var teams = await this._dataImportBusiness.ImportTeams(bootstrapRoot, teamsFromDb);
        var players = await this._dataImportBusiness.ImportPlayers(bootstrapRoot, teams, playersFromDb);
        var fixtureDtos = (await this._dataImportProvider.GetFixtures()).Where(e => e.@event != null).ToList();
        await this._dataImportBusiness.ImportFixtures(fixtureDtos, fixturesFromDb, events, teams);

        var playerHistoryRoots = players.Select(async p => await this._dataImportProvider.GetPlayerHistoryRoot(p.FantasyPlayerId))
            .Select(t => t.Result)
            .Where(t => t != null)
            .ToList();

        var leagueRoot = await this._dataImportProvider.GetLeagueRoot();
        var league = await this._dataImportBusiness.ImportLeague(leagueRoot, leaguesFromDb);
        var users = await this._dataImportBusiness.ImportUsers(leagueRoot, usersFromDb);
        await this._dataImportBusiness.ImportUserLeagues(users, league, userLeaguesFromDb);
        await _repository.SaveChangesAsync();

        foreach (var finishedEvent in events.Where(e => e.Finished && e.DataChecked))
        {
            if (playerEventsFromDb.Any(pe => pe.Event.GameWeek == finishedEvent.GameWeek))
            {
                continue;
            }

            var eventRootDto = await this._dataImportProvider.GetEventRoot(finishedEvent.FantasyEventId);
            var playerEvents = await this._dataImportBusiness.ImportPlayerEvents(eventRootDto, finishedEvent, players, playerEventsFromDb, playerHistoryRoots, fixtureDtos);

            var headToHeadEventRootDto = await this._dataImportProvider.GetHead2HeadEventRoot(finishedEvent.FantasyEventId);
            await this._dataImportBusiness.ImportUserHeadToHeadEvents(headToHeadEventRootDto, finishedEvent, users, userHeadToHeadEventsFromDb);

            foreach (var user in users)
            {
                var picksRoot = await this._dataImportProvider.GetPicksRoot(finishedEvent.FantasyEventId, user.FantasyEntryId);
                if (picksRoot.picks == null)
                {
                    continue;
                }

                var lineup = await this._dataImportBusiness.ImportLineup(finishedEvent, user, lineupsFromDb);
                await this._dataImportBusiness.ImportPlayerEventLineup(picksRoot, finishedEvent, lineup, playerEvents, playerEventLineupsFromDb);
            }

            await _repository.SaveChangesAsync();
        }

        await _repository.SaveChangesAsync();
    }
}