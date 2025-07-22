namespace LChess.Util.Converter;

/// <summary>
/// Boolean 값을 Visibility로 변환하는 컨버터
/// </summary>
public class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Boolean 값을 Visibility로 변환
    /// </summary>
    /// <param name="value">Boolean 값</param>
    /// <param name="targetType">타겟 타입</param>
    /// <param name="parameter">추가 파라미터</param>
    /// <param name="culture">문화 정보</param>
    /// <returns> Visibility 값 </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }
    /// <summary>
    /// Visibility 값을 Boolean으로 변환 (역변환)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }

        return false;
    }
}
