using Prism.Commands;
using Prism.Regions;

using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;

using System.Windows;
using System.Windows.Input;

namespace QianShi.Music.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isFullsScreen = false;
        private double _width;
        private double _height;
        private double _top;
        private double _left;


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                _width = this.Width;
                _height = this.Height;
                _top = this.Top;
                _left = this.Left;
            };
            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (_isFullsScreen)
                {
                    this.Left = _left;
                    this.Top = _top;
                    this.Width = _width;
                    this.Height = _height;
                }
                else
                {
                    this.Left = 0.0;
                    this.Top = 0.0;
                    this.Height = SystemParameters.WorkArea.Height;
                    this.Width = SystemParameters.WorkArea.Width;
                }
                _isFullsScreen = !_isFullsScreen;

                //if (this.WindowState == WindowState.Maximized)
                //    this.WindowState = WindowState.Normal;
                //else
                //{
                //    this.WindowState = WindowState.Maximized;
                //}
            };
            btnClose.Click += (s, e) =>
           {
                //var dialogResult = await dialogHostService.Question("温馨提示", "确认退出系统?");
                //if (dialogResult.Result != Prism.Services.Dialogs.ButtonResult.OK) return;
                this.Close();
           };
            ColorZone.MouseMove += (s, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            };

            ColorZone.MouseDoubleClick += (s, e) =>
            {
                if (this.WindowState == WindowState.Normal)
                    this.WindowState = WindowState.Maximized;
                else
                    this.WindowState = WindowState.Normal;
            };
        }
    }
}
