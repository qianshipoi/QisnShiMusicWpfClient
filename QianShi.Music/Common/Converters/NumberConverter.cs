using System.Globalization;
using System.Windows.Data;

namespace QianShi.Music.Common.Converters
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string CalculateResult(long val)
            {
                switch (val)
                {
                    case > 10_000 and < 100_000_000:
                        return $"{val / 10_000d:f2}万";
                    case > 100_000_000:
                        return $"{val / 100_000_000d:f2}亿";
                    default:
                        return val.ToString();
                }
            }

            if (value is long val)
            {
                return CalculateResult(val);
            }
            else if (value is int val1)
            {
                return CalculateResult(val1);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
