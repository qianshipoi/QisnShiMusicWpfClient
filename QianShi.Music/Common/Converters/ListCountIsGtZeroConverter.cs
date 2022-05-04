using System.Globalization;
using System.Windows.Data;

namespace QianShi.Music.Common.Converters
{
    internal class ListCountIsGtZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<object> vals)
            {
                return vals.Count() > 0;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
