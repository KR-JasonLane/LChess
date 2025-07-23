namespace LChess.Util.Converter;

/// <summary>
/// 기보를 꾸며주는 컨버터
/// </summary>
public class PrettyNotaionStringConverter : IValueConverter
{
    /// <summary>
    /// 기보를 꾸며주는 컨버터
    /// </summary>
    /// <param name="value">검사값</param>
    /// <param name="targetType">타겟 타입</param>
    /// <param name="parameter"> 현재 턴 </param>
    /// <param name="culture">문화 정보</param>
    /// <returns> Visibility 값 </returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string notation)
        {
            if(notation.Length == 5)
            {
                var promotion = notation.Substring(4, 1) switch
                {
                    "q" => "Queen",
                    "r" => "Rook",
                    "n" => "Night",
                    "b" => "Bishop",
                    _ => "NotFound"
                };

                return $"{notation.Substring(0, 2)} → {notation.Substring(2, 2)} [승격({promotion})]";
            }

            return $"{notation.Substring(0, 2)} → {notation.Substring(2, 2)}";
        }

        return new List<string>();
    }

    /// <summary>
    /// Null이면 Collapsed 반환
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
