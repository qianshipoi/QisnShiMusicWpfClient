using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Request;
using QianShi.Music.Services;

namespace QianShi.Music.ViewModels
{
    public class FoundViewModel : NavigationViewModel
    {
        public const string PlaylistTypeParameterName = "PlaylistType";
        private const string _catsKey = nameof(Cats);

        private readonly INavigationService _navigationService;
        private readonly IPlaylistService _playlistService;
        private readonly IPreferenceService _preferenceService;
        private DelegateCommand<Cat> _addCatCommand = default!;
        private long _before = 0;
        private Cat? _currentCat = null;
        private bool _more = false;
        private Visibility _moreCat = Visibility.Collapsed;
        private DelegateCommand<ItemsControl> _morePlaylistCommand = default!;
        private int _offset = 0;
        private DelegateCommand<IPlaylist> _openPlaylistCommand = default!;
        private DelegateCommand<Cat> _selectedCatCommand = default!;
        private DelegateCommand<Cat> _switchMoreCatCommand = default!;

        public FoundViewModel(
                                    IContainerProvider provider,
            IPlaylistService playlistService,
            INavigationService navigationService,
            IPreferenceService preferenceService)
            : base(provider)
        {
            _playlistService = playlistService;
            _navigationService = navigationService;
            _preferenceService = preferenceService;
        }

        public ObservableCollection<CatOption> CatOptions { get; } = new();

        public ObservableCollection<Cat> Cats { get; } = new();

        public ObservableCollection<IPlaylist> Playlists { get; } = new();

        public bool More
        {
            get => _more;
            set => SetProperty(ref _more, value);
        }

        public Visibility MoreCat
        {
            get => _moreCat;
            set => SetProperty(ref _moreCat, value);
        }

        public DelegateCommand<Cat> AddCatCommand
            => _addCatCommand ??= new(AddCat);

        public DelegateCommand<ItemsControl> MorePlaylistCommand
            => _morePlaylistCommand ??= new(MorePlaylist);

        public DelegateCommand<IPlaylist> OpenPlaylistCommand
            => _openPlaylistCommand ??= new(OpenPlaylist);

        public DelegateCommand<Cat> SelectedCatCommand
            => _selectedCatCommand ??= new(SelectedCat);

        public DelegateCommand<Cat> SwitchMoreCatCommand
            => _switchMoreCatCommand ??= new(SwitchMoreCat);

        public override async void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (Cats.Count == 0)
            {
                if (_preferenceService.ContainsKey(_catsKey))
                {
                    var value = _preferenceService.Get(_catsKey, string.Empty);
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        var cats = JsonSerializer.Deserialize<List<Cat>>(value);
                        if (cats != null)
                        {
                            Cats.AddRange(cats);
                        }
                    }
                }
                if (Cats.Count == 0)
                {
                    Cats.Add(new Cat { DisplayName = "全部", Name = "全部" });
                    Cats.Add(new Cat { DisplayName = "推荐歌单", Name = "推荐" });
                    Cats.Add(new Cat { DisplayName = "精品歌单", Name = "精品" });
                    Cats.Add(new Cat { DisplayName = "官方", Name = "官方" });
                    Cats.Add(new Cat { DisplayName = "排行榜", Name = "排行榜" });
                    Cats.Add(new Cat { DisplayName = "...", IsLastOne = true });
                }
            }

            var parametes = navigationContext.Parameters;
            var type = "全部";
            if (parametes.ContainsKey(PlaylistTypeParameterName))
            {
                var value = parametes.GetValue<string>("PlaylistType");
                type = string.IsNullOrWhiteSpace(value) ? "全部" : value;

                var first = Cats.FirstOrDefault(x => x.Name == type);
                if (first == null)
                    first = Cats[0];

                if (_currentCat == null || _currentCat.Name != type)
                {
                    SelectedCat(first);
                }
            }
            else
            {
                if (_currentCat == null)
                    SelectedCat(Cats[0]);
            }

