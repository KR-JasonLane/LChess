namespace LChess.Util.Converter;


/// <summary>
/// Null이면 Visible을 반환하는 컨버터
/// </summary>
public class NullToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Null이면 Visible을 반환
    /// </summary>
    /// <param name="value">검사값</param>
    /// <param name="targetType">타겟 타입</param>
    /// <param name="parameter">추가 파라미터</param>
    /// <param name="culture">문화 정보</param>
    /// <returns> Visibility 값 </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Visible : Visibility.Collapsed;
    }
    /// <summary>
    /// Null이면 Collapsed 반환
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Collapsed : Visibility.Visible;
    }
}
