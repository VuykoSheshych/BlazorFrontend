using System.Net.Http.Json;
using Frontend.Shared.Models;

namespace Frontend.Client.Services;
public class GameRecordService(IHttpClientFactory httpClientFactory)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("GamePlayServiceAPI");

	public async Task<List<GameRecord>?> GetGamesAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<GameRecord>>("api/games");
	}
	public async Task<GameRecord?> GetGameByIdAsync(Guid gameId)
	{
		return await _httpClient.GetFromJsonAsync<GameRecord>($"api/games/{gameId}");
	}
}
