using FluentAssertions;
using LChess.Models.Setting;
using LChess.Models.Setting.Sub;

namespace LChess.Tests.Domain.Setting;

public class SettingModelTests
{
    [Fact]
    public void UserSettingModel_DefaultStockfishSetting_NotNull()
    {
        var model = new UserSettingModel();

        model.StockfishSetting.Should().NotBeNull();
    }

    [Fact]
    public void StockfishSettingModel_DefaultDepth_Is20()
    {
        var model = new StockfishSettingModel();

        model.ThinkingDepth.Should().Be("20");
    }

    [Fact]
    public void SystemSettingModel_DefaultNotationPath_ContainsNotation()
    {
        var model = new SystemSettingModel();

        model.NotationSaveDirectory.Should().Contain("Notation");
    }

    [Fact]
    public void UserSettingModel_DefaultSystemSetting_NotNull()
    {
        var model = new UserSettingModel();

        model.SystemSetting.Should().NotBeNull();
    }
}
