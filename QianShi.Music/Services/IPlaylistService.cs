using QianShi.Music.Common.Models.Request;
using QianShi.Music.Common.Models.Response;

namespace QianShi.Music.Services
{
    public interface IPlaylistService
    {
        /// <summary>
        /// 获取已收藏专辑列表
        /// </summary>
        /// <returns></returns>
        Task<AlbumSublistResponse> AlbumSublist(PagedRequestBase parameters);

        /// <summary>
        /// 获取歌手专辑
        /// </summary>
        /// <param name="id">歌手id</param>
        /// <returns></returns>
        Task<ArtistAlbumResponse> ArtistAlbum(ArtistAlbumRequest parameters);

        /// <summary>
        /// 获取歌手MV
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<ArtistMvResponse> ArtistMv(ArtistMvRequest parameters);

        /// <summary>
        /// 获取歌手单曲
        /// </summary>
        /// <param name="id">歌手 id, 可由搜索接口获得</param>
        /// <returns></returns>
        Task<ArtistsResponse> Artists(long id);

        /// <summary>
        /// 收藏的歌手列表
        /// </summary>
        /// <returns></returns>
        Task<ArtistSublistResponse> ArtistSublist(PagedRequestBase parameters);

        /// <summary>
        /// 获取专辑内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AlbumResponse> GetAblumAsync(long id);

        /// <summary>
        /// 全部新专辑
        /// </summary>
        /// <returns></returns>
        Task<AlbumNewResponse> GetAlbumNewAsync(AlbumNewRequest parameters);

        /// <summary>
        /// 最新专辑
        /// </summary>
        /// <returns></returns>
        Task<AlbumNewestResponse> GetAlbumNewestAsync();

        /// <summary>
        /// 获取歌单分类
        /// </summary>
        /// <returns></returns>
        Task<CatlistResponse> GetCatlistAsync();

        CookieCollection? GetCookieCollection();

        /// <summary>
        /// 热门歌单分类
        /// </summary>
        /// <returns></returns>
        Task<PlaylistHotResponse> GetHotAsync();

        /// <summary>
        /// 获取推荐歌单
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<PersonalizedResponse> GetPersonalizedAsync(int? limit = null);

        /// <summary>
        /// 获取歌单详情
        /// </summary>
        /// <returns></returns>
        Task<PlaylistDetailResponse> GetPlaylistDetailAsync(long playlistId);

        /// <summary>
        /// 精品歌单标签列表
        /// </summary>
        /// <returns></returns>
        Task<PlaylistHighqualityTagsResponse> GetPlaylistHighqualityTagsAsync();

        /// <summary>
        /// 相关歌单推荐
        /// </summary>
        /// <returns></returns>
        Task<RelatedPlaylistResponse> GetRelatedPlaylistAsync(string id);

        /// <summary>
        /// 获取排行榜列表
        /// </summary>
        /// <returns></returns>
        Task<ToplistResponse> GetToplistAsync();

        /// <summary>
        /// 歌单（网友精选碟）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TopPlaylistResponse?> GetTopPlaylistAsync(TopPlaylistRequest parameter);

        /// <summary>
        /// 获取精品歌单
        /// </summary>
        /// <returns></returns>
        Task<TopPlaylistHighqualityResponse> GetTopPlaylistHighqualityAsnyc(TopPlaylistHighqualityRequest parameter);

        /// <summary>
        /// 喜欢音乐列表
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns></returns>
        Task<LikelistResponse> Likelist(long uid);

        /// <summary>
        /// 二维码登录第三步 检查授权
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<LoginQrCheckResponse> LoginQrCheck(string key);

        /// <summary>
        /// 二维码登录第二步 创建链接
        /// </summary>
        /// <param name="qrKey"></param>
        /// <returns></returns>
        Task<LoginQrCreateResponse> LoginQrCreate(string key, bool isBase64 = false);

        /// <summary>
        /// 二维码登录第一步 获取key
        /// </summary>
        /// <returns></returns>
        Task<LoginQrKeyResponse> LoginQrKey();

