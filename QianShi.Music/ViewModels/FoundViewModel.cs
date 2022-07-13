using QianShi.Music.Common;
using QianShi.Music.Common.Models;
using QianShi.Music.Common.Models.Request;
using QianShi.Music.Data;
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
        private readonly IFoundDataProvider _foundDataProvider;
        private DelegateCommand<Cat> _addCatCommand = default!;
        private Cat? _currentCat = null;
        private bool _more = false;
        private Visibility _moreCat = Visibility.Collapsed;
        private DelegateCommand _morePlaylistCommand = default!;
        private DelegateCommand<IPlaylist> _openPlaylistCommand = default!;
        private DelegateCommand<Cat> _selectedCatCommand = default!;
        private DelegateCommand<Cat> _switchMoreCatCommand = default!;
        private IFoundPlaylist? _foundPlaylist;

        public FoundViewModel(
            IContainerProvider provider,
            IPlaylistService playlistService,
            INavigationService navigationService,
            IPreferenceService preferenceService,
            IFoundDataProvider foundDataProvider)
            : base(provider)
        {
            _playlistService = playlistService;
            _navigationService = navigationService;
            _preferenceService = preferenceService;
            _foundDataProvider = foundDataProvider;
        }

        public IFoundPlaylist? FoundPlaylist
        {
            get => _foundPlaylist;
            set => SetProperty(ref _foundPlaylist, value);
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

        public DelegateCommand MorePlaylistCommand
            => _morePlaylistCommand ??= new(MorePlaylist, () => !IsBusy && FoundPlaylist is { } && FoundPlaylist.HasMore);

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

        /// <summary>
        /// 更多歌单
        /// </summary>
        private async void MorePlaylist()
        {
            IsBusy = true;
            try
            {
                await FoundPlaylist!.GetDataAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OpenPlaylist(IPlaylist obj)
        {
            _navigationService.NavigateToPlaylist(obj.Id);
        }

        private async void SelectedCat(Cat cat)
        {
            if (_currentCat == cat) return;
            if (_currentCat != null)
                _currentCat.IsActivation = false;
            cat.IsActivation = true;
            _currentCat = cat;

            IsBusy = true;
            try
            {
                FoundPlaylist = _foundDataProvider.CreatePlaylist(cat.Name);
                if (FoundPlaylist.Playlists.Count == 0)
                    await FoundPlaylist.GetDataAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SwitchMoreCat(Cat cat)
        {
            cat.IsActivation = !cat.IsActivation;
            MoreCat = MoreCat == Visibility.Collapsed
                ? MoreCat = Visibility.Visible
                : MoreCat = Visibility.Collapsed;
        }
    }
}