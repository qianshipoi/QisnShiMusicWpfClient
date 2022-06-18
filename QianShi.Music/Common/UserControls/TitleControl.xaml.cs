namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// TitleControl.xaml 的交互逻辑
    /// </summary>
    public partial class TitleControl : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(TitleControl), new PropertyMetadata(string.Empty));

        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }

        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(TitleControl), new PropertyMetadata(22d));

        public ICommand LookAllCammand
        {
            get { return (ICommand)GetValue(LookAllCammandProperty); }
            set { SetValue(LookAllCammandProperty, value); }
        }

        public static readonly DependencyProperty LookAllCammandProperty =
            DependencyProperty.Register(nameof(LookAllCammand), typeof(ICommand), typeof(TitleControl), new PropertyMetadata(null));

        public object LookAllCommandParameter
        {
            get { return (object)GetValue(LookAllCommandParameterProperty); }
            set { SetValue(LookAllCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty LookAllCommandParameterProperty =
            DependencyProperty.Register(nameof(LookAllCommandParameter), typeof(object), typeof(TitleControl), new PropertyMetadata(null));

        public bool ShowLookAll
        {
            get { return (bool)GetValue(ShowLookAllProperty); }
            set { SetValue(ShowLookAllProperty, value); }
        }

        public static readonly DependencyProperty ShowLookAllProperty =
            DependencyProperty.Register(nameof(ShowLookAll), typeof(bool), typeof(TitleControl), new PropertyMetadata(false));

        public TitleControl()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (null != LookAllCammand && LookAllCammand.CanExecute(LookAllCommandParameter))
                LookAllCammand.Execute(LookAllCommandParameter);
        }
    }
}