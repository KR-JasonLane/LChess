using LChess.Abstract.Service;

using LChess.Models.Chess;

using LChess.Util.Enums;
using LChess.Util.Extension;

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

	#region :: Services ::

	/// <summary>
	/// 엔진관리 서비스
	/// </summary>
	private readonly IStockfishEngineService _stockfishEngineService;

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 유저 기물색상 기억
    /// </summary>
    private PieceColorType _userPieceColor;

    /// <summary>
    /// 체스보드 유닛 모델 리스트
    /// </summary>
    private List<List<ChessBoardUnitModel>>? _chessBoardUnits;

    /// <summary>
    /// 선택된 유닛
    /// </summary>
    private ChessBoardUnitModel? _selectedUnit;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// 현재 보드상태
    /// </summary>
    /// <returns></returns>
    public async Task<List<List<ChessBoardUnitModel>>?> CurrentBoard()
	{
        if(_chessBoardUnits == null)
        {
            var stockfishOutput = await _stockfishEngineService.GetCurrentBoard();

            if (_userPieceColor == PieceColorType.Black)
            {
                _chessBoardUnits = CreateBlackBoardData(stockfishOutput);
            }
            else
            {
                _chessBoardUnits = CreateWhiteBoardData(stockfishOutput);
            }
        }

        return _chessBoardUnits;
    }

	/// <summary>
	/// 게임 초기화
	/// </summary>
	/// <returns></returns>
	public void ClearMoves()
    {
        _stockfishEngineService.SendCommandAsync("ucinewgame", string.Empty);
    }

    /// <summary>
    /// 사용자 기물색상 설정
    /// </summary>
    /// <param name="pieceColor"> 기물색상 타입 </param>
    public void SetUserPieceColor(PieceColorType pieceColor) => _userPieceColor = pieceColor;

	/// <summary>
	/// 체스보드 데이터 만들기 - 백색 기준 (정방향)
	/// </summary>
	/// <param name="stockfishOutput"> Stockfish 엔진에서 받은 보드 문자열 리스트 </param>
	/// <returns> 생성된 데이터 </returns>
	private List<List<ChessBoardUnitModel>>? CreateWhiteBoardData(List<string>? stockfishOutput)
    {
        var result = new List<List<ChessBoardUnitModel>>();
        var tileColor = ChessTileColorType.Dark;

        for (int row = 0; row < 8; row++)
        {
            var line = new List<ChessBoardUnitModel>();

            for (int column = 1; column <= 8; column++)
            {
                line.Add(new ChessBoardUnitModel(tileColor, row, column, stockfishOutput?[row][column * 4 - 1] ?? ' '));

                if (column != 8)
                {
                    tileColor = tileColor.ChangeColor();
                }
            }

            result.Add(line);
        }

		return result;
    }

    /// <summary>
    /// 체스보드 데이터 만들기 - 흑색 기준 (역방향)
    /// </summary>
    /// <param name="stockfishOutput"> Stockfish 엔진에서 받은 보드 문자열 리스트 </param>
    /// <returns> 생성된 데이터 </returns>
    private List<List<ChessBoardUnitModel>>? CreateBlackBoardData(List<string>? stockfishOutput)
    {
        var result = new List<List<ChessBoardUnitModel>>();
        var tileColor = ChessTileColorType.Dark;

        for (int row = 7; row >= 0; row--)
        {
            var line = new List<ChessBoardUnitModel>();

            for (int column = 8; column >= 1; column--)
            {
                line.Add(new ChessBoardUnitModel(tileColor, row, column, stockfishOutput?[row][column * 4 - 1] ?? ' '));

                if (column != 1)
                {
                    tileColor = tileColor.ChangeColor();
                }
            }

            result.Add(line);
        }

        return result;
    }

    /// <summary>
    /// 체스보드	유닛 선택
    /// </summary>
    /// <param name="selectedUnit"> 선택된 유닛 모델 </param>
    /// <returns> 유닛 선택 시 하이라이트 된 보드 반환 </returns>
    public List<List<ChessBoardUnitModel>> SelectUnit(ChessBoardUnitModel selectedUnit)
    {
        var result = new List<List<ChessBoardUnitModel>>();

        _selectedUnit = selectedUnit;
        //TODO : 유닛 선택 로직 구현 (타일색상 하이라이트)

        return result;
    }

    #endregion
}
