namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// FilletImage.xaml 的交互逻辑
    /// </summary>
    public partial class FilletImage : UserControl
    {
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusCornerRadiusProperty); }
            set { SetValue(CornerRadiusCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusCornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(FilletImage), new PropertyMetadata(new CornerRadius()));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(string),
                typeof(FilletImage), new PropertyMetadata("https://oss.kuriyama.top/static/background.png"));

        public FilletImage()
        {
            InitializeComponent();
        }
    }
}