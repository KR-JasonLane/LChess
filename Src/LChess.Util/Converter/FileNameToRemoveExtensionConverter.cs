namespace LChess.Util.Converter;

public class FileNameToRemoveExtensionConverter : IValueConverter
{
    /// <summary>
    /// 변환
    /// </summary>
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }

        return default;
    }

    /// <summary>
    /// 역변환 (구현하지 않음.)
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
