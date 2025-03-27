using Frontend.Server.Models;

namespace Frontend.Server.Services;
public interface IUserService
{
	Task<List<User>> GetUsersAsync();
	Task<User?> GetUserByIdAsync(string userId);
	Task<User?> GetUserByUserNameAsync(string userName);
}