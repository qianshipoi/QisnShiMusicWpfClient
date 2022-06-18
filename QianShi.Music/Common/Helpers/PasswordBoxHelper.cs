namespace QianShi.Music.Common.Helpers
{
    public static class PasswordBoxHelper
    {
        public static string GetPasswordContent(DependencyObject obj) => (string)obj.GetValue(PasswordContentProperty);

        public static void SetPasswordContent(DependencyObject obj, string value) => obj.SetValue(PasswordContentProperty, value);

        public static readonly DependencyProperty PasswordContentProperty =
            DependencyProperty.RegisterAttached("PasswordContent", typeof(string), typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnPasswordContentPropertyChanged));

        private static void OnPasswordContentPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox box)
            {
                box.PasswordChanged -= OnPasswordChanged;
                var password = (string)e.NewValue;
                if (box != null && box.Password != password)
                    box.Password = password;
                box!.PasswordChanged += OnPasswordChanged;
            }
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                SetPasswordContent(box, box.Password);
            }
        }
    }
}