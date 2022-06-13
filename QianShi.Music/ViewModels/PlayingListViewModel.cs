using Prism.Commands;
using Prism.Ioc;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class PlayingListViewModel : NavigationViewModel
    {
        private readonly IPlayStoreService _playStoreService;
        private Song? _currentSong;
        private DelegateCommand<Song> _playCommand = default!;

        public PlayingListViewModel(
            IContainerProvider containerProvider,
            IPlayStoreService playStoreService)
            : base(containerProvider)
        {
            _playStoreService = playStoreService;
            _currentSong = playStoreService.Current;
            playStoreService.CurrentChanged += (s, e) => CurrentSong = e.NewSong;
        }

        public Song? CurrentSong
        {
            get => _currentSong;
            set => SetProperty(ref _currentSong, value);
        }

        public ObservableCollection<Song> JumpPlayeds => _playStoreService.JumpTheQueuePlaylist;

        public DelegateCommand<Song> PlayCommand =>
            _playCommand ??= new((song) => _playStoreService.PlayAsync(song));

        public ObservableCollection<Song> ToBePlayeds => _playStoreService.LaterPlaylist;
    }
}