using Frontend.Server.Data;
using Frontend.Server.Models;
using Frontend.Shared.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Frontend.Server.Services;
public class UserService(UserDbContext context) : IUserService
{
	private readonly UserDbContext _context = context;
	public async Task<List<User>> GetUsersAsync()
	{
		return await _context.Users.ToListAsync();
	}
	public async Task<User?> GetUserByIdAsync(string userId)
	{
		return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
	}
	public async Task<User?> GetUserByUserNameAsync(string userName)
	{
		return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
	}
	public static UserDto CreateUserDto(User user)
	{
		var avatar = user.Avatar is not null ? Convert.ToBase64String(user.Avatar) : null;
		return new UserDto(user.Id, user.UserName!, user.EloRating, avatar);
	}
}