using System.Windows;
using System.Windows.Controls;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// 水平等距面板
    /// </summary>
    public class IsometricPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var size = new Size();
            foreach (var temp in Children.Cast<UIElement>())
            {
                temp.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                size = new Size(size.Width + temp.DesiredSize.Width, Math.Max(size.Height, temp.DesiredSize.Height));
            }
            return size;
        }

        protected override Size ArrangeOverride(Size availableSize)
        {
            var size = availableSize;

            var width = size.Width / Children.Count;

            for (int i = 0; i < Children.Count; i++)
            {
                var temp = Children[i];
                temp.Arrange(new Rect(new Point(i * width, 0), new Size(width, size.Height)));
            }

            return size;
        }
    }
}