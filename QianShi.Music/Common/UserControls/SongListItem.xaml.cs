using QianShi.Music.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// SongListItem.xaml 的交互逻辑
    /// </summary>
    public partial class SongListItem : UserControl
    {
        public ICommand PlayImmediatelyCommand
        {
            get { return (ICommand)GetValue(PlayImmediatelyCommandProperty); }
            set { SetValue(PlayImmediatelyCommandProperty, value); }
        }

        public static readonly DependencyProperty PlayImmediatelyCommandProperty =
            DependencyProperty.Register(nameof(PlayImmediatelyCommand), typeof(ICommand), typeof(SongListItem), new PropertyMetadata(null));

        public SongBindable Item
        {
            get { return (SongBindable)GetValue(SongBindableProperty); }
            set { SetValue(SongBindableProperty, value); }
        }

        public static readonly DependencyProperty SongBindableProperty =
            DependencyProperty.Register(nameof(Item), typeof(SongBindable), typeof(SongListItem), new PropertyMetadata(new SongBindable()));

        public SongListItem()
        {
            InitializeComponent();
        }
    }
}