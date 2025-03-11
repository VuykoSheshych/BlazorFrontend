using Microsoft.AspNetCore.SignalR.Client;
using Frontend.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Client.Services;
public class GameHubClient
{
	private readonly AuthenticationStateProvider _authenticationStateProvider;
	private readonly HubConnection _hubConnection;
	public event Func<GameSession, Task>? OnGameStateReceived;
	public event Action<string>? OnGameFound;
	public event Action<MoveResultDto>? OnMoveRecieved;
	public event Func<string, Task>? OnGameFinished;
	public GameHubClient(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
	{
		_authenticationStateProvider = authenticationStateProvider;

		_hubConnection = new HubConnectionBuilder()
			.WithUrl(navigationManager.ToAbsoluteUri("https://localhost:7251/gameHub"))
			.WithAutomaticReconnect()
			.Build();

		_hubConnection.On<MoveResultDto>("ReceiveMove", moveResult => OnMoveRecieved?.Invoke(moveResult));
		_hubConnection.On<string>("GameFound", gameId => OnGameFound?.Invoke(gameId));
		_hubConnection.On<GameSession>("ReceiveGameState", async gameSession =>
		{
			if (OnGameStateReceived != null)
				await OnGameStateReceived.Invoke(gameSession);
		});
		_hubConnection.On<string>("GameFinished", async looser =>
		{
			if (OnGameFinished != null)
				await OnGameFinished.Invoke(looser);
		});
		_hubConnection.Closed += async (exception) =>
		{
			var user = await GetCurrentUserAsync();
			await StopGameSearch(user);
		};
	}
	public async Task Connect()
	{
		if (_hubConnection.State == HubConnectionState.Disconnected)
		{
			await _hubConnection.StartAsync();
		}
	}
	public async Task<string> StartSearch(string player)
	{
		return await _hubConnection.InvokeAsync<string>("StartGameSearch", player);
	}
	public async Task<string> StopGameSearch(string player)
	{
		return await _hubConnection.InvokeAsync<string>("StopGameSearch", player);
	}
	public async Task<string> GetCurrentUserAsync()
	{
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
		return state.User.Identity?.Name ?? string.Empty;
	}
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		await _hubConnection.InvokeAsync("MakeMove", gameId, moveDto);
	}
	public async Task FinishGame(string gameId, string looser)
	{
		await _hubConnection.InvokeAsync("FinishGame", gameId, looser);
	}
}
