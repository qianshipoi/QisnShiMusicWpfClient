using QianShi.Music.Common.Models.Request;
using QianShi.Music.Extensions;
using QianShi.Music.ViewModels;
using QianShi.Music.Views;

namespace QianShi.Music.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IRegionManager _regionManager;

        public NavigationService(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Navigation(string region, string viewName, NavigationParameters? parameters = null)
        {
            _regionManager.Regions[region].RequestNavigate(viewName, parameters ?? new());
        }

        public void MainRegionNavigation(string viewName, NavigationParameters? parameters = null)
             => Navigation(PrismManager.MainViewRegionName, viewName, parameters);

        public void FullRegionNavigation(string viewName, NavigationParameters? parameters = null)
             => Navigation(PrismManager.FullScreenRegionName, viewName, parameters);

        /// <summary>
        /// 导航到专辑
        /// </summary>
        public void NavigateToAlbum(long id)
            => MainRegionNavigation(nameof(AlbumView), new NavigationParameters {
                {  AlbumViewModel.AlbumIdParameterName , id}
            });

        /// <summary>
        /// 导航到艺人
        /// </summary>
        public void NavigateToArtist(long id)
            => MainRegionNavigation(nameof(ArtistView), new NavigationParameters {
                {  ArtistViewModel.ArtistIdParameterName , id}
            });

        /// <summary>
        /// 导航到MV
        /// </summary>
        public void NavigateToMv(long id)
            => MainRegionNavigation(nameof(MvView), new NavigationParameters {
                {  MvViewModel.MvIdParameter , id}
            });

        /// <summary>
        /// 导航到歌单详情
        /// </summary>
        public void NavigateToPlaylist(long id)
            => MainRegionNavigation(nameof(PlaylistView), new NavigationParameters {
                {  PlaylistViewModel.IdParameters , id}
            });

        /// <summary>
        /// 导航到搜索结果
        /// </summary>
        public void NavigateToSearch(string keywords)
            => MainRegionNavigation(nameof(SearchView), new NavigationParameters {
                {  SearchViewModel.SearchTextParameter ,keywords}
            });

        /// <summary>
        /// 导航到搜索详情
        /// </summary>
        public void NavigateToSearchDetail(string keywords, SearchType type)
            => MainRegionNavigation(nameof(SearchDetailView), new NavigationParameters {
                {  SearchDetailViewModel.SearchKeywordsParameterName ,keywords},
                {  SearchDetailViewModel.SearchTypeParameterName ,type}
            });

        /// <summary>
        /// 导航到发现
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void NavigateToFound(string? type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                MainRegionNavigation(nameof(FoundView), new NavigationParameters {
                    {  FoundViewModel.PlaylistTypeParameterName ,type},
                });
            }
            else
            {
                MainRegionNavigation(nameof(FoundView));
            }
        }

        /// <summary>
        /// 导航到喜欢的歌单
        /// </summary>
        /// <param name="id">歌单编号</param>
        public void NavigateToFondPlaylist(long id)
            => MainRegionNavigation(nameof(FondPlaylistView), new NavigationParameters {
                    {  FondPlaylistViewModel.PlaylistIdParameterName ,id},
                });
    }
}