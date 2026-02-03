using FluentAssertions;
using LChess.Abstract.Service;
using LChess.Models.Setting;
using LChess.Service.Setting;
using Moq;

namespace LChess.Tests.Service;

/// <summary>
/// UserSettingService 유닛테스트
/// IJsonFileService를 Mock하여 설정 로드/저장 로직을 검증한다.
/// </summary>
public class UserSettingServiceTests
{
    private readonly Mock<IJsonFileService> _mockJsonFileService;
    private readonly UserSettingService _sut;

    public UserSettingServiceTests()
    {
        _mockJsonFileService = new Mock<IJsonFileService>();
        _sut = new UserSettingService(_mockJsonFileService.Object);
    }

    #region :: GetUserSetting ::

    [Fact]
    public void GetUserSetting_ParseSuccess_ReturnsModel()
    {
        // Arrange
        var expected = new UserSettingModel();
        expected.StockfishSetting.TinkingDepth = "25";

        _mockJsonFileService
            .Setup(j => j.TryParseJsonProperties(It.IsAny<string>(), out expected))
            .Returns(true);

        // Act
        var result = _sut.GetUserSetting();

        // Assert
        result.Should().NotBeNull();
        result.StockfishSetting.TinkingDepth.Should().Be("25");
    }

    [Fact]
    public void GetUserSetting_ParseFails_ReturnsDefaultModel()
    {
        // Arrange
        UserSettingModel? nullResult = null;

        _mockJsonFileService
            .Setup(j => j.TryParseJsonProperties(It.IsAny<string>(), out nullResult))
            .Returns(false);

        // Act
        var result = _sut.GetUserSetting();

        // Assert
        result.Should().NotBeNull();
        result.StockfishSetting.TinkingDepth.Should().Be("20");
    }

    [Fact]
    public void GetUserSetting_CallsJsonServiceWithCorrectPath()
    {
        // Arrange
        UserSettingModel? outVal = new UserSettingModel();

        _mockJsonFileService
            .Setup(j => j.TryParseJsonProperties(It.IsAny<string>(), out outVal))
            .Returns(true);

        // Act
        _sut.GetUserSetting();

        // Assert
        _mockJsonFileService.Verify(
            j => j.TryParseJsonProperties(
                It.Is<string>(p => p.EndsWith("LChessSetting.config")),
                out outVal),
            Times.Once);
    }

    #endregion

    #region :: SaveUserSettingModel ::

    [Fact]
    public void SaveUserSettingModel_CallsJsonServiceSave()
    {
        // Arrange
        var model = new UserSettingModel();

        _mockJsonFileService
            .Setup(j => j.SaveJsonProperties(It.IsAny<UserSettingModel>(), It.IsAny<string>()))
            .Returns(true);

        // Act
        _sut.SaveUserSettingModel(model);

        // Assert
        _mockJsonFileService.Verify(
            j => j.SaveJsonProperties(
                model,
                It.Is<string>(p => p.EndsWith("LChessSetting.config"))),
            Times.Once);
    }

    [Fact]
    public void SaveUserSettingModel_ReturnsSaveResult()
    {
        // Arrange
        _mockJsonFileService
            .Setup(j => j.SaveJsonProperties(It.IsAny<UserSettingModel>(), It.IsAny<string>()))
            .Returns(true);

        // Act & Assert
        _sut.SaveUserSettingModel(new UserSettingModel()).Should().BeTrue();

        // Arrange - false case
        _mockJsonFileService
            .Setup(j => j.SaveJsonProperties(It.IsAny<UserSettingModel>(), It.IsAny<string>()))
            .Returns(false);

        // Act & Assert
        _sut.SaveUserSettingModel(new UserSettingModel()).Should().BeFalse();
    }

    #endregion
}
