using Prism.Regions;

using QianShi.Music.Common.Models.Request;

namespace QianShi.Music.Services
{
    public interface INavigationService
    {
        /// <summary>
        /// 导航到专辑
        /// </summary>
        /// <param name="id">专辑ID</param>
        void NavigateToAlbum(long id);

        /// <summary>
        /// 导航到艺人
        /// </summary>
        /// <param name="id">艺人ID</param>
        void NavigateToArtist(long id);

        /// <summary>
        /// 导航到MV
        /// </summary>
        /// <param name="id">MVID</param>
        void NavigateToMv(long id);

        /// <summary>
        /// 导航到歌单详情
        /// </summary>
        /// <param name="id">歌单ID</param>
        void NavigateToPlaylist(long id);

        /// <summary>
        /// 导航到搜索结果
        /// </summary>
        /// <param name="keywords">搜索关键字</param>
        void NavigateToSearch(string keywords);

        /// <summary>
        /// 导航到搜索详情
        /// </summary>
        /// <param name="keywords">搜索关键字</param>
        /// <param name="type">搜索类型</param>
        void NavigateToSearchDetail(string keywords, SearchType type);

        /// <summary>
        /// 导航到发现
        /// </summary>
        /// <param name="type">发现类型</param>
        void NavigateToFound(string? type);

        /// <summary>
        /// 导航到喜欢的歌单
        /// </summary>
        /// <param name="id">歌单编号</param>
        void NavigateToFondPlaylist(long id);

        /// <summary>
        /// 基础导航
        /// </summary>
        /// <param name="region">区域</param>
        /// <param name="viewName">视图名称</param>
        /// <param name="parameters">参数</param>
        void Navigation(string region, string viewName, NavigationParameters? parameters = null);

        /// <summary>
        /// 全屏导航
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="parameters">参数</param>
        void FullRegionNavigation(string viewName, NavigationParameters? parameters = null);

        /// <summary>
        /// 主页面导航
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="parameters">参数</param>
        void MainRegionNavigation(string viewName, NavigationParameters? parameters = null);
    }
}