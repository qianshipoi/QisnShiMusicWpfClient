using MaterialDesignThemes.Wpf;

using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.ViewModels.Dialogs;
using QianShi.Music.Views.Dialogs;

using System.Collections.ObjectModel;
using System.Windows;

namespace QianShi.Music.ViewModels
{
    public class AlbumViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string AlbumIdParameterName = "PlaylistId";

        private readonly IContainerProvider _containerProvider;
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private PlaylistDetail _detail = new();
        private bool _loading;
        private long _playlistId;
        private ObservableCollection<Song> _songs;
        private string _title;
        public AlbumViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService)
            : base(containerProvider)
        {
            _title = string.Empty;
            _songs = new ObservableCollection<Song>();

            PlayCommand = new DelegateCommand<Song?>(Play);
            PlayImmediatelyCommand = new DelegateCommand<Song?>(Play);
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
            _containerProvider = containerProvider;
            ShowDescriptionCommand = new DelegateCommand(ShowDescription);
        }

        public PlaylistDetail Detail { get => _detail; set => SetProperty(ref _detail, value); }

        public bool KeepAlive => false;

        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        /// <summary>
        /// 播放歌单
        /// </summary>
        public DelegateCommand<Song?> PlayCommand { get; private set; }

        /// <summary>
        /// 立即播放
        /// </summary>
        public DelegateCommand<Song?> PlayImmediatelyCommand { get; private set; }

        public DelegateCommand ShowDescriptionCommand { get; private set; }

        public ObservableCollection<Song> Songs
        {
            get => _songs;
            set => SetProperty(ref _songs, value);
        }
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (DialogHost.IsDialogOpen(PrismManager.PlaylistDialogName))
            {
                var session = DialogHost.GetDialogSession(PrismManager.PlaylistDialogName);
                session?.UpdateContent(new LoadingDialog());
                DialogHost.Close(PrismManager.PlaylistDialogName);
            }
            _playStoreService.CurrentChanged -= CurrentChanged;
            base.OnNavigatedFrom(navigationContext);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _playlistId = navigationContext.Parameters.GetValue<long>(AlbumIdParameterName);
            Title = _playlistId.ToString();
            if (Detail.Id != _playlistId)
            {
                Loading = true;
                var response = await _playlistService.GetAblumAsync(_playlistId);
                if (response.Code == 200)
                {
                    Detail.Id = response.Album.Id;
                    Detail.Name = response.Album.Name;
                    Detail.Description = response.Album.Description;
                    Detail.LastUpdateTime = response.Album.PublishTime;
                    Detail.PicUrl = response.Album.CoverImgUrl;
                    Detail.Count = response.Album.Size;
                    Detail.Creator = response.Album.Artist.Name;
                    _songs.Clear();

                    Songs.AddRange(response.Songs);

                    if ((_playStoreService.Current?.Id).HasValue)
                    {
                        CurrentChanged(null, new(_playStoreService.Current));
                    }
                }
                Loading = false;
            }

            _playStoreService.CurrentChanged -= CurrentChanged;
            _playStoreService.CurrentChanged += CurrentChanged;
            base.OnNavigatedTo(navigationContext);
        }

        private void CurrentChanged(object? sender, SongChangedEventArgs e)
        {
            var songId = e.NewSong?.Id;
            if (songId.HasValue)
            {
                Songs.Where(x => x.IsPlaying).ToList().ForEach(item => item.IsPlaying = false);
                var song = Songs.FirstOrDefault(x => x.Id == songId.Value);
                if (song != null) song.IsPlaying = true;
            }
            else
            {
                Songs.Where(x => x.IsPlaying).ToList().ForEach(item => item.IsPlaying = false);
            }
        }

        private async void Play(Song? playlist)
        {
            if (playlist != null)
            {
                await _playStoreService.PlayAsync(playlist);
            }
            else
            {
                await _playStoreService.AddPlaylistAsync(_playlistId, Songs);
                _playStoreService.Pause();
                if (!_playService.IsPlaying)
                {
                    _playStoreService.Play();
                }
            }
        }

        private async void ShowDescription()
        {
            var dialog = _containerProvider.Resolve<DescriptionDialog>();

            if (dialog is FrameworkElement { DataContext: null } view && ViewModelLocator.GetAutoWireViewModel(view) is null)
                ViewModelLocator.SetAutoWireViewModel(view, true);

            if (dialog.DataContext is IDialogHostAware aware)
            {
                aware.DialogHostName = PrismManager.PlaylistDialogName;
            }

            await DialogHost.Show(dialog, PrismManager.PlaylistDialogName, openedEventHandler: (sender, eventArgs) =>
            {
                if (dialog.DataContext is IDialogHostAware dialogHostAware)
                {
                    dialogHostAware.OnDialogOpend(new DialogParameters { { DescriptionDialogViewModel.DescriptionParameterName, Detail.Description } });
                }
                eventArgs.Session.UpdateContent(dialog);
            });
        }
    }
}