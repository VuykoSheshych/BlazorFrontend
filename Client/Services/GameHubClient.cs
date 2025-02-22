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
			.WithUrl(navigationManager.ToAbsoluteUri("https://gameHub"))
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

	public async Task DisconnectAsync()
	{
		await _hubConnection.StopAsync();
	}
}
