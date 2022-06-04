using QianShi.Music.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// SongListItem.xaml 的交互逻辑
    /// </summary>
    public partial class SongListItem : UserControl
    {
        public ICommand PlayImmediatelyCommand
        {
            get => (ICommand)GetValue(PlayImmediatelyCommandProperty);
            set => SetValue(PlayImmediatelyCommandProperty, value);
        }

        public static readonly DependencyProperty PlayImmediatelyCommandProperty =
            DependencyProperty.Register(nameof(PlayImmediatelyCommand), typeof(ICommand), typeof(SongListItem), new PropertyMetadata(null));

        public Song Item
        {
            get => (Song)GetValue(SongProperty);
            set => SetValue(SongProperty, value);
        }

        public static readonly DependencyProperty SongProperty =
            DependencyProperty.Register(nameof(Item), typeof(Song), typeof(SongListItem), new PropertyMetadata(default));

        public SongListItem()
        {
            InitializeComponent();
        }
    }
}