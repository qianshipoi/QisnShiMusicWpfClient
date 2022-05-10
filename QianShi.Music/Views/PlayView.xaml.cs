using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace QianShi.Music.Views
{
    /// <summary>
    /// PlayView.xaml 的交互逻辑
    /// </summary>
    public partial class PlayView : UserControl
    {
        private Window _targetWindow = null!;
        private DispatcherTimer _dt = null!;
        private bool _playing = false;
        private bool _canChangProgressControlValue = true;

        public PlayView()
        {
            InitializeComponent();
            Loaded += PlayView_Loaded;
        }

        public void Init(string url, string lyricsString)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }
            LrcView.LoadLrc(lyricsString);
            LrcView.TimeChangedEvent += (s, e) =>
            {
                me.Position = TimeSpan.FromMilliseconds(e.Time);
                LrcRoll();
            };

            _dt = new DispatcherTimer();
            _dt.Interval = TimeSpan.FromSeconds(0.5);
            _dt.Tick += (e, c) => LrcRoll();
            me.MediaOpened += (e, c) =>
            {
                PlayProgressControl.Maximum = me.NaturalDuration.TimeSpan.TotalMilliseconds;
                CurrentTimeControl.Text = "00:00";
                TotalTimeControl.Text = me.NaturalDuration.TimeSpan.ToString(@"mm\:ss");
                me.LoadedBehavior = MediaState.Manual;
            };
            me.MediaFailed += (s, e) =>
            {

            };
            me.Source = new Uri(url);
            me.LoadedBehavior = MediaState.Pause;
            PlayProgressControl.PreviewMouseLeftButtonDown += (e, c) =>
            {
                _canChangProgressControlValue = false;
            };
            PlayProgressControl.PreviewMouseLeftButtonUp += (e, c) =>
            {
                me.Position = TimeSpan.FromMilliseconds(PlayProgressControl.Value);
                LrcRoll();
                _canChangProgressControlValue = true;
            };
            PlayProgressControl.ValueChanged += (e, c) =>
            {
                CurrentTimeControl.Text = me.Position.ToString(@"mm\:ss");
            };
        }

        private void LrcRoll()
        {
            CurrentTimeControl.Text = me.Position.ToString(@"mm\:ss");
            if (_canChangProgressControlValue)
                PlayProgressControl.Value = me.Position.TotalMilliseconds;
            LrcView.LrcRoll(me.Position.TotalMilliseconds);
        }

        private void PlayView_Loaded(object sender, RoutedEventArgs e)
        {
            _targetWindow = Window.GetWindow(this);
            Margin = new Thickness(0, _targetWindow.Height, 0, 0);
        }

        private void PlayView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _targetWindow!.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_playing)
            {
                _dt.Stop();
                me.Pause();
            }
            else
            {
                _dt.Start();
                me.Play();
            }
        }
    }
}