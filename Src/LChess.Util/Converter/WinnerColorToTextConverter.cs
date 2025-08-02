using LChess.Util.Enums;

namespace LChess.Util.Converter;

/// <summary>
/// 승자 색상타입을 텍스트로 변경해주는 컨버터
/// </summary>
public class WinnerColorToTextConverter : IValueConverter
{
    /// <summary>
    /// 승자 색상타입을 텍스트로 변경
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is PieceColorType color)
        {
            return color switch
            {
                PieceColorType.Black => "흑 승",
                PieceColorType.White => "백 승",
                _ => string.Empty
            };
        }

        return "무승부";
    }

    /// <summary>
    /// 역변환(미구현)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
