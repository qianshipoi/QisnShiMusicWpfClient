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
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private readonly IPlaylistService _playlistService;
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

        public PlayViewModel(IContainerProvider provider,
            IRegionManager regionManager, IPlaylistService playlistService) : base(provider)
        {
            _lyrics = new ObservableCollection<Lyric>();
            _containerProvider = provider;
            _regionManager = regionManager;
            CloseCommand = new DelegateCommand(() => Display = false);
            _playlistService = playlistService;
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
                var lyricResponse = await _playlistService.Lyric(songId);

                if (lyricResponse.Code == 200)
                {
                    var lyricsString = lyricResponse.Lrc.Lyric;
                    _playView.Init(url, lyricsString);
                }
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