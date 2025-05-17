namespace BlazorFrontend.Shared.Services;

public class Feedback
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Author { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	public DateTime DateTime { get; set; }
}