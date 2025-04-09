namespace BlazorFrontend.Features.Game.Services;

public static class ChessBoardService
{
	public static char[,] ConvertFenToMatrix(string fen)
	{
		char[,] board = new char[8, 8];

		var parts = fen.Split(' ');

		var rows = parts[0].Split('/');
		for (int rank = 0; rank < 8; rank++)
		{
			int file = 0;
			foreach (var symbol in rows[rank])
			{
				if (char.IsDigit(symbol))
				{
					file += symbol - '0';
				}
				else
				{
					board[rank, file++] = symbol;
				}
			}
		}

		return board;
	}
}