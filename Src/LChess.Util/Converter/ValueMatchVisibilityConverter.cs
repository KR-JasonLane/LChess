namespace LChess.Util.Converter;

/// <summary>
/// 같은문자열인지 여부를 Visibility형태로 변경해주는 컨버터
/// </summary>
public class StringMatchToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// value와 parameter를 비교하여 같은값으면 Visible, 가르면 Collapsed를 반환
    /// </summary>
    /// <param name="value"> 비교 값</param>
    /// <param name="targetType">타겟 타입</param>
    /// <param name="parameter"> 비교 파라미터</param>
    /// <param name="culture">문화 정보</param>
    /// <returns> Visibility 값 </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((value?.ToString() ?? "Null") == (parameter?.ToString() ?? "NULL"))
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }
    /// <summary>
    /// 역변환 (사용안함)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
