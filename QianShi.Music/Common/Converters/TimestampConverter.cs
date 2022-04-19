using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QianShi.Music.Common.Converters
{
    internal class TimestampConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long timestamp)
            {
                string Id = TimeZoneInfo.Local.Id;
                var start = new DateTime(1970, 1, 1) + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                var startTime = TimeZoneInfo.ConvertTime(start, TimeZoneInfo.FindSystemTimeZoneById(Id));
                if (timestamp > 999_999_999)
                {
                    return startTime.AddMilliseconds(timestamp);
                }
                return startTime.AddSeconds(timestamp);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
