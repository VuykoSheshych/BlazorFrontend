using Frontend.Server.Services;
using Frontend.Shared.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Server.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IUserService userService) : ControllerBase
{
	private readonly IUserService _userService = userService;

	[HttpGet]
	public async Task<IActionResult> GetUsersAsync()
	{
		var users = await _userService.GetUsersAsync();

		List<UserDto> userDtos = [];
		foreach (var user in users)
		{
			userDtos.Add(UserService.CreateUserDto(user));
		}

		return Ok(userDtos);
	}

	[HttpGet("id-{id}")]
	public async Task<IActionResult> GetUserById(string id)
	{
		var userDto = await _userService.GetUserByIdAsync(id);

		if (userDto is null) return BadRequest("User not found");

		return Ok(UserService.CreateUserDto(userDto));
	}

	[HttpGet("username-{userName}")]
	public async Task<IActionResult> GetUserByUsername(string userName)
	{
		var userDto = await _userService.GetUserByUserNameAsync(userName);

		if (userDto is null) return BadRequest("User not found");

		return Ok(UserService.CreateUserDto(userDto));
	}
}