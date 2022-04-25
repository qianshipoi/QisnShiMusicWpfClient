using QianShi.Music.Common.Models.Request;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;

using RestSharp;

namespace QianShi.Music.Services
{
    public class PlaylistService : IPlaylistService
    {
        private static RestClient _client => new RestClient(new RestClientOptions("http://150.158.194.185:3001")
        {
            Timeout = -1
        });

        private static Task<T?> Get<T>(RestRequest request)
        {
            //var client = new RestClient(new RestClientOptions("https://netease-cloud-music-api-qianshi.vercel.app")
            //{
            //    Timeout = -1
            //});
            return _client.GetAsync<T>(request);
        }

        public async Task<CatlistResponse> GetCatlistAsync()
            => await Get<CatlistResponse>(new RestRequest("/playlist/catlist"))
            ?? new CatlistResponse();

        public Task<PlaylistHighqualityTagsResponse> GetPlaylistHighqualityTagsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RelatedPlaylistResponse> GetRelatedPlaylistAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<TopPlaylistResponse?> GetTopPlaylistAsync(TopPlaylistRequest parameter)
        {
            var request = new RestRequest("/top/playlist");
            request.AddQueryParameters(parameter);

            return await Get<TopPlaylistResponse>(request);
        }

        public async Task<TopPlaylistHighqualityResponse> GetTopPlaylistHighqualityAsnyc(TopPlaylistHighqualityRequest parameter)
        {
            var request = new RestRequest("/top/playlist/highquality");

            request.AddQueryParameters(parameter);

            return await Get<TopPlaylistHighqualityResponse>(request) ?? new TopPlaylistHighqualityResponse();
        }

        public async Task<PersonalizedResponse> GetPersonalizedAsync(int? limit)
        {
            var request = new RestRequest("/personalized");
            if (limit != null)
                request.AddQueryParameter<int>("limit", limit.Value);
            return await Get<PersonalizedResponse>(request) ?? new PersonalizedResponse();
        }

        public async Task<ToplistResponse> GetToplistAsync()
        {
            var request = new RestRequest("/toplist");
            return await Get<ToplistResponse>(request) ?? new ToplistResponse();
        }

        public Task<PlaylistHotResponse> GetHotAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<AlbumNewestResponse> GetAlbumNewestAsync()
        {
            var request = new RestRequest("/album/newest");
            return await Get<AlbumNewestResponse>(request) ?? new AlbumNewestResponse();
        }

        public async Task<PlaylistDetailResponse> GetPlaylistDetailAsync(long playlistId)
        {
            var request = new RestRequest("/playlist/detail");
            request.AddParameter("id", playlistId);
            return await Get<PlaylistDetailResponse>(request) ?? new PlaylistDetailResponse();
        }

        public async Task<AlbumNewResponse> GetAlbumNewAsync(AlbumNewRequest parameters)
        {
            var request = new RestRequest("/album/new");
            request.AddQueryParameters(parameters);
            return await Get<AlbumNewResponse>(request) ?? new AlbumNewResponse();
        }

        public async Task<AlbumResponse> GetAblumAsync(long id)
        {
            var request = new RestRequest("/album");
            request.AddQueryParameter("id", id);
            return await Get<AlbumResponse>(request) ?? new AlbumResponse();
        }

        public async Task<ToplistArtistResponse> ToplistArtist(int? type = null)
        {
            var requset = new RestRequest("toplist/artist");
            if (type != null)
                requset.AddQueryParameter("type", type.ToString());
            return await Get<ToplistArtistResponse>(requset) ?? new ToplistArtistResponse();
        }

        public async Task<SearchResponse> Search(SearchRequest parasmeters)
        {
            var request = new RestRequest("/search");
            request.AddQueryParameters(parasmeters);

            Type type = null!;

            switch (parasmeters.Type)
            {
                case SearchRequest.SearchType.单曲:
                    type = typeof(SongSearchResult);
                    break;
                case SearchRequest.SearchType.专辑:
                    type = typeof(AlbumSearchResult);
                    break;
                case SearchRequest.SearchType.歌手:
                    type = typeof(ArtistSearchResult);
                    break;
                case SearchRequest.SearchType.歌单:
                    type = typeof(PlaylistSearchResult);
                    break;
                case SearchRequest.SearchType.用户:
                    type = typeof(ArtistSearchResult);
                    break;
                case SearchRequest.SearchType.MV:
                    type = typeof(MvSearchResult);
                    break;
                case SearchRequest.SearchType.歌词:
                    type = typeof(ArtistSearchResult);
                    break;
                case SearchRequest.SearchType.电台:
                    break;
                case SearchRequest.SearchType.视频:
                    break;
                case SearchRequest.SearchType.综合:
                    break;
                case SearchRequest.SearchType.声音:
                    break;
                default:
                    break;
            }

            if (type == null) throw new ArgumentException("type is not correct.");

            var method = GetType().GetMethod("Get", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            var responseType = typeof(SearchResponse<>).MakeGenericType(new Type[] { type });

            var genericMethod = method?.MakeGenericMethod(new Type[] { responseType });
            var task = genericMethod?.Invoke(this, new object[] { request }) as Task;
            if (task == null) return new SearchResponse();

            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty?.GetValue(task) as SearchResponse;

            return response ?? new SearchResponse();
        }

        public async Task<SongDetailResponse> SongDetail(string ids)
        {

            return new SongDetailResponse();
        }

    }
}
