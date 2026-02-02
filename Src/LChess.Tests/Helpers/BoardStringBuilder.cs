namespace LChess.Tests.Helpers;

/// <summary>
/// 자주 사용하는 보드 배치 프리셋을 제공하는 헬퍼 클래스
/// </summary>
public static class BoardStringBuilder
{
    /// <summary>
    /// 체스 초기 배치
    /// </summary>
    public static string[] StartingPosition => new[]
    {
        "rnbqkbnr",
        "pppppppp",
        "        ",
        "        ",
        "        ",
        "        ",
        "PPPPPPPP",
        "RNBQKBNR",
    };

    /// <summary>
    /// 빈 보드
    /// </summary>
    public static string[] EmptyBoard => new[]
    {
        "        ",
        "        ",
        "        ",
        "        ",
        "        ",
        "        ",
        "        ",
        "        ",
    };

    /// <summary>
    /// 킹만 있는 보드 (기본 테스트용)
    /// </summary>
    public static string[] KingsOnly => new[]
    {
        "    k   ",
        "        ",
        "        ",
        "        ",
        "        ",
        "        ",
        "        ",
        "    K   ",
    };
}
