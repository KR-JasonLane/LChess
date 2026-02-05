using Serilog;

namespace LChess.Util.Debugging;

/// <summary>
/// Serilog 로그 객체를 구성하는 빌더
/// </summary>
public static class LogBuilder
{
    private const string LogFilePath = "logs/lchess.log";

    /// <summary>
    /// Serilog 전역 로거를 일별 롤링 파일로 구성합니다.
    /// </summary>
    public static void Build()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(LogFilePath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
