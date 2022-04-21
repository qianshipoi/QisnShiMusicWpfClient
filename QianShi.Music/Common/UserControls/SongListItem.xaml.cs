using QianShi.Music.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
