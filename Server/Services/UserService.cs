using Frontend.Server.Data;
using Frontend.Server.Models;
using Frontend.Shared.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Server.Services;
public class UserService(IdentityDbContext context)
{
	private readonly IdentityDbContext _context = context;
	public async Task<List<string?>> GetUsersAsync()
	{
		return await _context.Users.Select(u => u.UserName).ToListAsync();
	}
	public async Task<UserDto?> GetUserByUserName(string userName)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

		if (user != null)
		{
			string? avatarBase64 = null;
			if (user.Avatar != null)
			{
				avatarBase64 = Convert.ToBase64String(user.Avatar);
			}
			return new UserDto()
			{
				Name = user.UserName!,
				EloRating = user.EloRating,
				Avatar = avatarBase64
			};
		}

		return null;
	}
}