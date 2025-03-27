using System.Net.Http.Json;
using Frontend.Shared.Models;

namespace Frontend.Client.Services;
public class GameRecordService(IHttpClientFactory httpClientFactory)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("GamePlayServiceAPI");

	public async Task<List<GameRecord>?> GetGamesAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<GameRecord>>("gameplay/games");
	}
	public async Task<GameRecord?> GetGameByIdAsync(string gameId)
	{
		return await _httpClient.GetFromJsonAsync<GameRecord>($"gameplay/games/{gameId}");
	}
}
