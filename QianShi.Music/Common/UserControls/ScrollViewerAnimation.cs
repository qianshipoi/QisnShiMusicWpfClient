using System.Windows.Media.Animation;

namespace QianShi.Music.Common.UserControls
{
    public class ScrollViewerAnimation : ScrollViewer
    {
        /// <summary> 记录上一次的滚动位置 </summary>
        private double LastLocation = 0;
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            double WheelChange = e.Delta;
            //可以更改一次滚动的距离倍数 (WheelChange可能为正负数!)
            double newOffset = LastLocation - (WheelChange * 2);
            //Animation并不会改变真正的VerticalOffset(只是它的依赖属性) 所以将VOffset设置到上一次的滚动位置 (相当于衔接上一个动画)
            ScrollToVerticalOffset(LastLocation);
            //碰到底部和顶部时的处理
            if (newOffset < 0)
                newOffset = 0;
            if (newOffset > ScrollableHeight)
                newOffset = ScrollableHeight;

            AnimateScroll(newOffset);
            LastLocation = newOffset;
            //告诉ScrollViewer我们已经完成了滚动
            e.Handled = true;
        }

        private void AnimateScroll(double ToValue)
        {
            //为了避免重复，先结束掉上一个动画
            BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, null);
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Animation.From = VerticalOffset;
            Animation.To = ToValue;
            //动画速度
            Animation.Duration = TimeSpan.FromMilliseconds(800);
            //考虑到性能，可以降低动画帧数
            //Timeline.SetDesiredFrameRate(Animation, 40);
            BeginAnimation(ScrollViewerBehavior.VerticalOffsetProperty, Animation);
        }

        /// <summary> 回到顶部 </summary>
        public void GoBackTop()
        {
            AnimateScroll(0);
            LastLocation = 0;
        }
    }
}