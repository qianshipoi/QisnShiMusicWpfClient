using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// PlaylistCover.xaml 的交互逻辑
    /// </summary>
    public partial class PlaylistCover : UserControl
    {
        public static readonly RoutedEvent ImageClickEvent =
           EventManager.RegisterRoutedEvent(nameof(ImageClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FilletImage));

        public event RoutedEventHandler ImageClick
        {
            add => AddHandler(ImageClickEvent, value);
            remove => RemoveHandler(ImageClickEvent, value);
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusCornerRadiusProperty); }
            set { SetValue(CornerRadiusCornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusCornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(PlaylistCover), new PropertyMetadata(new CornerRadius()));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register(nameof(ImageSource), typeof(string),
                typeof(PlaylistCover), new PropertyMetadata("https://oss.kuriyama.top/static/background.png"));

        public ICommand PlayCommand
        {
            get { return (ICommand)GetValue(PlayCommandProperty); }
            set { SetValue(PlayCommandProperty, value); }
        }

        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register(nameof(PlayCommand), typeof(ICommand), typeof(PlaylistCover), new PropertyMetadata(null));

        public ICommand OpenPlaylistCommand
        {
            get { return (ICommand)GetValue(OpenPlaylistCommandProperty); }
            set { SetValue(OpenPlaylistCommandProperty, value); }
        }

        public static readonly DependencyProperty OpenPlaylistCommandProperty =
            DependencyProperty.Register(nameof(OpenPlaylistCommand), typeof(ICommand), typeof(PlaylistCover), new PropertyMetadata(null));

        public object OpenPlaylistCommandParameter
        {
            get { return (object)GetValue(OpenPlaylistCommandParameterProperty); }
            set { SetValue(OpenPlaylistCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty OpenPlaylistCommandParameterProperty =
            DependencyProperty.Register(nameof(OpenPlaylistCommandParameter), typeof(object), typeof(PlaylistCover), new PropertyMetadata(null));



        public PlaylistCover()
        {
            InitializeComponent();
        }

        private void ImageControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var args = new RoutedEventArgs(ImageClickEvent, this);
            RaiseEvent(args);
            if (OpenPlaylistCommand != null && OpenPlaylistCommand.CanExecute(OpenPlaylistCommandParameter))
                OpenPlaylistCommand.Execute(OpenPlaylistCommandParameter);
        }
    }
}