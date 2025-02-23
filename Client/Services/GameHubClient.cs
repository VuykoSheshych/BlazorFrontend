using Microsoft.AspNetCore.SignalR.Client;
using Frontend.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Frontend.Client.Services;
public class GameHubClient
{
	private readonly HubConnection _hubConnection;

	public event Action<GameSession>? OnGameStateReceived;
	public event Action<MoveDto>? OnMoveReceived;

	public GameHubClient(NavigationManager navigationManager)
	{
		_hubConnection = new HubConnectionBuilder()
			.WithUrl(navigationManager.ToAbsoluteUri("https://localhost:7251/gameHub"))
			.WithAutomaticReconnect()
			.Build();

		_hubConnection.On<GameSession>("ReceiveGameState", gameSession => OnGameStateReceived?.Invoke(gameSession));
		_hubConnection.On<MoveDto>("ReceiveMove", move => OnMoveReceived?.Invoke(move));
	}

	public async Task ConnectAsync()
	{
		if (_hubConnection.State == HubConnectionState.Disconnected)
		{
			await _hubConnection.StartAsync();
		}
	}

	public async Task<string> CreateGameAsync(string player1, string player2)
	{
		if (_hubConnection is not null)
		{
			return await _hubConnection.InvokeAsync<string>("CreateGame", player1, player2);
		}

		throw new InvalidOperationException("SignalR is not connected!");
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
