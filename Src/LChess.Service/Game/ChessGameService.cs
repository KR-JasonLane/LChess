using LChess.Abstract.Service;

using LChess.Models.Chess;

using LChess.Util.Enums;
using System.Collections.ObjectModel;

namespace LChess.Service.Game;

/// <summary>
/// 게임 관리 서비스
/// </summary>
public class ChessGameService : IChessGameService
{
	#region :: Constructure ::

	/// <summary>
	/// 생성자
	/// </summary>
	public ChessGameService(IStockfishEngineService stockfishEngineService)
	{
		_stockfishEngineService = stockfishEngineService;
	}

	#endregion

	#region :: Properties ::

	/// <summary>
	/// 엔진관리 서비스
	/// </summary>
	private readonly IStockfishEngineService _stockfishEngineService;

	#endregion

	#region :: Methods ::

	/// <summary>
	/// 현재 보드상태
	/// </summary>
	/// <returns></returns>
	public async Task<List<List<ChessBoardUnitModel>>?> DrawBoard()
	{
		var stockfishOutput = await _stockfishEngineService.GetCurrentBoard();

		var result = new List<List<ChessBoardUnitModel>>();
		var currentTile = ChessTileColorType.DeepSkyBlue;

		for (int i = 0; i < 8; i++)
		{
			var line = new List<ChessBoardUnitModel>();

            for (int j = 1; j <= 8; j++)
			{
				var tile = new ChessBoardUnitModel()
				{
					TileColorType = currentTile,
					UnitType = GetUnitType(stockfishOutput?[i][ j * 4 - 1] ?? ' ', out var pieceColor),
					PieceColorType = pieceColor
                };

				line.Add(tile);

				if(j != 8)
                {
                    currentTile = currentTile == ChessTileColorType.SkyBlue ? ChessTileColorType.DeepSkyBlue : ChessTileColorType.SkyBlue;
                }
            }

			result.Add(line);
        }


        return result;
	}

	/// <summary>
	/// 게임 초기화
	/// </summary>
	/// <returns></returns>
	public void ClearMoves()
	{
		throw new NotImplementedException();
	}

	private ChessUnitType GetUnitType(char stockfishUnit, out PieceColorType color)
	{
		color = char.IsLower(stockfishUnit) ? PieceColorType.White : PieceColorType.Black;

        return char.ToUpper(stockfishUnit) switch
		{
			'P' => ChessUnitType.Pawn,
			'R' => ChessUnitType.Rook,
			'N' => ChessUnitType.Knight,
			'B' => ChessUnitType.Bishop,
			'Q' => ChessUnitType.Queen,
			'K' => ChessUnitType.King,
			_ => ChessUnitType.Empty
		};
    }

	#endregion
}
