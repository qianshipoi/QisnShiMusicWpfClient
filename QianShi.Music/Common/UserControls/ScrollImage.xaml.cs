namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// ScrollImage.xaml 的交互逻辑
    /// </summary>
    public partial class ScrollImage : UserControl
    {
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusCornerRadiusProperty); }
            set { SetValue(CornerRadiusCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusCornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(ScrollImage), new PropertyMetadata(new CornerRadius(10)));

        public ScrollImage()
        {
            InitializeComponent();
        }
    }

    public class WidthAndHeightToRectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.All(x => x is double))
            {
                double width = (double)values[0];
                double height = (double)values[1];
                return new Rect(0, 0, width, height);
            }
            return new Rect(0, 0, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ControlToAnimationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var result = 0d;

            if (values.Length > 1)
            {
                if (values[0] is double height)
                {
                    result += height;
                }
                else
                {
                    return result;
                }

                foreach (var value in values.Skip(1))
                {
                    if (value is double height1)
                    {
                        result -= height1;
                    }
                }
            }
            else
            {
                if (values[0] is double height)
                {
                    result += height;
                }
            }

            return -result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}