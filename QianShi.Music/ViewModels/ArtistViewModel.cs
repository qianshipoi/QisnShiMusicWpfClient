using System.Collections.ObjectModel;

using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class ArtistViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public const string ArtistIdParameterName = "ArtistId";

        private readonly IPlaylistService _playlistService;
        private ObservableCollection<Song> _songs;
        public ObservableCollection<Song> Songs
        {
            get { return _songs; }
            set { SetProperty(ref _songs, value); }
        }
        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set { SetProperty(ref _loading, value); }
        }
        public ArtistViewModel(IContainerProvider containerProvider, IPlaylistService playlistService) : base(containerProvider)
        {
            _playlistService = playlistService;
            _songs = new();
            _loading = false;
        }

        public bool KeepAlive => false;

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            var parameters = navigationContext.Parameters;
            if (parameters.ContainsKey(ArtistIdParameterName))
            {
                var artistId = parameters.GetValue<long>(ArtistIdParameterName);
                Loading = true;
                try
                {
                    var response = await _playlistService.Artists(artistId);
                    if (response.Code == 200)
                    {
                        Songs.AddRange(response.HotSongs);
                    }
                }
                finally
                {
                    Loading = false;
                }
            }
        }
    }
}
