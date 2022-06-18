namespace QianShi.Music.Common.Converters;

public class TimeSpanToDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan) return timeSpan.TotalMilliseconds;
        else return 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double val) return TimeSpan.FromMilliseconds(val);
        return TimeSpan.Zero;
    }
}

public class DoubleToTimeSpanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double val) return TimeSpan.FromMilliseconds(val);
        return TimeSpan.Zero;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan) return timeSpan.TotalMilliseconds;
        return 0;
    }
}