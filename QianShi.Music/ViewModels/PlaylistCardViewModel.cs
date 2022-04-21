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
        private int _offset = 0;
        private int _limit = 100;

        private bool _loading;
        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }

        private ObservableCollection<IPlaylist> _playlists;
        public ObservableCollection<IPlaylist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }
        private bool _more = false;
        public bool More
        {
            get => _more;
            set => SetProperty(ref _more, value);
        }

        public DelegateCommand<IPlaylist> OpenPlaylistCommand { get; private set; }
        public DelegateCommand<ItemsControl> MorePlaylistCommand { get; private set; }

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
        /// <summary>
        /// 更多歌单
        /// </summary>
        async void MorePlaylist(ItemsControl el)
        {
            el.Focus();
            _offset += _limit;
            await LoadPlaylist();
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

        private void OpenPlaylist(IPlaylist obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("PlaylistId", obj.Id);

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("AlbumView", parameters);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_playlists.Count == 0)
            {
                await LoadPlaylist();
            }

            base.OnNavigatedTo(navigationContext);
        }

        async Task UpdatePalylist(IEnumerable<IPlaylist> source, bool isClear = false)
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
