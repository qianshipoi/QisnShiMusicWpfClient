using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Ioc;
using Prism.Navigation;
using Prism.Regions;

using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class ArtistViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        private readonly IPlaylistService _playlistService;

        public ArtistViewModel(IContainerProvider containerProvider, IPlaylistService playlistService) : base(containerProvider)
        {
            _playlistService = playlistService;
        }

        public bool KeepAlive => false;

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);



        }
    }
}
