using Prism.Commands;
using Prism.Ioc;

using QianShi.Music.Services;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// PlaylistCard.xaml 的交互逻辑
    /// </summary>
    public partial class PlaylistCard : UserControl
    {
        private INavigationService _navigationService => App.Current.Container.Resolve<INavigationService>();
        private IPlayStoreService _playStoreService => App.Current.Container.Resolve<IPlayStoreService>();


        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(PlaylistCard), new PropertyMetadata(string.Empty));

        public double TitleFontSize
        {
            get => (double)GetValue(TitleFontSizeProperty);
            set => SetValue(TitleFontSizeProperty, value);
        }

        public static readonly DependencyProperty TitleFontSizeProperty =
            DependencyProperty.Register(nameof(TitleFontSize), typeof(double), typeof(PlaylistCard), new PropertyMetadata(16d));

        public long AmountOfPlay
        {
            get => (long)GetValue(AmountOfPlayProperty);
            set => SetValue(AmountOfPlayProperty, value);
        }

        public static readonly DependencyProperty AmountOfPlayProperty =
            DependencyProperty.Register(nameof(AmountOfPlay), typeof(long), typeof(PlaylistCard), new FrameworkPropertyMetadata(0L, (dp, e) =>
            {
                if (dp is PlaylistCard card)
                {
                    card.PlayCountControl.Visibility = e.NewValue == default ? Visibility.Collapsed : Visibility.Visible;
                }
            }));

        public string Cover
        {
            get => (string)GetValue(CoverProperty);
            set => SetValue(CoverProperty, value);
        }

        public static readonly DependencyProperty CoverProperty =
            DependencyProperty.Register(nameof(Cover), typeof(string), typeof(PlaylistCard), new PropertyMetadata(string.Empty));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(PlaylistCard), new FrameworkPropertyMetadata(string.Empty, (dp, e) =>
            {
                if (dp is PlaylistCard card)
                {
                    card.DescriptionControl.Visibility = string.IsNullOrWhiteSpace(e.ToString()) ? Visibility.Collapsed : Visibility.Visible;
                }
            }));

        public bool DescriptionCanClick
        {
            get => (bool)GetValue(DescriptionCanClickProperty);
            set => SetValue(DescriptionCanClickProperty, value);
        }

        public static readonly DependencyProperty DescriptionCanClickProperty =
            DependencyProperty.Register(nameof(DescriptionCanClick), typeof(bool), typeof(PlaylistCard), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ICommand DescriptionCommand
        {
            get => (ICommand)GetValue(DescriptionCommandProperty);
            set => SetValue(DescriptionCommandProperty, value);
        }

        public static readonly DependencyProperty DescriptionCommandProperty =
            DependencyProperty.Register(nameof(DescriptionCommand), typeof(ICommand), typeof(PlaylistCard), new PropertyMetadata(default));

        public object DescriptionCommandParameter
        {
            get => (object)GetValue(DescriptionCommandParameterProperty);
            set => SetValue(DescriptionCommandParameterProperty, value);
        }

        public static readonly DependencyProperty DescriptionCommandParameterProperty =
            DependencyProperty.Register(nameof(DescriptionCommandParameter), typeof(object), typeof(PlaylistCard), new PropertyMetadata(default));

        public IPlaylist Playlist
        {
            get => (IPlaylist)GetValue(PlaylistProperty);
            set => SetValue(PlaylistProperty, value);
        }

        public static readonly DependencyProperty PlaylistProperty =
            DependencyProperty.Register(nameof(Playlist), typeof(IPlaylist), typeof(PlaylistCard), new PropertyMetadata(null));

        public ICommand OpenPlaylistCommand
        {
            get => (ICommand)GetValue(OpenPlaylistCommandProperty);
            set => SetValue(OpenPlaylistCommandProperty, value);
        }

        public static readonly DependencyProperty OpenPlaylistCommandProperty =
            DependencyProperty.Register(nameof(OpenPlaylistCommand), typeof(ICommand), typeof(PlaylistCard), new PropertyMetadata(null));

        public PlaylistCard()
        {
            InitializeComponent();
        }

        private void FilletImage_ImageClick(object sender, RoutedEventArgs e)
        {
            if (OpenPlaylistCommand != null && OpenPlaylistCommand.CanExecute(Playlist))
                OpenPlaylistCommand.Execute(Playlist);
        }

        private DelegateCommand _playCommand = default!;
        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(ExecutePlayCommand));

        async void ExecutePlayCommand()
        {
            await _playStoreService.AddPlaylistAsync(Playlist.Id);
            _playStoreService.Play();
        }
    }
}