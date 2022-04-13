using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models;
using QianShi.Music.Services;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{

    public class IndexViewModel : NavigationViewModel
    {
        private readonly IMusicService _musicService;
        private ObservableCollection<PlayList> _applyMusic;
        private ObservableCollection<PlayList> _recommendPlayList;
        private ObservableCollection<Singer> _recommendSingerList;
        private ObservableCollection<Album> _newAlbumList;
        private ObservableCollection<Ranking> _rankingList;
        
        public ObservableCollection<PlayList> ApplyMusic
        {
            get => _applyMusic;
            set { _applyMusic = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<PlayList> RecommendPlayList
        {
            get => _recommendPlayList;
            set { _recommendPlayList = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<Singer> RecommendSingerList
        {
            get => _recommendSingerList;
            set { _recommendSingerList = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<Album> NewAlbumList
        {
            get => _newAlbumList;
            set { _newAlbumList = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<Ranking> RankingList
        {
            get => _rankingList;
            set { _rankingList = value; RaisePropertyChanged(); }   
        }

        public IndexViewModel(IContainerProvider provider,
            IMusicService musicService) : base(provider)
        {
            _musicService = musicService;
            _applyMusic = new ObservableCollection<PlayList>();
            _recommendPlayList = new ObservableCollection<PlayList>();
            _recommendSingerList = new ObservableCollection<Singer>();
            _newAlbumList = new ObservableCollection<Album>();
            _rankingList = new ObservableCollection<Ranking>();
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var music = await _musicService.GetApplyMusic();
            _applyMusic.Clear();
            _applyMusic.AddRange(music);

            var recommendPlayList = await _musicService.GetRecommendMusic();
            _recommendPlayList.Clear();
            _recommendPlayList.AddRange(recommendPlayList);

            var recommendSingerList = await _musicService.GetRecommmendSinger();
            _recommendSingerList.Clear();   
            _recommendSingerList.AddRange(recommendSingerList);

            var newAlbumList = await _musicService.GetNewAlbum();
            _newAlbumList.Clear();
            _newAlbumList.AddRange(newAlbumList);

            var rankingList = await _musicService.GetRanking();
            _rankingList.Clear();
            _rankingList.AddRange(rankingList);

            base.OnNavigatedTo(navigationContext);
        }
    }
}
