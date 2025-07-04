using LChess.Util.Enums;

namespace LChess.Util.Extension;

/// <summary>
/// 체스 타일 색상 타입 확장
/// </summary>
public static class ChessTileColorTypeExtension
{
    /// <summary>
    /// 체스 타일 색상을 변경
    /// </summary>
    /// <param name="tileColor"> 변경대상 </param>
    /// <returns> 변경 후 색상 </returns>
    public static ChessTileColorType ChangeColor(this ChessTileColorType tileColor)
    {
        return tileColor switch
        {
            ChessTileColorType.Dark  => ChessTileColorType.Light,
            ChessTileColorType.Light => ChessTileColorType.Dark ,
            _ => tileColor
        };
    }
}
