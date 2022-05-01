﻿using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;

using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace QianShi.Music.ViewModels
{
    public class MainWindowViewModel : BindableBase, IConfigureService
    {
        private string _title = "Prism Application";
        private ObservableCollection<MenuBar> menuBars;
        private readonly IContainerProvider _containerProvider;
        private readonly IRegionManager _regionManager;
        private readonly IPlaylistService _playlistService;
        private IRegionNavigationJournal _journal = null!;

        private UserData _userData = default!;
        public UserData UserData
        {
            get { return _userData; }
            set { SetProperty(ref _userData, value); }
        }
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
        private MenuBar? _navigateCurrentItem;
        public MenuBar? NavigateCurrentItem
        {
            get => _navigateCurrentItem;
            set
            {
                if (_navigateCurrentItem != value && null != value)
                {
                    NavigateCommand.Execute(value);
                }
                SetProperty(ref _navigateCurrentItem, value);
            }
        }
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand<ContentControl> OpenPlayViewCommand { get; private set; }
        public DelegateCommand<string> SearchCommand { get; private set; }
        public DelegateCommand LoginCommand { get; private set; }
        public MainWindowViewModel(IContainerProvider containerProvider,
            IRegionManager regionManager, IPlaylistService playlistService)
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
            LogoutCommand = new DelegateCommand(Logout);
            LoginCommand = new DelegateCommand(Login);
            OpenPlayViewCommand = new DelegateCommand<ContentControl>(OpenPlayView);
            SearchCommand = new DelegateCommand<string>(Search);
            _playlistService = playlistService;
            _userData = UserData.Instance;
        }

        void Search(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) return;
            var parameters = new NavigationParameters();
            parameters.Add("SearchText", searchText);

            var regions = _regionManager.Regions[PrismManager.MainViewRegionName];

            var uri = _journal.CurrentEntry.Uri;   // TODO 导航BUG 待修复
            regions.RequestNavigate("SearchView", parameters);
        }

        void OpenPlayView(ContentControl obj)
        {
            if (obj.Content is PlayView playView)
            {
                ((PlayViewModel)playView.DataContext).Display = true;
            }
        }

        async void Logout()
        {
            var response = await _playlistService.Logout();
            if (response.Code == 200)
            {
                UserData.Clear();
                UserData.Save();
            }
        }

        void Login()
        {
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(LoginView));
        }

        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "发现", NameSpace = "FoundView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "音乐库", NameSpace = "LibraryView" });
        }

        void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrWhiteSpace(obj.NameSpace))
                return;

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace);
        }

        private void NavigationService_Navigated(object? sender, RegionNavigationEventArgs e)
        {
            var viewName = e.Uri.OriginalString;
            var menuBar = menuBars.FirstOrDefault(x => x.NameSpace == viewName);

            if (menuBar != null && menuBar != NavigateCurrentItem)
            {
                _navigateCurrentItem = menuBar;
            }
            else
            {
                _navigateCurrentItem = null;
            }
            RaisePropertyChanged(nameof(NavigateCurrentItem));
        }

        /// <summary>
        /// 配置首页初始化参数
        /// </summary>
        public async void Configure()
        {
            CreateMenuBar();

            var response = await _playlistService.LoginStatus();
            if (response.Data.Code == 200)
            {
                UserData.Cover = response.Data.Profile?.AvatarUrl;
                UserData.NickName = response.Data.Profile?.Nickname;
                UserData.Save();
            }

            _regionManager.Regions[PrismManager.MainViewRegionName].NavigationService.Navigated += NavigationService_Navigated;
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(MenuBars[0].NameSpace, back =>
            {
                _journal = back.Context.NavigationService.Journal;
            });
            _regionManager.Regions[PrismManager.FullScreenRegionName].RequestNavigate("PlayView");

        }
    }
}
