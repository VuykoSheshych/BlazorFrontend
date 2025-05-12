using System.Security.Claims;
using ChessShared.Dtos;
using ChessShared.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorFrontend.Features.Auth.Services;

public class CustomStateProvider(AuthService authService) : AuthenticationStateProvider
{
	public CurrentUser? _cachedUser;

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		var identity = new ClaimsIdentity();

		try
		{
			var userInfo = await GetCachedUserAsync();
			if (userInfo is { IsAuthenticated: true })
			{
				var claims = new[] { new Claim(ClaimTypes.Name, userInfo.UserName) }
					.Concat(userInfo.Claims.Select(c => new Claim(c.Key, c.Value)));

				identity = new ClaimsIdentity(claims, "Server authentication");
			}
		}
		catch (HttpRequestException ex)
		{
			Console.WriteLine("GetAuthenticationStateAsync failed: " + ex.Message);
		}

		return new AuthenticationState(new ClaimsPrincipal(identity));
	}

	public async Task<CurrentUser?> GetCachedUserAsync()
	{
		if (_cachedUser is { IsAuthenticated: true })
			return _cachedUser;

		_cachedUser = await authService.GetCurrentUserInfo();
		return _cachedUser;
	}

	public async Task Login(LoginDto loginRequest)
	{
		await authService.LoginAsync(loginRequest);
		_cachedUser = await authService.GetCurrentUserInfo();
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task Register(RegisterDto registerRequest)
	{
		await authService.RegisterAsync(registerRequest);
		_cachedUser = await authService.GetCurrentUserInfo();
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}

	public async Task Logout()
	{
		await authService.LogoutAsync();
		_cachedUser = null;
		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}