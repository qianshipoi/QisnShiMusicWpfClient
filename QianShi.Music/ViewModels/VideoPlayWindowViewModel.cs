using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;
using MvUrl = QianShi.Music.Common.Models.MvUrl;

namespace QianShi.Music.ViewModels
{
    public class VideoPlayWindowViewModel : BindableBase
    {
        private readonly IVideoPlayService _videoPlayService;
        private readonly IVideoPlayStoreService _videoPlayStoreService;
        private object? _videoControl;
        private bool _isDragProgress = false;

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

        public MvDetail? Detail => _videoPlayStoreService.Current;
        public MvUrl? MvUrl => _videoPlayStoreService.Url;
        public ObservableCollection<MvUrl> MvUrls => _videoPlayStoreService.MvUrls;
        public ObservableCollection<MovieVideo> MovieVideos => _videoPlayStoreService.Related;


        private DelegateCommand<MvUrl> _switchBrCommand = default!;
        public DelegateCommand<MvUrl> SwitchBrCommand =>
            _switchBrCommand ??= new((mvUrl) =>
            {
                if (MvUrl == mvUrl) return;
                _videoPlayStoreService.SetUrl(mvUrl);
                ShowSwitchDialog = false;
            });

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

        public VideoPlayWindowViewModel(IVideoPlayService videoPlayService, IVideoPlayStoreService videoPlayStoreService)
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
            };
            _videoPlayService.VolumeChanged += (_, _) => RaisePropertyChanged(nameof(Volume));
            _videoPlayService.IsMutedChanged += (_, _) => RaisePropertyChanged(nameof(IsMuted));
            _videoPlayService.IsFullScreenChanged += (_, e) =>
            {
                VideoControl = e.NewValue ? _videoPlayService.Control : null;
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
    }
}
