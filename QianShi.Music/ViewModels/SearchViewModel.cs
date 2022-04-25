using Prism.Ioc;
using Prism.Regions;

using QianShi.Music.Common.Data;
using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static QianShi.Music.Common.Models.Response.SongSearchResult;

namespace QianShi.Music.ViewModels
{
    public class SearchViewModel : NavigationViewModel
    {
        private readonly IPlaylistService _playlistService;

        public ObservableCollection<Artist> Artists { get; set; }
        public ObservableCollection<Album> Albums { get; set; }
        public ObservableCollection<Song> Songs { get; set; }
        public ObservableCollection<Playlist> Playlists { get; set; }
        public ObservableCollection<MovieVideo> MovieVideos { get; set; }

        public SearchViewModel(IContainerProvider containerProvider, IPlaylistService playlistService) : base(containerProvider)
        {
            _playlistService = playlistService;
        }




        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var parameters = navigationContext.Parameters;
            if (parameters.ContainsKey("SearchText"))
            {
                var searchText = parameters.GetValue<string>("SearchText");
                var response = await _playlistService.Search(new Common.Models.Request.SearchRequest
                {
                    Keywords = searchText,
                    Type = Common.Models.Request.SearchRequest.SearchType.单曲
                });

                var result  = response.Result;

            }





            base.OnNavigatedTo(navigationContext);
        }

    }
}
