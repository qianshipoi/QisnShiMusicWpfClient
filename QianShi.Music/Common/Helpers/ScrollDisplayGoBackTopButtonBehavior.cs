using Microsoft.Xaml.Behaviors;

namespace QianShi.Music.Common.Helpers
{
    public class ScrollDisplayGoBackTopButtonBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty ElementProperty =
            DependencyProperty.Register(nameof(Element), typeof(object), typeof(ScrollDisplayGoBackTopButtonBehavior), null);

        public static readonly DependencyProperty ThresholdProperty =
            DependencyProperty.Register(nameof(Threshold), typeof(double), typeof(ScrollDisplayGoBackTopButtonBehavior), new PropertyMetadata(200d));

        public object Element
        {
            get { return (object)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        public double Threshold
        {
            get { return (double)GetValue(ThresholdProperty); }
            set { SetValue(ThresholdProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }

        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (Element is FrameworkElement framework)
            {

                if (e.VerticalOffset > Threshold)
                {
                    framework.Visibility = Visibility.Visible;
                }
                else
                {
                    framework.Visibility = Visibility.Collapsed;
                }
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
        }
    }
}
