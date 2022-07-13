using Microsoft.Xaml.Behaviors;

using System.Diagnostics;

namespace QianShi.Music.Common.Helpers
{
    public class ScrollToEndCallCommandBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ScrollToEndCallCommandBehavior), null);
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(ScrollToEndCallCommandBehavior), null);
        public static readonly DependencyProperty ToBottomProperty = DependencyProperty.Register(nameof(ToBottom), typeof(double), typeof(ScrollToEndCallCommandBehavior), null);

        public double ToBottom
        {
            get => (double)GetValue(ToBottomProperty);
            set => SetValue(ToBottomProperty, value);
        }
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
            AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
        }

        private void AssociatedObject_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer && scrollViewer.Content is FrameworkElement frameworkElement && e.VerticalChange > 0)
            {
                if (frameworkElement.ActualHeight - scrollViewer.ActualHeight - e.VerticalOffset <= ToBottom)
                {
                    if (Command?.CanExecute(CommandParameter) ?? false)
                    {
                        Command.Execute(CommandParameter);
                    }
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
