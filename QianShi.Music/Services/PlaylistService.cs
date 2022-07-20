using QianShi.Music.Common.Models.Request;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Extensions;

namespace QianShi.Music.Services
{
    public class PlaylistService : IPlaylistService
    {
        public const string SearchApi = "/search";
        private CookieContainer _cookieContainer;

        public PlaylistService()
        {
            _cookieContainer = new CookieContainer();
        }

        private RestClient Client => new(new RestClientOptions("http://150.158.194.185:3001")
        {
            MaxTimeout = -1,
            CookieContainer = _cookieContainer
        });

        public async Task<AlbumSublistResponse> AlbumSublist(PagedRequestBase parameters)
            => await Request<AlbumSublistResponse>("/album/sublist", parameters);

        public async Task<ArtistAlbumResponse> ArtistAlbum(ArtistAlbumRequest parameters)
            => await Request<ArtistAlbumResponse>("/artist/album", parameters);

        public async Task<ArtistMvResponse> ArtistMv(ArtistMvRequest parameters)
            => await Request<ArtistMvResponse>("/artist/mv", parameters);

        public async Task<ArtistsResponse> Artists(long id)
        {
            var request = new RestRequest("/artists");
            request.AddQueryParameter("id", id);
            return (await Get<ArtistsResponse>(request)) ?? new();
        }

        public async Task<ArtistSublistResponse> ArtistSublist(PagedRequestBase parameters)
            => await Request<ArtistSublistResponse>("/artist/sublist", parameters);

        public async Task<AlbumResponse> GetAblumAsync(long id)
        {
            var request = new RestRequest("/album");
            request.AddQueryParameter("id", id);
            return await Get<AlbumResponse>(request) ?? new AlbumResponse();
        }

        public async Task<AlbumNewResponse> GetAlbumNewAsync(AlbumNewRequest parameters)
        {
            var request = new RestRequest("/album/new");
            request.AddQueryParameters(parameters);
            return await Get<AlbumNewResponse>(request) ?? new AlbumNewResponse();
        }

        public async Task<AlbumNewestResponse> GetAlbumNewestAsync()
        {
            var request = new RestRequest("/album/newest");
            return await Get<AlbumNewestResponse>(request) ?? new AlbumNewestResponse();
        }

        public async Task<CatlistResponse> GetCatlistAsync()
            => await Get<CatlistResponse>(new RestRequest("/playlist/catlist"))
               ?? new CatlistResponse();

        public CookieCollection? GetCookieCollection() => _cookieContainer.GetAllCookies();

        public Task<PlaylistHotResponse> GetHotAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PersonalizedResponse> GetPersonalizedAsync(int? limit)
        {
            var request = new RestRequest("/personalized");
            if (limit != null)
                request.AddQueryParameter<int>("limit", limit.Value);
            return await Get<PersonalizedResponse>(request) ?? new PersonalizedResponse();
        }

        public async Task<PlaylistDetailResponse> GetPlaylistDetailAsync(long playlistId)
        {
            var request = new RestRequest("/playlist/detail");
            request.AddParameter("id", playlistId);
            return await Get<PlaylistDetailResponse>(request) ?? new PlaylistDetailResponse();
        }

        public Task<PlaylistHighqualityTagsResponse> GetPlaylistHighqualityTagsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<RelatedPlaylistResponse> GetRelatedPlaylistAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ToplistResponse> GetToplistAsync()
        {
            var request = new RestRequest("/toplist");
            return await Get<ToplistResponse>(request) ?? new ToplistResponse();
        }

        public async Task<TopPlaylistResponse?> GetTopPlaylistAsync(TopPlaylistRequest parameter)
        {
            var request = new RestRequest("/top/playlist");
            request.AddQueryParameters(parameter);

            return await Get<TopPlaylistResponse>(request);
        }

        public async Task<TopPlaylistHighqualityResponse> GetTopPlaylistHighqualityAsnyc(
            TopPlaylistHighqualityRequest parameter)
        {
            var request = new RestRequest("/top/playlist/highquality");

            request.AddQueryParameters(parameter);

            return await Get<TopPlaylistHighqualityResponse>(request) ?? new TopPlaylistHighqualityResponse();
        }

