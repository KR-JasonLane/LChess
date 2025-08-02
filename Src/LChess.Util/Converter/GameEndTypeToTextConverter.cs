using LChess.Util.Enums;

namespace LChess.Util.Converter;

/// <summary>
/// 게임 종료 사유를 텍스트로 변경해주는 컨버터
/// </summary>
public class GameEndTypeToTextConverter : IValueConverter
{
    /// <summary>
    /// 게임 종료 사유를 텍스트로 변경
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is EndGameType type)
        {
            return type switch
            {
                EndGameType.CheckMate => "체크메이트" ,
                EndGameType.Draw      => "스테일메이트",
                EndGameType.Resign    => "기권"       ,
                _ => string.Empty
            };
        }

        return string.Empty;
    }

    /// <summary>
    /// 역변환(미구현)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
