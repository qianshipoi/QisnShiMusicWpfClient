using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace QianShi.Music.ViewModels
{
    public class PlaylistCardViewModel : NavigationViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPlaylistService _playlistService;
        private int _limit = 100;
        private bool _more = false;
        private DelegateCommand<ItemsControl> _morePlaylistCommand = default!;
        private int _offset = 0;
        private DelegateCommand<IPlaylist> _openPlaylistCommand = default!;

        public PlaylistCardViewModel(
                    IContainerProvider containerProvider,
            IPlaylistService playlistService,
            INavigationService navigationService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
            _navigationService = navigationService;
        }

        public bool More
        {
            get => _more;
            set => SetProperty(ref _more, value);
        }

        public DelegateCommand<ItemsControl> MorePlaylistCommand =>
            _morePlaylistCommand ??= new(MorePlaylist);
        public DelegateCommand<IPlaylist> OpenPlaylistCommand =>
            _openPlaylistCommand ??= new(OpenPlaylist);

        public ObservableCollection<IPlaylist> Playlists { get; } = new();

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (Playlists.Count == 0)
            {
                await LoadPlaylist();
            }

            base.OnNavigatedTo(navigationContext);
        }

        private async Task LoadPlaylist(bool clear = false)
        {
            IsBusy = true;
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
            IsBusy = false;
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
            _navigationService.NavigateToAlbum(obj.Id);
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