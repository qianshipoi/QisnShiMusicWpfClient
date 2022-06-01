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
        private readonly IRegionManager _regionManager;
        private string _currentSearchKeywords = string.Empty;

        public SearchViewModel(IContainerProvider containerProvider, IPlaylistService playlistService, IRegionManager regionManager) : base(containerProvider)
        {
            _playlistService = playlistService;
            Artists = new();
            Albums = new();
            Songs = new();
            Playlists = new();
            MovieVideos = new();
            MoreCommand = new(More);
            _regionManager = regionManager;
        }

        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<Artist> Artists { get; set; }
        public DelegateCommand<SearchType?> MoreCommand { get; private set; }
        public ObservableCollection<MovieVideo> MovieVideos { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }
        public ObservableCollection<Song> Songs { get; set; }

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

                var searchTypes = new List<(SearchType type, int limit)>();
                searchTypes.Add((SearchType.专辑, 3));
                searchTypes.Add((SearchType.歌手, 3));
                searchTypes.Add((SearchType.MV, 5));
                searchTypes.Add((SearchType.歌单, 12));
                searchTypes.Add((SearchType.单曲, 16));

                void FormatCover(IPlaylist playlist, int w = 200, int h = 200)
                    => playlist.CoverImgUrl += $"?param={w}y{h}";

                void FormatCoverDefault(IPlaylist playlist)
                    => FormatCover(playlist);

                foreach (var searchType in searchTypes)
                {
                    var response = await _playlistService.Search(new Common.Models.Request.SearchRequest
                    {
                        Keywords = searchText,
                        Type = searchType.type,
                        Limit = searchType.limit,
                    });
                    if (response.Code == 200)
                    {
                        switch (searchType.type)
                        {
                            case SearchType.单曲:
                                var songSearchResult = (SongSearchResult)response.Result;
                                if (songSearchResult.SongCount > 0)
                                {
                                    var ids = string.Join(',', songSearchResult.Songs.Select(s => s.Id));
                                    var songDetailResponse = await _playlistService.SongDetail(ids);
                                    if (songDetailResponse.Code == 200)
                                    {
                                        songDetailResponse.Songs.ForEach(s => FormatCover(s.Album, 48, 48));
                                        Songs.AddRange(songDetailResponse.Songs);
                                    }
                                }
                                break;

                            case SearchType.专辑:
                                var albumSearchResult = (AlbumSearchResult)response.Result;
                                albumSearchResult.Albums.ForEach(FormatCoverDefault);
                                Albums.AddRange(albumSearchResult.Albums);
                                break;

                            case SearchType.歌手:
                                var artistSearchResult = (ArtistSearchResult)response.Result;
                                artistSearchResult.Artists.ForEach(FormatCoverDefault);
                                Artists.AddRange(artistSearchResult.Artists);
                                break;

                            case SearchType.歌单:
                                var playlistSearchResult = (PlaylistSearchResult)response.Result;
                                playlistSearchResult.Playlists.ForEach(FormatCoverDefault);
                                Playlists.AddRange(playlistSearchResult.Playlists);
                                break;

                            case SearchType.MV:
                                var mvSearchResult = (MovieVideoSearchResult)response.Result;
                                if (mvSearchResult.MovieVideos != null)
                                    MovieVideos.AddRange(mvSearchResult.MovieVideos);
                                break;

                            case SearchType.用户:
                            case SearchType.歌词:
                            case SearchType.电台:
                            case SearchType.视频:
                            case SearchType.综合:
                            case SearchType.声音:
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void More(SearchType? type)
        {
            if (type == null) return;
            var parameters = new NavigationParameters
            {
                { SearchDetailViewModel.SearchTypeParameterName, type },
                { SearchDetailViewModel.SearchKeywordsParameterName, _currentSearchKeywords }
            };
            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(nameof(SearchDetailView), parameters);
        }
    }
}