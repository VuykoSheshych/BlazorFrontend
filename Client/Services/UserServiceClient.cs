using System.Net.Http.Json;
using Frontend.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Frontend.Client.Services;

public class UserServiceClient(IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Frontend.ServerAPI");
	private readonly AuthenticationStateProvider _authenticationStateProvider = authenticationStateProvider;
	public async Task<UserDto?> GetUserByUserNameAsync(string username)
	{
		return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{username}"); ;
	}
	public async Task<string> GetCurrentUserAsync()
	{
		var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
		return state.User.Identity?.Name ?? string.Empty;
	}
}
