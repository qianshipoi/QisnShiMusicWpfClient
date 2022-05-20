using System.Collections.ObjectModel;

using Prism.Commands;
using Prism.Mvvm;

using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class VideoPlayWindowViewModel : BindableBase
    {
        private readonly IVideoPlayService _videoPlayService;
        private object? _videoControl;

        public object? VideoControl
        {
            get => _videoControl;
            set => SetProperty(ref _videoControl, value);
        }

        private Common.Models.MvUrl? _mvUrl;
        public Common.Models.MvUrl? MvUrl
        {
            get => _mvUrl;
            set
            {
                if (value == _mvUrl) return;
                if (null != _mvUrl) _mvUrl.IsActive = false;
                if (null != value)
                {
                    value.IsActive = true;
                    _videoPlayService.Url = value.Url;
                }
                SetProperty(ref _mvUrl, value);
            }
        }

        private bool _showSwitchDialog = false;
        public bool ShowSwitchDialog
        {
            get => _showSwitchDialog;
            set => SetProperty(ref _showSwitchDialog, value);
        }

        private ObservableCollection<Common.Models.MvUrl> _mvUrls = new();
        public ObservableCollection<Common.Models.MvUrl> MvUrls
        {
            get => _mvUrls;
            set => SetProperty(ref _mvUrls, value);
        }

        private DelegateCommand<Common.Models.MvUrl> _switchBrCommand = default!;
        public DelegateCommand<Common.Models.MvUrl> SwitchBrCommand =>
            _switchBrCommand ??= new((mvUrl) =>
            {
                if (MvUrl == mvUrl) return;
                PauseCommand.Execute();
                MvUrl = mvUrl;
                SetPositionCommand.Execute(0d);
                ShowSwitchDialog = false;
                PlayCommand.Execute();
            });

        private bool _isPlaying = false;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                SetProperty(ref _isPlaying, value);
                if (_isPlaying && ShowCover) ShowCover = false;
            }
        }

        private bool _isMuted = false;
        public bool IsMuted
        {
            get => _isMuted;
            set => SetProperty(ref _isMuted, value);
        }

        private double _volume = 0.5d;
        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
        }

        private double _position = 0d;
        public double Position
        {
            get => _position;
            set => SetProperty(ref _position, value);
        }

        private double _totalTime = 0d;
        public double TotalTime
        {
            get => _totalTime;
            set => SetProperty(ref _totalTime, value);
        }

        private string? _cover;
        public string? Cover
        {
            get => _cover;
            set => SetProperty(ref _cover, value);
        }

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
            _dragStartedCommand ??= new(_ => _videoPlayService.Pause());

        private DelegateCommand _fullScreenCommand = default!;
        public DelegateCommand FullScreenCommand =>
            _fullScreenCommand ??= new(_videoPlayService.FullScreen);

        private bool _showCover = true;
        public bool ShowCover
        {
            get => _showCover;
            set => SetProperty(ref _showCover, value);
        }

        public VideoPlayWindowViewModel(IVideoPlayService videoPlayService)
        {
            _videoPlayService = videoPlayService;
            Volume = _videoPlayService.Volume;
            IsPlaying = _videoPlayService.IsPlaying;
            IsMuted = _videoPlayService.IsMuted;
            Cover = _videoPlayService.Cover;
            VideoControl = _videoPlayService.Control;
            TotalTime = _videoPlayService.Total;
            Position = _videoPlayService.Position;
            _videoPlayService.IsFullScreenChanged += (_, e) =>
            {
                VideoControl = e.NewValue ? _videoPlayService.Control : null;
            };
            _videoPlayService.IsPlayingChanged += (_, e) => IsPlaying = e.NewValue;
            _videoPlayService.VolumeChanged += (_, e) => Volume = e.NewValue;
            _videoPlayService.IsMutedChanged += (_, e) => IsMuted = e.NewValue;
            _videoPlayService.CoverChanged += (_, e) => Cover = e.NewValue;
            _videoPlayService.ProgressChanged += (_, e) =>
            {
                Position = e.Value;
                TotalTime = e.Total;
            };
        }
    }
}
