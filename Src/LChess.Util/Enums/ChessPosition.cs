namespace LChess.Util.Enums;

/// <summary>
/// 체스 위치 열거형
/// </summary>
public enum ChessPosition
{
    /// <summary>
    /// 잘못된 위치일때, 초기화 시 적용
    /// </summary>
    Invalid = -1,

    /// <summary>
    /// 체스 보드 좌표를 숫자로 표기
    /// </summary>
    A8 = 0,  B8, C8, D8, E8, F8, G8, H8,
    A7 = 10, B7, C7, D7, E7, F7, G7, H7,
    A6 = 20, B6, C6, D6, E6, F6, G6, H6,
    A5 = 30, B5, C5, D5, E5, F5, G5, H5,
    A4 = 40, B4, C4, D4, E4, F4, G4, H4,
    A3 = 50, B3, C3, D3, E3, F3, G3, H3,
    A2 = 60, B2, C2, D2, E2, F2, G2, H2,
    A1 = 70, B1, C1, D1, E1, F1, G1, H1,
}