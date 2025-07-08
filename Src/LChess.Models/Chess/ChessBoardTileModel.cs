using LChess.Util.Enums;
using System.Data.Common;

namespace LChess.Models.Chess;

/// <summary>
/// 체스보드 요소 모델
/// </summary>
public partial class ChessBoardTileModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessBoardTileModel(ChessTileColorType tileColor, int row, int column, char unitCode)
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
    /// 타일 선택 여부
    /// </summary>
    [ObservableProperty]
    private bool _isSelected;

    /// <summary>
    /// 기물 이동경로 하이라이트 여부
    /// </summary>
    [ObservableProperty]
    private bool _isHighLightMove;

    /// <summary>
    /// 적 기물 하이라이트 여부
    /// </summary>
    [ObservableProperty]
    private bool _isHighLightEnemy;

    /// <summary>
    /// 해당 모델의 위치
    /// </summary>
    public readonly ChessPositionModel Position;

    /// <summary>
    /// 기물이 해당 타일로 이동 가능한지 여부
    /// </summary>
    public bool IsMovableTarget => IsHighLightEnemy || IsHighLightMove;

    /// <summary>
    /// 타일이 빈 칸인지 여부
    /// </summary>
    public bool IsEmpty => UnitType == ChessUnitType.Empty && PieceColorType == PieceColorType.None;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// Stockfish 기물코드 문자를 기물코드로 변환
    /// </summary>
    /// <param name="unitCode"> Stockfish 기물코드 문자 </param>
    /// <param name="color"> 기물색상 </param>
    /// <returns> 체스 기물타입 </returns>
    private static ChessUnitType CreateUnitType(char unitCode, out PieceColorType color)
    {
        var unitType = char.ToUpper(unitCode) switch
        {
            'P' => ChessUnitType.Pawn,
            'R' => ChessUnitType.Rook,
            'N' => ChessUnitType.Knight,
            'B' => ChessUnitType.Bishop,
            'Q' => ChessUnitType.Queen,
            'K' => ChessUnitType.King,
            _ => ChessUnitType.Empty
        };

        if(unitType == ChessUnitType.Empty)
        {
            color = PieceColorType.None; // 빈 칸은 색상 없음
        }
        else
        {
            color = char.IsLower(unitCode) ? PieceColorType.Black : PieceColorType.White;
        }

        return unitType;
    }

    /// <summary>
    /// 모든 하이라이트 끄기
    /// </summary>
    public void TurnOffHighLight()
    {
        IsHighLightMove  = false;
        IsHighLightEnemy = false;
        IsSelected       = false;
    }

    /// <summary>
    /// 현재 기물 색상과 같은 색상인지 판단
    /// </summary>
    /// <param name="color"> 비교할 색상 </param>
    /// <returns> 색상이 같은지 여부 </returns>
    public bool IsSameColor(PieceColorType color) => PieceColorType == color;

    /// <summary>
    /// 타일에 적 기물이 있는지 판단
    /// </summary>
    /// <param name="otherTile"> 확인할 타일 </param>
    /// <returns> 적 기물 유무 </returns>
    public bool IsEnemyTile(ChessBoardTileModel otherTile) => !otherTile.IsEmpty && otherTile.PieceColorType != PieceColorType;

    /// <summary>
    /// 색깔로 적군 기물인지 판단
    /// </summary>
    /// <param name="color"> 확인할 색상 </param>
    /// <returns> 적 색상 유무 </returns>
    public bool IsEnemyColor(PieceColorType color) => PieceColorType != color && PieceColorType != PieceColorType.None && color != PieceColorType.None;

    #endregion
}
