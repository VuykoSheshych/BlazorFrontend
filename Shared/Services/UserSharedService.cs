using System.Net.Http.Json;
using ChessShared.Dtos;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorFrontend.Shared.Services;

public class UserSharedService(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("UsersAndAuthAPI");
	private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;

	public async Task<UserDto?> GetUserByIdAsync(string id)
	{
		return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/id-{id}");
	}

	public async Task<UserDto?> GetUserByUserNameAsync(string userName)
	{
		return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/username-{userName}");
	}

	public async Task<List<UserDto>?> GetUsersAsync()
	{
		return await _httpClient.GetFromJsonAsync<List<UserDto>>("api/users");
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
}