        /// <summary>
        /// 登录状态
        /// </summary>
        /// <returns></returns>
        Task<LoginStatusResponse> LoginStatus();

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        Task<LogoutResponse> Logout();

        /// <summary>
        /// 收藏的 MV 列表
        /// </summary>
        /// <returns></returns>
        Task<MvSublistResponse> MvSublist(PagedRequestBase parameters);

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="parasmeters"></param>
        /// <returns></returns>
        Task<SearchResponse> Search(SearchRequest parasmeters);

        /// <summary>
        /// 搜索专辑
        /// </summary>
        /// <param name="parasmeters"></param>
        /// <returns></returns>
        Task<SearchResponse<AlbumSearchResult>> SearchAlbum(SearchRequest parameters);

        /// <summary>
        /// 搜索歌手
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<SearchResponse<ArtistSearchResult>> SearchArtist(SearchRequest parameters);

        /// <summary>
        /// 搜索MV
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<SearchResponse<MovieVideoSearchResult>> SearchMovieVideo(SearchRequest parameters);

        /// <summary>
        /// 搜索歌单
        /// </summary>
        /// <param name="parasmeters"></param>
        /// <returns></returns>
        Task<SearchResponse<PlaylistSearchResult>> SearchPlaylist(SearchRequest parameters);

        /// <summary>
        /// 搜索单曲
        /// </summary>
        /// <param name="parasmeters"></param>
        /// <returns></returns>
        Task<SearchResponse<SongSearchResult>> SearchSong(SearchRequest parameters);

        void SetCookie(CookieCollection cookies);

        void SetCookie(Cookie cookie);

        /// <summary>
        /// 获取相似歌手
        /// </summary>
        /// <param name="id">歌手 id</param>
        /// <returns></returns>
        Task<SimiArtistResponse> SimiArtist(long id);

        /// <summary>
        /// 获取歌曲详情
        /// </summary>
        /// <param name="ids">音乐 id, 如 ids=347230</param>
        /// <returns></returns>
        Task<SongDetailResponse> SongDetail(string ids);

        /// <summary>
        /// 歌手榜
        /// </summary>
        /// <param name="type">地区 1: 华语 2: 欧美 3: 韩国 4: 日本</param>
        /// <returns></returns>
        Task<ToplistArtistResponse> ToplistArtist(int? type = null);

        /// <summary>
        /// 云盘
        /// </summary>
        /// <returns></returns>
        Task<UserCloudResponse> UserCloud(PagedRequestBase parameters);

        /// <summary>
        /// 获取用户歌单
        /// </summary>
        /// <returns></returns>
        Task<UserPlaylistResponse> UserPlaylist(UserPlaylistRequest parameters);

        /// <summary>
        /// 获取用户播放记录
        /// </summary>
        /// <returns></returns>
        Task<UserRecordResponse> UserRecord(UserRecordRequest parameters);

        /// <summary>
        /// 获取音乐 url
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<SongUrlResponse> SongUrl(SongUrlRequest parameters);

        /// <summary>
        /// 获取歌词
        /// </summary>
        /// <param name="id">音乐 id</param>
        /// <returns></returns>
        Task<LyricResponse> Lyric(long id);

        /// <summary>
        /// 获取 mv 数据
        /// </summary>
        /// <param name="mvid">mv 的 id</param>
        /// <returns></returns>
        Task<MvDetailResponse> MvDetail(long mvid);

        /// <summary>
        /// mv 地址
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<MvUrlResponse> MvUrl(MvUrlRequest parameters);

        /// <summary>
        /// 相似 mv
        /// </summary>
        /// <param name="mvid"> mv id</param>
        /// <returns></returns>
        Task<SimiMvResponse> SimiMv(long mvid);

        /// <summary>
        /// 邮箱登录
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<LoginResponse> Login(LoginRequest parameters);

        /// <summary>
        /// 手机号登录
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<LoginResponse> LoginCellPhone(LoginCellphoneRequest parameters);
    }
}