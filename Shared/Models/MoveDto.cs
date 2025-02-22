namespace Frontend.Shared.Models;
public class MoveDto
{
	public string Player { get; set; } = string.Empty;
	public string From { get; set; } = string.Empty;
	public string To { get; set; } = string.Empty;
	public string? Promotion { get; set; }
}
