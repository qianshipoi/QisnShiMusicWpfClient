using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace QianShi.Music.Common.UserControls
{
    public class TimeChangedEventArgs : EventArgs
    {
        public TimeChangedEventArgs(double time)
        {
            Time = time;
        }

        public double Time { get; set; }
    }

    /// <summary>
    /// LrcView.xaml 的交互逻辑
    /// </summary>
    public partial class LrcView : UserControl
    {
        public Dictionary<double, LrcModel> Lrcs = new Dictionary<double, LrcModel>();
        //当前焦点所在歌词集合位置
        public int FoucsLrcLocation { get; set; } = -1;
        //添加当前焦点歌词变量
        public LrcModel foucslrc { get; set; }
        //非焦点歌词颜色
        public SolidColorBrush NoramlLrcColor = new SolidColorBrush(Colors.Black);
        //焦点歌词颜色
        public SolidColorBrush FoucsLrcColor = new SolidColorBrush(Colors.OrangeRed);
        DispatcherTimer _dt;
        private bool _rolling = true;
        public LrcView()
        {
            InitializeComponent();
            _dt = new DispatcherTimer();
            _dt.Interval = TimeSpan.FromSeconds(5);
            _dt.Tick += (s, e) =>
            {
                _rolling = true;
                ResetLrcviewScroll();
            };
        }

        public event EventHandler<TimeChangedEventArgs> TimeChangedEvent;

        public void LoadLrc(string lyricsString)
        {
            lyricsString.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(lyric =>
            {
                var regex = new Regex(@"^\[(\d{2}\:\d{2}\.\d{3})\](.+)$");
                var match = regex.Match(lyric);
                if (match.Success)
                {
                    var timeStr = match.Groups[1].ToString();
                    var lyricStr = match.Groups[2].ToString();
                    var time = TimeSpan.ParseExact(timeStr, @"mm\:ss\.fff", CultureInfo.CurrentCulture).TotalMilliseconds;

                    var textBlock = new TextBlock();
                    textBlock.Text = lyricStr;
                    textBlock.FontSize = 28;
                    textBlock.Opacity = .28;
                    textBlock.Margin = new Thickness(18);
                    textBlock.TextWrapping = TextWrapping.WrapWithOverflow;
                    textBlock.Cursor = Cursors.Hand;
                    textBlock.MouseLeftButtonUp += (s, e) =>
                    {
                        if (TimeChangedEvent != null)
                        {
                            var args = new TimeChangedEventArgs(time);
                            TimeChangedEvent(this, args);
                            _dt.Stop();
                            _rolling = true;
                            LrcRoll(time);
                        }
                    };
                    var l = new LrcModel
                    {
                        LrcTb = textBlock,
                        LrcText = lyricStr,
                        Time = time,
                    };
                    Lrcs.Add(l.Time, l);

                    LrcItemsControl.Children.Add(textBlock);
                }
            });
        }

        private void ResetLrcviewScroll()
        {
            //获得焦点歌词位置
            if (foucslrc == null) return;
            GeneralTransform gf = foucslrc.LrcTb.TransformToVisual(LrcItemsControl);
            if (gf != null) return;
            Point p = gf.Transform(new Point(0, 0));
            //滚动条当前位置
            Debug.WriteLine(ScrollViewerControl.VerticalOffset + "/" + p.Y);

            //计算滚动位置（p.Y是焦点歌词控件(c_LrcTb)相对于父级控件c_lrc_items(StackPanel)的位置）
            //拿焦点歌词位置减去滚动区域控件高度除以2的值得到的【大概】就是歌词焦点在滚动区域控件的位置
            double os = p.Y - (ScrollViewerControl.ActualHeight / 2) + 10 + 200;
            //滚动
            ScrollViewerControl.ScrollToVerticalOffset(os);
        }

        public void LrcRoll(double nowTime)
        {
            if (foucslrc == null)
            {
                foucslrc = Lrcs.Values.First();
                foucslrc.LrcTb.Foreground = FoucsLrcColor;
            }
            else
            {
                //查找焦点歌词
                var s = Lrcs.Where(m => nowTime >= m.Key);
                if (s.Count() > 0)
                {
                    LrcModel lm = s.Last().Value;
                    foucslrc.LrcTb.Foreground = NoramlLrcColor;

                    foucslrc = lm;
                    foucslrc.LrcTb.Foreground = FoucsLrcColor;
                    //定位歌词在控件中间区域
                    if (_rolling)
                        ResetLrcviewScroll();
                }
            }
        }

        private void LrcItemsControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _rolling = false;
            _dt.Stop();
            _dt.Start();
        }
    }

    public class LrcModel
    {
        /// <summary>
        /// 歌词所在控件
        /// </summary>
        public TextBlock LrcTb { get; set; }

        /// <summary>
        /// 歌词字符串
        /// </summary>
        public string LrcText { get; set; }

        /// <summary>
        /// 时间（读取格式参照网易云音乐歌词格式：xx:xx.xx，即分:秒.毫秒，秒小数点保留2位。如：00:28.64）
        /// </summary>
        public double Time { get; set; }
    }

}
