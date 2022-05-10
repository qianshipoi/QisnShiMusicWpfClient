using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace QianShi.Music.ViewModels
{
    public class PlayViewModel : NavigationViewModel
    {
        public static PlayViewModel PlayViewModelDesign => App.Current.Container.Resolve<PlayViewModel>();

        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private ObservableCollection<Lyric> _lyrics;
        private PlayView? _playView = null;
        private bool _display = false;

        public bool Display
        {
            get => _display;
            set
            {
                if (_playView != null && _display != value)
                {
                    var marginAnimation = new ThicknessAnimation();
                    var window = Window.GetWindow(_playView);
                    if (value)
                    {
                        SetProperty(ref _display, value);
                        marginAnimation.From = new Thickness(0, window.Height, 0, 0);
                        marginAnimation.To = new Thickness(0);
                    }
                    else
                    {
                        marginAnimation.From = new Thickness(0);
                        marginAnimation.To = new Thickness(0, window.Height, 0, 0);
                        marginAnimation.Completed += (s, e) => SetProperty(ref _display, value);
                    }

                    marginAnimation.Duration = TimeSpan.FromSeconds(0.5);

                    _playView.BeginAnimation(FrameworkElement.MarginProperty, marginAnimation);
                }
                else
                {
                    SetProperty(ref _display, value);
                }
            }
        }

        public ObservableCollection<Lyric> Lyrics { get => _lyrics; set => SetProperty(ref _lyrics, value); }

        public DelegateCommand CloseCommand { get; private set; }

        private double _duration = 1d;
        public double Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        private double _position = 0d;
        public double Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set { SetProperty(ref _isPlaying, value); }
        }

        private DelegateCommand _playCommand;
        public DelegateCommand PlayCommand =>
            _playCommand ?? (_playCommand = new DelegateCommand(_playService.Play));

        private DelegateCommand _pauseCommand;
        public DelegateCommand PauseCommand =>
            _pauseCommand ?? (_pauseCommand = new DelegateCommand(_playService.Pause));

        public PlayViewModel(IContainerProvider provider,
            IRegionManager regionManager, IPlaylistService playlistService, IPlayService playService) : base(provider)
        {
            _lyrics = new ObservableCollection<Lyric>();
            _containerProvider = provider;
            _regionManager = regionManager;
            CloseCommand = new DelegateCommand(() => Display = false);
            _playlistService = playlistService;
            _playService = playService;
            _playService.ProgressChanged += (s, e) =>
            {
                Position = e.Value;
                Duration = e.Total;
            };
            _playService.IsPlayingChanged += (s, e) => IsPlaying = e.IsPlaying;
            _playService.CurrentChanged += async (s, e) =>
            {
                if (e.NewSong != null)
                {
                    var lyric = await GetLyric(e.NewSong.Id);
                    _playView?.Init("", lyric);
                }
            };
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var region = _regionManager.Regions[PrismManager.FullScreenRegionName];
            if (region != null)
            {
                var view = region.Views.FirstOrDefault(x => x.GetType() == typeof(PlayView));
                if (view != null)
                {
                    _playView = view as PlayView;
                    await Init();
                }
            }
        }

        private async Task<string> GetLyric(long songId)
        {
            var lyricResponse = await _playlistService.Lyric(songId);

            if (lyricResponse.Code == 200)
            {
                return lyricResponse.Lrc.Lyric;
            }
            return string.Empty;
        }

        private async Task Init()
        {
            if (_playView == null) return;
            var songId = 33894312;

            var response = await _playlistService.SongUrl(new Common.Models.Request.SongUrlRequest
            {
                Ids = songId.ToString()
            });
            if (response.Code == 200)
            {
                var url = response.Data[0].Url;

                var lyric = await GetLyric(songId);

                if (!string.IsNullOrEmpty(lyric))
                {
                    lyric = "[00:00:00.000]无歌词";
                }
                _playView.Init(url, lyric);

                //var lyricResponse = await _playlistService.Lyric(songId);

                //if (lyricResponse.Code == 200)
                //{
                //    var lyricsString = lyricResponse.Lrc.Lyric;
                //    _playView.Init(url, lyricsString);
                //}
            }
        }
    }

    public class Lyric : BindableBase
    {
        public TimeSpan Time { get; set; }
        public string Content { get; set; } = null!;

        private bool _activating = false;
        public bool Activating { get => _activating; set => SetProperty(ref _activating, value); }

        public override string ToString() => Content;
    }
}