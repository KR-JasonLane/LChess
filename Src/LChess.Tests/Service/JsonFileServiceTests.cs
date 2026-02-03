using System.IO;
using FluentAssertions;
using LChess.Service.Json;

namespace LChess.Tests.Service;

/// <summary>
/// JsonFileService 유닛테스트
/// 실제 파일 I/O를 사용하며, 임시 디렉토리에서 테스트 후 정리한다.
/// </summary>
public class JsonFileServiceTests : IDisposable
{
    private readonly JsonFileService _sut;
    private readonly string _tempDir;

    public JsonFileServiceTests()
    {
        _sut = new JsonFileService();
        _tempDir = Path.Combine(Path.GetTempPath(), "LChessTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, recursive: true);
    }

    #region :: Test DTO ::

    private class TestJsonModel
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    #endregion

    #region :: SaveJsonProperties ::

    [Fact]
    public void SaveJsonProperties_ValidPath_CreatesFile()
    {
        // Arrange
        var path = Path.Combine(_tempDir, "test.json");
        var model = new TestJsonModel { Name = "chess", Value = 42 };

        // Act
        var result = _sut.SaveJsonProperties(model, path);

        // Assert
        result.Should().BeTrue();
        File.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void SaveJsonProperties_EmptyPath_ReturnsFalse()
    {
        // Arrange
        var model = new TestJsonModel { Name = "test", Value = 1 };

        // Act
        var result = _sut.SaveJsonProperties(model, string.Empty);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void SaveJsonProperties_NullPath_ReturnsFalse()
    {
        // Arrange
        var model = new TestJsonModel { Name = "test", Value = 1 };

        // Act
        var result = _sut.SaveJsonProperties(model, null!);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region :: TryParseJsonProperties ::

    [Fact]
    public void TryParseJsonProperties_FileNotExists_CreatesDefaultAndReturnsTrue()
    {
        // Arrange
        var path = Path.Combine(_tempDir, "newfile.json");

        // Act
        var result = _sut.TryParseJsonProperties(path, out TestJsonModel? parsed);

        // Assert
        result.Should().BeTrue();
        parsed.Should().NotBeNull();
        parsed!.Name.Should().Be(string.Empty);
        parsed.Value.Should().Be(0);
        File.Exists(path).Should().BeTrue();
    }

    [Fact]
    public void SaveThenParse_RoundTrip_DataMatches()
    {
        // Arrange
        var path = Path.Combine(_tempDir, "roundtrip.json");
        var original = new TestJsonModel { Name = "chess", Value = 100 };
        _sut.SaveJsonProperties(original, path);

        // Act
        var result = _sut.TryParseJsonProperties(path, out TestJsonModel? parsed);

        // Assert
        result.Should().BeTrue();
        parsed.Should().NotBeNull();
        parsed!.Name.Should().Be("chess");
        parsed.Value.Should().Be(100);
    }

    [Fact]
    public void TryParseJsonProperties_CorruptFile_ReturnsFalse()
    {
        // Arrange
        var path = Path.Combine(_tempDir, "corrupt.json");
        File.WriteAllText(path, "not valid json {{{");

        // Act
        var result = _sut.TryParseJsonProperties(path, out TestJsonModel? parsed);

        // Assert
        result.Should().BeFalse();
        parsed.Should().BeNull();
    }

    #endregion

    #region :: TryParseJsonPropertiesInDirectory ::

    [Fact]
    public void TryParseJsonPropertiesInDirectory_MultipleFiles_ParsesAll()
    {
        // Arrange
        var subDir = Path.Combine(_tempDir, "multi");
        Directory.CreateDirectory(subDir);

        for (int i = 1; i <= 3; i++)
        {
            var model = new TestJsonModel { Name = $"item{i}", Value = i };
            _sut.SaveJsonProperties(model, Path.Combine(subDir, $"file{i}.json"));
        }

        // Act
        var result = _sut.TryParseJsonPropertiesInDirectory(subDir, out List<TestJsonModel> parsed);

        // Assert
        result.Should().BeTrue();
        parsed.Should().HaveCount(3);
    }

    [Fact]
    public void TryParseJsonPropertiesInDirectory_EmptyDirectory_ReturnsFalse()
    {
        // Arrange
        var emptyDir = Path.Combine(_tempDir, "empty");
        Directory.CreateDirectory(emptyDir);

        // Act
        var result = _sut.TryParseJsonPropertiesInDirectory(emptyDir, out List<TestJsonModel> parsed);

        // Assert
        result.Should().BeFalse();
        parsed.Should().BeEmpty();
    }

    [Fact]
    public void TryParseJsonPropertiesInDirectory_NonExistentDirectory_ReturnsFalse()
    {
        // Arrange
        var nonExistentDir = Path.Combine(_tempDir, "nonexistent");

        // Act
        var result = _sut.TryParseJsonPropertiesInDirectory(nonExistentDir, out List<TestJsonModel> parsed);

        // Assert
        result.Should().BeFalse();
    }

    #endregion
}
