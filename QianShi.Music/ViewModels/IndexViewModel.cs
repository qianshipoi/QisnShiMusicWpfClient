using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Extensions;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using System.Windows;

namespace QianShi.Music.ViewModels
{

    public class IndexViewModel : NavigationViewModel
    {
        private readonly IMusicService _musicService;
        private readonly IRegionManager _regionManager;
        private readonly IPlaylistService _playlistService;
        private ObservableCollection<PlayList> _applyMusic;
        private ObservableCollection<IPlaylist> _recommendPlayList;
        private ObservableCollection<Singer> _recommendSingerList;
        private ObservableCollection<IPlaylist> _newAlbumList;
        private ObservableCollection<IPlaylist> _rankingList;

        public ObservableCollection<PlayList> ApplyMusic
        {
            get => _applyMusic;
            set { _applyMusic = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<IPlaylist> RecommendPlayList
        {
            get => _recommendPlayList;
            set { _recommendPlayList = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<Singer> RecommendSingerList
        {
            get => _recommendSingerList;
            set { _recommendSingerList = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<IPlaylist> NewAlbumList
        {
            get => _newAlbumList;
            set { _newAlbumList = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<IPlaylist> RankingList
        {
            get => _rankingList;
            set { _rankingList = value; RaisePropertyChanged(); }
        }
        public DelegateCommand<IPlaylist> OpenPlaylistCommand { get; private set; }

        public IndexViewModel(IContainerProvider provider,
            IMusicService musicService, IPlaylistService playlistService, IRegionManager regionManager) : base(provider)
        {
            _musicService = musicService;
            _playlistService = playlistService;
            _applyMusic = new ObservableCollection<PlayList>();
            _recommendPlayList = new ObservableCollection<IPlaylist>();
            _recommendSingerList = new ObservableCollection<Singer>();
            _newAlbumList = new ObservableCollection<IPlaylist>();
            _rankingList = new ObservableCollection<IPlaylist>();
            OpenPlaylistCommand = new DelegateCommand<IPlaylist>(OpenPlaylist);
            _regionManager = regionManager;
        }

        private void OpenPlaylist(IPlaylist obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("PlaylistId", obj.Id);

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("PlaylistView", parameters);
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var music = await _musicService.GetApplyMusic();
            _applyMusic.Clear();
            _applyMusic.AddRange(music);

            var actions = new List<Action>();

            if (_recommendPlayList.Count == 0)
            {
                actions.Add(async () =>
                {
                    var recommendPlaylistResponse = await _playlistService.GetPersonalizedAsync(10);
                    if (recommendPlaylistResponse != null && recommendPlaylistResponse.Code == 200)
                    {
                        await UpdatePlaylist(_recommendPlayList, recommendPlaylistResponse.Result);
                    }
                });
            }

            var recommendSingerList = await _musicService.GetRecommmendSinger();
            _recommendSingerList.Clear();
            _recommendSingerList.AddRange(recommendSingerList);

            if (_newAlbumList.Count == 0)
            {
                actions.Add(async () =>
                {
                    var newAlbumsResponse = await _playlistService.GetAlbumNewestAsync();
                    if (newAlbumsResponse.Code == 200)
                    {
                        await UpdatePlaylist(_newAlbumList, newAlbumsResponse.Albums.Take(10));
                    }
                });
            }

            if (_rankingList.Count == 0)
            {
                actions.Add(async () =>
                {
                    var rankingResponse = await _playlistService.GetToplistAsync();
                    if (rankingResponse.Code == 200)
                    {
                        await UpdatePlaylist(_rankingList, rankingResponse.List.Take(10));
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
