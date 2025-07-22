using Serilog;

namespace LChess.Util.Debugging;

/// <summary>
/// 로그객체빌더
/// </summary>
public class LogBuilder
{
    /// <summary>
    /// 로그 객체를 빌드
    /// </summary>
    public static void Build()
    {
        Log.Logger = new LoggerConfiguration()
        .WriteTo.File($"logs/.log", rollingInterval: RollingInterval.Day)
        .CreateLogger();
    }
}
