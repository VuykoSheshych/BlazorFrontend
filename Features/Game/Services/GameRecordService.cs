using System.Net.Http.Json;
using ChessShared.Models;

namespace BlazorFrontend.Features.Game.Services;
public class GameRecordService(IHttpClientFactory httpClientFactory)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("GamePlayAPI");

	public async Task<List<GameRecord>?> GetGamesAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<GameRecord>>("gameplay/games");
	}
	public async Task<GameRecord?> GetGameByIdAsync(string gameId)
	{
		return await _httpClient.GetFromJsonAsync<GameRecord>($"gameplay/games/{gameId}");
	}
}
