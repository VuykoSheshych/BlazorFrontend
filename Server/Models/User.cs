using Microsoft.AspNetCore.Identity;

namespace Frontend.Server.Models;

public class User : IdentityUser
{
	public byte[]? Avatar { get; set; }
	public int EloRating { get; set; } = 2200;
}
