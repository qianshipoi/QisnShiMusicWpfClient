using System.Globalization;
using System.Windows.Data;

namespace QianShi.Music.Common.Converters
{
    public class NewlineFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str.Replace("\r\n", "").Replace("\n", "");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}