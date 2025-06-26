using System.Text.Json;
using Milabowl.Processing.DataImport.FplDtos;

namespace Milabowl.Processing.DataImport;

public interface IFplService
{
    Task<BootstrapRootDTO> GetBootstrapRoot();
    Task<LeagueRootDTO> GetLeagueRoot();
    Task<EventRootDTO> GetEventRoot(int eventID);
    Task<HeadToHeadEventRootDTO> GetHead2HeadEventRoot(int eventID);
    Task<PicksRootDTO> GetPicksRoot(int eventID, int userID);
    Task<ElementHistoryRootDto> GetPlayerHistoryRoot(int playerId);
    Task<IList<FixtureDTO>> GetFixtures();
    Task<EntryRootDTO> GetEntryRoot(int userID);
}

public class FplService : IFplService
{
    private readonly HttpClient _httpClient;

    public FplService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<BootstrapRootDTO> GetBootstrapRoot()
    {
        return await _httpClient.GetDeserializedAsync<BootstrapRootDTO>(
            "https://fantasy.premierleague.com/api/bootstrap-static/"
        );
    }

    public async Task<LeagueRootDTO> GetLeagueRoot()
    {
        return await _httpClient.GetDeserializedAsync<LeagueRootDTO>(
            "https://fantasy.premierleague.com/api/leagues-classic/1650213/standings/?page_new_entries=1&page_standings=1&phase=1"
        );
    }

    private Dictionary<int, EventRootDTO> _eventRootCache = new();

    public async Task<EventRootDTO> GetEventRoot(int eventID)
    {
        if (_eventRootCache.TryGetValue(eventID, out var eventRootDto))
        {
            return eventRootDto;
        }

        eventRootDto = await _httpClient.GetDeserializedAsync<EventRootDTO>(
            $"https://fantasy.premierleague.com/api/event/{eventID}/live/"
        );
        _eventRootCache.Add(eventID, eventRootDto);
        return eventRootDto;
    }

    private Dictionary<string, PicksRootDTO> _picsRootCache = new();

    public async Task<PicksRootDTO> GetPicksRoot(int eventID, int userID)
    {
        if (_picsRootCache.TryGetValue($"{eventID},{userID}", out var picksRootDto))
        {
            return picksRootDto;
        }

        picksRootDto = await _httpClient.GetDeserializedAsync<PicksRootDTO>(
            $@"https://fantasy.premierleague.com/api/entry/{userID}/event/{eventID}/picks/"
        );
        _picsRootCache.Add($"{eventID},{userID}", picksRootDto);
        return picksRootDto;
    }

    private Dictionary<int, HeadToHeadEventRootDTO> _headToHeadEventCache = new();

    public async Task<HeadToHeadEventRootDTO> GetHead2HeadEventRoot(int eventID)
    {
        if (_headToHeadEventCache.TryGetValue(eventID, out var headToHeadEventRoot))
        {
            return headToHeadEventRoot;
        }

        headToHeadEventRoot = await _httpClient.GetDeserializedAsync<HeadToHeadEventRootDTO>(
            $"https://fantasy.premierleague.com/api/leagues-h2h-matches/league/1649633/?page=1&event={eventID}"
        );
        _headToHeadEventCache.Add(eventID, headToHeadEventRoot);
        return headToHeadEventRoot;
    }

    public async Task<ElementHistoryRootDto> GetPlayerHistoryRoot(int playerId)
    {
        var root = await _httpClient.GetDeserializedAsync<ElementHistoryRootDto>(
            $"https://fantasy.premierleague.com/api/element-summary/{playerId}/"
        );
        root.FantasyElementId = playerId;
        return root;
    }

    public async Task<IList<FixtureDTO>> GetFixtures()
    {
        return await _httpClient.GetDeserializedAsync<IList<FixtureDTO>>(
            $"https://fantasy.premierleague.com/api/fixtures/"
        );
    }

    private Dictionary<int, EntryRootDTO> _entryRootDtoCache = new();

    public async Task<EntryRootDTO> GetEntryRoot(int userId)
    {
        if (_entryRootDtoCache.TryGetValue(userId, out var entryRootDto))
        {
            return entryRootDto;
        }

        entryRootDto = await _httpClient.GetDeserializedAsync<EntryRootDTO>(
            $"https://fantasy.premierleague.com/api/entry/{userId}/history/"
        );
        _entryRootDtoCache.Add(userId, entryRootDto);
        return entryRootDto;
    }
}

public static class HttpClientExtensions
{
    public static async Task<T> GetDeserializedAsync<T>(this HttpClient httpClient, string url)
    {
        var response = await httpClient.GetAsync(url);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(
            responseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
        )!;
    }
}
