using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Frontend.Shared.Models.Dtos;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Client.Services;

public class UserServiceClient(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Frontend.ServerAPI");
	private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
	public async Task<UserDto?> GetUserByIdAsync(string id)
	{
		return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/id-{id}"); ;
	}
	public async Task<UserDto?> GetUserByUserNameAsync(string userName)
	{
		return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/username-{userName}"); ;
	}
	public async Task<List<UserDto>?> GetUsersAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<UserDto>>($"api/users");
	}
	public async Task<List<NotificationDto>?> GetUserNotificationsAsync(string userId)
	{
		return await _httpClient.GetFromJsonAsync<List<NotificationDto>>($"api/users/notifications/{userId}");
	}
	public async Task<string> GetCurrentUserNameAsync()
	{
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
		return state.User.Identity?.Name ?? string.Empty;
	}
	public async Task<string> GetCurrentUserIdAsync()
	{
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();

		return state.User.FindFirst("sub")?.Value ?? string.Empty;
	}
	public async Task<HttpResponseMessage> SendFriendRequestAsync(string receiverId)
	{
		var senderId = await GetCurrentUserIdAsync();

		if (string.IsNullOrEmpty(senderId))
		{
			return new HttpResponseMessage(HttpStatusCode.Unauthorized)
			{
				ReasonPhrase = "You are not authorized"
			};
		}

		Console.WriteLine($"receiverId = {receiverId}");
		Console.WriteLine($"senderId = {senderId}");

		return await _httpClient.PostAsJsonAsync("/api/users/friendrequests", new FriendRequestDto(senderId, receiverId, DateTime.UtcNow));
	}
	public async Task ConfirmFriendRequestAsync(string senderId)
	{
		var receiverId = await GetCurrentUserIdAsync();

		if (string.IsNullOrEmpty(receiverId)) return;

		await _httpClient.PostAsJsonAsync("/api/users/friendrequests/confirm", new FriendRequestDto(receiverId, senderId, DateTime.UtcNow));
	}
	public async Task MarkNotificationAsReadAsync(string notificationId)
	{
		await _httpClient.PostAsJsonAsync($"notifications/{await GetCurrentUserIdAsync()}", notificationId);
	}
}
