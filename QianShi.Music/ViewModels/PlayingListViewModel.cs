using Prism.Ioc;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using Prism.Commands;

namespace QianShi.Music.ViewModels
{
    public class PlayingListViewModel : NavigationViewModel
    {
        private readonly IPlayService _playService;
        private readonly IPlayStoreService _playStoreService;
        private Song? _currentSong;
        private ObservableCollection<Song> _jumpPlayeds;
        private DelegateCommand<Song> _playCommand = default!;
        private ObservableCollection<Song> _toBePlayeds;
        public PlayingListViewModel(
            IContainerProvider containerProvider,
            IPlayStoreService playStoreService,
            IPlayService playService)
            : base(containerProvider)
        {
            _playStoreService = playStoreService;
            _playService = playService;
            _toBePlayeds = playStoreService.LaterPlaylist;
            _jumpPlayeds = playStoreService.JumpTheQueuePlaylist;
            _currentSong = playStoreService.Current;
            playStoreService.CurrentChanged += (s, e) => CurrentSong = e.NewSong;
        }

        public Song? CurrentSong
        {
            get => _currentSong;
            set => SetProperty(ref _currentSong, value);
        }

        public ObservableCollection<Song> JumpPlayeds
        {
            get => _jumpPlayeds;
            set => SetProperty(ref _jumpPlayeds, value);
        }

        public DelegateCommand<Song> PlayCommand =>
                                    _playCommand ??= new((song) => _playStoreService.PlayAsync(song));
        public ObservableCollection<Song> ToBePlayeds
        {
            get => _toBePlayeds;
            set => SetProperty(ref _toBePlayeds, value);
        }
    }
}