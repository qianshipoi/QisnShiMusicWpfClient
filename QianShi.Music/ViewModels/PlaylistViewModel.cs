using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class Playlist1 : BindableBase
    {
        public string AlbumName { get; set; }
        public long Size { get; set; }
        private bool _isPlaying = false;
        public bool IsPlaying { get => _isPlaying; set => SetProperty(ref _isPlaying, value); }
    }

    public class PlaylistViewModel : NavigationViewModel
    {
        private string _title;
        private ObservableCollection<Playlist1> _playlists;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
        public ObservableCollection<Playlist1> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }
        /// <summary>
        /// 播放歌单
        /// </summary>
        public DelegateCommand<Playlist1> PlayCommand { get; private set; }
        /// <summary>
        /// 立即播放
        /// </summary>
        public DelegateCommand<Playlist1> PlayImmediatelyCommand { get; private set; }

        public PlaylistViewModel(IContainerProvider containerProvider) : base(containerProvider)
        {
            _title = string.Empty;
            _playlists = new ObservableCollection<Playlist1>();
            PlayCommand = new DelegateCommand<Playlist1>(Play);
            PlayImmediatelyCommand = new DelegateCommand<Playlist1>(Play);
        }

        void Play(Playlist1 palylist)
        {
            Playlists.Where(x => x.IsPlaying).ToList().ForEach(i => i.IsPlaying = false);
            palylist.IsPlaying = true;
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            var PlaylistId = navigationContext.Parameters.GetValue<long>("PlaylistId");
            Title = PlaylistId.ToString();

            for (int i = 0; i < 10; i++)
            {
                Playlists.Add(new Playlist1
                {
                    AlbumName = $"专辑{i}",
                    Size = i * 1000,
                });
            }

            base.OnNavigatedTo(navigationContext);
        }

    }
}
