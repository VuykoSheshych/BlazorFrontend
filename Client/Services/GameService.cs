using System.Net.Http.Json;
using Frontend.Shared;

namespace Frontend.Client.Services;
public class GameService(HttpClient httpClient)
{
	private readonly HttpClient _httpClient = httpClient;

	public async Task<List<GameRecord>> GetGamesAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<GameRecord>>("api/games") ?? new List<GameRecord>();
	}
}
