using System.Net.Http.Json;
using Frontend.Shared.Models;

namespace Frontend.Client.Services;

public class UserServiceClient(IHttpClientFactory httpClientFactory)
{
	private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Frontend.ServerAPI");

	public async Task<UserDto?> GetUserByUserNameAsync(string username)
	{
		try
		{
			return await _httpClient.GetFromJsonAsync<UserDto>($"api/users/{username}");
		}
		catch
		{
			return null;
		}
	}
}
