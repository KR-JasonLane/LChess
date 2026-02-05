namespace LChess.Util.Converter;

/// <summary>
/// 두 값의 문자열 표현을 비교하여 Visibility로 변환하는 컨버터
/// </summary>
public class ValueMatchVisibilityConverter : IValueConverter
{
    /// <summary>
    /// value와 parameter의 문자열 표현을 비교하여 같으면 Visible, 다르면 Collapsed를 반환
    /// </summary>
    /// <param name="value">비교 값</param>
    /// <param name="targetType">타겟 타입</param>
    /// <param name="parameter">비교 파라미터</param>
    /// <param name="culture">문화 정보</param>
    /// <returns>Visibility 값</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var valueString = value?.ToString();
        var parameterString = parameter?.ToString();

        return valueString == parameterString ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// 역변환 (사용하지 않음)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
