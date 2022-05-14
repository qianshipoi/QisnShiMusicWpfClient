using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

using QianShi.Music.Common.Models.Response;
using QianShi.Music.Services;

using System.Collections.ObjectModel;

namespace QianShi.Music.ViewModels
{
    public class MvViewModel : NavigationViewModel, IRegionMemberLifetime
    {
        public MvViewModel() : base(App.Current.Container.Resolve<IContainerProvider>()) { }
        public const string MvIdParameter = nameof(MvIdParameter);


        private readonly IPlaylistService _playlistService;
        private long _mvId;
        private Dictionary<int, string> _urls = new();

        private string _url = String.Empty;
        public string Url
        {
            get { return _url; }
            set { SetProperty(ref _url, value); }
        }

        private MvDetail? _detail;
        public MvDetail? Detail
        {
            get { return _detail; }
            set { SetProperty(ref _detail, value); }
        }

        private ObservableCollection<MovieVideo> _movieVideos = new();
        public ObservableCollection<MovieVideo> MovieVideos
        {
            get { return _movieVideos; }
            set { SetProperty(ref _movieVideos, value); }
        }

        private DelegateCommand<MovieVideo> _playMvCommand;
        public DelegateCommand<MovieVideo> PlayMvCommand =>
            _playMvCommand ?? (_playMvCommand = new DelegateCommand<MovieVideo>(async (mv) =>
            {
                if (mv != null)
                {
                    _mvId = mv.Id;
                    await GetMvDetail();
                    await GetSimiMv();
                }
            }));

        public MvViewModel(
            IContainerProvider containerProvider,
            IPlaylistService playlistService)
            : base(containerProvider)
        {
            _playlistService = playlistService;
        }

        public bool KeepAlive => true;

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            if (navigationContext.Parameters.ContainsKey(MvIdParameter))
            {
                _mvId = navigationContext.Parameters.GetValue<long>(MvIdParameter);
                await GetMvDetail();
                await GetSimiMv();
            }
        }

        private async Task GetMvDetail()
        {
            var detailResponse = await _playlistService.MvDetail(_mvId);

            if (detailResponse.Code == 200)
            {
                Detail = detailResponse.Data;
                _urls.Clear();
                foreach (var br in Detail.Brs)
                {
                    var urlResponse = await _playlistService.MvUrl(new Common.Models.Request.MvUrlRequest
                    {
                        Id = _mvId,
                        R = br.Br
                    });
                    if (urlResponse.Code == 200)
                    {
                        _urls.Add(br.Br, urlResponse.Data.Url);
                    }
                }
                if (_urls.Count > 0)
                {
                    Url = _urls[_urls.Max(x => x.Key)];
                }
            }
        }

        private async Task GetSimiMv()
        {
            var response = await _playlistService.SimiMv(_mvId);

            if (response.Code == 200)
            {
                MovieVideos.Clear();
                MovieVideos.AddRange(response.Mvs);
            }
        }
    }
}
