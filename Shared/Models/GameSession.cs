namespace Frontend.Shared.Models;
public class GameSession
{
	public string Id { get; set; } = string.Empty;
	public required PlayerDto WhitePlayer { get; set; }
	public required PlayerDto BlackPlayer { get; set; }
	public List<Move> Moves { get; set; } = [];
	public string CurrentFen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
