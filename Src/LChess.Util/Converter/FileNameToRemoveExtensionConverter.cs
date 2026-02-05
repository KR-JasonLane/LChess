namespace LChess.Util.Converter;

/// <summary>
/// 파일명에서 확장자를 제거하여 반환하는 컨버터
/// </summary>
public class FileNameToRemoveExtensionConverter : IValueConverter
{
    /// <summary>
    /// 파일 경로에서 확장자를 제거한 파일명을 반환
    /// </summary>
    /// <param name="value">파일 경로 문자열</param>
    /// <param name="targetType">타겟 타입</param>
    /// <param name="parameter">추가 파라미터</param>
    /// <param name="culture">문화 정보</param>
    /// <returns>확장자가 제거된 파일명</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        return string.Empty;
    }

    /// <summary>
    /// 역변환 (사용하지 않음)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
