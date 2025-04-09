using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChessShared.Dtos;
using ChessShared.Models;

namespace BlazorFrontend.Features.Auth.Services;

public class AuthService
{
	private readonly HttpClient _httpClient;

	public AuthService(IHttpClientFactory httpClientFactory)
	{
		_httpClient = httpClientFactory.CreateClient("UsersAndAuthAPI");
		_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		_httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
	}

	public async Task RegisterAsync(RegisterDto model)
	{
		var result = await _httpClient.PostAsJsonAsync("api/auth/register", model);

		if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
			throw new Exception(await result.Content.ReadAsStringAsync());

		result.EnsureSuccessStatusCode();
	}

	public async Task LoginAsync(LoginDto model)
	{
		var result = await _httpClient.PostAsJsonAsync("api/auth/login", model);

		if (result.StatusCode == System.Net.HttpStatusCode.BadRequest)
			throw new Exception(await result.Content.ReadAsStringAsync());

		result.EnsureSuccessStatusCode();
	}

	public async Task LogoutAsync()
	{
		var result = await _httpClient.PostAsync("api/auth/logout", null);
		result.EnsureSuccessStatusCode();
	}

	public async Task<CurrentUser?> GetCurrentUserInfo()
	{
		var result = await _httpClient.GetFromJsonAsync<CurrentUser>("api/auth/currentuserinfo");
		return result;
	}
}