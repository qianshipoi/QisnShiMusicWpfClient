namespace QianShi.Music.Common.Controls
{
    /// <summary>
    /// FilletImage.xaml 的交互逻辑
    /// </summary>
    public partial class FilletImage : UserControl
    {
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(FilletImage), new PropertyMetadata(new CornerRadius(20)));
        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(FilletImage), new PropertyMetadata());

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(FilletImage), new PropertyMetadata(Stretch.UniformToFill));

        public FilletImage()
        {
            InitializeComponent();
        }
    }
}
