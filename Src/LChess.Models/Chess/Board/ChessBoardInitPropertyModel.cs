using LChess.Models.Chess.Match;
using LChess.Util.Enums;

namespace LChess.Models.Chess.Board;

public class ChessBoardInitPropertyModel
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public ChessBoardInitPropertyModel(ChessBoardMode mode, PieceColorType userColor, List<NotationModel>? initNotations, TimeSpan correctionTime)
    {
        Mode               = mode          ;
        UserColor          = userColor     ;
        InitialNotations   = initNotations ;
        PlayTimeCorrection = correctionTime;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 체스보드 운영모드
    /// </summary>
    public ChessBoardMode Mode { get; init; }

    /// <summary>
    /// 사용자 색상
    /// </summary>
    public PieceColorType UserColor { get; init; }

    /// <summary>
    /// 보드 초기세팅
    /// </summary>
    public List<NotationModel>? InitialNotations { get; init; }

    /// <summary>
    /// 플레이타임 보정값(이어하기)
    /// </summary>
    public TimeSpan PlayTimeCorrection { get; init; }

    #endregion
}
