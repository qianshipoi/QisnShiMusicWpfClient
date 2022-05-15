using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;

namespace QianShi.Music.ViewModels
{
    public class MvViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public MvViewModel() : base(App.Current.Container.Resolve<IContainerProvider>()) { }
        public const string MvIdParameter = nameof(MvIdParameter);


        private readonly IPlaylistService _playlistService;
        private readonly Dictionary<int, string> _urls = new();
        private readonly DispatcherTimer _timer = default!;
        private long _mvId;

        private MediaElement _videoControl = new();
        public MediaElement VideoControl
        {
            get => _videoControl;
            set => SetProperty(ref _videoControl, value);
        }

        private string _url = string.Empty;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private MvDetail? _detail;
        public MvDetail? Detail
        {
            get { return _detail; }
            set { SetProperty(ref _detail, value); }
        }

        private ObservableCollection<MovieVideo> _movieVideos = new();
        public ObservableCollection<MovieVideo> MovieVideos
        {
            get { return _movieVideos; }
            set { SetProperty(ref _movieVideos, value); }
        }

        private DelegateCommand<MovieVideo> _playMvCommand = default!;
        public DelegateCommand<MovieVideo> PlayMvCommand =>
            _playMvCommand ?? (_playMvCommand = new DelegateCommand<MovieVideo>(async (mv) =>
            {
                if (mv != null)
                {
                    _mvId = mv.Id;
                    await GetMvDetail();
                    await GetSimiMv();
                }
            }));

        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (value == _isPlaying) return;
                SetProperty(ref _isPlaying, value);
                if (value)
                {
                    _videoControl.Play();
                    _timer.Start();
                }
                else
                {
                    _videoControl.Pause();
                    _timer.Stop();
                }
            }
        }

        private bool _isMuted = false;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                SetProperty(ref _isMuted, value);
                _videoControl.IsMuted = value;
            }
        }

        private double _volume = 0.5d;
        public double Volume
        {
            get => _volume;
            set
            {
                SetProperty(ref _volume, value);
                _videoControl.Volume = value;
            }
        }

        private double _position = 0d;
        public double Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private DelegateCommand _playCommand = default!;
        public DelegateCommand PlayCommand =>
            _playCommand ??= new(() =>
            {
                if (!IsPlaying)
                {
                    IsPlaying = true;
                    if (ShowCover) ShowCover = false;
                }
            });

        private DelegateCommand _pauseCommand = default!;
        public DelegateCommand PauseCommand =>
            _pauseCommand ??= new(() =>
            {
                if (IsPlaying) IsPlaying = false;
            });

        private DelegateCommand<bool?> _setMutedCommand = default!;
        public DelegateCommand<bool?> SetMutedCommand =>
            _setMutedCommand ??= new(value =>
            {
                if (value.HasValue)
                {
                    IsMuted = value.Value;
                }
            });

        private DelegateCommand<double?> _setPositionCommand = default!;
        public DelegateCommand<double?> SetPositionCommand =>
            _setPositionCommand ??= new((value) =>
            {
                if (value.HasValue)
                {
                    try
                    {
                        _timer.Stop();
                        _videoControl.Position = TimeSpan.FromMilliseconds(value.Value);
                    }
                    finally
                    {
                        if (IsPlaying)
                        {
                            _timer.Start();
                        }
                    }
                }
            });

        private DelegateCommand<double?> _setVolumeCommand = default!;
        public DelegateCommand<double?> SetVolumeCommand =>
            _setVolumeCommand ??= new(value =>
            {
                if (value.HasValue)
                {
                    Volume = value.Value;
                }
            });

        private DelegateCommand<double?> _dragStartedCommand;
        public DelegateCommand<double?> DragStartedCommand =>
            _dragStartedCommand ??= new((value) =>
            {
                _timer.Stop();
            });


        private bool _showCover = true;
        public bool ShowCover
        {
            get => _showCover;
            set => SetProperty(ref _showCover, value);
        }

        public MvViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _videoControl.Volume = Volume;
            _videoControl.LoadedBehavior = MediaState.Manual;
            _videoControl.SetBinding(MediaElement.SourceProperty, nameof(Url));

            _timer = new()
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            _timer.Tick += (_, _) =>
            {
                Position = _videoControl.Position.TotalMilliseconds;
            };
        }

        public bool KeepAlive => true;

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            if (navigationContext.Parameters.ContainsKey(MvIdParameter))
            {
                _mvId = navigationContext.Parameters.GetValue<long>(MvIdParameter);
                await GetMvDetail();
                await GetSimiMv();
            }
        }

        private async Task GetMvDetail()
        {
            var detailResponse = await _playlistService.MvDetail(_mvId);

            if (detailResponse.Code == 200)
            {
                Detail = detailResponse.Data;
                _urls.Clear();
                foreach (var br in Detail.Brs)
                {
                    var urlResponse = await _playlistService.MvUrl(new Common.Models.Request.MvUrlRequest
                    {
                        Id = _mvId,
                        R = br.Br
                    });
                    if (urlResponse.Code == 200)
                    {
                        _urls.Add(br.Br, urlResponse.Data.Url);
                    }
                }
                if (_urls.Count > 0)
                {
                    Url = _urls[_urls.Max(x => x.Key)];
                }
            }
        }

        private async Task GetSimiMv()
        {
            var response = await _playlistService.SimiMv(_mvId);

            if (response.Code == 200)
            {
                MovieVideos.Clear();
                MovieVideos.AddRange(response.Mvs);
            }
        }
    }
}
