namespace LChess.Util.Enums;

/// <summary>
/// 게임종료 종류
/// </summary>
public enum EndGameType
{
    /// <summary>
    /// 초기화
    /// </summary>
    Init,
    /// <summary>
    /// 체크메이트
    /// </summary>
    CheckMate,
    /// <summary>
    /// 무승부
    /// </summary>
    Draw,
    /// <summary>
    /// 기권
    /// </summary>
    Resign
}