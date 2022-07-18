using QianShi.Music.Common.Models.Response;
using QianShi.Music.Data;
using QianShi.Music.Extensions;
using QianShi.Music.Models;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class PlaylistViewModel : NavigationViewModel
    {
        public const string IdParameters = "PlaylistId";

        private readonly IPlaylistService _playlistService;
        private readonly IPlaylistStoreService _playlistStoreService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private readonly IDataProvider<PlaylistDetailModel, long> _dataProvider;
        private readonly ISnackbarMessageQueue _snackbarMessageQueue;
        private readonly INavigationService _navigationService;
        private readonly object _locker = new();
        private DelegateCommand<Song?> _playCommand = default!;
        private DelegateCommand<string?> _toBottomCommand = default!;
        private PlaylistDetailModel? _detail;
        private long _playlistId;

        public PlaylistViewModel(IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IPlayService playService,
            IPlayStoreService playStoreService,
            IPlaylistStoreService playlistStoreService,
            IDataProvider<PlaylistDetailModel, long> dataProvider,
            ISnackbarMessageQueue snackbarMessageQueue,
            INavigationService navigationService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
            _playlistStoreService = playlistStoreService;
            _dataProvider = dataProvider;
            _snackbarMessageQueue = snackbarMessageQueue;
            _navigationService = navigationService;
        }

        public PlaylistDetailModel? Detail
        {
            get => _detail;
            set => SetProperty(ref _detail, value);
        }

        public DelegateCommand<Song?> PlayCommand =>
            _playCommand ??= new(Play);

        public ObservableCollection<Song> Songs { get; set; } = new();

        public DelegateCommand<string?> ToBottomCommand =>
            _toBottomCommand ?? (_toBottomCommand = new DelegateCommand<string?>(ExecuteToBottomCommand, (_) => !IsBusy && Detail != null && Detail.Songs.Count < Detail.SongsIds.Count));


        async void ExecuteToBottomCommand(string? parameter)
        {
            if (IsBusy || Detail == null || Detail.Songs.Count == Detail.SongsIds.Count) return;
            lock (_locker)
            {
                if (IsBusy || Detail == null || Detail.Songs.Count == Detail.SongsIds.Count)
                {
                    return;
                }
                IsBusy = true;
            }

            try
            {
                var ids = string.Join(',', Detail.SongsIds.Skip(Detail.Songs.Count).Take(40));
                var songsResponse = await _playlistService.SongDetail(ids);
                if (songsResponse.Code != 200)
                {
                    _snackbarMessageQueue.Enqueue(songsResponse.Msg!, TimeSpan.FromSeconds(5));
                    return;
                }
                Detail.AddSongs(songsResponse.Songs);
                await PushAsync(songsResponse.Songs);
            }
            finally
            {
                IsBusy = false;
            }

        }

        private async Task PushAsync(IEnumerable<Song> songs)
        {
            songs.ToList().ForEach(song =>
            {
                song.Album.CoverImgUrl += "?param=48y48";
                song.IsLike = _playlistStoreService.HasLikedSong(song);
            });

            var i = 0;
            var songChunks = songs.Chunk(5);
            var chunkLength = songChunks.Count();

            foreach (var songChunk in songChunks)
            {
                Songs.AddRange(songChunk);
                if (i < chunkLength - 1)
                {
                    await Task.Delay(200);
                }
                i++;
            }
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //if (DialogHost.IsDialogOpen(Extensions.PrismManager.PlaylistDialogName))
            //{
            //    var session = DialogHost.GetDialogSession(Extensions.PrismManager.PlaylistDialogName);
            //    if (session != null)
            //        session.UpdateContent(new LoadingDialog());
            //    DialogHost.Close(Extensions.PrismManager.PlaylistDialogName);
            //}
            _playStoreService.CurrentChanged -= CurrentChanged;
            base.OnNavigatedFrom(navigationContext);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            _playlistId = navigationContext.Parameters.GetValue<long>(IdParameters);
            if (Detail == null || Detail.Id != _playlistId)
            {
                IsBusy = true;

                try
                {
                    var result = await _dataProvider.GetDataAsync(_playlistId);
                    if (result == null)
                    {
                        navigationContext.NavigationService.Journal.GoBack();
                        return;
                    }

                    Detail = result;
                    Songs.Clear();

                    await PushAsync(Detail.Songs);
                }
                catch (Exception ex)
                {
                    _snackbarMessageQueue.Enqueue(ex.Message, TimeSpan.FromSeconds(5));
                    _navigationService.GoBack();
                    return;
                }
                finally
                {
                    IsBusy = false;
                }

                if ((_playStoreService.Current?.Id).HasValue)
                {
                    CurrentChanged(null, new(_playStoreService.Current));
                }
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

        private async void Play(Song? song)
        {
            if (song != null)
            {
                await _playStoreService.PlayAsync(song);
            }
            else
            {
                if (Songs.Count == 0)
                {
                    _snackbarMessageQueue.Enqueue("该歌单没有歌曲", TimeSpan.FromSeconds(5));
                    return;
                }

                await _playStoreService.AddPlaylistAsync(_playlistId, Songs);
                _playStoreService.Pause();
                if (!_playService.IsPlaying)
                {
                    _playStoreService.Play();
                }
            }
        }
    }
}