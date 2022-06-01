using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Extensions;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace QianShi.Music.ViewModels
{
    public class PlaylistCardViewModel : NavigationViewModel
    {
        private readonly IPlaylistService _playlistService;
        private readonly IRegionManager _regionManager;
        private int _limit = 100;
        private bool _loading;
        private bool _more = false;
        private int _offset = 0;
        private ObservableCollection<IPlaylist> _playlists;

        public PlaylistCardViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService,
            IRegionManager regionManager)
            : base(containerProvider)
        {
            _loading = false;
            _playlists = new ObservableCollection<IPlaylist>();
            _playlistService = playlistService;
            _regionManager = regionManager;
            OpenPlaylistCommand = new DelegateCommand<IPlaylist>(OpenPlaylist);
            MorePlaylistCommand = new DelegateCommand<ItemsControl>(MorePlaylist);
        }

        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }
        public bool More
        {
            get => _more;
            set => SetProperty(ref _more, value);
        }

        public DelegateCommand<ItemsControl> MorePlaylistCommand { get; private set; }

        public DelegateCommand<IPlaylist> OpenPlaylistCommand { get; private set; }

        public ObservableCollection<IPlaylist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }
        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_playlists.Count == 0)
            {
                await LoadPlaylist();
            }

            base.OnNavigatedTo(navigationContext);
        }

        private async Task LoadPlaylist(bool clear = false)
        {
            Loading = true;
            var response = await _playlistService.GetAlbumNewAsync(new Common.Models.Request.AlbumNewRequest
            {
                Limit = _limit,
                Offset = _offset
            });
            if (response.Code == 200)
            {
                await UpdatePalylist(response.Albums, clear);
                if (response.Total > _offset + _limit)
                {
                    More = true;
                }
            }
            Loading = false;
        }

        /// <summary>
        /// 更多歌单
        /// </summary>
        private async void MorePlaylist(ItemsControl el)
        {
            el.Focus();
            _offset += _limit;
            await LoadPlaylist();
        }
        private void OpenPlaylist(IPlaylist obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("PlaylistId", obj.Id);

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("AlbumView", parameters);
        }
        private async Task UpdatePalylist(IEnumerable<IPlaylist> source, bool isClear = false)
        {
            if (isClear)
                Playlists.Clear();
            int i = 0;
            foreach (var sourceItem in source.Where(x => !string.IsNullOrWhiteSpace(x.CoverImgUrl)))
            {
                sourceItem.CoverImgUrl += "?param=200y200";
                Playlists.Add(sourceItem);
                if (i % 5 == 0)
                    await Task.Delay(20);
                i++;
            }
        }
    }
}