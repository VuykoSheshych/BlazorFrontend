using System.Net.Http.Json;
using BlazorFrontend.Features.Auth.Services;
using ChessShared.Dtos;

namespace BlazorFrontend.Shared.Services;

public class UserSharedService(IHttpClientFactory httpClientFactory, CustomStateProvider customStateProvider)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("UsersAndAuthAPI");
	private readonly CustomStateProvider _customStateProvider = customStateProvider;

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
		var state = await _customStateProvider.GetAuthenticationStateAsync();
		return state.User.Identity?.Name ?? string.Empty;
	}

	public async Task<string> GetCurrentUserIdAsync()
	{
		var state = await _customStateProvider.GetAuthenticationStateAsync();
		return state.User.FindFirst("sub")?.Value ?? string.Empty;
	}
}

