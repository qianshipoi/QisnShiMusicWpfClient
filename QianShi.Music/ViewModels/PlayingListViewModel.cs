using Prism.Ioc;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class PlayingListViewModel : NavigationViewModel
    {
        private readonly IPlayService _playService;
        private ObservableCollection<Song> _toBePlayeds;
        public ObservableCollection<Song> ToBePlayeds
        {
            get { return _toBePlayeds; }
            set { SetProperty(ref _toBePlayeds, value); }
        }

        private ObservableCollection<Song> _jumpPlayeds;
        public ObservableCollection<Song> JumpPlayeds
        {
            get { return _jumpPlayeds; }
            set { SetProperty(ref _jumpPlayeds, value); }
        }

        private Song? _currentSong;
        public Song? CurrentSong
        {
            get { return _currentSong; }
            set { SetProperty(ref _currentSong, value); }
        }

        public PlayingListViewModel(IContainerProvider containerProvider, IPlayService playService) : base(containerProvider)
        {
            _playService = playService;
            _toBePlayeds = _playService.ToPlay;
            _jumpPlayeds = _playService.JumpPlay;
            _currentSong = _playService.Current;
            _playService.CurrentChanged += (s, e) => CurrentSong = e.NewSong;
        }
    }
}
