using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace QianShi.Music.ViewModels
{
    public class FondPlaylistViewModel : NavigationViewModel
    {
        public const string PlaylistIdParameterName = $"{nameof(FondPlaylistViewModel)}.PlaylistId";

        private readonly IPlaylistService _playlistService;
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private DelegateCommand<Song?> _playCommand = default!;
        private long _playlistId;
        private string? _searchText;
        private ObservableCollection<Song> _songs = new();

        public FondPlaylistViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService, IPlayService playService, IPlayStoreService playStoreService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _playService = playService;
            _playStoreService = playStoreService;

            SongsListView = new ListCollectionView(_songs)
            {
                Filter = (obj) =>
                {
                    if (obj is Song song)
                    {
                        if (string.IsNullOrWhiteSpace(SearchText))
                        {
                            return true;
                        }
                        else
                        {
                            return song.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                                   || song.Album.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase)
                                   || song.Artists.Any(x => x.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            };
        }

        public DelegateCommand<Song?> PlayCommand =>
            _playCommand ??= new(Play);

        public string? SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SongsListView.Refresh();
            }
        }

        public ListCollectionView SongsListView { get; set; }

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

            var playlistId = parameters.GetValue<long>(PlaylistIdParameterName);

            if (_playlistId == playlistId)
            {
                return;
            }

            _playlistId = playlistId;

            _songs.Clear();

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
                _songs.Add(song);
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
                _songs.Where(x => x.IsPlaying).ToList().ForEach(item => item.IsPlaying = false);
                var song = _songs.FirstOrDefault(x => x.Id == songId.Value);
                if (song != null) song.IsPlaying = true;
            }
            else
            {
                _songs.Where(x => x.IsPlaying).ToList().ForEach(item => item.IsPlaying = false);
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
                await _playStoreService.AddPlaylistAsync(_playlistId, _songs);
                _playStoreService.Pause();
                if (!_playService.IsPlaying)
                {
                    _playStoreService.Play();
                }
            }
        }
    }
}