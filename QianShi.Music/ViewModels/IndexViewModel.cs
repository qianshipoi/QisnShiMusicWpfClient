using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Services;
using QianShi.Music.Views;

using System.Collections.ObjectModel;
using System.Windows;

namespace QianShi.Music.ViewModels
{
    public class IndexViewModel : NavigationViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPlaylistService _playlistService;
        private DelegateCommand<string> _jumpFoundCommand = default!;
        private DelegateCommand<IPlaylist> _openArtistCommand = default!;
        private DelegateCommand<IPlaylist> _openPlaylistCommand = default!;

        public IndexViewModel(
            IContainerProvider provider,
            IPlaylistService playlistService,
            INavigationService navigationService)
            : base(provider)
        {
            _playlistService = playlistService;
            _navigationService = navigationService;
        }

        public ObservableCollection<IPlaylist> NewAlbumList { get; } = new();

        public ObservableCollection<IPlaylist> RankingList { get; } = new();

        public ObservableCollection<IPlaylist> RecommendPlayList { get; } = new();

        public ObservableCollection<IPlaylist> RecommendSingerList { get; } = new();

        public DelegateCommand<string> JumpFoundCommand =>
            _jumpFoundCommand ?? (_jumpFoundCommand = new DelegateCommand<string>((obj) =>
            {
                if (obj == "新专速递")
                {
                    _navigationService.MainRegionNavigation(nameof(PlaylistCardView));
                }
                else
                {
                    _navigationService.NavigateToFound(obj);
                }
            }));

        public DelegateCommand<IPlaylist> OpenArtistCommand =>
            _openArtistCommand ??= new((playlist) => _navigationService.NavigateToArtist(playlist.Id));

        public DelegateCommand<IPlaylist> OpenPlaylistCommand =>
            _openPlaylistCommand ??= new((obj) =>
            {
                if (obj is Common.Models.Response.Album)
                {
                    _navigationService.NavigateToAlbum(obj.Id);
                }
                else
                {
                    _navigationService.NavigateToPlaylist(obj.Id);
                }
            });

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var actions = new List<Action>();

            if (RecommendPlayList.Count == 0)
            {
                actions.Add(async () =>
                {
                    var recommendPlaylistResponse = await _playlistService.GetPersonalizedAsync(10);
                    if (recommendPlaylistResponse != null && recommendPlaylistResponse.Code == 200)
                    {
                        await UpdatePlaylist(RecommendPlayList, recommendPlaylistResponse.Result);
                    }
                });
            }

            if (RecommendSingerList.Count == 0)
            {
                actions.Add(async () =>
                {
                    var response = await _playlistService.ToplistArtist();
                    if (response != null && response.Code == 200)
                    {
                        await UpdatePlaylist(RecommendSingerList, response.List.Artists.Take(5));
                    }
                });
            }

            if (NewAlbumList.Count == 0)
            {
                actions.Add(async () =>
                {
                    var newAlbumsResponse = await _playlistService.GetAlbumNewestAsync();
                    if (newAlbumsResponse.Code == 200)
                    {
                        await UpdatePlaylist(NewAlbumList, newAlbumsResponse.Albums.Take(10));
                    }
                });
            }

            if (RankingList.Count == 0)
            {
                actions.Add(async () =>
                {
                    var rankingResponse = await _playlistService.GetToplistAsync();
                    if (rankingResponse.Code == 200)
                    {
                        await UpdatePlaylist(RankingList, rankingResponse.List.Take(10));
                    }
                });
            }

            Parallel.For(0, actions.Count, i => actions[i].Invoke());

            base.OnNavigatedTo(navigationContext);
        }

        private async Task UpdatePlaylist(ObservableCollection<IPlaylist> source, IEnumerable<IPlaylist> target)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                source.Clear();
                int i = 0;
                foreach (var sourceItem in target.Where(x => !string.IsNullOrWhiteSpace(x.CoverImgUrl)))
                {
                    sourceItem.CoverImgUrl += "?param=200y200";
                    source.Add(sourceItem);
                    if (i % 5 == 0)
                        await Task.Delay(20);
                    i++;
                }
            });
        }
    }
}