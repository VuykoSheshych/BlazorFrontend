using Frontend.Server.Data;
using Frontend.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
}