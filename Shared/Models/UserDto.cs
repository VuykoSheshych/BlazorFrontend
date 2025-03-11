namespace Frontend.Shared.Models;

public record UserDto
{
	public required string Name { get; set; }
	public required int EloRating { get; set; }
	public string? Avatar { get; set; }
	public string? AvatarUrl => Avatar != null ? $"data:image/jpeg;base64,{Avatar}" : null;
}