using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models.Request;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;
using QianShi.Music.Services;
using QianShi.Music.Views;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class SearchViewModel : NavigationViewModel
    {
        public const string SearchTextParameter = nameof(SearchTextParameter);

        private readonly IPlaylistService _playlistService;
        private readonly INavigationService _navigationService;
        private string _currentSearchKeywords = string.Empty;

        public SearchViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService,
            INavigationService navigationService) : base(containerProvider)
        {
            _playlistService = playlistService;
            Artists = new();
            Albums = new();
            Songs = new();
            Playlists = new();
            MovieVideos = new();
            MoreCommand = new(More);
            _navigationService = navigationService;
        }

        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<Artist> Artists { get; set; }
        public DelegateCommand<SearchType?> MoreCommand { get; private set; }
        public ObservableCollection<MovieVideo> MovieVideos { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }
        public ObservableCollection<Song> Songs { get; set; }

        void FormatCover(IPlaylist playlist, int w = 200, int h = 200)
            => playlist.CoverImgUrl += $"?param={w}y{h}";

        void FormatCoverDefault(IPlaylist playlist)
            => FormatCover(playlist);

        private async Task GetSongsAsync()
        {
            var response = await _playlistService.SearchSong(new SearchRequest
            {
                Limit = 16,
                Keywords = _currentSearchKeywords,
                Type = SearchType.单曲
            });

            if (response.Code == 200)
            {
                if (response.Result.SongCount > 0)
                {
                    var ids = string.Join(',', response.Result.Songs.Select(s => s.Id));
                    var songDetailResponse = await _playlistService.SongDetail(ids);
                    if (songDetailResponse.Code == 200)
                    {
                        songDetailResponse.Songs.ForEach(s => FormatCover(s.Album, 48, 48));
                        Songs.AddRange(songDetailResponse.Songs);
                    }
                }
            }
        }
        private async Task GetAlbumsAsync()
        {
            var response = await _playlistService.SearchAlbum(new SearchRequest
            {
                Limit = 3,
                Keywords = _currentSearchKeywords,
                Type = SearchType.专辑
            });
            if (response.Code == 200)
            {
                response.Result.Albums.ForEach(FormatCoverDefault);
                Albums.AddRange(response.Result.Albums);
            }
        }
        private async Task GetArtistsAsync()
        {
            var response = await _playlistService.SearchArtist(new SearchRequest
            {
                Limit = 3,
                Keywords = _currentSearchKeywords,
                Type = SearchType.歌手
            });
            if (response.Code == 200)
            {
                response.Result.Artists.ForEach(FormatCoverDefault);
                Artists.AddRange(response.Result.Artists);
            }
        }
        private async Task GetPlaylistsAsync()
        {
            var response = await _playlistService.SearchPlaylist(new SearchRequest
            {
                Limit = 3,
                Keywords = _currentSearchKeywords,
                Type = SearchType.歌单
            });
            if (response.Code == 200)
            {
                response.Result.Playlists.ForEach(FormatCoverDefault);
                Playlists.AddRange(response.Result.Playlists);
            }
        }
        private async Task GetMovieVideosAsync()
        {
            var response = await _playlistService.SearchMovieVideo(new SearchRequest
            {
                Limit = 3,
                Keywords = _currentSearchKeywords,
                Type = SearchType.MV
            });
            if (response.Code == 200 && response.Result.MovieVideos?.Count > 0)
            {
                MovieVideos.AddRange(response.Result.MovieVideos);
            }
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            var parameters = navigationContext.Parameters;

            if (parameters.ContainsKey(SearchTextParameter))
            {
                var searchText = parameters.GetValue<string>(SearchTextParameter);

                if (_currentSearchKeywords == searchText)
                {
                    return;
                }
                else
                {
                    _currentSearchKeywords = searchText;
                    Artists.Clear();
                    Albums.Clear();
                    Songs.Clear();
                    Playlists.Clear();
                    MovieVideos.Clear();
                }
                await Task.WhenAll(GetSongsAsync(), GetAlbumsAsync(), GetArtistsAsync(), GetPlaylistsAsync(), GetMovieVideosAsync());
            }
        }

        private void More(SearchType? type)
        {
            if (type == null) return;
            _navigationService.NavigateToSearchDetail(_currentSearchKeywords, type.Value);
        }
    }
}