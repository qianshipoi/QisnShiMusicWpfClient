﻿using Prism.Ioc;

using QianShi.Music.Common;
using QianShi.Music.Common.Data;
using QianShi.Music.Services;
using QianShi.Music.ViewModels;
using QianShi.Music.ViewModels.Dialogs;
using QianShi.Music.Views;
using QianShi.Music.Views.Dialogs;

using System.Windows;

namespace QianShi.Music
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }
        protected override void OnInitialized()
        {
            var service = App.Current.MainWindow.DataContext as IConfigureService;
            if (service != null)
                service.Configure();
            base.OnInitialized();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IPlaylistService, PlaylistService>();
            containerRegistry.Register<IDialogHostService, DialogHostService>();

            //containerRegistry.RegisterDialog<LoadingDialog>();
            containerRegistry.RegisterForNavigation<DescriptionDialog, DescriptionDialogViewModel>();
            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<FoundView, FoundViewModel>();
            containerRegistry.RegisterForNavigation<LibraryView, LibraryViewModel>();
            containerRegistry.RegisterForNavigation<PlaylistView, PlaylistViewModel>();
            containerRegistry.RegisterForNavigation<PlayView, PlayViewModel>();
            containerRegistry.RegisterForNavigation<PlaylistCardView, PlaylistCardViewModel>();
            containerRegistry.RegisterForNavigation<AlbumView, AlbumViewModel>();
            containerRegistry.RegisterForNavigation<SearchView, SearchViewModel>();
            //containerRegistry.RegisterForNavigation<SearchView, DesignSearchViewModel>();
        }
    }
}
