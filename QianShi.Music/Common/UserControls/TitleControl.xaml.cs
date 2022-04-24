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
    /// TitleControl.xaml 的交互逻辑
    /// </summary>
    public partial class TitleControl : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(TitleControl), new PropertyMetadata(string.Empty));

        public ICommand LookAllCammand
        {
            get { return (ICommand)GetValue(LookAllCammandProperty); }
            set { SetValue(LookAllCammandProperty, value); }
        }

        public static readonly DependencyProperty LookAllCammandProperty =
            DependencyProperty.Register(nameof(LookAllCammand), typeof(ICommand), typeof(TitleControl), new PropertyMetadata(null));

        public object LookAllCommandParameter
        {
            get { return (object)GetValue(LookAllCommandParameterProperty); }
            set { SetValue(LookAllCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty LookAllCommandParameterProperty =
            DependencyProperty.Register(nameof(LookAllCommandParameter), typeof(object), typeof(TitleControl), new PropertyMetadata(null));

        public bool ShowLookAll
        {
            get { return (bool)GetValue(ShowLookAllProperty); }
            set { SetValue(ShowLookAllProperty, value); }
        }

        public static readonly DependencyProperty ShowLookAllProperty =
            DependencyProperty.Register(nameof(ShowLookAll), typeof(bool), typeof(TitleControl), new PropertyMetadata(false));

        public TitleControl()
        {
            InitializeComponent();
        }
    }
}
