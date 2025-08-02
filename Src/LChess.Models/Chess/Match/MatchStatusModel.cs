using LChess.Util.Enums;

namespace LChess.Models.Chess.Match;

/// <summary>
/// 체스 매치 진행상태를 저장하는 모델
/// </summary>
public partial class MatchStatusModel : ObservableObject
{
    #region :: Constructor ::

    /// <summary>
    /// 생성자
    /// </summary>
    public MatchStatusModel(List<NotationModel> notation, PieceColorType? currentTurn, bool isCheck)
    {
        Notation = [.. notation];

        //컬러가 null이면 초기상태이므로, 다음턴이 백색이 될 수 있게 Black값을 넣어준다.
        CurrentTurn = currentTurn ?? PieceColorType.Black;

        IsCheck = isCheck;

        NextTurn = currentTurn == PieceColorType.White ? PieceColorType.Black : PieceColorType.White;
    }

    #endregion

    #region :: Properties ::

    /// <summary>
    /// 기보
    /// </summary>
    [ObservableProperty]
    private List<NotationModel> _notation;

    /// <summary>
    /// 현재 턴
    /// </summary>
    [ObservableProperty]
    private PieceColorType _currentTurn;

    /// <summary>
    /// 다음 턴
    /// </summary>
    [ObservableProperty]
    private PieceColorType _nextTurn;

    /// <summary>
    /// 현재 체크상태인지 여부
    /// </summary>
    public bool IsCheck;

    #endregion


    #region :: Methods ::

    #endregion
}
