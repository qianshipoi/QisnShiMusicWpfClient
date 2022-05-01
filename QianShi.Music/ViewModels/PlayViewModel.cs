using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Extensions;
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
            IRegionManager regionManager) : base(provider)
        {
            _lyrics = new ObservableCollection<Lyric>();
            _containerProvider = provider;
            _regionManager = regionManager;
            CloseCommand = new DelegateCommand(() => Display = false);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var region = _regionManager.Regions[PrismManager.FullScreenRegionName];
            if (region != null)
            {
                var view = region.Views.FirstOrDefault(x => x.GetType() == typeof(PlayView));
                if (view != null)
                {
                    _playView = view as PlayView;
                    Init();
                }
            }
        }

        private void Init()
        {
            if (_playView == null) return;
            var url = "http://m8.music.126.net/20220415220645/3f0cc86720ed8a3c5d4ecbd64b3231da/ymusic/0fd6/4f65/43ed/a8772889f38dfcb91c04da915b301617.mp3";
            var lyricsString = "[00:04.050]\n[00:12.570]难以忘记初次见你\n[00:16.860]一双迷人的眼睛\n[00:21.460]在我脑海里\n[00:23.960]你的身影 挥散不去\n[00:30.160]握你的双手感觉你的温柔\n[00:34.940]真的有点透不过气\n[00:39.680]你的天真 我想珍惜\n[00:43.880]看到你受委屈 我会伤心\n[00:48.180]喔\n[00:50.340]只怕我自己会爱上你\n[00:55.070]不敢让自己靠的太近\n[00:59.550]怕我没什么能够给你\n[01:04.030]爱你也需要很大的勇气\n[01:08.190]只怕我自己会爱上你\n[01:12.910]也许有天会情不自禁\n[01:17.380]想念只让自己苦了自己\n[01:21.840]爱上你是我情非得已\n[01:28.810]难以忘记初次见你\n[01:33.170]一双迷人的眼睛\n[01:37.700]在我脑海里 你的身影 挥散不去\n[01:46.360]握你的双手感觉你的温柔\n[01:51.120]真的有点透不过气\n[01:55.910]你的天真 我想珍惜\n[02:00.150]看到你受委屈 我会伤心\n[02:04.490]喔\n[02:06.540]只怕我自己会爱上你\n[02:11.240]不敢让自己靠的太近\n[02:15.750]怕我没什么能够给你\n[02:20.200]爱你也需要很大的勇气\n[02:24.570]只怕我自己会爱上你\n[02:29.230]也许有天会情不自禁\n[02:33.680]想念只让自己苦了自己\n[02:38.140]爱上你是我情非得已\n[03:04.060]什么原因 耶\n[03:07.730]我竟然又会遇见你\n[03:13.020]我真的真的不愿意\n[03:16.630]就这样陷入爱的陷阱\n[03:20.700]喔\n[03:22.910]只怕我自己会爱上你\n[03:27.570]不敢让自己靠的太近\n[03:32.040]怕我没什么能够给你\n[03:36.560]爱你也需要很大的勇气\n[03:40.740]只怕我自己会爱上你\n[03:45.460]也许有天会情不自禁\n[03:49.990]想念只让自己苦了自己\n[03:54.510]爱上你是我情非得已\n[03:58.970]爱上你是我情非得已\n[04:03.000]\n";

            _playView.Init(url, lyricsString);
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