using QianShi.Music.Common.Models;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// VideoPlay.xaml 的交互逻辑
    /// </summary>
    public partial class VideoPlay : UserControl
    {
        public static readonly DependencyProperty PlayControlProperty =
            DependencyProperty.Register(nameof(PlayControl), typeof(FrameworkElement), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty MvUrlsProperty =
            DependencyProperty.Register(nameof(MvUrls), typeof(ObservableCollection<MvUrl>), typeof(VideoPlay), new PropertyMetadata(default(ObservableCollection<MvUrl>)));

        public static readonly DependencyProperty CoverProperty =
            DependencyProperty.Register(nameof(Cover), typeof(string), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowCoverProperty =
            DependencyProperty.Register(nameof(ShowCover), typeof(bool), typeof(VideoPlay), new PropertyMetadata(false));

        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register(nameof(IsPlaying), typeof(bool), typeof(VideoPlay), new PropertyMetadata(false));

        public static readonly DependencyProperty IsMutedProperty =
            DependencyProperty.Register(nameof(IsMuted), typeof(bool), typeof(VideoPlay), new PropertyMetadata(false));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(double), typeof(VideoPlay), new PropertyMetadata(0d));

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(double), typeof(VideoPlay), new PropertyMetadata(0d));

        public static readonly DependencyProperty VolumeProperty =
            DependencyProperty.Register(nameof(Volume), typeof(double), typeof(VideoPlay), new PropertyMetadata(0d));

        public static readonly DependencyProperty ShowSwitchDialogProperty =
            DependencyProperty.Register(nameof(ShowSwitchDialog), typeof(bool), typeof(VideoPlay), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SwitchBrCommandProperty =
            DependencyProperty.Register(nameof(SwitchBrCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty PlayCommandProperty =
            DependencyProperty.Register(nameof(PlayCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty PauseCommandProperty =
            DependencyProperty.Register(nameof(PauseCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty SetMutedCommandProperty =
            DependencyProperty.Register(nameof(SetMutedCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty SetPositionCommandProperty =
            DependencyProperty.Register(nameof(SetPositionCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty DragStartedCommandProperty =
            DependencyProperty.Register(nameof(DragStartedCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty SetVolumeCommandProperty =
            DependencyProperty.Register(nameof(SetVolumeCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(null));

        public static readonly DependencyProperty FullScreenCommandProperty =
            DependencyProperty.Register(nameof(FullScreenCommand), typeof(ICommand), typeof(VideoPlay), new PropertyMetadata(default));

        public FrameworkElement? PlayControl
        {
            get => (FrameworkElement?)GetValue(PlayControlProperty);
            set => SetValue(PlayControlProperty, value);
        }

        public string Cover
        {
            get => (string)GetValue(CoverProperty);
            set => SetValue(CoverProperty, value);
        }

        public bool ShowCover
        {
            get => (bool)GetValue(ShowCoverProperty);
            set => SetValue(ShowCoverProperty, value);
        }

        public bool IsPlaying
        {
            get => (bool)GetValue(IsPlayingProperty);
            set => SetValue(IsPlayingProperty, value);
        }

        public bool IsMuted
        {
            get => (bool)GetValue(IsMutedProperty);
            set => SetValue(IsMutedProperty, value);
        }

        public ObservableCollection<MvUrl> MvUrls
        {
            get => (ObservableCollection<MvUrl>)GetValue(MvUrlsProperty);
            set => SetValue(MvUrlsProperty, value);
        }

        public double Position
        {
            get => (double)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public double Duration
        {
            get => (double)GetValue(DurationProperty);
            set => SetValue(DurationProperty, value);
        }

        public double Volume
        {
            get => (double)GetValue(VolumeProperty);
            set => SetValue(VolumeProperty, value);
        }

        public bool ShowSwitchDialog
        {
            get => (bool)GetValue(ShowSwitchDialogProperty);
            set => SetValue(ShowSwitchDialogProperty, value);
        }

        public ICommand SwitchBrCommand
        {
            get => (ICommand)GetValue(SwitchBrCommandProperty);
            set => SetValue(SwitchBrCommandProperty, value);
        }

        public ICommand PlayCommand
        {
            get => (ICommand)GetValue(PlayCommandProperty);
            set => SetValue(PlayCommandProperty, value);
        }

        public ICommand PauseCommand
        {
            get => (ICommand)GetValue(PauseCommandProperty);
            set => SetValue(PauseCommandProperty, value);
        }

        public ICommand SetMutedCommand
        {
            get => (ICommand)GetValue(SetMutedCommandProperty);
            set => SetValue(SetMutedCommandProperty, value);
        }

        public ICommand SetPositionCommand
        {
            get => (ICommand)GetValue(SetPositionCommandProperty);
            set => SetValue(SetPositionCommandProperty, value);
        }

        public ICommand DragStartedCommand
        {
            get => (ICommand)GetValue(DragStartedCommandProperty);
            set => SetValue(DragStartedCommandProperty, value);
        }

        public ICommand SetVolumeCommand
        {
            get => (ICommand)GetValue(SetVolumeCommandProperty);
            set => SetValue(SetVolumeCommandProperty, value);
        }

        public ICommand FullScreenCommand
        {
            get => (ICommand)GetValue(FullScreenCommandProperty);
            set => SetValue(FullScreenCommandProperty, value);
        }

        public VideoPlay()
        {
            InitializeComponent();
        }
    }
}