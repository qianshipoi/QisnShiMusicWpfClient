using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Request;
using QianShi.Music.Extensions;
using QianShi.Music.Services;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace QianShi.Music.ViewModels
{
    public class FoundViewModel : NavigationViewModel
    {
        private readonly IPlaylistService _playlistService;
        private readonly IRegionManager _regionManager;
        private readonly IDialogHostService _dialogHostService;
        private ObservableCollection<Cat> _cats;
        private ObservableCollection<CatOption> _catOptions;
        private Visibility _moreCat = Visibility.Collapsed;
        private ObservableCollection<IPlaylist> _playlists;
        private Cat? _currentCat;
        private bool _loading = false;
        private bool _more = false;
        private int _offset = 0;
        private long _before = 0;
        public ObservableCollection<Cat> Cats
        {
            get => _cats;
            set { _cats = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<CatOption> CatOptions
        {
            get => _catOptions;
            set { _catOptions = value; RaisePropertyChanged(); }
        }
        public Visibility MoreCat
        {
            get => _moreCat;
            set { _moreCat = value; RaisePropertyChanged(); }
        }
        public bool Loading
        {
            get => _loading;
            set => SetProperty(ref _loading, value);
        }
        public bool More
        {
            get => _more;
            set => SetProperty(ref _more, value);
        }
        public ObservableCollection<IPlaylist> Playlists
        {
            get => _playlists;
            set => SetProperty(ref _playlists, value);
        }
        public DelegateCommand<Cat> SwitchMoreCatCommand { get; private set; }
        public DelegateCommand<Cat> AddCatCommand { get; private set; }
        public DelegateCommand<Cat> SelectedCatCommand { get; private set; }
        public DelegateCommand<ItemsControl> MorePlaylistCommand { get; private set; }
        public DelegateCommand<IPlaylist> OpenPlaylistCommand { get; private set; }
        public FoundViewModel(
            IContainerProvider provider,
            IRegionManager regionManager,
            IPlaylistService playlistService,
            IDialogHostService dialogHostService)
            : base(provider)
        {
            _playlistService = playlistService;
            _regionManager = regionManager;
            _dialogHostService = dialogHostService;
            _cats = new ObservableCollection<Cat>();
            _catOptions = new ObservableCollection<CatOption>();
            _playlists = new ObservableCollection<IPlaylist>();
            _currentCat = null;
            SwitchMoreCatCommand = new DelegateCommand<Cat>(SwitchMoreCat);
            AddCatCommand = new DelegateCommand<Cat>(AddCat);
            SelectedCatCommand = new DelegateCommand<Cat>(SelectedCat);
            MorePlaylistCommand = new DelegateCommand<ItemsControl>(MorePlaylist);
            OpenPlaylistCommand = new DelegateCommand<IPlaylist>(OpenPlaylist);
        }

        void OpenPlaylist(IPlaylist obj)
        {
            var parameters = new NavigationParameters();
            parameters.Add("PlaylistId", obj.Id);

            _regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("PlaylistView", parameters);
        }

        /// <summary>
        /// 更多歌单
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        async void MorePlaylist(ItemsControl el)
        {
            if (_currentCat != null)
            {
                el.Focus();
                await CallApi(_currentCat);
            }
        }

        /// <summary>
        /// 获取精选歌单
        /// </summary>
        /// <returns></returns>
        async Task QuerySelectPlaylist(bool isClear = false)
        {
            var response = await _playlistService.GetTopPlaylistHighqualityAsnyc(new TopPlaylistHighqualityRequest
            {
                Limit = 100,
                Before = _before == 0 ? null : _before,
            });

            if (response != null)
            {
                await UpdatePalylist(response.Playlists, isClear);
                More = response.More;
                _before = response.Lasttime;
            }
        }

        async Task UpdatePalylist(IEnumerable<IPlaylist> source, bool isClear = false)
        {
            if (isClear)
                Playlists.Clear();
            int i = 0;
            foreach (var sourceItem in source)
            {
                sourceItem.CoverImgUrl += "?param=200y200";
                Playlists.Add(sourceItem);
                if (i % 5 == 0)
                    await Task.Delay(20);
                i++;
            }
        }

        /// <summary>
        /// 获取类别歌单
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        async Task QueryCatPlaylist(string catName, bool isClear = false)
        {
            var response = await _playlistService.GetTopPlaylistAsync(new TopPlaylistRequest
            {
                Cat = catName,
                Offset = _offset
            });
            if (response != null)
            {
                await UpdatePalylist(response.Playlists, isClear);
                More = response.More;
                _offset += response.Playlists.Count;
            }
        }

        /// <summary>
        /// 获取推荐歌单
        /// </summary>
        /// <returns></returns>
        async Task QueryRecommendedPalylist()
        {
            var response = await _playlistService.GetPersonalizedAsync();
            if (response != null)
            {
                await UpdatePalylist(response.Result, true);
                More = false;
            }
        }

        /// <summary>
        /// 获取排行榜列表
        /// </summary>
        /// <returns></returns>
        async Task QueryToplist()
        {
            var response = await _playlistService.GetToplistAsync();
            if (response != null)
            {
                await UpdatePalylist(response.List, true);
                More = false;
            }
        }

        async void SelectedCat(Cat cat)
        {
            if (_currentCat == cat) return;
            if (_currentCat != null)
                _currentCat.IsActivation = false;
            cat.IsActivation = true;
            _currentCat = cat;
            _offset = 0;
            _before = 0;
            _more = false;

            await CallApi(cat, true);

        }

        async Task CallApi(Cat cat, bool isClear = false)
        {
            Loading = true;
            try
            {
                List<IPlaylist> playlists = new List<IPlaylist>();
                switch (cat.Name)
                {
                    case "精品":
                        await QuerySelectPlaylist(isClear);
                        break;
                    case "推荐":
                        await QueryRecommendedPalylist();
                        break;
                    case "排行榜":
                        await QueryToplist();
                        break;
                    default:
                        await QueryCatPlaylist(cat.Name, isClear);
                        break;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Loading = false;
            }
        }

        void AddCat(Cat cat)
        {
            if (_cats.Any(x => x.Equals(cat)))
            {
                cat.IsSelected = false;
                _cats.Remove(cat);
            }
            else
            {
                cat.IsSelected = true;
                _cats.Insert(_cats.Count - 1, cat);
            }
        }

        void SwitchMoreCat(Cat cat)
        {
            cat.IsActivation = !cat.IsActivation;
            MoreCat = MoreCat == Visibility.Collapsed
                ? MoreCat = Visibility.Visible
                : MoreCat = Visibility.Collapsed;
        }

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (Cats.Count == 0)
            {
                Cats.Add(new Cat { DisplayName = "全部", Name = "全部" });
                Cats.Add(new Cat { DisplayName = "推荐歌单", Name = "推荐" });
                Cats.Add(new Cat { DisplayName = "精品歌单", Name = "精品" });
                Cats.Add(new Cat { DisplayName = "官方", Name = "官方" });
                Cats.Add(new Cat { DisplayName = "排行榜", Name = "排行榜" });
                Cats.Add(new Cat { DisplayName = "...", IsLastOne = true });

                SelectedCat(Cats[0]);

                var catlistResponse = await _playlistService.GetCatlistAsync();
                catlistResponse.Sub?.ForEach(x => x.DisplayName = x.Name);
                foreach (var cat in catlistResponse.Categories)
                {
                    _catOptions.Add(new CatOption
                    {
                        Type = cat.Key,
                        Name = cat.Value,
                        Cats = catlistResponse.Sub?.Where(x => x.Category == cat.Key).ToList()
                    });
                }
            }

            base.OnNavigatedTo(navigationContext);
        }

    }
}
