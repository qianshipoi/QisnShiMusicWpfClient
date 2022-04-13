using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QianShi.Music.Common.UserControls
{
    /// <summary>
    /// MusicCard.xaml 的交互逻辑
    /// </summary>
    public partial class MusicCard : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(MusicCard));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register(nameof(Description), typeof(string), typeof(MusicCard), new PropertyMetadata(""));

        public string Cover
        {
            get { return (string)GetValue(CoverProperty); }
            set { SetValue(CoverProperty, value); }
        }

        public static readonly DependencyProperty CoverProperty =
            DependencyProperty.Register(nameof(Cover), typeof(string), typeof(MusicCard), new PropertyMetadata(""));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(MusicCard), new PropertyMetadata(null));

        public MusicCard()
        {
            InitializeComponent();
        }
    }
}
