using LChess.Models.Chess.Base;
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

        Unit = CreateUnitType(unitCode);

        TileColorType  = tileColor ;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 체스판 색상
    /// </summary>
    [ObservableProperty]
    private ChessTileColorType _tileColorType;

    /// <summary>
    /// 기물
    /// </summary>
    [ObservableProperty]
    private ChessUnitModelBase? _unit;

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
    public bool IsEmpty => Unit == null;

    #endregion

    #region :: Methods ::

    /// <summary>
    /// Stockfish 기물코드 문자를 기물코드로 변환
    /// </summary>
    /// <param name="unitCode"> Stockfish 기물코드 문자 </param>
    /// <param name="color"> 기물색상 </param>
    /// <returns> 체스 기물타입 </returns>
    private static ChessUnitModelBase? CreateUnitType(char unitCode)
    {
        if (unitCode == ' ') return null; // 빈 칸인 경우

        var unitColor = char.IsLower(unitCode) ? PieceColorType.Black : PieceColorType.White;

        return char.ToUpper(unitCode) switch
        {
            'P' => new PawnModel  (unitColor),
            'R' => new RookModel  (unitColor),
            'N' => new KnightModel(unitColor),
            'B' => new BishopModel(unitColor),
            'Q' => new QueenModel (unitColor),
            'K' => new KingModel  (unitColor),
            _   => null
        };
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

    #endregion
}
