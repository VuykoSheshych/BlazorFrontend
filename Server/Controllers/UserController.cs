using Frontend.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Server.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(UserService userService) : ControllerBase
{
	private readonly UserService _userService = userService;

	[HttpGet]
	public async Task<IActionResult> GetUsersAsync()
	{
		var users = await _userService.GetUsersAsync();
		return Ok(users);
	}

	[HttpGet("{userName}")]
	public async Task<IActionResult> GetUserByUserName(string userName)
	{
		var userDto = await _userService.GetUserByUserName(userName);

		if (userDto != null) return NotFound();
		return Ok(userDto);
	}
}