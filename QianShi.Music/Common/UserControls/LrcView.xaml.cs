using System.Diagnostics;
using System.Text.RegularExpressions;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// LrcView.xaml 的交互逻辑
    /// </summary>
    public partial class LrcView : UserControl
    {
        public Dictionary<double, LrcModel> Lrcs = new Dictionary<double, LrcModel>();

        //当前焦点所在歌词集合位置
        public int FoucsLrcLocation { get; set; } = -1;

        //添加当前焦点歌词变量
        public LrcModel? foucslrc { get; set; }

        //非焦点歌词颜色
        public SolidColorBrush NoramlLrcColor = new SolidColorBrush(Colors.Black);

        //焦点歌词颜色
        public SolidColorBrush FoucsLrcColor = new SolidColorBrush(Colors.Red);

        public string LyricString
        {
            get { return (string)GetValue(LyricStringProperty); }
            set { SetValue(LyricStringProperty, value); }
        }

        public static readonly DependencyProperty LyricStringProperty =
            DependencyProperty.Register(nameof(LyricString), typeof(string), typeof(LrcView), new PropertyMetadata(string.Empty, LyrucStringChanged));

        private static void LyrucStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LrcView view)
            {
                view.LoadLrc(e.NewValue.ToString() ?? string.Empty);
                view.ScrollViewerControl.ScrollToVerticalOffset(0);
            }
        }

        public double Position
        {
            get { return (double)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(double), typeof(LrcView), new PropertyMetadata(0d, (d, e) =>
            {
                if (d is LrcView view)
                {
                    view.LrcRoll((double)e.NewValue);
                }
            }));

        public ICommand SetPositionCommand
        {
            get { return (ICommand)GetValue(SetPositionCommandProperty); }
            set { SetValue(SetPositionCommandProperty, value); }
        }

        public static readonly DependencyProperty SetPositionCommandProperty =
            DependencyProperty.Register(nameof(SetPositionCommand), typeof(ICommand), typeof(LrcView), new PropertyMetadata(null));

        private DispatcherTimer _dt;
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

        public void LoadLrc(string lyricsString)
        {
            void AddLyricControl(Match match, string format = @"mm\:ss\.fff")
            {
                var timeStr = match.Groups[1].ToString();
                var lyricStr = match.Groups[2].ToString();

                var textBlock = new TextBlock();
                textBlock.Text = lyricStr;
                textBlock.FontSize = 28;
                textBlock.Opacity = .28;
                textBlock.Margin = new Thickness(18);
                textBlock.TextWrapping = TextWrapping.WrapWithOverflow;

                double time = 0d;
                if (timeStr.StartsWith("99:"))
                {
                    time = double.MaxValue;
                }
                else
                {
                    time = TimeSpan.ParseExact(timeStr, format, CultureInfo.CurrentCulture).TotalMilliseconds;
                    textBlock.Cursor = Cursors.Hand;
                    textBlock.MouseLeftButtonUp += LrcClick;
                }

                var l = new LrcModel
                {
                    LrcTb = textBlock,
                    LrcText = lyricStr,
                    Time = time,
                };
                if (Lrcs.ContainsKey(l.Time))
                {
                    Lrcs[l.Time].LrcText += "\\n" + lyricsString;
                }
                else
                {
                    Lrcs.Add(l.Time, l);
                    LrcItemsControl.Children.Add(textBlock);
                }
            }
            foucslrc = null;
            Lrcs.Clear();
            LrcItemsControl.Children.Clear();
            foucslrc = null;
            lyricsString.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(lyric =>
            {
                var regex = new Regex(@"^\[(\d{2}\:\d{2}\.\d{3})\](.+)$");
                var match = regex.Match(lyric);
                if (match.Success)
                {
                    AddLyricControl(match);
                }
                var regex2 = new Regex(@"^\[(\d{2}\:\d{2}\.\d{2})\](.+)$");
                var match2 = regex2.Match(lyric);
                if (match2.Success)
                {
                    AddLyricControl(match2, @"mm\:ss\.ff");
                }
            });

            if (Lrcs.Count == 0)
            {
                var textBlock = new TextBlock();
                textBlock.Text = lyricsString;
                textBlock.FontSize = 28;
                textBlock.Opacity = .28;
                textBlock.Margin = new Thickness(18);
                textBlock.TextWrapping = TextWrapping.WrapWithOverflow;
                var l = new LrcModel
                {
                    LrcTb = textBlock,
                    LrcText = lyricsString,
                    Time = double.MaxValue,
                };
                Lrcs.Add(l.Time, l);
                LrcItemsControl.Children.Add(textBlock);
            }
        }

        private void LrcClick(object s, MouseButtonEventArgs e)
        {
            if (s is TextBlock textControl)
            {
                var lrcModel = Lrcs.Where(x => x.Value.LrcTb == textControl).Select(x => x.Value).FirstOrDefault();
                if (lrcModel != null)
                {
                    if (null != SetPositionCommand && SetPositionCommand.CanExecute(lrcModel.Time))
                    {
                        SetPositionCommand.Execute(lrcModel.Time);
                        LrcRoll(lrcModel.Time);
                    }
                }
            }
        }

        private void ResetLrcviewScroll()
        {
            //获得焦点歌词位置
            if (foucslrc == null) return;
            GeneralTransform gf = foucslrc.LrcTb.TransformToVisual(LrcItemsControl);
            if (gf == null) return;
            Point p = gf.Transform(new Point(0, 0));
            //滚动条当前位置
            //Debug.WriteLine(ScrollViewerControl.VerticalOffset + "/" + p.Y);

            //计算滚动位置（p.Y是焦点歌词控件(c_LrcTb)相对于父级控件c_lrc_items(StackPanel)的位置）
            //拿焦点歌词位置减去滚动区域控件高度除以2的值得到的【大概】就是歌词焦点在滚动区域控件的位置
            double os = p.Y - (ScrollViewerControl.ActualHeight / 2) + 10 + 200;
            //滚动
            ScrollViewerControl.ScrollToVerticalOffset(os);
        }

        public void LrcRoll(double nowTime)
        {
            if (Lrcs.Values.Count == 0) return;
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
        public TextBlock LrcTb { get; set; } = default!;

        /// <summary>
        /// 歌词字符串
        /// </summary>
        public string LrcText { get; set; } = String.Empty;

        /// <summary>
        /// 时间（读取格式参照网易云音乐歌词格式：xx:xx.xx，即分:秒.毫秒，秒小数点保留2位。如：00:28.64）
        /// </summary>
        public double Time { get; set; }
    }
}