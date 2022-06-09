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
        public const string MvIdParameter = nameof(MvIdParameter);

        private readonly IVideoPlayService _videoPlayService;
        private readonly IVideoPlayStoreService _videoPlayStoreService;
        private DelegateCommand<double?> _dragStartedCommand = default!;
        private DelegateCommand _fullScreenCommand = default!;
        private bool _isDragProgress = false;
        private long _mvId;
        private DelegateCommand _pauseCommand = default!;
        private DelegateCommand _playCommand = default!;
        private DelegateCommand<MovieVideo> _playMvCommand = default!;
        private DelegateCommand<bool?> _setMutedCommand = default!;
        private DelegateCommand<double?> _setPositionCommand = default!;
        private DelegateCommand<double?> _setVolumeCommand = default!;
        private bool _showCover = true;
        private bool _showSwitchDialog = false;
        private DelegateCommand<MvUrl> _switchBrCommand = default!;
        private object? _videoControl;

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

        public string? Cover => _videoPlayStoreService.Current?.Cover;

        public MvDetail? Detail => _videoPlayStoreService.Current;

        public DelegateCommand<double?> DragStartedCommand =>
            _dragStartedCommand ??= new(_ => _isDragProgress = true);

        public DelegateCommand FullScreenCommand =>
            _fullScreenCommand ??= new(_videoPlayService.FullScreen);

        public bool IsMuted => _videoPlayService.IsMuted;

        public bool IsPlaying => _videoPlayService.IsPlaying;

        public bool KeepAlive => true;

        public ObservableCollection<MovieVideo> MovieVideos => _videoPlayStoreService.Related;

        public MvUrl? MvUrl => _videoPlayStoreService.Url;

        public ObservableCollection<MvUrl> MvUrls => _videoPlayStoreService.MvUrls;

        public DelegateCommand PauseCommand =>
            _pauseCommand ??= new(_videoPlayService.Pause);

        public DelegateCommand PlayCommand =>
            _playCommand ??= new(_videoPlayService.Play);

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

        public double Position => _videoPlayService.Position;

        public DelegateCommand<bool?> SetMutedCommand =>
            _setMutedCommand ??= new(value =>
            {
                if (value.HasValue)
                {
                    _videoPlayService.SetMute(value.Value);
                }
            });

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

        public DelegateCommand<double?> SetVolumeCommand =>
            _setVolumeCommand ??= new(val =>
            {
                if (val.HasValue)
                {
                    _videoPlayService.SetVolume(val.Value);
                }
            });

        public bool ShowCover
        {
            get => _showCover;
            set => SetProperty(ref _showCover, value);
        }

        public bool ShowSwitchDialog
        {
            get => _showSwitchDialog;
            set => SetProperty(ref _showSwitchDialog, value);
        }

        public DelegateCommand<MvUrl> SwitchBrCommand =>
            _switchBrCommand ??= new((mvUrl) =>
            {
                if (MvUrl == mvUrl) return;
                _videoPlayStoreService.SetUrl(mvUrl);
                ShowSwitchDialog = false;
            });

        public double TotalTime => _videoPlayService.Total;

        public object? VideoControl
        {
            get => _videoControl;
            set => SetProperty(ref _videoControl, value);
        }
        public double Volume => _videoPlayService.Volume;
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