using Milabowl.Domain.Import.FantasyDTOs;

namespace Milabowl.Domain.Import;

public interface IDataImportProvider
{
    Task<BootstrapRootDTO> GetBootstrapRoot();
    Task<LeagueRootDTO> GetLeagueRoot();
    Task<EventRootDTO> GetEventRoot(int eventID);
    Task<HeadToHeadEventRootDTO> GetHead2HeadEventRoot(int eventID);
    Task<PicksRootDTO> GetPicksRoot(int eventID, int userID);
    Task<ElementHistoryRootDTO> GetPlayerHistoryRoot(int playerId);
    Task<IList<FixtureDTO>> GetFixtures();
}

public class DataImportProvider: IDataImportProvider
{
    private readonly HttpClient _httpClient;

    public DataImportProvider(IHttpClientFactory httpClientFactory)
    {
        this._httpClient = httpClientFactory.CreateClient();
    }

    public async Task<BootstrapRootDTO> GetBootstrapRoot()
    {
        return await this._httpClient.GetDeserializedAsync<BootstrapRootDTO>("https://fantasy.premierleague.com/api/bootstrap-static/");
    }

    public async Task<LeagueRootDTO> GetLeagueRoot()
    {
        return await this._httpClient.GetDeserializedAsync<LeagueRootDTO>("https://fantasy.premierleague.com/api/leagues-classic/883637/standings/?page_new_entries=1&page_standings=1&phase=1");
    }

    public async Task<EventRootDTO> GetEventRoot(int eventID)
    {
        return await this._httpClient.GetDeserializedAsync<EventRootDTO>($"https://fantasy.premierleague.com/api/event/{eventID}/live/");
    }

    public async Task<PicksRootDTO> GetPicksRoot(int eventID, int userID)
    {
        return await this._httpClient.GetDeserializedAsync<PicksRootDTO>($@"https://fantasy.premierleague.com/api/entry/{userID}/event/{eventID}/picks/");
    }

    public async Task<HeadToHeadEventRootDTO> GetHead2HeadEventRoot(int eventID)
    {
        return await this._httpClient.GetDeserializedAsync<HeadToHeadEventRootDTO>($"https://fantasy.premierleague.com/api/leagues-h2h-matches/league/528112/?page=1&event={eventID}");
    }

    public async Task<ElementHistoryRootDTO> GetPlayerHistoryRoot(int playerId)
    {
        var root = await this._httpClient.GetDeserializedAsync<ElementHistoryRootDTO>($"https://fantasy.premierleague.com/api/element-summary/{playerId}/");
        root.FantasyElementId = playerId;
        return root;
    }

    public async Task<IList<FixtureDTO>> GetFixtures()
    {
        return await this._httpClient.GetDeserializedAsync<IList<FixtureDTO>>($"https://fantasy.premierleague.com/api/fixtures/");
    }
}