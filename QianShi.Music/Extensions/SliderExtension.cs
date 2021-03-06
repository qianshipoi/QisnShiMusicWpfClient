namespace QianShi.Music.Extensions
{
    internal class SliderExtension
    {
        public static readonly DependencyProperty DragCompletedCommandProperty =
            DependencyProperty.RegisterAttached("DragCompletedCommand", typeof(ICommand), typeof(SliderExtension), new PropertyMetadata(default(ICommand), OnDragCompletedCommandChanged));

        public static readonly DependencyProperty DragStartedCommandProperty =
            DependencyProperty.RegisterAttached("DragStartedCommand", typeof(ICommand), typeof(SliderExtension), new PropertyMetadata(default(ICommand), OnDragStartedCommandChanged));

        public static readonly DependencyProperty MouseLeftButtonUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseLeftButtonUpCommand", typeof(ICommand), typeof(SliderExtension), new PropertyMetadata(default(ICommand), OnMouseLeftButtonUpCommandChanged));

        private static void OnDragStartedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && e.NewValue is ICommand command)
            {
                slider.Loaded += SliderOnLoaded;
            }
        }

        private static void OnDragCompletedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && e.NewValue is ICommand command)
            {
                slider.Loaded += SliderOnLoaded;
            }
        }

        private static void OnMouseLeftButtonUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Slider slider && e.NewValue is ICommand command)
            {
                slider.Loaded += SliderOnLoaded;
            }
        }

        private static void SliderOnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                slider.Loaded -= SliderOnLoaded;
                if (slider.Template.FindName("PART_Track", slider) is Track track)
                {
                    var dragCompletedCommand = GetDragCompletedCommand(slider);
                    if (dragCompletedCommand != null)
                    {
                        track.Thumb.DragCompleted += (dragCompletedSender, dragCompletedArgs) =>
                        {
                            dragCompletedCommand.Execute(track.Value);
                        };
                    }
                    var dragStartedCommand = GetDragStartedCommand(slider);
                    if (null != dragStartedCommand)
                    {
                        track.Thumb.DragStarted += (dragStartedSender, dragStartedArgs) =>
                        {
                            dragStartedCommand.Execute(track.Value);
                        };
                    }
                    var mouseLeftButtonUpCommand = GetMouseLeftButtonUpCommand(slider);
                    if (null != mouseLeftButtonUpCommand)
                    {
                        track.MouseLeftButtonUp += (mouseLeftButtonUpSender, mouseLeftButtonUpArgs) =>
                        {
                            mouseLeftButtonUpCommand.Execute(track.Value);
                        };
                    }
                }
            }
        }

        public static void SetDragCompletedCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(DragCompletedCommandProperty, value);
        }

        public static ICommand GetDragCompletedCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(DragCompletedCommandProperty);
        }

        public static ICommand GetDragStartedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(DragStartedCommandProperty);
        }

        public static void SetDragStartedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(DragStartedCommandProperty, value);
        }

        public static ICommand GetMouseLeftButtonUpCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(MouseLeftButtonUpCommandProperty);
        }

        public static void SetMouseLeftButtonUpCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(MouseLeftButtonUpCommandProperty, value);
        }
    }
}