        public async Task<LikelistResponse> Likelist(long uid)
        {
            var request = new RestRequest();
            request.AddQueryParameter("uid", uid);
            return (await Get<LikelistResponse>(request)) ?? new();
        }

        public async Task<LoginResponse> Login(LoginRequest parameters)
        {
            return await Request<LoginResponse>("/login", parameters);
        }

        public async Task<LoginResponse> LoginCellPhone(LoginCellphoneRequest parameters)
        {
            return await Request<LoginResponse>("/login/cellphone", parameters);
        }

        public async Task<LoginQrCheckResponse> LoginQrCheck(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"“{nameof(key)}”不能为 null 或空白。", nameof(key));
            }

            var request = new RestRequest("/login/qr/check");
            request.AddQueryParameter("key", key);
            request.AddQueryParameter("time", DateTime.Now.Ticks);
            return await Get<LoginQrCheckResponse>(request) ?? new();
        }

        public async Task<LoginQrCreateResponse> LoginQrCreate(string key, bool isBase64 = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"“{nameof(key)}”不能为 null 或空白。", nameof(key));
            }

            var request = new RestRequest("/login/qr/create");
            request.AddQueryParameter("key", key);
            if (isBase64)
                request.AddQueryParameter("qrimg", true);
            request.AddQueryParameter("time", DateTime.Now.Ticks);
            return await Get<LoginQrCreateResponse>(request) ?? new();
        }

        public async Task<LoginQrKeyResponse> LoginQrKey()
        {
            var request = new RestRequest("login/qr/key");
            request.AddQueryParameter("time", DateTime.Now.Ticks);
            return await Get<LoginQrKeyResponse>(request) ?? new();
        }

        public async Task<LoginStatusResponse> LoginStatus()
        {
            var request = new RestRequest("/login/status");
            request.AddQueryParameter("time", DateTime.Now.Ticks);
            return await Get<LoginStatusResponse>(request) ?? new();
        }

        public async Task<LogoutResponse> Logout()
        {
            try
            {
                var request = new RestRequest("/logout");
                request.AddQueryParameter("time", DateTime.Now.Ticks);
                return (await Get<LogoutResponse>(request)) ?? new LogoutResponse();
            }
            finally
            {
                _cookieContainer = new CookieContainer();
            }
        }

        public async Task<LyricResponse> Lyric(long id)
        {
            var request = new RestRequest("/lyric");
            request.AddQueryParameter("id", id);
            return await Get<LyricResponse>(request) ?? new LyricResponse();
        }

        public async Task<MvDetailResponse> MvDetail(long mvid)
        {
            var request = new RestRequest("/mv/detail");
            request.AddQueryParameter("mvid", mvid);
            return await Get<MvDetailResponse>(request) ?? new MvDetailResponse();
        }

        public async Task<MvSublistResponse> MvSublist(PagedRequestBase parameters)
            => await Request<MvSublistResponse>("/mv/sublist", parameters);

        public async Task<MvUrlResponse> MvUrl(MvUrlRequest parameters)
            => await Request<MvUrlResponse>("/mv/url", parameters);

        public async Task<SearchResponse> Search(SearchRequest parasmeters)
        {
            var request = new RestRequest("/search");
            request.AddQueryParameters(parasmeters);

            Type type = null!;

            switch (parasmeters.Type)
            {
                case SearchType.单曲:
                    type = typeof(SongSearchResult);
                    break;

                case SearchType.专辑:
                    type = typeof(AlbumSearchResult);
                    break;

                case SearchType.歌手:
                    type = typeof(ArtistSearchResult);
                    break;

                case SearchType.歌单:
                    type = typeof(PlaylistSearchResult);
                    break;

                case SearchType.用户:
                    type = typeof(ArtistSearchResult);
                    break;

                case SearchType.MV:
                    type = typeof(MovieVideoSearchResult);
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

            if (type == null) throw new ArgumentException("type is not correct.");

            var method = GetType().GetMethod("Get",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            var responseType = typeof(SearchResponse<>).MakeGenericType(new Type[] { type });

            var genericMethod = method?.MakeGenericMethod(new Type[] { responseType });
            var task = genericMethod?.Invoke(this, new object[] { request }) as Task;
            if (task == null) return new SearchResponse();

            await task.ConfigureAwait(false);
            var resultProperty = task.GetType().GetProperty("Result");
            var response = resultProperty?.GetValue(task) as SearchResponse;
            if (response == null || !string.IsNullOrEmpty(response.Msg))
            {
                return new SearchResponse();
            }

            return response;
        }

        public async Task<SearchResponse<AlbumSearchResult>> SearchAlbum(SearchRequest parameters)
        {
            var request = new RestRequest(SearchApi);
            parameters.Type = SearchType.专辑;
            request.AddQueryParameters(parameters);
            return await Get<SearchResponse<AlbumSearchResult>>(request) ?? new();
        }

        public async Task<SearchResponse<ArtistSearchResult>> SearchArtist(SearchRequest parameters)
        {
            var request = new RestRequest(SearchApi);
            parameters.Type = SearchType.歌手;
            request.AddQueryParameters(parameters);
            return await Get<SearchResponse<ArtistSearchResult>>(request) ?? new();
        }

        public async Task<SearchResponse<MovieVideoSearchResult>> SearchMovieVideo(SearchRequest parameters)
        {
            var request = new RestRequest(SearchApi);
            parameters.Type = SearchType.MV;
            request.AddQueryParameters(parameters);
            return await Get<SearchResponse<MovieVideoSearchResult>>(request) ?? new();
        }

        public async Task<SearchResponse<PlaylistSearchResult>> SearchPlaylist(SearchRequest parameters)
        {
            var request = new RestRequest(SearchApi);
            parameters.Type = SearchType.歌单;
            request.AddQueryParameters(parameters);
            return await Get<SearchResponse<PlaylistSearchResult>>(request) ??
                   new SearchResponse<PlaylistSearchResult>();
        }

        public async Task<SearchResponse<SongSearchResult>> SearchSong(SearchRequest parameters)
        {
            var request = new RestRequest(SearchApi);
            parameters.Type = SearchType.单曲;
            request.AddQueryParameters(parameters);

            return await Get<SearchResponse<SongSearchResult>>(request) ?? new();
        }

        public void SetCookie(CookieCollection cookies) => _cookieContainer.Add(cookies);

        public void SetCookie(Cookie cookie) => _cookieContainer.Add(cookie);

        public async Task<SimiArtistResponse> SimiArtist(long id)
        {
            var request = new RestRequest("/simi/artist");
            request.AddQueryParameter("id", id);
            return (await Get<SimiArtistResponse>(request)) ?? new();
        }

        public async Task<SimiMvResponse> SimiMv(long mvid)
        {
            var request = new RestRequest("/simi/mv");
            request.AddQueryParameter("mvid", mvid);
            return await Get<SimiMvResponse>(request) ?? new SimiMvResponse();
        }

        public async Task<SongDetailResponse> SongDetail(string ids)
        {
            var request = new RestRequest("/song/detail");
            request.AddQueryParameter("ids", ids);
            return await Get<SongDetailResponse>(request) ?? new SongDetailResponse();
        }

        public async Task<SongUrlResponse> SongUrl(SongUrlRequest parameters)
            => await Request<SongUrlResponse>("/song/url", parameters);

        public async Task<ToplistArtistResponse> ToplistArtist(int? type = null)
        {
            var requset = new RestRequest("toplist/artist");
            if (type != null)
                requset.AddQueryParameter("type", type.ToString());
            return await Get<ToplistArtistResponse>(requset) ?? new ToplistArtistResponse();
        }

        public async Task<UserCloudResponse> UserCloud(PagedRequestBase parameters)
            => await Request<UserCloudResponse>("/user/cloud", parameters);

        public async Task<UserPlaylistResponse> UserPlaylist(UserPlaylistRequest parameters)
            => await Request<UserPlaylistResponse>("/user/playlist", parameters);

        public async Task<UserRecordResponse> UserRecord(UserRecordRequest parameters)
            => await Request<UserRecordResponse>("/user/record", parameters);

        private Task<T?> Get<T>(RestRequest request) => Client.GetAsync<T>(request);

        private async Task<T> Request<T>(string route, object parameters) where T : new()
            => (await Get<T>(new RestRequest(route).AddQueryParameters(parameters))) ?? new T();
    }
}