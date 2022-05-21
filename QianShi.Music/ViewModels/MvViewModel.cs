using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using MvUrl = QianShi.Music.Common.Models.MvUrl;

namespace QianShi.Music.ViewModels
{
    public class MvViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public MvViewModel() : base(App.Current.Container.Resolve<IContainerProvider>()) { }
        public const string MvIdParameter = nameof(MvIdParameter);

        private readonly IVideoPlayService _videoPlayService;
        private readonly IVideoPlayStoreService _videoPlayStoreService;
        private long _mvId;
        private bool _isDragProgress = false;

        private object? _videoControl;
        public object? VideoControl
        {
            get => _videoControl;
            set => SetProperty(ref _videoControl, value);
        }

        private bool _showSwitchDialog = false;
        public bool ShowSwitchDialog
        {
            get => _showSwitchDialog;
            set => SetProperty(ref _showSwitchDialog, value);
        }
        public bool IsPlaying => _videoPlayService.IsPlaying;
        public bool IsMuted => _videoPlayService.IsMuted;
        public double Volume => _videoPlayService.Volume;
        public double Position => _videoPlayService.Position;
        public double TotalTime => _videoPlayService.Total;
        public string? Cover => _videoPlayStoreService.Current?.Cover;

        private bool _showCover = true;
        public bool ShowCover
        {
            get => _showCover;
            set => SetProperty(ref _showCover, value);
        }

        public MvDetail? Detail => _videoPlayStoreService.Current;
        public MvUrl? MvUrl => _videoPlayStoreService.Url;
        public ObservableCollection<MvUrl> MvUrls => _videoPlayStoreService.MvUrls;
        public ObservableCollection<MovieVideo> MovieVideos => _videoPlayStoreService.Related;

        private DelegateCommand _playCommand = default!;
        public DelegateCommand PlayCommand =>
            _playCommand ??= new(_videoPlayService.Play);

        private DelegateCommand _pauseCommand = default!;
        public DelegateCommand PauseCommand =>
            _pauseCommand ??= new(_videoPlayService.Pause);

        private DelegateCommand<bool?> _setMutedCommand = default!;
        public DelegateCommand<bool?> SetMutedCommand =>
            _setMutedCommand ??= new(value =>
            {
                if (value.HasValue)
                {
                    _videoPlayService.SetMute(value.Value);
                }
            });

        private DelegateCommand<double?> _setPositionCommand = default!;
        public DelegateCommand<double?> SetPositionCommand =>
            _setPositionCommand ??= new((value) =>
            {
                _isDragProgress = false;
                if (value.HasValue)
                {
                    _videoPlayService.SetProgress(value.Value);
                    _videoPlayService.Play();
                }
            });

        private DelegateCommand<double?> _setVolumeCommand = default!;
        public DelegateCommand<double?> SetVolumeCommand =>
            _setVolumeCommand ??= new(val =>
            {
                if (val.HasValue)
                {
                    _videoPlayService.SetVolume(val.Value);
                }
            });

        private DelegateCommand<double?> _dragStartedCommand = default!;

        public DelegateCommand<double?> DragStartedCommand =>
            _dragStartedCommand ??= new(_ => _isDragProgress = true);

        private DelegateCommand _fullScreenCommand = default!;
        public DelegateCommand FullScreenCommand =>
            _fullScreenCommand ??= new(_videoPlayService.FullScreen);

        private DelegateCommand<MvUrl> _switchBrCommand = default!;
        public DelegateCommand<MvUrl> SwitchBrCommand =>
            _switchBrCommand ??= new((mvUrl) =>
            {
                if (MvUrl == mvUrl) return;
                _videoPlayStoreService.SetUrl(mvUrl);
                ShowSwitchDialog = false;
            });
        private DelegateCommand<MovieVideo> _playMvCommand = default!;
        public DelegateCommand<MovieVideo> PlayMvCommand =>
            _playMvCommand ?? (_playMvCommand = new DelegateCommand<MovieVideo>(async (mv) =>
            {
                if (mv != null && _mvId != mv.Id)
                {
                    _mvId = mv.Id;
                    PauseCommand.Execute();
                    SetPositionCommand.Execute(0d);
                    ShowCover = true;
                    await _videoPlayStoreService.SetMovieVideo(_mvId);
                }
            }));

        public MvViewModel(
            IContainerProvider containerProvider,
            IVideoPlayService videoPlayService, 
            IVideoPlayStoreService videoPlayStoreService)
            : base(containerProvider)
        {
            _videoPlayService = videoPlayService;
            _videoPlayStoreService = videoPlayStoreService;
            _videoPlayStoreService.CurrentChanged += (_, e) =>
            {
                RaisePropertyChanged(nameof(Detail));
                RaisePropertyChanged(nameof(Cover));
            };

            VideoControl = _videoPlayService.Control;
            _videoPlayService.IsPlayingChanged += (_, e) =>
            {
                RaisePropertyChanged(nameof(IsPlaying));
                if (e.NewValue && ShowCover) ShowCover = false;
            };
            _videoPlayService.VolumeChanged += (_, _) => RaisePropertyChanged(nameof(Volume));
            _videoPlayService.IsMutedChanged += (_, _) => RaisePropertyChanged(nameof(IsMuted)); 
            _videoPlayService.IsFullScreenChanged += (_, e) =>
            {
                VideoControl = e.NewValue ? null : _videoPlayService.Control;
            };
            _videoPlayService.ProgressChanged += (_, e) =>
            {
                if (!_isDragProgress)
                {
                    RaisePropertyChanged(nameof(Position));
                    RaisePropertyChanged(nameof(TotalTime));
                }
            };
        }

        public bool KeepAlive => true;

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            if (navigationContext.Parameters.ContainsKey(MvIdParameter))
            {
                _mvId = navigationContext.Parameters.GetValue<long>(MvIdParameter);

                await _videoPlayStoreService.SetMovieVideo(_mvId);
            }
        }
    }
}
