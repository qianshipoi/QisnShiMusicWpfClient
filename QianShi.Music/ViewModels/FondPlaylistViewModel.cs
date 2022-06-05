using System.Collections.ObjectModel;
using System.Windows;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class FondPlaylistViewModel : NavigationViewModel
    {
        public const string PlaylistIdParameterName = $"{nameof(FondPlaylistViewModel)}.PlaylistId";
        private readonly IPlaylistService _playlistService;
        private readonly IPlayStoreService _playStoreService;
        private readonly IPlayService _playService;
        private long _playlistId;

        private Playlist _playlist = default!;
        public Playlist Playlist
        {
            get => _playlist;
            set => SetProperty(ref _playlist, value);
        }

        private ObservableCollection<Song> _songs = new();

        public ObservableCollection<Song> Songs
        {
            get => _songs;
            set => SetProperty(ref _songs, value);
        }

        private DelegateCommand<Song?> _playCommand = default!;
        public DelegateCommand<Song?> PlayCommand =>
            _playCommand ??= new(Play);

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

        public FondPlaylistViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService, IPlayService playService, IPlayStoreService playStoreService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;
        }


        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            base.OnNavigatedFrom(navigationContext);

            _playStoreService.CurrentChanged -= CurrentChanged;
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var parameters = navigationContext.Parameters;
            if (!parameters.ContainsKey(PlaylistIdParameterName))
            {
                MessageBox.Show("请传递PlaylistId");
                navigationContext.NavigationService.Journal.GoBack();
                return;
            }

            var playlistId  = parameters.GetValue<long>(PlaylistIdParameterName);

            if (_playlistId == playlistId)
            {
                return;
            }

            _playlistId = playlistId;

            Songs.Clear();

            var response = await _playlistService.GetPlaylistDetailAsync(_playlistId);

            if (response.Code != 200)
            {
                MessageBox.Show($"获取歌单信息失败:{response.Msg}");
                navigationContext.NavigationService.Journal.GoBack();
                return;
            }

            int i = 0;
            foreach (var song in response.PlaylistDetail.Tracks)
            {
                song.Album.CoverImgUrl += "?param=48y48";
                song.IsLike = true;
                Songs.Add(song);
                i++;
                if (i % 5 == 0)
                {
                    await Task.Delay(20);
                }
            }

            _playStoreService.CurrentChanged -= CurrentChanged;
            _playStoreService.CurrentChanged += CurrentChanged;

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
    }
}
