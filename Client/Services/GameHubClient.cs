using Microsoft.AspNetCore.SignalR.Client;
using Frontend.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Client.Services;
public class GameHubClient
{
	private readonly AuthenticationStateProvider _authenticationStateProvider;
	private readonly HubConnection _hubConnection;
	public event Action<GameSession>? OnGameStateReceived;
	public event Action<MoveDto>? OnMoveReceived;
	public event Action<string>? OnGameFound;
	public GameHubClient(NavigationManager navigationManager, AuthenticationStateProvider authenticationStateProvider)
	{
		_authenticationStateProvider = authenticationStateProvider;

		_hubConnection = new HubConnectionBuilder()
			.WithUrl(navigationManager.ToAbsoluteUri("https://localhost:7251/gameHub"))
			.WithAutomaticReconnect()
			.Build();

		_hubConnection.On<GameSession>("ReceiveGameState", gameSession => OnGameStateReceived?.Invoke(gameSession));
		_hubConnection.On<MoveDto>("ReceiveMove", move => OnMoveReceived?.Invoke(move));
		_hubConnection.On<string>("GameFound", gameId => OnGameFound?.Invoke(gameId));

		_hubConnection.Closed += async (exception) =>
		{
			var user = await GetCurrentUserAsync();
			await TerminateGameSearch(user);
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
	private async Task<string> GetCurrentUserAsync()
	{
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
		return state.User.Identity?.Name ?? string.Empty;
	}
	public async Task<string> TerminateGameSearch(string player)
	{
		return await _hubConnection.InvokeAsync<string>("TerminateGameSearch", player);
	}
	public async Task JoinGame(string gameId)
	{
		await _hubConnection.InvokeAsync("JoinGame", gameId);
	}
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		await _hubConnection.InvokeAsync("MakeMove", gameId, moveDto);
	}
	public async Task LeaveGame(string gameId)
	{
		await _hubConnection.InvokeAsync("LeaveGame", gameId);
	}
	public async Task FinishGame(string gameId, string result)
	{
		await _hubConnection.InvokeAsync("FinishGame", gameId, result);
	}
	public async Task DisconnectAsync()
	{
		await _hubConnection.StopAsync();
	}
}
