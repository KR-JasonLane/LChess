namespace LChess.Util.Converter;

/// <summary>
/// 체스 기보 노테이션을 읽기 쉬운 형태로 변환하는 컨버터
/// </summary>
public class PrettyNotationStringConverter : IValueConverter
{
    private const int PromotionNotationLength = 5;
    private const int SquareLength = 2;

    /// <summary>
    /// 노테이션 문자열을 "출발지 → 도착지 [승격(기물)]" 형태로 변환
    /// </summary>
    /// <param name="value">노테이션 문자열 (예: "e2e4", "e7e8q")</param>
    /// <param name="targetType">타겟 타입</param>
    /// <param name="parameter">추가 파라미터</param>
    /// <param name="culture">문화 정보</param>
    /// <returns>포맷팅된 기보 문자열</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string notation)
            return string.Empty;

        var from = notation.Substring(0, SquareLength);
        var to = notation.Substring(SquareLength, SquareLength);

        if (notation.Length == PromotionNotationLength)
        {
            var promotion = notation.Substring(SquareLength * 2) switch
            {
                "q" => "Queen",
                "r" => "Rook",
                "n" => "Knight",
                "b" => "Bishop",
                _ => "NotFound"
            };

            return $"{from} → {to} [승격({promotion})]";
        }

        return $"{from} → {to}";
    }

    /// <summary>
    /// 역변환 (사용하지 않음)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
