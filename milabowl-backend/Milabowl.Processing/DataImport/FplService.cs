using System.Text.Json;
using Milabowl.Processing.DataImport.FplDtos;

namespace Milabowl.Processing.DataImport;

public interface IFplService
{
    Task<BootstrapRootDto> GetBootstrapRoot();
    Task<LeagueRootDto> GetLeagueRoot();
    Task<EventRootDto> GetEventRoot(int eventId);
    Task<HeadToHeadEventRootDto> GetHead2HeadEventRoot(int eventId);
    Task<PicksRootDto> GetPicksRoot(int eventId, int userId);
    Task<ElementHistoryRootDto> GetPlayerHistoryRoot(int playerId);
    Task<IList<FixtureDto>> GetFixtures();
    Task<EntryRootDto> GetEntryRoot(int userId);
}

public class FplService : IFplService
{
    private readonly HttpClient _httpClient;

    public FplService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BootstrapRootDto> GetBootstrapRoot()
    {
        return await _httpClient.GetDeserializedAsync<BootstrapRootDto>(
            "https://fantasy.premierleague.com/api/bootstrap-static/"
        );
    }

    public async Task<LeagueRootDto> GetLeagueRoot()
    {
        return await _httpClient.GetDeserializedAsync<LeagueRootDto>(
            "https://fantasy.premierleague.com/api/leagues-classic/1650213/standings/?page_new_entries=1&page_standings=1&phase=1"
        );
    }

    private Dictionary<int, EventRootDto> _eventRootCache = new();

    public async Task<EventRootDto> GetEventRoot(int eventId)
    {
        if (_eventRootCache.TryGetValue(eventId, out var eventRootDto))
        {
            return eventRootDto;
        }

        eventRootDto = await _httpClient.GetDeserializedAsync<EventRootDto>(
            $"https://fantasy.premierleague.com/api/event/{eventId}/live/"
        );
        _eventRootCache.Add(eventId, eventRootDto);
        return eventRootDto;
    }

    private Dictionary<string, PicksRootDto> _picsRootCache = new();

    public async Task<PicksRootDto> GetPicksRoot(int eventId, int userId)
    {
        if (_picsRootCache.TryGetValue($"{eventId},{userId}", out var picksRootDto))
        {
            return picksRootDto;
        }

        picksRootDto = await _httpClient.GetDeserializedAsync<PicksRootDto>(
            $@"https://fantasy.premierleague.com/api/entry/{userId}/event/{eventId}/picks/"
        );
        _picsRootCache.Add($"{eventId},{userId}", picksRootDto);
        return picksRootDto;
    }

    private Dictionary<int, HeadToHeadEventRootDto> _headToHeadEventCache = new();

    public async Task<HeadToHeadEventRootDto> GetHead2HeadEventRoot(int eventId)
    {
        if (_headToHeadEventCache.TryGetValue(eventId, out var headToHeadEventRoot))
        {
            return headToHeadEventRoot;
        }

        headToHeadEventRoot = await _httpClient.GetDeserializedAsync<HeadToHeadEventRootDto>(
            $"https://fantasy.premierleague.com/api/leagues-h2h-matches/league/1649633/?page=1&event={eventId}"
        );
        _headToHeadEventCache.Add(eventId, headToHeadEventRoot);
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

    public async Task<IList<FixtureDto>> GetFixtures()
    {
        return await _httpClient.GetDeserializedAsync<IList<FixtureDto>>(
            $"https://fantasy.premierleague.com/api/fixtures/"
        );
    }

    private Dictionary<int, EntryRootDto> _entryRootDtoCache = new();

    public async Task<EntryRootDto> GetEntryRoot(int userId)
    {
        if (_entryRootDtoCache.TryGetValue(userId, out var entryRootDto))
        {
            return entryRootDto;
        }

        entryRootDto = await _httpClient.GetDeserializedAsync<EntryRootDto>(
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
