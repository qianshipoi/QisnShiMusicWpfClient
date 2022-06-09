using System.Globalization;
using System.Windows.Data;

namespace QianShi.Music.Common.Converters
{
    public class MillisecondToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double val)
                return TimeSpan.FromMilliseconds(val).ToString(@"mm\:ss");
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}