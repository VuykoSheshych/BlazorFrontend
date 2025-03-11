namespace Frontend.Shared.Models;
public record MoveDto
{
	public required string From { get; set; }
	public required string To { get; set; }
	public string? Promotion { get; set; }
}
