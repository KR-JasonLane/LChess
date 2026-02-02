using FluentAssertions;
using LChess.Util.Enums;
using LChess.Util.Extension;

namespace LChess.Tests.Util;

public class ChessTileColorTypeExtensionTests
{
    [Fact]
    public void ChangeColor_Dark_ReturnsLight()
    {
        ChessTileColorType.Dark.ChangeColor().Should().Be(ChessTileColorType.Light);
    }

    [Fact]
    public void ChangeColor_Light_ReturnsDark()
    {
        ChessTileColorType.Light.ChangeColor().Should().Be(ChessTileColorType.Dark);
    }

    [Fact]
    public void ChangeColor_HighLight_ReturnsSelf()
    {
        ChessTileColorType.HighLight.ChangeColor().Should().Be(ChessTileColorType.HighLight);
    }
}
