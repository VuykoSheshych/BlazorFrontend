using Frontend.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Server.Services;
public class UserService(IdentityDbContext context)
{
	private readonly IdentityDbContext _context = context;
	public async Task<List<string?>> GetUsersAsync()
	{
		return await _context.Users.Select(u => u.UserName).ToListAsync();
	}
}