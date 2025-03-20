using Microsoft.AspNetCore.SignalR.Client;
using Frontend.Shared.Models;
using Microsoft.AspNetCore.Components;
using Frontend.Shared.Models.Dtos;

namespace Frontend.Client.Services;
public class GameHubClient
{
	private readonly UserServiceClient _userServiceClient;
	private readonly HubConnection _hubConnection;
	public event Func<GameSession, Task>? OnGameStateReceived;
	public event Action<string>? OnGameFound;
	public event Action<MoveResultDto>? OnMoveRecieved;
	public event Func<string, Task>? OnGameFinished;
	public GameHubClient(NavigationManager navigationManager, UserServiceClient userServiceClient)
	{
		_userServiceClient = userServiceClient;

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
	}
	public async Task Connect()
	{
		if (_hubConnection.State == HubConnectionState.Disconnected)
		{
			await _hubConnection.StartAsync();
		}
	}
	public async Task Disconnect()
	{
		if (_hubConnection.State == HubConnectionState.Connected)
		{
			await _hubConnection.StopAsync();
		}
	}
	public async Task StartGameSearch(string player)
	{
		await Connect();
		await _hubConnection.InvokeAsync<string>("StartGameSearch", player);
	}
	public async Task StopGameSearch(string player)
	{
		await _hubConnection.InvokeAsync<string>("StopGameSearch", player);
		await Disconnect();
	}
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		await _hubConnection.InvokeAsync("MakeMove", gameId, moveDto);
	}
	public async Task SendMessage(string gameId, ChatMessageDto chatMessage)
	{
		await _hubConnection.InvokeAsync("SendMessage", gameId, chatMessage);
	}
	public async Task FinishGame(string gameId, string looser)
	{
		await _hubConnection.InvokeAsync("FinishGame", gameId, looser);
	}
}
