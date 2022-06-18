using QianShi.Music.Common;
using QianShi.Music.Common.Models.Request;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class SearchDetailViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public static string SearchKeywordsParameterName = "Keywords";
        public static string SearchTypeParameterName = "Type";

        private readonly IPlaylistService _playlistService;
        private bool _hasMore;
        private string _keywords = string.Empty;
        private int _limit = 30;
        private DelegateCommand<ItemsControl> _moreCommand = default!;
        private int _offset = 0;
        private SearchType _searchType = SearchType.单曲;

        public SearchDetailViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
        }

        public bool HasMore
        {
            get { return _hasMore; }
            set { SetProperty(ref _hasMore, value); }
        }

        public ObservableCollection<object> Items { get; } = new();

        public bool KeepAlive => false;

        public string Keywords
        {
            get { return _keywords; }
            set { SetProperty(ref _keywords, value); }
        }

        public DelegateCommand<ItemsControl> MoreCommand
            => _moreCommand ??= new(More);

        public SearchType SearchType
        {
            get { return _searchType; }
            set { SetProperty(ref _searchType, value); }
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            var parameters = navigationContext.Parameters;
            if (parameters.ContainsKey(SearchTypeParameterName) && parameters.ContainsKey(SearchKeywordsParameterName))
            {
                SearchType = parameters.GetValue<SearchType>(SearchTypeParameterName);
                Keywords = parameters.GetValue<string>(SearchKeywordsParameterName);
                await Search(true);
            }
        }

        private async void More(ItemsControl control)
        {
            control.Focus();
            await Search();
        }

        private async Task Search(bool clear = false)
        {
            if (clear)
            {
                _offset = 0;
                HasMore = false;
                Items.Clear();
            }
            IsBusy = true;
            var request = new SearchRequest
            {
                Keywords = Keywords,
                Limit = _limit,
                Offset = _offset,
            };

            void FormatCover(IPlaylist playlist) => playlist.CoverImgUrl += "?param=200y200";
            switch (SearchType)
            {
                case SearchType.单曲:
                    {
                        var response = await _playlistService.SearchSong(request);
                        if (response.Code == 200 && string.IsNullOrWhiteSpace(response.Msg))
                        {
                            var ids = string.Join(',', response.Result.Songs.Select(x => x.Id));
                            var songResponse = await _playlistService.SongDetail(ids);
                            if (songResponse.Code == 200)
                            {
                                songResponse.Songs.ForEach((song) => FormatCover(song.Album));
                                Items.AddRange(songResponse.Songs);
                                HasMore = response.Result.HasMore;
                            }
                        }
                    }
                    break;

                case SearchType.专辑:
                    {
                        var response = await _playlistService.SearchAlbum(request);
                        if (response.Code == 200 && string.IsNullOrWhiteSpace(response.Msg))
                        {
                            response.Result.Albums.ForEach(FormatCover);
                            Items.AddRange(response.Result.Albums);
                            HasMore = response.Result.AlbumCount > _offset + response.Result.Albums.Count;
                        }
                    }
                    break;

                case SearchType.歌手:
                    {
                        var response = await _playlistService.SearchArtist(request);
                        if (response.Code == 200 && string.IsNullOrWhiteSpace(response.Msg))
                        {
                            response.Result.Artists.ForEach(FormatCover);
                            Items.AddRange(response.Result.Artists);
                            HasMore = response.Result.HasMore;
                        }
                    }
                    break;

                case SearchType.歌单:
                    {
                        var response = await _playlistService.SearchPlaylist(request);
                        if (response.Code == 200 && string.IsNullOrWhiteSpace(response.Msg))
                        {
                            response.Result.Playlists.ForEach(FormatCover);
                            Items.AddRange(response.Result.Playlists);
                            HasMore = response.Result.HasMore;
                        }
                    }
                    break;

                case SearchType.用户:
                    break;

                case SearchType.MV:
                    {
                        var response = await _playlistService.SearchMovieVideo(request);
                        if (response.Code == 200
                            && string.IsNullOrWhiteSpace(response.Msg)
                            && response.Result.MovieVideos != null)
                        {
                            response.Result.MovieVideos.ForEach(FormatCover);
                            Items.AddRange(response.Result.MovieVideos);
                            HasMore = response.Result.MovieVideoCount > _offset + response.Result.MovieVideos.Count;
                        }
                    }
                    break;

                case SearchType.歌词:
                    break;

                case SearchType.电台:
                    break;

                case SearchType.视频:
                    break;

                case SearchType.综合:
                    break;

                case SearchType.声音:
                    break;

                default:
                    break;
            }

            _offset += _limit;
            IsBusy = false;
        }
    }
}