            if (CatOptions.Count == 0)
            {
                var catlistResponse = await _playlistService.GetCatlistAsync();
                catlistResponse.Sub?.ForEach(x => x.DisplayName = x.Name);
                var catNames = Cats.Select(x => x.Name).ToHashSet();
                foreach (var cat in catlistResponse.Categories)
                {
                    var cats = catlistResponse.Sub?.Where(x => x.Category == cat.Key).ToList();
                    cats?.Where(x => catNames.Contains(x.Name)).ToList().ForEach(item => item.IsSelected = true);
                    CatOptions.Add(new CatOption
                    {
                        Type = cat.Key,
                        Name = cat.Value,
                        Cats = cats
                    });
                }
            }

            base.OnNavigatedTo(navigationContext);
        }

        private void AddCat(Cat cat)
        {
            if (Cats.Any(x => x.Name != null && x.Name.Equals(cat.Name)))
            {
                cat.IsSelected = false;
                var toBeRemoved = Cats.FirstOrDefault(x => x.Name.Equals(cat.Name));
                if (toBeRemoved != null)
                {
                    Cats.Remove(toBeRemoved);
                }
            }
            else
            {
                cat.IsSelected = true;
                Cats.Insert(Cats.Count - 1, cat);
            }

            _preferenceService.Set(_catsKey, JsonSerializer.Serialize(Cats));
        }

        private async Task CallApi(Cat cat, bool isClear = false)
        {
            IsBusy = true;
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
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// 更多歌单
        /// </summary>
        private async void MorePlaylist(ItemsControl el)
        {
            if (_currentCat != null)
            {
                el.Focus();
                await CallApi(_currentCat);
            }
        }

        private void OpenPlaylist(IPlaylist obj)
        {
            _navigationService.NavigateToPlaylist(obj.Id);
        }

        /// <summary>
        /// 获取类别歌单
        /// </summary>
        /// <param name="catName"></param>
        /// <returns></returns>
        private async Task QueryCatPlaylist(string catName, bool isClear = false)
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
        private async Task QueryRecommendedPalylist()
        {
            var response = await _playlistService.GetPersonalizedAsync();
            if (response != null)
            {
                More = false;
                await UpdatePalylist(response.Result, true);
                More = false;
            }
        }

        /// <summary>
        /// 获取精选歌单
        /// </summary>
        /// <returns></returns>
        private async Task QuerySelectPlaylist(bool isClear = false)
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

        /// <summary>
        /// 获取排行榜列表
        /// </summary>
        /// <returns></returns>
        private async Task QueryToplist()
        {
            var response = await _playlistService.GetToplistAsync();
            if (response != null)
            {
                await UpdatePalylist(response.List, true);
                More = false;
            }
        }

        private async void SelectedCat(Cat cat)
        {
            if (_currentCat == cat) return;
            if (_currentCat != null)
                _currentCat.IsActivation = false;
            cat.IsActivation = true;
            _currentCat = cat;
            _offset = 0;
            _before = 0;
            More = false;

            await CallApi(cat, true);
        }

        private void SwitchMoreCat(Cat cat)
        {
            cat.IsActivation = !cat.IsActivation;
            MoreCat = MoreCat == Visibility.Collapsed
                ? MoreCat = Visibility.Visible
                : MoreCat = Visibility.Collapsed;
        }

        private async Task UpdatePalylist(IEnumerable<IPlaylist> source, bool isClear = false)
        {
            if (isClear)
                Playlists.Clear();
            int i = 0;
            foreach (var sourceItem in source.Where(x => !string.IsNullOrWhiteSpace(x.CoverImgUrl)))
            {
                sourceItem.CoverImgUrl += "?param=200y200";
                Playlists.Add(sourceItem);
                if (i % 5 == 0)
                    await Task.Delay(20);
                i++;
            }
        }
    }
}