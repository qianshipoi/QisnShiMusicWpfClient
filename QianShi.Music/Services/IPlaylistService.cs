using QianShi.Music.Common.Models.Request;
using QianShi.Music.Common.Models.Response;

using System.ComponentModel;

namespace QianShi.Music.Services
{
    public interface IPlaylistService
    {
        /// <summary>
        /// 获取歌单分类
        /// </summary>
        /// <returns></returns>
        Task<CatlistResponse> GetCatlistAsync();
        /// <summary>
        /// 热门歌单分类
        /// </summary>
        /// <returns></returns>
        Task<PlaylistHotResponse> GetHotAsync();
        /// <summary>
        /// 歌单（网友精选碟）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TopPlaylistResponse?> GetTopPlaylistAsync(TopPlaylistRequest parameter);
        /// <summary>
        /// 精品歌单标签列表
        /// </summary>
        /// <returns></returns>
        Task<PlaylistHighqualityTagsResponse> GetPlaylistHighqualityTagsAsync();
        /// <summary>
        /// 获取精品歌单
        /// </summary>
        /// <returns></returns>
        Task<TopPlaylistHighqualityResponse> GetTopPlaylistHighqualityAsnyc(TopPlaylistHighqualityRequest parameter);
        /// <summary>
        /// 相关歌单推荐
        /// </summary>
        /// <returns></returns>
        Task<RelatedPlaylistResponse> GetRelatedPlaylistAsync(string id);

        /// <summary>
        /// 获取推荐歌单
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        Task<PersonalizedResponse> GetPersonalizedAsync(int? limit = null);
        /// <summary>
        /// 获取排行榜列表
        /// </summary>
        /// <returns></returns>
        Task<ToplistResponse> GetToplistAsync();
        /// <summary>
        /// 最新专辑
        /// </summary>
        /// <returns></returns>
        Task<AlbumNewestResponse> GetAlbumNewestAsync();

        /// <summary>
        /// 获取歌单详情
        /// </summary>
        /// <returns></returns>
        Task<PlaylistDetailResponse> GetPlaylistDetailAsync(long playlistId);

        /// <summary>
        /// 全部新专辑
        /// </summary>
        /// <returns></returns>
        Task<AlbumNewResponse> GetAlbumNewAsync(AlbumNewRequest parameters);

        /// <summary>
        /// 获取专辑内容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AlbumResponse> GetAblumAsync(long id);

        /// <summary>
        /// 歌手榜
        /// </summary>
        /// <param name="type">地区 1: 华语 2: 欧美 3: 韩国 4: 日本</param>
        /// <returns></returns>
        Task<ToplistArtistResponse> ToplistArtist(int? type = null);

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="parasmeters"></param>
        /// <returns></returns>
        Task<SearchResponse> Search(SearchRequest parasmeters);
    }
}
