using Microsoft.AspNetCore.SignalR.Client;
using ChessShared.Models;
using ChessShared.Dtos;
using System.Text.Json;

namespace BlazorFrontend.Features.Game.Services;

public class GameHubClient
{
	private readonly HubConnection _hubConnection;
	public event Func<GameSession, Task>? OnGameStateReceived;
	public event Func<string, Task>? OnGameFound;
	public event Action<MoveResultDto>? OnMoveRecieved;
	public event Func<string, Task>? OnGameFinished;
	public event Action<ChatMessageDto>? OnMessageReceived;
	public GameHubClient(IHttpClientFactory httpClientFactory)
	{
		_hubConnection = new HubConnectionBuilder()
			.WithUrl(httpClientFactory.CreateClient("GamePlayAPI").BaseAddress + "gameplay/gameHub")
			.WithAutomaticReconnect()
			.Build();

		_hubConnection.On<MoveResultDto>("ReceiveMove", moveResult => OnMoveRecieved?.Invoke(moveResult));
		_hubConnection.On<ChatMessageDto>("ReceiveMessage", chatMessage => OnMessageReceived?.Invoke(chatMessage));
		_hubConnection.On<string>("GameFound", async gameId =>
		{
			if (OnGameFound != null)
				await OnGameFound.Invoke(gameId);
		});
		_hubConnection.On<string>("ReceiveGameState", async json =>
		{
			var gameSession = JsonSerializer.Deserialize<GameSession>(json);

			if (gameSession != null && OnGameStateReceived != null)
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
	public async Task FinishGame(Guid gameId, string looser)
	{
		await _hubConnection.InvokeAsync("FinishGame", gameId, looser);
	}
}
