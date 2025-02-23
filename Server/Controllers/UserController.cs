using Frontend.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Server.Controllers;

[ApiController]
[Route("api/users")]
public class UserController(IdentityDbContext context) : ControllerBase
{
	private readonly IdentityDbContext _context = context;

	[HttpGet]
	public async Task<IActionResult> GetUsersAsync()
	{
		var users = await _context.Users.Select(u => u.UserName).ToListAsync();
		return Ok(users);
	}
}