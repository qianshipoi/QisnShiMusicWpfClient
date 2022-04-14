using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;

using System;
using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class MainWindowViewModel : BindableBase, IConfigureService
    {
        private string _title = "Prism Application";
        private ObservableCollection<MenuBar> menuBars;
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private IRegionNavigationJournal _journal;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<MenuBar> MenuBars
        {
            get { return menuBars; }
            set { menuBars = value; RaisePropertyChanged(); }
        }

        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LogoutComand { get; private set; }

        public MainWindowViewModel(IContainerProvider containerProvider,
            IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _containerProvider = containerProvider;
            menuBars = new ObservableCollection<MenuBar>();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoBack)
                    _journal.GoBack();
            });
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (_journal != null && _journal.CanGoForward)
                    _journal.GoForward();
            });
            LogoutComand = new DelegateCommand(Logout);
        }

        private void Logout()
        {
            throw new NotImplementedException();
        }

        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "发现", NameSpace = "FoundView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "音乐库", NameSpace = "LibraryView" });
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
            {
                _journal = back.Context.NavigationService.Journal;
            });
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public void Configure()
        {
            CreateMenuBar();
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
            _regionManager.Regions[PrismManager.FullScreenRegionName].RequestNavigate("PlayView");
        }
    }
}
