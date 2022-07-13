using QianShi.Music.Common;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Models;
using QianShi.Music.Services;
using QianShi.Music.ViewModels.Dialogs;
using QianShi.Music.Views.Dialogs;

namespace QianShi.Music.ViewModels
{
    public class AlbumViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string AlbumIdParameterName = "PlaylistId";

        private readonly IContainerProvider _containerProvider;
        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private DelegateCommand<Song?> _playCommand = default!;
        private long _playlistId;
        private DelegateCommand _showDescriptionCommand = default!;

        public AlbumViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
            _containerProvider = containerProvider;
        }

        public PlaylistDetailModel Detail { get; } = new();

        public bool KeepAlive => false;

        /// <summary>
        /// 播放歌单
        /// </summary>
        public DelegateCommand<Song?> PlayCommand
            => _playCommand ??= new(Play);

        /// <summary>
        /// 立即播放
        /// </summary>
        public DelegateCommand<Song?> PlayImmediatelyCommand
            => _playCommand ??= new(Play);

        public DelegateCommand ShowDescriptionCommand
            => _showDescriptionCommand ??= new(ShowDescription);

        public ObservableCollection<Song> Songs { get; } = new();

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //if (DialogHost.IsDialogOpen(PrismManager.PlaylistDialogName))
            //{
            //    var session = DialogHost.GetDialogSession(PrismManager.PlaylistDialogName);
            //    session?.UpdateContent(new LoadingDialog());
            //    DialogHost.Close(PrismManager.PlaylistDialogName);
            //}
            _playStoreService.CurrentChanged -= CurrentChanged;
            base.OnNavigatedFrom(navigationContext);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _playlistId = navigationContext.Parameters.GetValue<long>(AlbumIdParameterName);
            Title = _playlistId.ToString();
            if (Detail.Id != _playlistId)
            {
                IsBusy = true;
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
                    Songs.Clear();

                    Songs.AddRange(response.Songs);

                    if ((_playStoreService.Current?.Id).HasValue)
                    {
                        CurrentChanged(null, new(_playStoreService.Current));
                    }
                }
                IsBusy = false;
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

            if (dialog is FrameworkElement { DataContext: null } view && Prism.Mvvm.ViewModelLocator.GetAutoWireViewModel(view) is null)
                Prism.Mvvm.ViewModelLocator.SetAutoWireViewModel(view, true);

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