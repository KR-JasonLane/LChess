using LChess.Util.Enums;
using System.Data.Common;

namespace LChess.Models.Chess;

/// <summary>
/// 체스보드 요소 모델
/// </summary>
public partial class ChessBoardUnitModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessBoardUnitModel(ChessTileColorType tileColor, int row, int column, char unitCode)
    {
        Position = new ChessPositionModel(row, column);

        UnitType = CreateUnitType(unitCode, out var pieceColor);

        PieceColorType = pieceColor;
        TileColorType  = tileColor ;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 기물 색상
    /// </summary>
    [ObservableProperty]
    private PieceColorType _pieceColorType;

    /// <summary>
    /// 체스판 색상
    /// </summary>
    [ObservableProperty]
    private ChessTileColorType _tileColorType;

    /// <summary>
    /// 말
    /// </summary>
    [ObservableProperty]
    private ChessUnitType _unitType;

    /// <summary>
    /// 타일 하이라이트 여부
    /// </summary>
    [ObservableProperty]
    private bool _isHighLight;

    /// <summary>
    /// 해당 모델의 위치
    /// </summary>
    public readonly ChessPositionModel Position;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// Stockfish 유닛 타입을 체스 유닛 타입으로 변환
    /// </summary>
    /// <param name="unitCode"> Stockfish 유닛 타입 </param>
    /// <param name="color"> 기물색상 </param>
    /// <returns> 체스 유닛타입 </returns>
    private static ChessUnitType CreateUnitType(char unitCode, out PieceColorType color)
    {
        color = char.IsLower(unitCode) ? PieceColorType.Black : PieceColorType.White;

        return char.ToUpper(unitCode) switch
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
