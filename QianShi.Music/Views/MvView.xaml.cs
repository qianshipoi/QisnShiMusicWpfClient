using System.Windows.Controls;

namespace QianShi.Music.Views
{
    /// <summary>
    /// MvView.xaml 的交互逻辑
    /// </summary>
    public partial class MvView
    {
        public MvView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (SettingControl.IsChecked ?? false)
                SettingPopup.IsOpen = false;
        }
    }
}
